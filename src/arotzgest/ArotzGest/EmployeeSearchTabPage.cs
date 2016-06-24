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

class EmployeeSearchTabPage : TabPage {
  Button acceptButton, cancelButton, createButton, detailButton, searchButton;
  ComboBox categoryComboBox;
  ListView itemListView;
  TextBox nameTextBox;
  public EmployeeSearchTabPage () {
    Name = "employeeSearch";
    Text = "Empleados";
    acceptButton = Interface.Button_Create (acceptButton_Click);
    Interface.HeaderPanel_Create (this, 0, 8, "Employee", Text, "Buscar");
    Interface.Label_Create (this, 0, 48, "Nombre");
    nameTextBox = Interface.TextBox_Create (this, 128, 48, 128, 1, 255);
    Interface.Label_Create (this, 0, 76, "Categoría");
    categoryComboBox = Interface.ComboBox_Create (this, 128, 76, 128);
    Interface.ComboBox_AddItem (categoryComboBox, "", true);
    foreach (Database.EmployeeCategory category in Database.EmployeeCategory.List ()) Interface.ComboBox_AddItem (categoryComboBox, category, false);
    searchButton = Interface.Button_Create (this, 0, 104, "Buscar", searchButton_Click);
    itemListView = Interface.ListView_Create (this, 0, 144, 0, 0, itemListView_SelectedIndexChanged, itemListView_DoubleClick);
    Interface.ListView_AddColumnHeader (itemListView, 192, "Nombre");
    Interface.ListView_AddColumnHeader (itemListView, 128, "Categoría");
    createButton = Interface.Button_Create (this, 0, 0, "Crear", createButton_Click);
    detailButton = Interface.Button_Create (this, 88, 0, "Detalle", detailButton_Click);
    detailButton.Enabled = false;
    cancelButton = Interface.Button_Create (this, 0, 0, "Cerrar", cancelButton_Click);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    (itemListView.Items.Count == 0 ? nameTextBox as Control : itemListView).Focus ();
  }
  protected override void OnResize (EventArgs e) {
    base.OnResize (e);
    itemListView.Size = new Size (Width, Height - itemListView.Top - 32);
    createButton.Top = Height - 24;
    detailButton.Top = Height - 24;
    cancelButton.Location = new Point (Width - 80, Height - 24);
  }
  void acceptButton_Click (object o, EventArgs e) {
    (!itemListView.ContainsFocus ? searchButton : detailButton).PerformClick ();
  }
  void cancelButton_Click (object o, EventArgs e) {
    Parent.Controls.Remove (this);
  }
  void createButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).EmployeeDetailTabPage_Open (0);
  }
  void detailButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).EmployeeDetailTabPage_Open ((int) itemListView.SelectedItems [0].Tag);
  }
  void itemListView_DoubleClick (object o, EventArgs e) {
    detailButton.PerformClick ();
  }
  void itemListView_SelectedIndexChanged (object o, EventArgs e) {
    detailButton.Enabled = itemListView.SelectedItems.Count > 0;
  }
  void searchButton_Click (object o, EventArgs e) {
    Interface.ListView_Clear (itemListView);
    int categoryId = 0;
    if (categoryComboBox.SelectedIndex > 0) categoryId = (categoryComboBox.SelectedItem as Database.EmployeeCategory).Id;
    foreach (Database.Employee item in Database.Employee.Search (nameTextBox.Text, categoryId)) Interface.ListView_AddListViewItem (itemListView, new string [] { item.Name, item.CategoryName }, item.Id);
    if (itemListView.Items.Count > 0) itemListView.Focus ();
  }
}

