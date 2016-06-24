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

class CompanyDetailTabPage : TabPage {
  bool modifying;
  Button acceptButton, cancelButton, logoImageLinkButton, modifyButton;
  ComboBox logoTypeComboBox;
  Interface.ErrorManager errorManager;
  Panel headerPanel;
  TextBox nameTextBox, ficTextBox, addressTextBox, cityTextBox, provinceTextBox, zipTextBox, budgetValidDaysTextBox, logoTypeTextBox, logoImageTextBox;
  public CompanyDetailTabPage () {
    Name = "companyDetail";
    Text = "Empresa";
    errorManager = new Interface.ErrorManager ();
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "Company", "", "");
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
    Interface.Label_Create (this, 0, 216, "Validez prespuesto");
    budgetValidDaysTextBox = Interface.TextBox_Create (this, 128, 216, 18, 1, 3);
    budgetValidDaysTextBox.TextAlign = HorizontalAlignment.Right;
    Interface.Label_Create (this, 0, 244, "Logotipo");
    logoTypeTextBox = Interface.TextBox_Create (this, 128, 244, 128, 1, 255);
    logoTypeTextBox.ReadOnly = true;
    logoTypeComboBox = Interface.ComboBox_Create (this, 128, 244, 128);
    logoTypeComboBox.SelectedIndexChanged += logoTypeComboBox_SelectedIndexChanged;
    Interface.Label_Create (this, 0, 272, "Imagen");
    logoImageTextBox = Interface.TextBox_Create (this, 128, 272, 192, 1, 255);
    logoImageTextBox.ReadOnly = true;
    logoImageLinkButton = Interface.Button_CreateSmall (this, 336, 272, "...", logoImageLinkButton_Click);
    modifyButton = Interface.Button_Create (this, 0, 0, "", modifyButton_Click);
    cancelButton = Interface.Button_Create (this, 0, 0, "", cancelButton_Click);
    Database.Company.Get ();
    setModifying (false);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    (modifying ? nameTextBox as Control : headerPanel).Focus ();
  }
  protected override void OnResize (EventArgs e) {
    base.OnResize (e);
    modifyButton.Top = Height - 24;
    cancelButton.Location = new Point (Width - 80, Height - 24);
  }
  void acceptButton_Click (object o, EventArgs e) {
    if (modifying) modifyButton.PerformClick ();
  }
  void cancelButton_Click (object o, EventArgs e) {
    if (modifying) setModifying (false);
    else Parent.Controls.Remove (this);
  }
  void logoImageLinkButton_Click (object o, EventArgs e) {
    OpenFileDialog openFileDialog = new OpenFileDialog ();
    if (openFileDialog.ShowDialog () == DialogResult.OK) {
      logoImageTextBox.Text = openFileDialog.FileName;
    }
  }
  void logoTypeComboBox_SelectedIndexChanged (object o, EventArgs e) {
    logoImageLinkButton.Visible = logoTypeComboBox.SelectedIndex >= 1;
  }
  void modifyButton_Click (object o, EventArgs e) {
    if (modifying) {
      errorManager.Clear ();
      if (nameTextBox.Text == "") errorManager.Add (nameTextBox, "El nombre ha de especificarse");
      int? budgetValidDays = Data.Int_Parse (budgetValidDaysTextBox.Text);
      if (budgetValidDaysTextBox.Text == "") errorManager.Add (budgetValidDaysTextBox, "La validez de presupuesto ha de especificarse");
      else if (budgetValidDays == null || budgetValidDays < 0 || budgetValidDays >= 1000) errorManager.Add (budgetValidDaysTextBox, "La validez de presupuesto especificada no es válida");
      if (logoTypeComboBox.SelectedIndex >= 1 && logoImageTextBox.Text == "") errorManager.Add (logoImageLinkButton, "La imagen ha de especificarse");
      if (errorManager.Controls.Count > 0) {
        errorManager.Controls [0].Focus ();
        return;
      }
      Database.Company.Update (nameTextBox.Text, ficTextBox.Text, addressTextBox.Text, cityTextBox.Text, provinceTextBox.Text, zipTextBox.Text, (int) budgetValidDays, logoTypeComboBox.SelectedIndex, logoTypeComboBox.SelectedIndex >= 1 ? logoImageTextBox.Text : "");
      Database.Company.Get ();
    }
    setModifying (!modifying);
  }
  void setModifying (bool value) {
    modifying = value;
    if (!modifying) errorManager.Clear ();
    Interface.HeaderPanel_SetText (headerPanel, Text, modifying ? "Modificar" : "Detalle");
    nameTextBox.ReadOnly = !modifying;
    nameTextBox.Text = Database.Company.Name;
    ficTextBox.ReadOnly = !modifying;
    ficTextBox.Text = Database.Company.FIC;
    addressTextBox.ReadOnly = !modifying;
    addressTextBox.Text = Database.Company.Address;
    cityTextBox.ReadOnly = !modifying;
    cityTextBox.Text = Database.Company.City;
    provinceTextBox.ReadOnly = !modifying;
    provinceTextBox.Text = Database.Company.Province;
    zipTextBox.ReadOnly = !modifying;
    zipTextBox.Text = Database.Company.ZIP;
    budgetValidDaysTextBox.ReadOnly = !modifying;
    budgetValidDaysTextBox.Text = Database.Company.BudgetValidDays.ToString ();
    logoTypeTextBox.Visible = !modifying;
    logoTypeTextBox.Text = Database.LogoTypes [Database.Company.LogoType];
    logoTypeComboBox.Visible = modifying;
    if (modifying) for (int i = 0; i < Database.LogoTypes.Length; i++) Interface.ComboBox_AddItem (logoTypeComboBox, Database.LogoTypes [i], i == Database.Company.LogoType);
    else logoTypeComboBox.Items.Clear ();
    logoImageTextBox.Text = Database.Company.LogoImage;
    logoImageLinkButton.Visible = modifying && Database.Company.LogoType >= 1;
    modifyButton.Text = modifying ? "Aceptar" : "Modificar";
    cancelButton.Text = modifying ? "Cancelar" : "Cerrar";
    (modifying ? nameTextBox as Control : headerPanel).Focus ();
  }
}

