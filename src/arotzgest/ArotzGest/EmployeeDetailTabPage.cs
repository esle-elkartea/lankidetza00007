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

class EmployeeDetailTabPage : TabPage {
  bool modifying;
  Button acceptButton, cancelButton, categoryLinkButton, deleteButton, modifyButton;
  ComboBox categoryComboBox;
  Database.Employee item;
  Interface.ErrorManager errorManager;
  Panel headerPanel;
  TextBox nameTextBox, categoryTextBox, finTextBox, ssnTextBox, addressTextBox, cityTextBox, provinceTextBox, zipTextBox, phoneTextBox, mobileTextBox, emailTextBox, accountTextBox, notesTextBox;
  public EmployeeDetailTabPage (int id) {
    errorManager = new Interface.ErrorManager ();
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "Employee", "", "");
    Interface.Label_Create (this, 0, 48, "Nombre");
    nameTextBox = Interface.TextBox_Create (this, 128, 48, 192, 1, 255);
    Interface.Label_Create (this, 0, 76, "Categoría");
    categoryTextBox = Interface.TextBox_Create (this, 128, 76, 128, 1, 255);
    categoryTextBox.ReadOnly = true;
    categoryLinkButton = Interface.Button_CreateSmall (this, 272, 76, ">", categoryLinkButton_Click);
    categoryComboBox = Interface.ComboBox_Create (this, 128, 76, 128);
    Interface.Label_Create (this, 0, 104, "NIF");
    finTextBox = Interface.TextBox_Create (this, 128, 104, 59, 1, 10);
    Interface.Label_Create (this, 0, 132, "NSS");
    ssnTextBox = Interface.TextBox_Create (this, 128, 132, 78, 1, 13);
    Interface.Label_Create (this, 0, 160, "Dirección");
    addressTextBox = Interface.TextBox_Create (this, 128, 160, 192, 1, 255);
    Interface.Label_Create (this, 0, 188, "Localidad");
    cityTextBox = Interface.TextBox_Create (this, 128, 188, 128, 1, 255);
    Interface.Label_Create (this, 0, 216, "Provincia");
    provinceTextBox = Interface.TextBox_Create (this, 128, 216, 128, 1, 255);
    Interface.Label_Create (this, 0, 244, "Código postal");
    zipTextBox = Interface.TextBox_Create (this, 128, 244, 30, 1, 5);
    Interface.Label_Create (this, 0, 272, "Teléfono");
    phoneTextBox = Interface.TextBox_Create (this, 128, 272, 57, 1, 10);
    Interface.Label_Create (this, 0, 300, "Móvil");
    mobileTextBox = Interface.TextBox_Create (this, 128, 300, 57, 1, 10);
    Interface.Label_Create (this, 0, 328, "Email");
    emailTextBox = Interface.TextBox_Create (this, 128, 328, 192, 1, 255);
    Interface.Label_Create (this, 0, 356, "Cuenta");
    accountTextBox = Interface.TextBox_Create (this, 128, 356, 129, 1, 23);
    Interface.Label_Create (this, 360, 48, "Notas");
    notesTextBox = Interface.TextBox_Create (this, 360, 76, 256, 4, 65535);
    modifyButton = Interface.Button_Create (this, 0, 0, "", modifyButton_Click);
    deleteButton = Interface.Button_Create (this, 88, 0, "Borrar", deleteButton_Click);
    cancelButton = Interface.Button_Create (this, 0, 0, "", cancelButton_Click);
    if (id > 0) item = Database.Employee.Get (id);
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
  void categoryLinkButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).EmployeeCategoryDetailTabPage_Open (item.CategoryId);
  }
  void deleteButton_Click (object o, EventArgs e) {
    if (!Interface.ConfirmationDialog_Show ("Está seguro de que desea borrar el empleado actual?")) return;
    if (item.CheckUsed ()) {
      Interface.ErrorDialog_Show ("El empleado actual no se puede borrar por que tiene entidades relacionadas");
      return;
    }
    Database.Employee.Delete (item.Id);
    Parent.Controls.Remove (this);
  }
  void modifyButton_Click (object o, EventArgs e) {
    if (modifying) {
      errorManager.Clear ();
      if (nameTextBox.Text == "") errorManager.Add (nameTextBox, "El nombre ha de especificarse");
      else if (!Database.Employee.CheckUnique (item, nameTextBox.Text)) errorManager.Add (nameTextBox, "Ya existe un empleado con el nombre especificado");
      Database.EmployeeCategory category = categoryComboBox.SelectedItem as Database.EmployeeCategory;
      if (categoryComboBox.SelectedItem == null) errorManager.Add (categoryComboBox, "La categoría ha de especificarse");
      else if (Database.EmployeeCategory.Get (category.Id) == null) errorManager.Add (categoryComboBox, "La categoría especificada no existe");
      if (errorManager.Controls.Count > 0) {
        errorManager.Controls [0].Focus ();
        return;
      }
      Database.Employee criteria = new Database.Employee (nameTextBox.Text, category.Id, finTextBox.Text, ssnTextBox.Text, addressTextBox.Text, cityTextBox.Text, provinceTextBox.Text, zipTextBox.Text, phoneTextBox.Text, mobileTextBox.Text, emailTextBox.Text, accountTextBox.Text, notesTextBox.Text);
      item = item == null ? Database.Employee.Create (criteria) : item.Update (criteria);
    }
    setModifying (!modifying);
  }
  void setModifying (bool value) {
    modifying = value;
    if (!modifying) errorManager.Clear ();
    Name = "employeeDetail_" + (item == null ? 0 : item.Id);
    Text = "Empleado: " + (item == null ? "" : item.Name);
    Interface.HeaderPanel_SetText (headerPanel, Text, modifying ? item == null ? "Crear" : "Modificar" : "Detalle");
    nameTextBox.ReadOnly = !modifying;
    nameTextBox.Text = item == null ? "" : item.Name;
    categoryTextBox.Visible = !modifying;
    categoryTextBox.Text = item == null ? "" : item.CategoryName;
    categoryLinkButton.Visible = !modifying;
    categoryComboBox.Visible = modifying;
    if (modifying) foreach (Database.EmployeeCategory category in Database.EmployeeCategory.List ()) Interface.ComboBox_AddItem (categoryComboBox, category, item != null && category.Id == item.CategoryId);
    else categoryComboBox.Items.Clear ();
    finTextBox.ReadOnly = !modifying;
    finTextBox.Text = item == null ? "" : item.FIN;
    ssnTextBox.ReadOnly = !modifying;
    ssnTextBox.Text = item == null ? "" : item.SSN;
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
    accountTextBox.ReadOnly = !modifying;
    accountTextBox.Text = item == null ? "" : item.Account;
    notesTextBox.ReadOnly = !modifying;
    notesTextBox.Text = item == null ? "" : item.Notes;
    modifyButton.Text = modifying ? "Aceptar" : "Modificar";
    deleteButton.Enabled = item != null;
    cancelButton.Text = modifying ? "Cancelar" : "Cerrar";
    (!modifying ? headerPanel as Control : nameTextBox).Focus ();
  }
}

