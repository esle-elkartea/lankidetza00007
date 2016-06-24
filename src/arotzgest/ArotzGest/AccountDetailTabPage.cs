/*  ArotzGest
 *  (c) 2006 Webalianza T.I S.L.
 *  Licenciado bajo la GNU General Public License
 *  
 *  Programaci�n: Jorge Moreiro
 *                Iker Est�banez
 * 
 *  Este fichero forma parte de ArotzGest
 *
 * ArotzGest es software libre; puede redistribuirlo y/o modificarlo
 * bajo los t�rminos de la GNU General Public License, tal y como se haya
 * publicada por la Free Software Foundation, en la versi�n 2 de la licencia o
 * (seg�n su elecci�n) cualquier versi�n posterior.
 * 
 * ArotzGest es redistribuido con la intenci�n que sea �til, pero SIN NINGUNA
 * GARANT�A, ni tan solo las garant�as impl�citas de MERCANTABILIDA o ADECUACI�N 
 * A UN DETERMINADO MOTIVO. Lea la GNU General Public License para m�s detalles.
 * 
 * Deber�a haber recibido una copia de la GNU General Public License acompa�ando a 
 * ArotzGest.
 * 
 * �STE PROYECTO HA SIDO SUBVENCIONADO POR SPRI S.A. DENTRO DEL MARCO DEL PROGRAMA
 * KZ LANKIDETZA - m�s informaci�n en http://www.spri.es
 * 
 *
 * */

using System;
using System.Drawing;
using System.Windows.Forms;

class AccountDetailTabPage : TabPage {
  bool modifying; 
  Button acceptButton, cancelButton, deleteButton, modifyButton;
  Database.Account item;
  Interface.ErrorManager errorManager;
  Panel headerPanel;
  TextBox nameTextBox, codeTextBox;
  public AccountDetailTabPage (int id) {
    errorManager = new Interface.ErrorManager ();
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "Account", "", "");
    Interface.Label_Create (this, 0, 48, "C�digo");
    codeTextBox = Interface.TextBox_Create (this, 128, 48, 54, 1, 9);
    Interface.Label_Create (this, 0, 76, "Nombre");
    nameTextBox = Interface.TextBox_Create (this, 128, 76, 192, 1, 255);
    modifyButton = Interface.Button_Create (this, 0, 0, "", modifyButton_Click);
    deleteButton = Interface.Button_Create (this, 88, 0, "Borrar", deleteButton_Click);
    cancelButton = Interface.Button_Create (this, 0, 0, "", cancelButton_Click);
    if (id > 0) item = Database.Account.Get (id);
    setModifying (item == null);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    (modifying ? codeTextBox as Control : headerPanel).Focus ();
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
    if (!Interface.ConfirmationDialog_Show ("Est� seguro de que desea borrar la cuenta actual?")) return;
    if (item.CheckUsed ()) {
      Interface.ErrorDialog_Show ("La cuenta actual no se puede borrar por que tiene entidades relacionadas");
      return;
    }
    Database.Account.Delete (item.Id);
    Parent.Controls.Remove (this);
  }
  void modifyButton_Click (object o, EventArgs e) {
    if (modifying) {
      errorManager.Clear ();
      if (codeTextBox.Text == "") errorManager.Add (codeTextBox, "La cuenta ha de especificarse");
      else if (Data.Int_Parse (codeTextBox.Text) == null) errorManager.Add (codeTextBox, "El c�digo especificado no es v�lido");
      else {
        codeTextBox.Text = codeTextBox.Text.PadRight (9, '0');
        if (!Database.Account.CheckUnique (item, codeTextBox.Text)) errorManager.Add (codeTextBox, "Ya existe una cuenta con el c�digo especificado");
      }
      if (nameTextBox.Text == "") errorManager.Add (nameTextBox, "El nombre ha de especificarse");
      if (errorManager.Controls.Count > 0) {
        errorManager.Controls [0].Focus ();
        return;
      }
      Database.Account criteria = new Database.Account (codeTextBox.Text, nameTextBox.Text);
      item = item == null ? Database.Account.Create (criteria) : item.Update (criteria);
    }
    setModifying (!modifying);
  }
  void setModifying (bool value) {
    modifying = value;
    if (!modifying) errorManager.Clear ();
    Name = "accountDetail_" + (item == null ? 0 : item.Id);
    Text = "Cuenta: " + (item == null ? "" : item.Name);
    Interface.HeaderPanel_SetText (headerPanel, Text, modifying ? item == null ? "Crear" : "Modificar" : "Detalle");
    codeTextBox.ReadOnly = !modifying;
    codeTextBox.Text = item == null ? "" : item.Code;
    nameTextBox.ReadOnly = !modifying;
    nameTextBox.Text = item == null ? "" : item.Name;
    modifyButton.Text = modifying ? "Aceptar" : "Modificar";
    deleteButton.Enabled = item != null;
    cancelButton.Text = modifying ? "Cancelar" : "Cerrar";
    (modifying ? codeTextBox as Control : headerPanel).Focus ();
  }
}

