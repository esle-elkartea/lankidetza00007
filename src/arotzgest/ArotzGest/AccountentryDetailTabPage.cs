/*  ArotzGest
 *  (c) 2006 Webalianza T.I S.L.
 *  Licenciado bajo la GNU General Public License
 *  
 *  Programación: Jorge Moreiro
 *                Iker Estébanez
 * 
 *  Este fichero forma parte de ArotzGest
 *
 * ArotzGest es software libre; puede redistribuirlo y/o modificarlo
 * bajo los términos de la GNU General Public License, tal y como se haya
 * publicada por la Free Software Foundation, en la versión 2 de la licencia o
 * (según su elección) cualquier versión posterior.
 * 
 * ArotzGest es redistribuido con la intención que sea útil, pero SIN NINGUNA
 * GARANTÍA, ni tan solo las garantías implícitas de MERCANTABILIDA o ADECUACIÓN 
 * A UN DETERMINADO MOTIVO. Lea la GNU General Public License para más detalles.
 * 
 * Debería haber recibido una copia de la GNU General Public License acompañando a 
 * ArotzGest.
 * 
 * ÉSTE PROYECTO HA SIDO SUBVENCIONADO POR SPRI S.A. DENTRO DEL MARCO DEL PROGRAMA
 * KZ LANKIDETZA - más información en http://www.spri.es
 * 
 *
 * */

using System;
using System.Drawing;
using System.Windows.Forms;

class AccountEntryDetailTabPage : TabPage {
  bool modifying; 
  Button acceptButton, accountLinkButton, cancelButton, deleteButton, modifyButton;
  ComboBox accountComboBox;
  Database.AccountEntry item;
  Interface.ErrorManager errorManager;
  Panel headerPanel;
  TextBox nameTextBox, accountTextBox, dateTextBox, priceTextBox;
  public AccountEntryDetailTabPage (int id) {
    errorManager = new Interface.ErrorManager ();
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "AccountEntry", "", "");
    Interface.Label_Create (this, 0, 48, "Cuenta");
    accountTextBox = Interface.TextBox_Create (this, 128, 48, 255, 1, 0);
    accountTextBox.ReadOnly = true;
    accountLinkButton = Interface.Button_CreateSmall (this, 399, 48, ">", accountLinkButton_Click);
    accountComboBox = Interface.ComboBox_Create (this, 128, 48, 255);
    Interface.Label_Create (this, 0, 76, "Fecha");
    dateTextBox = Interface.TextBox_Create (this, 128, 76, 54, 1, 10);
    Interface.Label_Create (this, 0, 104, "Concepto");
    nameTextBox = Interface.TextBox_Create (this, 128, 104, 192, 1, 255);
    Interface.Label_Create (this, 0, 132, "Importe");
    priceTextBox = Interface.TextBox_Create (this, 128, 132, 57, 1, 10);
    priceTextBox.TextAlign = HorizontalAlignment.Right;
    modifyButton = Interface.Button_Create (this, 0, 0, "", modifyButton_Click);
    deleteButton = Interface.Button_Create (this, 88, 0, "Borrar", deleteButton_Click);
    cancelButton = Interface.Button_Create (this, 0, 0, "", cancelButton_Click);
    if (id > 0) item = Database.AccountEntry.Get (id);
    setModifying (item == null);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    (modifying ? accountComboBox as Control : headerPanel).Focus ();
  }
  protected override void OnResize (EventArgs e) {
    base.OnResize (e);
    modifyButton.Top = Height - 24;
    deleteButton.Top = Height - 24;
    cancelButton.Location = new Point (Width - 80, Height - 24);
  }
  void acceptButton_Click (object o, EventArgs e) {
    if (modifying) modifyButton.PerformClick ();
  }
  void accountLinkButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).AccountDetailTabPage_Open (item.AccountId);
  }
  void cancelButton_Click (object o, EventArgs e) {
    if (modifying && item != null) setModifying (false);
    else Parent.Controls.Remove (this);
  }
  void deleteButton_Click (object o, EventArgs e) {
    if (!Interface.ConfirmationDialog_Show ("Está seguro de que desea borrar el movimiento actual?")) return;
    Database.AccountEntry.Delete (item.Id);
    Parent.Controls.Remove (this);
  }
  void modifyButton_Click (object o, EventArgs e) {
    if (modifying) {
      errorManager.Clear ();
      Database.Account account = accountComboBox.SelectedItem as Database.Account;
      if (accountComboBox.SelectedItem == null) errorManager.Add (accountComboBox, "La cuenta ha de especificarse");
      else if (Database.Account.Get (account.Id) == null) errorManager.Add (accountComboBox, "La cuenta especificada no existe");
      DateTime? date = Data.Date_Parse (dateTextBox.Text);
      if (dateTextBox.Text == "") errorManager.Add (dateTextBox, "La fecha ha de especificarse");
      else if (date == null) errorManager.Add (dateTextBox, "La fecha especificada no es válida");
      if (nameTextBox.Text == "") errorManager.Add (nameTextBox, "El nombre ha de especificarse");
      double? price = Data.Double_Parse (priceTextBox.Text);
      if (priceTextBox.Text == "") errorManager.Add (priceTextBox, "El importe ha de especificarse");
      else if (price == null || price <= -1000000 || price >= 1000000) errorManager.Add (priceTextBox, "El importe especificado no es válido");
      if (errorManager.Controls.Count > 0) {
        errorManager.Controls [0].Focus ();
        return;
      }
      Database.AccountEntry criteria = new Database.AccountEntry (nameTextBox.Text, account.Id, (DateTime) date, (double) price);
      item = item == null ? Database.AccountEntry.Create (criteria) : item.Update (criteria);
    }
    setModifying (!modifying);
  }
  void setModifying (bool value) {
    modifying = value;
    if (!modifying) errorManager.Clear ();
    Name = "accountEntryDetail_" + (item == null ? 0 : item.Id);
    Text = "Movimiento: " + (item == null ? "" : item.Name);
    Interface.HeaderPanel_SetText (headerPanel, Text, modifying ? item == null ? "Crear" : "Modificar" : "Detalle");
    accountTextBox.Visible = !modifying;
    accountTextBox.Text = item == null ? "" : item.AccountCode + " - " + item.AccountName;
    accountLinkButton.Visible = !modifying;
    accountComboBox.Visible = modifying;
    if (modifying) foreach (Database.Account account in Database.Account.List ()) Interface.ComboBox_AddItem (accountComboBox, account, item != null && account.Id == item.AccountId);
    else accountComboBox.Items.Clear ();
    dateTextBox.ReadOnly = !modifying;
    dateTextBox.Text = item == null ? Data.Date_Format (DateTime.Now) : Data.Date_Format (item.Date);
    nameTextBox.ReadOnly = !modifying;
    nameTextBox.Text = item == null ? "" : item.Name;
    priceTextBox.ReadOnly = !modifying;
    priceTextBox.Text = item == null ? "" : Data.Double_Format (item.Price);
    modifyButton.Text = modifying ? "Aceptar" : "Modificar";
    deleteButton.Enabled = item != null;
    cancelButton.Text = modifying ? "Cancelar" : "Cerrar";
    (modifying ? accountComboBox as Control : headerPanel).Focus ();
  }
}

