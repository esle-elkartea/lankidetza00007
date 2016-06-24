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

class TemplateDetailTabPage : TabPage {
  bool modifying;
  Button acceptButton, cancelButton, categoryLinkButton, deleteButton, modifyButton, statisticsButton, subitemAddButton, subitemModifyButton, subitemRemoveButton;
  ComboBox categoryComboBox;
  Database.Template item;
  Interface.ErrorManager errorManager;
  Label costLabel, priceLabel;
  ListView subitemListView;
  Panel headerPanel;
  TextBox costTextBox, descriptionTextBox, nameTextBox, categoryTextBox, priceTextBox;
  public TemplateDetailTabPage (int id) {
    errorManager = new Interface.ErrorManager ();
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "Template", "", "");
    statisticsButton = Interface.Button_Create (this, 0, 48, "Estadísticas", statisticsButton_Click);
    Interface.Label_Create (this, 0, 48, "Nombre");
    nameTextBox = Interface.TextBox_Create (this, 128, 48, 192, 1, 255);
    Interface.Label_Create (this, 0, 76, "Categoría");
    categoryTextBox = Interface.TextBox_Create (this, 128, 76, 128, 1, 255);
    categoryTextBox.ReadOnly = true;
    categoryLinkButton = Interface.Button_CreateSmall (this, 272, 76, ">", categoryLinkButton_Click);
    categoryComboBox = Interface.ComboBox_Create (this, 128, 76, 128);
    Interface.Label_Create (this, 0, 104, "Descripción");
    descriptionTextBox = Interface.TextBox_Create (this, 128, 104, 256, 4, 65535);
    subitemListView = Interface.ListView_Create (this, 0, 179, 0, 0, subitemListView_SelectedIndexChanged, subitemListView_DoubleClick);
    Interface.ListView_AddColumnHeader (subitemListView, 64, "Tipo");
    Interface.ListView_AddColumnHeader (subitemListView, 192, "Descripción");
    ColumnHeader columnHeader = Interface.ListView_AddColumnHeader (subitemListView, 51, "Cant");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    columnHeader = Interface.ListView_AddColumnHeader (subitemListView, 51, "Coste/u");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    columnHeader = Interface.ListView_AddColumnHeader (subitemListView, 51, "Precio/u");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    columnHeader = Interface.ListView_AddColumnHeader (subitemListView, 51, "Coste");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    columnHeader = Interface.ListView_AddColumnHeader (subitemListView, 51, "Precio");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    subitemListView.ListViewItemSorter = new Interface.ListViewComprarer ();
    subitemAddButton = Interface.Button_Create (this, 0, 179, "Añadir", subitemAddButton_Click);
    subitemModifyButton = Interface.Button_Create (this, 0, 211, "Modificar", subitemModifyButton_Click);
    subitemRemoveButton = Interface.Button_Create (this, 0, 243, "Eliminar", subitemRemoveButton_Click);
    costLabel = Interface.Label_Create (this, 0, 0, "Coste");
    costTextBox = Interface.TextBox_Create (this, 128, 0, 51, 1, 0);
    costTextBox.TextAlign = HorizontalAlignment.Right;
    costTextBox.ReadOnly = true;
    priceLabel = Interface.Label_Create (this, 0, 0, "Precio");
    priceTextBox = Interface.TextBox_Create (this, 128, 0, 51, 1, 0);
    priceTextBox.TextAlign = HorizontalAlignment.Right;
    priceTextBox.ReadOnly = true;
    modifyButton = Interface.Button_Create (this, 0, 0, "", modifyButton_Click);
    deleteButton = Interface.Button_Create (this, 88, 0, "Borrar", deleteButton_Click);
    cancelButton = Interface.Button_Create (this, 0, 0, "", cancelButton_Click);
    if (id > 0) item = Database.Template.Get (id);
    setModifying (item == null);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    (modifying ? nameTextBox as Control : headerPanel).Focus ();
  }
  protected override void OnResize (EventArgs e) {
    base.OnResize (e);
    statisticsButton.Left = Width - 80;
    subitemListView.Size = new Size (Width - 88, Height - subitemListView.Top - 96);
    subitemAddButton.Left = Width - 80;
    subitemModifyButton.Left = Width - 80;
    subitemRemoveButton.Left = Width - 80;
    costLabel.Top = Height - 88;
    costTextBox.Top = Height - 88;
    priceLabel.Top = Height - 60;
    priceTextBox.Top = Height - 60;
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
    (TopLevelControl as MainForm).TemplateCategoryDetailTabPage_Open (item.CategoryId);
  }
  void deleteButton_Click (object o, EventArgs e) {
    if (!Interface.ConfirmationDialog_Show ("Está seguro de que desea borrar la plantilla actual?")) return;
    if (item.CheckUsed ()) {
      Interface.ErrorDialog_Show ("La plantilla actual no se puede borrar por que tiene entidades relacionadas");
      return;
    }
    Database.Template.Delete (item.Id);
    Database.Concept.DeleteForTemplate (item.Id);
    Parent.Controls.Remove (this);
  }
  void modifyButton_Click (object o, EventArgs e) {
    if (modifying) {
      errorManager.Clear ();
      if (nameTextBox.Text == "") errorManager.Add (nameTextBox, "El nombre ha de especificarse");
      else if (!Database.Template.CheckUnique (item, nameTextBox.Text)) errorManager.Add (nameTextBox, "Ya existe una plantilla con el nombre especificado");
      Database.TemplateCategory category = categoryComboBox.SelectedItem as Database.TemplateCategory;
      if (categoryComboBox.SelectedItem == null) errorManager.Add (categoryComboBox, "La cateogría ha de especificarse");
      if (descriptionTextBox.Text == "") errorManager.Add (descriptionTextBox, "La descripción ha de especificarse");
      foreach (ListViewItem subitem in subitemListView.Items) {
        if (subitem.Tag is Database.EmployeeCategoryConcept) { if (Database.EmployeeCategory.Get ((subitem.Tag as Database.EmployeeCategoryConcept).Id) == null) errorManager.Add (subitemListView, "La categoría de empleado especificada no existe"); } else if (subitem.Tag is Database.MaterialConcept) { if (Database.Material.Get ((subitem.Tag as Database.MaterialConcept).Id) == null) errorManager.Add (subitemListView, "El material especificado existe"); }
      }
      if (errorManager.Controls.Count > 0) {
        errorManager.Controls [0].Focus ();
        return;
      }
      Database.Template criteria = new Database.Template (nameTextBox.Text, category.Id, descriptionTextBox.Text);
      item = item == null ? Database.Template.Create (criteria) : item.Update (criteria);
      Database.Concept.DeleteForTemplate (item.Id);
      foreach (ListViewItem subitem in subitemListView.Items) (subitem.Tag as Database.Concept).CreateForTemplate (item.Id);
    }
    setModifying (!modifying);
  }
  void recalculate () {
    double cost = 0;
    double price = 0;
    foreach (ListViewItem subitem in subitemListView.Items) {
      Database.Concept concept = subitem.Tag as Database.Concept;
      cost += concept.Quantity * concept.Cost;
      price += concept.Quantity * concept.Price;
    }
    costTextBox.Text = Data.Double_Format (cost);
    priceTextBox.Text = Data.Double_Format (price);
  }
  void setModifying (bool value) {
    modifying = value;
    if (!modifying) errorManager.Clear ();
    Name = "templateDetail_" + (item == null ? 0 : item.Id);
    Text = "Plantilla: " + (item == null ? "" : item.Name);
    Interface.HeaderPanel_SetText (headerPanel, Text, modifying ? item == null ? "Crear" : "Modificar" : "Detalle");
    statisticsButton.Enabled = !modifying;
    nameTextBox.ReadOnly = !modifying;
    nameTextBox.Text = item == null ? "" : item.Name;
    categoryTextBox.Visible = !modifying;
    categoryTextBox.Text = item == null ? "" : item.CategoryName;
    categoryLinkButton.Visible = !modifying;
    categoryComboBox.Visible = modifying;
    if (modifying) foreach (Database.TemplateCategory category in Database.TemplateCategory.List ()) Interface.ComboBox_AddItem (categoryComboBox, category, item != null && category.Id == item.CategoryId);
    else categoryComboBox.Items.Clear ();
    descriptionTextBox.ReadOnly = !modifying;
    descriptionTextBox.Text = (item == null ? "" : item.Description);
    subitemListView.Enabled = modifying;
    if (item != null && !modifying) {
      Interface.ListView_Clear (subitemListView);
      foreach (Database.Concept concept in Database.EmployeeCategoryConcept.GetForTemplate (item.Id)) Interface.ListView_AddListViewItem (subitemListView, new string [] { "Empleado", concept.Name, Data.Quantity_Format (concept.Quantity), Data.Double_Format (concept.Cost), Data.Double_Format (concept.Price), Data.Double_Format (concept.Quantity * concept.Cost), Data.Double_Format (concept.Quantity * concept.Price) } , concept);
      foreach (Database.Concept concept in Database.MaterialConcept.GetForTemplate (item.Id)) Interface.ListView_AddListViewItem (subitemListView, new string [] { "Material", concept.Name, Data.Quantity_Format (concept.Quantity), Data.Double_Format (concept.Cost), Data.Double_Format (concept.Price), Data.Double_Format (concept.Quantity * concept.Cost), Data.Double_Format (concept.Quantity * concept.Price) } , concept);
      foreach (Database.Concept concept in Database.ArbitraryConcept.GetForTemplate (item.Id)) Interface.ListView_AddListViewItem (subitemListView, new string [] { "Otro", concept.Name, Data.Quantity_Format (concept.Quantity), Data.Double_Format (concept.Cost), Data.Double_Format (concept.Price), Data.Double_Format (concept.Quantity * concept.Cost), Data.Double_Format (concept.Quantity * concept.Price) } , concept);
    }
    subitemAddButton.Enabled = modifying;
    subitemModifyButton.Enabled = false;
    subitemRemoveButton.Enabled = false;
    recalculate ();
    modifyButton.Text = modifying ? "Aceptar" : "Modificar";
    deleteButton.Enabled = item != null;
    cancelButton.Text = modifying ? "Cancelar" : "Cerrar";
    (!modifying ? headerPanel as Control : nameTextBox).Focus ();
  }
  void statisticsButton_Click (object o, EventArgs e) {
    Form form = new Form ();
    Interface.Form_Prepare (form, 298, 284);
    Interface.HeaderPanel_Create (form, 8, 8, "Template", "Plantilla : " + (item == null ? "" : item.Name), "Estadísticas");
    ListView listView = Interface.ListView_Create (form, 8, 56, 282, 180, null, null);
    Interface.ListView_AddColumnHeader (listView, 128, "Categoría");
    ColumnHeader columnHeader = Interface.ListView_AddColumnHeader (listView, 51, "Coste");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    columnHeader = Interface.ListView_AddColumnHeader (listView, 51, "Precio");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    form.CancelButton = Interface.Button_Create (form, form.ClientSize.Width - 88, form.ClientSize.Height - 32, "Cerrar", null);
    int count1 = 0, count2 = 0;
    double cost1 = 0, price1 = 0, cost2 = 0, price2 = 0;
    foreach (ListViewItem listViewItem in subitemListView.Items) {
      Database.Concept concept = listViewItem.Tag as Database.Concept;
      if (concept is Database.EmployeeCategoryConcept) {
        count1++;
        cost1 += concept.Quantity * concept.Cost;
        price1 += concept.Quantity * concept.Price;
      } else if (concept is Database.ArbitraryConcept) {
        count2++;
        cost2 += concept.Quantity * concept.Cost;
        price2 += concept.Quantity * concept.Price;
      }
    }
    if (count1 > 0) Interface.ListView_AddListViewItem (listView, new string [] { "Empleados", Data.Double_Format (cost1), Data.Double_Format (price1) }, null);
    if (count2 > 0) Interface.ListView_AddListViewItem (listView, new string [] { "Otros", Data.Double_Format (cost2), Data.Double_Format (price2) }, null);
    foreach (Database.Concept concept in Database.MaterialConcept.StatisticsForTemplate (item.Id)) Interface.ListView_AddListViewItem (listView, new string [] { concept.Name, Data.Double_Format (concept.Cost), Data.Double_Format (concept.Price) }, null);
    form.ShowDialog ();
  }
  void subitemAddButton_Click (object o, EventArgs e) {
    ContextMenu contextMenu = new ContextMenu ();
    contextMenu.MenuItems.Add ("Empleado", delegate (object o2, EventArgs e2) { subitemAddForm_Show (new EmployeeCategoryConceptForm (null), "Empleado"); });
    contextMenu.MenuItems.Add ("Material", delegate (object o2, EventArgs e2) { subitemAddForm_Show (new MaterialConceptForm (null), "Material"); });
    contextMenu.MenuItems.Add ("Otro", delegate (object o2, EventArgs e2) { subitemAddForm_Show (new ArbitraryConceptForm (null), "Otro"); });
    contextMenu.Show (subitemAddButton, new Point (0, 24));
  }
  void subitemAddForm_Show (Form form, string type) {
    if (form.ShowDialog () == DialogResult.Cancel) return;
    Database.Concept concept = form.Tag as Database.Concept;
    Interface.ListView_AddListViewItem (subitemListView, new string [] { type, concept.Name, Data.Quantity_Format (concept.Quantity), Data.Double_Format (concept.Cost), Data.Double_Format (concept.Price), Data.Double_Format (concept.Quantity * concept.Cost), Data.Double_Format (concept.Quantity * concept.Price) }, concept);
    recalculate ();
  }
  void subitemListView_DoubleClick (object o, EventArgs e) {
    subitemModifyButton.PerformClick ();
  }
  void subitemListView_SelectedIndexChanged (object o, EventArgs e) {
    bool selected = subitemListView.SelectedItems.Count > 0;
    subitemModifyButton.Enabled = selected;
    subitemRemoveButton.Enabled = selected;
  }
  void subitemModifyButton_Click (object o, EventArgs e) {
    ListViewItem selectedSubitem = subitemListView.SelectedItems [0];
    Database.Concept concept = selectedSubitem.Tag as Database.Concept;
    Form form;
    if (concept is Database.EmployeeCategoryConcept) form = new EmployeeCategoryConceptForm (concept as Database.EmployeeCategoryConcept);
    else if (concept is Database.MaterialConcept) form = new MaterialConceptForm (concept as Database.MaterialConcept);
    else if (concept is Database.ArbitraryConcept) form = new ArbitraryConceptForm (concept as Database.ArbitraryConcept);
    else return;
    if (form.ShowDialog () == DialogResult.Cancel) return;
    concept = form.Tag as Database.Concept;
    Interface.ListViewItem_Modify (selectedSubitem, new string [] { selectedSubitem.Text, concept.Name, Data.Quantity_Format (concept.Quantity), Data.Double_Format (concept.Cost), Data.Double_Format (concept.Price), Data.Double_Format (concept.Quantity * concept.Cost), Data.Double_Format (concept.Quantity * concept.Price) }, concept);
    subitemListView.Sort ();
    recalculate ();
  }
  void subitemRemoveButton_Click (object o, EventArgs e) {
    subitemListView.SelectedItems [0].Remove ();
    recalculate ();
    headerPanel.Focus ();
  }
}


