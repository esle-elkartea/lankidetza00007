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

class EmployeeCategoryDetailTabPage : TabPage {
  bool modifying; 
  Button acceptButton, cancelButton, deleteButton, modifyButton;
  Database.EmployeeCategory item;
  Interface.ErrorManager errorManager;
  Panel headerPanel;
  TextBox costTextBox, nameTextBox, priceTextBox;
  public EmployeeCategoryDetailTabPage (int id) {
    errorManager = new Interface.ErrorManager ();
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "Client", "", "");
    Interface.Label_Create (this, 0, 48, "Nombre");
    nameTextBox = Interface.TextBox_Create (this, 128, 48, 128, 1, 255);
    Interface.Label_Create (this, 0, 76, "Coste / hora");
    costTextBox = Interface.TextBox_Create (this, 128, 76, 51, 1, 9);
    costTextBox.TextAlign = HorizontalAlignment.Right;
    Interface.Label_Create (this, 0, 104, "Precio / hora");
    priceTextBox = Interface.TextBox_Create (this, 128, 104, 51, 1, 9);
    priceTextBox.TextAlign = HorizontalAlignment.Right;
    modifyButton = Interface.Button_Create (this, 0, 0, "", modifyButton_Click);
    deleteButton = Interface.Button_Create (this, 88, 0, "Borrar", deleteButton_Click);
    cancelButton = Interface.Button_Create (this, 0, 0, "", cancelButton_Click);
    if (id > 0) item = Database.EmployeeCategory.Get (id);
    setModifying (item == null);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (Parent.Parent as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
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
    if (!Interface.ConfirmationDialog_Show ("Está seguro de que desea borrar la categoría de empleado actual?")) return;
    if (item.CheckUsed ()) {
      Interface.ErrorDialog_Show ("La categoría de empleado actual no se puede borrar por que tiene entidades relacionadas");
      return;
    }
    Database.EmployeeCategory.Delete (item.Id);
    Parent.Controls.Remove (this);
  }
  void modifyButton_Click (object o, EventArgs e) {
    if (modifying) {
      errorManager.Clear ();
      if (nameTextBox.Text == "") errorManager.Add (nameTextBox, "El nombre ha de especificarse");
      else if (!Database.EmployeeCategory.CheckUnique (item, nameTextBox.Text)) errorManager.Add (nameTextBox, "Ya existe una categoría de empleado con el nombre especificado");
      double? cost = Data.Double_Parse (costTextBox.Text);
      if (costTextBox.Text == "") errorManager.Add (costTextBox, "El coste / hora ha de especificarse");
      else if (cost == null || cost < 0 || cost >= 1000000) errorManager.Add (costTextBox, "El coste / hora especificado no es válido");
      double? price = Data.Double_Parse (priceTextBox.Text);
      if (priceTextBox.Text == "") errorManager.Add (priceTextBox, "El precio / hora ha de especificarse");
      else if (price == null || price < 0 || price >= 1000000) errorManager.Add (priceTextBox, "El precio / hora especificado no es válido");
      if (errorManager.Controls.Count > 0) {
        errorManager.Controls [0].Focus ();
        return;
      }
      Database.EmployeeCategory criteria = new Database.EmployeeCategory (nameTextBox.Text, (double) cost, (double) price);
      item = item == null ? Database.EmployeeCategory.Create (criteria) : item.Update (criteria);
    }
    setModifying (!modifying);
  }
  void setModifying (bool value) {
    modifying = value;
    if (!modifying) errorManager.Clear ();
    Name = "employeeCategoryDetail_" + (item == null ? 0 : item.Id);
    Text = "Categoría de empleado: " + (item == null ? "" : item.Name);
    Interface.HeaderPanel_SetText (headerPanel, Text, modifying ? item == null ? "Crear" : "Modificar" : "Detalle");
    nameTextBox.ReadOnly = !modifying;
    nameTextBox.Text = item == null ? "" : item.Name;
    costTextBox.ReadOnly = !modifying;
    costTextBox.Text = item == null ? "" : Data.Double_Format (item.Cost);
    priceTextBox.ReadOnly = !modifying;
    priceTextBox.Text = item == null ? "" : Data.Double_Format (item.Price);
    modifyButton.Text = modifying ? "Aceptar" : "Modificar";
    deleteButton.Enabled = item != null;
    cancelButton.Text = modifying ? "Cancelar" : "Cerrar";
    (modifying ? nameTextBox as Control : headerPanel).Focus ();
  }
}

