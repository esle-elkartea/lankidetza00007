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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

class BudgetDetailTabPage : TabPage {
  bool modifying;
  Button acceptButton, cancelButton, clientLinkButton, deleteButton, modifyButton, printButton, reportGenerateButton, reportModifyButton, subitemAddButton, subitemModifyButton, subitemRemoveButton, warehouseLinkButton;
  ComboBox clientComboBox, stateComboBox, warehouseComboBox;
  Database.Budget item;
  Interface.ErrorManager errorManager;
  Label estimatedHoursLabel, costLabel, hoursLabel, priceLabel;
  ListView subitemListView, reportListView;
  Panel headerPanel;
  TextBox estimatedHoursTextBox, clientTextBox, costTextBox, dateTextBox, hoursTextBox, numberTextBox, priceTextBox, stateTextBox, warehouseTextBox;
  TabControl sectionTabControl;
  public BudgetDetailTabPage (int id) {
    errorManager = new Interface.ErrorManager ();
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "Budget", "", "");
    printButton = Interface.Button_Create (this, 0, 48, "Imprimir", printButton_Click);
    Interface.Label_Create (this, 0, 48, "Número");
    numberTextBox = Interface.TextBox_Create (this, 128, 48, 24, 1, 4);
    numberTextBox.TextAlign = HorizontalAlignment.Right;
    Interface.Label_Create (this, 0, 76, "Cliente");
    clientTextBox = Interface.TextBox_Create (this, 128, 76, 192, 1, 255);
    clientTextBox.ReadOnly = true;
    clientLinkButton = Interface.Button_CreateSmall (this, 336, 76, ">", clientLinkButton_Click);
    clientComboBox = Interface.ComboBox_Create (this, 128, 76, 192);
    Interface.Label_Create (this, 0, 104, "Fecha");
    dateTextBox = Interface.TextBox_Create (this, 128, 104, 54, 1, 10);
    Interface.Label_Create (this, 0, 132, "Estado");
    stateTextBox = Interface.TextBox_Create (this, 128, 132, 64, 1, 255);
    stateTextBox.ReadOnly = true;
    stateComboBox = Interface.ComboBox_Create (this, 128, 132, 64);
    Interface.Label_Create (this, 0, 160, "Almacén");
    warehouseTextBox = Interface.TextBox_Create (this, 128, 160, 128, 1, 255);
    warehouseTextBox.ReadOnly = true;
    warehouseComboBox = Interface.ComboBox_Create (this, 128, 160, 128);
    warehouseLinkButton = Interface.Button_CreateSmall (this, 272, 160, ">", warehouseLinkButton_Click);
    sectionTabControl = Interface.TabControl_Create (this, 0, 164, 126, new string [] { "Conceptos", "Partes" }, sectionTabControl_SelectedIndexChanged);
    subitemListView = Interface.ListView_Create (this, 0, 196, 0, 0, subitemListView_SelectedIndexChanged, subitemListView_DoubleClick);
    Interface.ListView_AddColumnHeader (subitemListView, 64, "Tipo");
    Interface.ListView_AddColumnHeader (subitemListView, 192, "Nombre");
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
    subitemAddButton = Interface.Button_Create (this, 0, 196, "Añadir", subitemAddButton_Click);
    subitemModifyButton = Interface.Button_Create (this, 0, 228, "Modificar", subitemModifyButton_Click);
    subitemRemoveButton = Interface.Button_Create (this, 0, 260, "Eliminar", subitemRemoveButton_Click);
    reportListView = Interface.ListView_Create (this, 0, 196, 0, 0, reportListView_SelectedIndexChanged, reportListView_DoubleClick);
    Interface.ListView_AddColumnHeader (reportListView, 64, "Tipo"); 
    Interface.ListView_AddColumnHeader (reportListView, 192, "Nombre");
    Interface.ListView_AddColumnHeader (reportListView, 192, "Empleado");
    columnHeader = Interface.ListView_AddColumnHeader (reportListView, 51, "Horas");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    columnHeader = Interface.ListView_AddColumnHeader (reportListView, 51, "Imputadas");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    subitemListView.ListViewItemSorter = new Interface.ListViewComprarer ();
    reportListView.Visible = false;
    reportGenerateButton = Interface.Button_Create (this, 0, 196, "Generar", reportGenerateButton_Click);
    reportGenerateButton.Visible = false;
    reportModifyButton = Interface.Button_Create (this, 0, 228, "Modificar", reportModifyButton_Click);
    reportModifyButton.Visible = false;
    costLabel = Interface.Label_Create (this, 0, 0, "Coste");
    costTextBox = Interface.TextBox_Create (this, 128, 0, 51, 1, 0);
    costTextBox.TextAlign = HorizontalAlignment.Right;
    costTextBox.ReadOnly = true;
    priceLabel = Interface.Label_Create (this, 0, 0, "Precio");
    priceTextBox = Interface.TextBox_Create (this, 128, 0, 51, 1, 0);
    priceTextBox.TextAlign = HorizontalAlignment.Right;
    priceTextBox.ReadOnly = true;
    estimatedHoursLabel = Interface.Label_Create (this, 0, 0, "Horas");
    estimatedHoursTextBox = Interface.TextBox_Create (this, 128, 0, 51, 1, 0);
    estimatedHoursTextBox.TextAlign = HorizontalAlignment.Right;
    estimatedHoursTextBox.ReadOnly = true;
    hoursLabel = Interface.Label_Create (this, 0, 0, "Imputadas");
    hoursTextBox = Interface.TextBox_Create (this, 128, 0, 51, 1, 0);
    hoursTextBox.TextAlign = HorizontalAlignment.Right;
    hoursTextBox.ReadOnly = true;
    modifyButton = Interface.Button_Create (this, 0, 0, "", modifyButton_Click);
    deleteButton = Interface.Button_Create (this, 88, 0, "Borrar", deleteButton_Click);
    cancelButton = Interface.Button_Create (this, 0, 0, "", cancelButton_Click);
    if (id > 0) item = Database.Budget.Get (id);
    setModifying (item == null);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    (modifying ? numberTextBox as Control : headerPanel).Focus ();
  }
  protected override void OnResize (EventArgs e) {
    base.OnResize (e);
    printButton.Left = Width - 80;
    sectionTabControl.Left = Width - sectionTabControl.Width - 86;
    subitemListView.Size = new Size (Width - 88, Height - subitemListView.Top - 96);
    subitemAddButton.Left = Width - 80;
    subitemModifyButton.Left = Width - 80;
    subitemRemoveButton.Left = Width - 80;
    reportListView.Size = new Size (Width - 88, Height - reportListView.Top - 96);
    reportGenerateButton.Left = Width - 80;
    reportModifyButton.Left = Width - 80;
    costLabel.Top = Height - 85;
    costTextBox.Top = Height - 88;
    priceLabel.Top = Height - 57;
    priceTextBox.Top = Height - 60;
    estimatedHoursLabel.Left = Width - estimatedHoursTextBox.Width - 224;
    estimatedHoursLabel.Top = Height - 85;
    estimatedHoursTextBox.Left = Width - estimatedHoursTextBox.Width - 88;
    estimatedHoursTextBox.Top = Height - 88;
    hoursLabel.Left = Width - hoursTextBox.Width - 224;
    hoursLabel.Top = Height - 57;
    hoursTextBox.Left = Width - hoursTextBox.Width - 88;
    hoursTextBox.Top = Height - 60;
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
  void clientLinkButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).ClientDetailTabPage_Open (item.ClientId);
  }
  void warehouseLinkButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).WarehouseDetailTabPage_Open ((int)item.WarehouseId);
  }
  void deleteButton_Click (object o, EventArgs e) {
    if (!Interface.ConfirmationDialog_Show ("Está seguro de que desea borrar el presupuesto actual?")) return;
    if (item.CheckUsed ()) {
      Interface.ErrorDialog_Show ("El presupuesto actual no se puede borrar por que tiene entidades relacionadas");
      return;
    }
    Database.Budget.Delete (item.Id);
    Database.Concept.DeleteForBudget (item.Id);
    Parent.Controls.Remove (this);
  }
  void modifyButton_Click (object o, EventArgs e) {
    if (modifying) {
      errorManager.Clear ();
      int? number = Data.Int_Parse (numberTextBox.Text);
      if (numberTextBox.Text == "") errorManager.Add (numberTextBox, "El número ha de especificarse");
      else if (number == null || number <= 0 || number >= 10000) errorManager.Add (numberTextBox, "El número especificado no es válido");
      Database.Client client = clientComboBox.SelectedItem as Database.Client;
      if (clientComboBox.SelectedItem == null) errorManager.Add (clientComboBox, "El cliente ha de especificarse");
      else if (Database.Client.Get (client.Id) == null) errorManager.Add (clientComboBox, "El cliente especificado no existe");
      DateTime? date = Data.Date_Parse (dateTextBox.Text);
      if (dateTextBox.Text == "") errorManager.Add (dateTextBox, "La fecha ha de especificarse");
      else if (date == null) errorManager.Add (dateTextBox, "La fecha especificada no es válida");
      else if (!Database.Budget.CheckUnique (item, (int) number, (DateTime) date)) errorManager.Add (numberTextBox, "Ya existe un presupuesto con el número especificado");
      Database.Warehouse warehouse = warehouseComboBox.SelectedItem as Database.Warehouse;
      if (errorManager.Controls.Count > 0) {
        errorManager.Controls [0].Focus ();
        return;
      }
      Database.Budget criteria = new Database.Budget ((int) number, client.Id, (DateTime) date, stateComboBox.SelectedIndex, warehouse == null ? (int?) null : warehouse.Id);
      item = item == null ? Database.Budget.Create (criteria) : item.Update (criteria);
      Database.Concept.DeleteForBudget (item.Id);
      foreach (ListViewItem subitem in subitemListView.Items) (subitem.Tag as Database.Concept).CreateForBudget (item.Id);
      foreach (ListViewItem subitem in reportListView.Items) (subitem.Tag as Database.Concept).CreateForBudget (item.Id);
    }
    setModifying (!modifying);
  }
  void printButton_Click (object o, EventArgs e) {
    ContextMenu contextMenu = new ContextMenu ();
    contextMenu.MenuItems.Add ("Presupuesto", delegate (object o2, EventArgs e2) { printBudget (); });
    contextMenu.MenuItems.Add ("Partes", delegate (object o2, EventArgs e2) { printReport (); });
    contextMenu.Show (printButton, new Point (0, 24));
  }
  void printBudget () {
    BudgetPrintDocument budgetPrintDocument = new BudgetPrintDocument ();
    Database.Company.Get ();
    budgetPrintDocument.CompanyName = Database.Company.Name;
    budgetPrintDocument.CompanyDetails = Database.Company.FIC + "\n" + Database.Company.Address + "\n" + Database.Company.ZIP + " " + Database.Company.City + "\n" + Database.Company.Province;
    budgetPrintDocument.LogoType = Database.Company.LogoType;
    try { budgetPrintDocument.LogoImage = new Bitmap (Database.Company.LogoImage); }
    catch { budgetPrintDocument.LogoImage = null; }
    budgetPrintDocument.Number = item.Number;
    budgetPrintDocument.Date = item.Date;
    budgetPrintDocument.ClientName = item.ClientName;
    Database.Client client = Database.Client.Get (item.ClientId);
    budgetPrintDocument.ClientDetails = client.FIC + "\n" + client.Address + "\n" + client.ZIP + " " + client.City + "\n" + client.Province;
    foreach (ListViewItem subitem in subitemListView.Items) {
      Database.Concept concept = subitem.Tag as Database.Concept;
      budgetPrintDocument.AddConcept (concept.Quantity, concept.Name + (concept is Database.TemplateConcept ? "\n" + (concept as Database.TemplateConcept).Description : ""), concept.Price);
    }
    budgetPrintDocument.ValidDays = Database.Company.BudgetValidDays;
    PrintDialog printDialog = new PrintDialog ();
    printDialog.Document = budgetPrintDocument;
    if (printDialog.ShowDialog () == DialogResult.OK) budgetPrintDocument.Print ();
  }
  void printReport () {
    ReportPrintDocument reportPrintDocument = new ReportPrintDocument ();
    reportPrintDocument.Number = item.Number;
    reportPrintDocument.Date = item.Date;
    foreach (ListViewItem subitem in subitemListView.Items) if (subitem.Tag is Database.TemplateConcept) {
      Database.TemplateConcept templateConcept = subitem.Tag as Database.TemplateConcept;
      reportPrintDocument.AddConcept (templateConcept.Quantity.ToString (), "Plantilla - " + templateConcept.Name, "", "");
      foreach (Database.MaterialConcept templateMaterialConcept in Database.MaterialConcept.GetForTemplate (templateConcept.Id)) reportPrintDocument.AddConcept (templateMaterialConcept.Quantity.ToString (), "    Material - " + templateMaterialConcept.Name, "", "");
      foreach (Database.EmployeeCategoryConcept templateEmployeeCategoryConcept in Database.EmployeeCategoryConcept.GetForTemplate (templateConcept.Id)) reportPrintDocument.AddConcept (templateConcept.Quantity.ToString (), "    Empleado - " + templateEmployeeCategoryConcept.Name, templateEmployeeCategoryConcept.Quantity.ToString (), (templateConcept.Quantity * templateEmployeeCategoryConcept.Quantity).ToString ());
    }
    foreach (ListViewItem subitem in subitemListView.Items) if (subitem.Tag is Database.EmployeeCategoryConcept) {
      Database.EmployeeCategoryConcept employeeCategoryConcept = subitem.Tag as Database.EmployeeCategoryConcept;
      reportPrintDocument.AddConcept ("1", "Empleado - " + employeeCategoryConcept.Name, employeeCategoryConcept.Quantity.ToString (), employeeCategoryConcept.Quantity.ToString ());
    }

    PrintDialog printDialog = new PrintDialog ();
    printDialog.Document = reportPrintDocument;
    if (printDialog.ShowDialog () == DialogResult.OK) reportPrintDocument.Print ();
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
    double estimatedHours = 0;
    double hours = 0;
    foreach (ListViewItem subitem in reportListView.Items) {
      Database.ReportConcept concept = subitem.Tag as Database.ReportConcept;
      estimatedHours += concept.EstimatedQuantity;
      hours += concept.Quantity == null ? (double) 0 : (double) concept.Quantity;
    }
    estimatedHoursTextBox.Text = estimatedHours.ToString ();
    hoursTextBox.Text = hours.ToString ();
  }
  void reportGenerateButton_Click (object o, EventArgs e) {
    Interface.ListView_Clear (reportListView);
    List<Database.ReportConcept> reportConcepts = new List<Database.ReportConcept> ();
    foreach (ListViewItem subitem in subitemListView.Items) if (subitem.Tag is Database.TemplateConcept) {
      Database.TemplateConcept templateConcept = subitem.Tag as Database.TemplateConcept;
      List<Database.EmployeeCategoryConcept> employeeCategoryConcepts = Database.EmployeeCategoryConcept.GetForTemplate (templateConcept.Id);
      foreach (Database.EmployeeCategoryConcept employeeCategoryConcept in employeeCategoryConcepts) reportConcepts.Add (new Database.ReportConcept (employeeCategoryConcept.Id, 0, employeeCategoryConcept.Name, employeeCategoryConcept.Quantity * templateConcept.Quantity));
    }
    foreach (ListViewItem subitem in subitemListView.Items) if (subitem.Tag is Database.EmployeeCategoryConcept) {
      Database.EmployeeCategoryConcept employeeCategoryConcept = subitem.Tag as Database.EmployeeCategoryConcept;
      reportConcepts.Add (new Database.ReportConcept (employeeCategoryConcept.Id, 1, employeeCategoryConcept.Name, employeeCategoryConcept.Quantity));
    }
    foreach (Database.ReportConcept reportConcept in reportConcepts) {
      Interface.ListView_AddListViewItem (reportListView, new string [] { Database.ReportTypes [reportConcept.Type], reportConcept.Name, "", reportConcept.EstimatedQuantity.ToString (), "" }, reportConcept);
    }
    recalculate ();
  }
  void reportListView_SelectedIndexChanged (object o, EventArgs e) {
    bool selected = reportListView.SelectedItems.Count > 0;
    reportModifyButton.Enabled = selected;
  }
  void reportListView_DoubleClick (object o, EventArgs e) {
    reportModifyButton.PerformClick ();
  }
  void reportModifyButton_Click (object o, EventArgs e) {
    ListViewItem selectedItem = reportListView.SelectedItems [0];
    Database.ReportConcept reportConcept = selectedItem.Tag as Database.ReportConcept;
    Form form = new ReportConceptForm (reportConcept);
    if (form.ShowDialog () == DialogResult.Cancel) return;
    reportConcept = form.Tag as Database.ReportConcept;
    Interface.ListViewItem_Modify (selectedItem, new string [] { Database.ReportTypes [reportConcept.Type], reportConcept.Name, reportConcept.EmployeeName, reportConcept.EstimatedQuantity.ToString (), reportConcept.Quantity.ToString () }, reportConcept);
    subitemListView.Sort ();
    recalculate ();
  }
  void sectionTabControl_SelectedIndexChanged (object o, EventArgs e) {
    subitemListView.Visible = sectionTabControl.SelectedIndex == 0;
    subitemAddButton.Visible = sectionTabControl.SelectedIndex == 0;
    subitemModifyButton.Visible = sectionTabControl.SelectedIndex == 0;
    subitemRemoveButton.Visible = sectionTabControl.SelectedIndex == 0;
    reportListView.Visible = sectionTabControl.SelectedIndex == 1;
    reportGenerateButton.Visible = sectionTabControl.SelectedIndex == 1;
    reportModifyButton.Visible = sectionTabControl.SelectedIndex == 1;
  }
  void setModifying (bool value) {
    modifying = value;
    if (!modifying) errorManager.Clear ();
    Name = "budgetDetail_" + (item == null ? 0 : item.Id);
    Text = "Presupuesto: " + (item == null ? "" : item.Number + "/" + item.Date.Year);
    Interface.HeaderPanel_SetText (headerPanel, Text, modifying ? item == null ? "Crear" : "Modificar" : "Detalle");
    printButton.Enabled = !modifying;
    numberTextBox.ReadOnly = !modifying;
    numberTextBox.Text = (item == null ? Database.Budget.CalculateNumber (DateTime.Now) : item.Number).ToString ();
    clientTextBox.Visible = !modifying;
    clientTextBox.Text = item == null ? "" : item.ClientName;
    clientLinkButton.Visible = !modifying;
    clientComboBox.Visible = modifying;
    if (modifying) foreach (Database.Client client in Database.Client.List ()) Interface.ComboBox_AddItem (clientComboBox, client, item != null && client.Id == item.ClientId);
    else clientComboBox.Items.Clear ();
    dateTextBox.ReadOnly = !modifying;
    dateTextBox.Text = item == null ? Data.Date_Format (DateTime.Now) : Data.Date_Format (item.Date);
    stateTextBox.Visible = !modifying;
    stateTextBox.Text = item == null ? "" : Database.BudgetStates [item.State];
    stateComboBox.Visible = modifying;
    if (modifying) for (int i = 0; i < Database.BudgetStates.Length; i++) Interface.ComboBox_AddItem (stateComboBox, Database.BudgetStates [i], i == 0 || item != null && i == item.State);
    else stateComboBox.Items.Clear ();
    warehouseTextBox.Visible = !modifying;
    warehouseTextBox.Text = item == null ? "" : item.WarehouseName;
    warehouseLinkButton.Visible = !modifying;
    warehouseComboBox.Visible = modifying;
    Interface.ComboBox_AddItem (warehouseComboBox, "", true);
    if (modifying) foreach (Database.Warehouse warehouse in Database.Warehouse.List ()) Interface.ComboBox_AddItem (warehouseComboBox, warehouse, item != null && item.WarehouseId != null && warehouse.Id == item.WarehouseId);
    else warehouseComboBox.Items.Clear ();
    subitemListView.Enabled = modifying;
    if (item != null && !modifying) {
      Interface.ListView_Clear (subitemListView);
      foreach (Database.Concept concept in Database.TemplateConcept.GetForBudget (item.Id)) Interface.ListView_AddListViewItem (subitemListView, new string [] { "Plantilla", concept.Name, Data.Quantity_Format (concept.Quantity), Data.Double_Format (concept.Cost), Data.Double_Format (concept.Price), Data.Double_Format (concept.Quantity * concept.Cost), Data.Double_Format (concept.Quantity * concept.Price) } , concept);
      foreach (Database.Concept concept in Database.EmployeeCategoryConcept.GetForBudget (item.Id)) Interface.ListView_AddListViewItem (subitemListView, new string [] { "Empleado", concept.Name, Data.Quantity_Format (concept.Quantity), Data.Double_Format (concept.Cost), Data.Double_Format (concept.Price), Data.Double_Format (concept.Quantity * concept.Cost), Data.Double_Format (concept.Quantity * concept.Price) } , concept);
      foreach (Database.Concept concept in Database.ArbitraryConcept.GetForBudget (item.Id)) Interface.ListView_AddListViewItem (subitemListView, new string [] { "Otro", concept.Name, Data.Quantity_Format (concept.Quantity), Data.Double_Format (concept.Cost), Data.Double_Format (concept.Price), Data.Double_Format (concept.Quantity * concept.Cost), Data.Double_Format (concept.Quantity * concept.Price) } , concept);
    }
    subitemAddButton.Enabled = modifying;
    subitemModifyButton.Enabled = false;
    subitemRemoveButton.Enabled = false;
    reportListView.Enabled = modifying;
    if (item != null && !modifying) {
      Interface.ListView_Clear (reportListView);
      foreach (Database.ReportConcept concept in Database.ReportConcept.GetForBudget (item.Id)) Interface.ListView_AddListViewItem (reportListView, new string [] { Database.ReportTypes [concept.Type], concept.Name, concept.EmployeeName, concept.EstimatedQuantity.ToString (), concept.Quantity.ToString () }, concept);
    }
    reportGenerateButton.Enabled = modifying && item != null;
    reportModifyButton.Enabled = false;
    recalculate ();
    modifyButton.Text = modifying ? "Aceptar" : "Modificar";
    deleteButton.Enabled = item != null;
    cancelButton.Text = modifying ? "Cancelar" : "Cerrar";
    (modifying ? numberTextBox as Control : headerPanel).Focus ();
  }
  void subitemAddButton_Click (object o, EventArgs e) {
    ContextMenu contextMenu = new ContextMenu ();
    contextMenu.MenuItems.Add ("Empleado", delegate (object o2, EventArgs e2) { subitemAddForm_Show (new EmployeeCategoryConceptForm (null), "Empleado"); });
    contextMenu.MenuItems.Add ("Plantilla", delegate (object o2, EventArgs e2) { subitemAddForm_Show (new TemplateConceptForm (null), "Plantilla"); });
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
    else if (concept is Database.TemplateConcept) form = new TemplateConceptForm (concept as Database.TemplateConcept);
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

