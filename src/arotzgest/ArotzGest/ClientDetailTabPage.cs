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

class ClientDetailTabPage : TabPage {
  bool modifying; 
  Button acceptButton, cancelButton, deleteButton, modifyButton;
  Database.Client item;
  Interface.ErrorManager errorManager;
  Panel headerPanel;
  TextBox nameTextBox, ficTextBox, addressTextBox, cityTextBox, provinceTextBox, zipTextBox, phoneTextBox, mobileTextBox, emailTextBox, homepageTextBox, accountTextBox, notesTextBox;
  public ClientDetailTabPage (int id) {
    errorManager = new Interface.ErrorManager ();
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "Client", "", "");
    Interface.Label_Create (this, 0, 48, "Nombre");
    nameTextBox = Interface.TextBox_Create (this, 128, 48, 192, 1, 255);
    Interface.Label_Create (this, 0, 76, "CIF/NIF");
    ficTextBox = Interface.TextBox_Create (this, 128, 76, 59, 1, 10);
    Interface.Label_Create (this, 0, 104, "Dirección");
    addressTextBox = Interface.TextBox_Create (this, 128, 104, 192, 1, 255);
    Interface.Label_Create (this, 0, 132, "Localidad");
    cityTextBox = Interface.TextBox_Create (this, 128, 132, 128, 1, 255);
    Interface.Label_Create (this, 0, 160, "Provincia");
    provinceTextBox = Interface.TextBox_Create (this, 128, 160, 128, 1, 255);
    Interface.Label_Create (this, 0, 188, "Código postal");
    zipTextBox = Interface.TextBox_Create (this, 128, 188, 30, 1, 5);
    Interface.Label_Create (this, 0, 216, "Teléfono");
    phoneTextBox = Interface.TextBox_Create (this, 128, 216, 57, 1, 10);
    Interface.Label_Create (this, 0, 244, "Móvil");
    mobileTextBox = Interface.TextBox_Create (this, 128, 244, 57, 1, 10);
    Interface.Label_Create (this, 0, 272, "Email");
    emailTextBox = Interface.TextBox_Create (this, 128, 272, 192, 1, 255);
    Interface.Label_Create (this, 0, 300, "Web");
    homepageTextBox = Interface.TextBox_Create (this, 128, 300, 192, 1, 255);
    Interface.Label_Create (this, 0, 328, "Cuenta");
    accountTextBox = Interface.TextBox_Create (this, 128, 328, 129, 1, 23);
    Interface.Label_Create (this, 360, 48, "Notas");
    notesTextBox = Interface.TextBox_Create (this, 360, 76, 256, 4, 65535);
    modifyButton = Interface.Button_Create (this, 0, 0, "", modifyButton_Click);
    deleteButton = Interface.Button_Create (this, 88, 0, "Borrar", deleteButton_Click);
    cancelButton = Interface.Button_Create (this, 0, 0, "", cancelButton_Click);
    if (id > 0) item = Database.Client.Get (id);
    setModifying (item == null);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    (modifying ? nameTextBox as Control : headerPanel).Focus ();
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
  void cancelButton_Click (object o, EventArgs e) {
    if (modifying && item != null) setModifying (false);
    else Parent.Controls.Remove (this);
  }
  void deleteButton_Click (object o, EventArgs e) {
    if (!Interface.ConfirmationDialog_Show ("Está seguro de que desea borrar el cliente actual?")) return;
    if (item.CheckUsed ()) {
      Interface.ErrorDialog_Show ("El cliente actual no se puede borrar por que tiene entidades relacionadas");
      return;
    }
    Database.Client.Delete (item.Id);
    Parent.Controls.Remove (this);
  }
  void modifyButton_Click (object o, EventArgs e) {
    if (modifying) {
      errorManager.Clear ();
      if (nameTextBox.Text == "") errorManager.Add (nameTextBox, "El nombre ha de especificarse");
      else if (!Database.Client.CheckUnique (item, nameTextBox.Text)) errorManager.Add (nameTextBox, "Ya existe un cliente con el nombre especificado");
      if (errorManager.Controls.Count > 0) {
        errorManager.Controls [0].Focus ();
        return;
      }
      Database.Client criteria = new Database.Client (nameTextBox.Text, ficTextBox.Text, addressTextBox.Text, cityTextBox.Text, provinceTextBox.Text, zipTextBox.Text, phoneTextBox.Text, mobileTextBox.Text, emailTextBox.Text, homepageTextBox.Text, accountTextBox.Text, notesTextBox.Text);
      item = item == null ? Database.Client.Create (criteria) : item.Update (criteria);
    }
    setModifying (!modifying);
  }
  void setModifying (bool value) {
    modifying = value;
    if (!modifying) errorManager.Clear ();
    Name = "clientDetail_" + (item == null ? 0 : item.Id);
    Text = "Cliente: " + (item == null ? "" : item.Name);
    Interface.HeaderPanel_SetText (headerPanel, Text, modifying ? item == null ? "Crear" : "Modificar" : "Detalle");
    nameTextBox.ReadOnly = !modifying;
    nameTextBox.Text = item == null ? "" : item.Name;
    ficTextBox.ReadOnly = !modifying;
    ficTextBox.Text = item == null ? "" : item.FIC;
    addressTextBox.ReadOnly = !modifying;
    addressTextBox.Text = item == null ? "" : item.Address;
    cityTextBox.ReadOnly = !modifying;
    cityTextBox.Text = item == null ? "" : item.City;
    provinceTextBox.ReadOnly = !modifying;
    provinceTextBox.Text = item == null ? "" : item.Province;
    zipTextBox.ReadOnly = !modifying;
    zipTextBox.Text = item == null ? "" : item.ZIP;
    phoneTextBox.ReadOnly = !modifying;
    phoneTextBox.Text = item == null ? "" : item.Phone;
    mobileTextBox.ReadOnly = !modifying;
    mobileTextBox.Text = item == null ? "" : item.Mobile;
    emailTextBox.ReadOnly = !modifying;
    emailTextBox.Text = item == null ? "" : item.Email;
    homepageTextBox.ReadOnly = !modifying;
    homepageTextBox.Text = item == null ? "" : item.Homepage;
    accountTextBox.ReadOnly = !modifying;
    accountTextBox.Text = item == null ? "" : item.Account;
    notesTextBox.ReadOnly = !modifying;
    notesTextBox.Text = item == null ? "" : item.Notes;
    modifyButton.Text = modifying ? "Aceptar" : "Modificar";
    deleteButton.Enabled = item != null;
    cancelButton.Text = modifying ? "Cancelar" : "Cerrar";
    (modifying ? nameTextBox as Control : headerPanel).Focus ();
  }
}

