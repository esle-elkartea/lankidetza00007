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

class InvoiceDetailTabPage : TabPage {
  bool modifying;
  Button acceptButton, cancelButton, clientLinkButton, deleteButton, modifyButton, printButton, receiptAddButton, receiptModifyButton, receiptRemoveButton, subitemAddButton, subitemModifyButton, subitemRemoveButton;
  ComboBox clientComboBox;
  Database.Invoice item;
  Interface.ErrorManager errorManager;
  Label costLabel, priceLabel;
  ListView subitemListView, receiptListView;
  Panel headerPanel;
  TabControl sectionTabControl;
  TextBox clientTextBox, costTextBox, dateTextBox, numberTextBox, priceTextBox, retentionTextBox, vatTextBox;
  public InvoiceDetailTabPage (int id) {
    errorManager = new Interface.ErrorManager ();
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "Invoice", "", "");
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
    Interface.Label_Create (this, 0, 132, "IVA");
    vatTextBox = Interface.TextBox_Create (this, 128, 132, 12, 1, 2);
    vatTextBox.TextAlign = HorizontalAlignment.Right;
    Interface.Label_Create (this, 0, 160, "Retención");
    retentionTextBox = Interface.TextBox_Create (this, 128, 160, 12, 1, 2);
    retentionTextBox.TextAlign = HorizontalAlignment.Right;
    sectionTabControl = Interface.TabControl_Create (this, 0, 164, 135, new string [] { "Conceptos", "Recibos" }, sectionTabControl_SelectedIndexChanged);
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
    receiptListView = Interface.ListView_Create (this, 0, 196, 0, 0, receiptListView_SelectedIndexChanged, receiptListView_DoubleClick);
    Interface.ListView_AddColumnHeader (receiptListView, 41, "Número");
    columnHeader = Interface.ListView_AddColumnHeader (receiptListView, 45, "Importe");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    Interface.ListView_AddColumnHeader (receiptListView, 54, "Fecha vencimiento");
    Interface.ListView_AddColumnHeader (receiptListView, 54, "Fecha cobro");
    Interface.ListView_AddColumnHeader (receiptListView, 192, "Cuenta");
    subitemListView.ListViewItemSorter = new Interface.ListViewComprarer ();
    receiptListView.Visible = false;
    receiptAddButton = Interface.Button_Create (this, 0, 196, "Añadir", receiptAddButton_Click);
    receiptAddButton.Visible = false;
    receiptModifyButton = Interface.Button_Create (this, 0, 228, "Modificar", receiptModifyButton_Click);
    receiptModifyButton.Visible = false;
    receiptRemoveButton = Interface.Button_Create (this, 0, 260, "Eliminar", receiptRemoveButton_Click);
    receiptRemoveButton.Visible = false;
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
    if (id > 0) item = Database.Invoice.Get (id);
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
    receiptListView.Size = new Size (Width - 88, Height - receiptListView.Top - 96);
    receiptAddButton.Left = Width - 80;
    receiptModifyButton.Left = Width - 80;
    receiptRemoveButton.Left = Width - 80;
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
  void clientLinkButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).ClientDetailTabPage_Open (item.ClientId);
  }
  void deleteButton_Click (object o, EventArgs e) {
    if (!Interface.ConfirmationDialog_Show ("Está seguro de que desea borrar la factura actual?")) return;
    if (item.CheckUsed ()) {
      Interface.ErrorDialog_Show ("La factura actual no se puede borrar por que tiene entidades relacionadas");
      return;
    }
    Database.Invoice.Delete (item.Id);
    Database.Concept.DeleteForInvoice (item.Id);
    Parent.Controls.Remove (this);
  }
  void invoicePrint () {
    InvoicePrintDocument invoicePrintDocument = new InvoicePrintDocument ();
    Database.Company.Get ();
    invoicePrintDocument.CompanyName = Database.Company.Name;
    invoicePrintDocument.CompanyDetails = Database.Company.FIC + "\n" + Database.Company.Address + "\n" + Database.Company.ZIP + " " + Database.Company.City + "\n" + Database.Company.Province;
    invoicePrintDocument.LogoType = Database.Company.LogoType;
    try { invoicePrintDocument.LogoImage = new Bitmap (Database.Company.LogoImage); } catch { invoicePrintDocument.LogoImage = null; }
    invoicePrintDocument.Number = item.Number;
    invoicePrintDocument.Date = item.Date;
    invoicePrintDocument.ClientName = item.ClientName;
    Database.Client client = Database.Client.Get (item.ClientId);
    invoicePrintDocument.ClientDetails = client.FIC + "\n" + client.Address + "\n" + client.ZIP + " " + client.City + "\n" + client.Province;
    foreach (ListViewItem subitem in subitemListView.Items) {
      Database.Concept concept = subitem.Tag as Database.Concept;
      invoicePrintDocument.AddConcept (concept.Quantity, (concept is Database.BudgetConcept ? "Presupuesto nº " : "") + concept.Name, concept.Price);
    }
    invoicePrintDocument.VAT = item.VAT;
    invoicePrintDocument.Retention = item.Retention;
    PrintDialog printDialog = new PrintDialog ();
    printDialog.Document = invoicePrintDocument;
    if (printDialog.ShowDialog () == DialogResult.OK) invoicePrintDocument.Print ();
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
      else if (!Database.Invoice.CheckUnique (item, (int) number, (DateTime) date)) errorManager.Add (numberTextBox, "Ya existe una factura con el número especificado");
      int? vat = Data.Int_Parse (vatTextBox.Text);
      if (vatTextBox.Text == "") errorManager.Add (vatTextBox, "El IVA ha de especificarse");
      else if (vat == null || vat < 0 || vat >= 100) errorManager.Add (vatTextBox, "El IVA especificado no es válido");
      int? retention = Data.Int_Parse (retentionTextBox.Text);
      if (retentionTextBox.Text == "") errorManager.Add (retentionTextBox, "La retención ha de especificarse");
      else if (retention == null || retention < 0 || retention >= 100) errorManager.Add (retentionTextBox, "La retención especificada no es válida");
      if (errorManager.Controls.Count > 0) {
        errorManager.Controls [0].Focus ();
        return;
      }
      bool state = receiptListView.Items.Count > 0;
      foreach (ListViewItem receipt in receiptListView.Items) if ((receipt.Tag as Database.ReceiptConcept).PayDate == null) state = false;
      Database.Invoice criteria = new Database.Invoice ((int) number, client.Id, (DateTime) date, (int) vat, (int) retention, state ? 1 : 0);
      item = item == null ? Database.Invoice.Create (criteria) : item.Update (criteria);
      Database.Concept.DeleteForInvoice (item.Id);
      foreach (ListViewItem subitem in subitemListView.Items) (subitem.Tag as Database.Concept).CreateForInvoice (item.Id);
      foreach (ListViewItem receipt in receiptListView.Items) {
        Database.ReceiptConcept concept = receipt.Tag as Database.ReceiptConcept;
        if (concept.AccountId != null) {
          Database.AccountEntry accountEntry = new Database.AccountEntry ("Recibo nº " + item.Number + "/" + item.Date.Year + "-" + concept.Number, (int) concept.AccountId, (DateTime) concept.PayDate, concept.Price);
          accountEntry = Database.AccountEntry.Create (accountEntry);
          concept.AccountEntryId = accountEntry.Id;
        }
        concept.CreateForInvoice (item.Id);
      }
    }
    setModifying (!modifying);
  }
  void printButton_Click (object o, EventArgs e) {
    ContextMenu contextMenu = new ContextMenu ();
    contextMenu.MenuItems.Add ("Factura", delegate (object o2, EventArgs e2) { invoicePrint (); });
    contextMenu.MenuItems.Add ("Recibos", delegate (object o2, EventArgs e2) { receiptPrint (); });
    contextMenu.Show (printButton, new Point (0, 24));
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
  void receiptAddButton_Click (object o, EventArgs e) {
    int [] usedNumbers = new int [receiptListView.Items.Count];
    double price = (double) Data.Double_Parse (priceTextBox.Text);
    for (int i = 0; i < receiptListView.Items.Count; i++) {
      Database.ReceiptConcept receipt = receiptListView.Items [i].Tag as Database.ReceiptConcept;
      usedNumbers [i] = receipt.Number;
      price -= receipt.Price;
    }
    Form form = new ReceiptConceptForm (null, receiptListView.Items.Count + 1, usedNumbers, price);
    if (form.ShowDialog () == DialogResult.Cancel) return;
    Database.ReceiptConcept concept = form.Tag as Database.ReceiptConcept;
    Interface.ListView_AddListViewItem (receiptListView, new string [] { concept.Name, Data.Double_Format (concept.Price), Data.Date_Format (concept.DueDate), concept.PayDate == null ? "" : Data.Date_Format ((DateTime) concept.PayDate), concept.AccountName }, concept);
  }
  void receiptListView_SelectedIndexChanged (object o, EventArgs e) {
    bool selected = receiptListView.SelectedItems.Count > 0;
    receiptModifyButton.Enabled = selected;
    receiptRemoveButton.Enabled = selected;
  }
  void receiptListView_DoubleClick (object o, EventArgs e) {
    receiptModifyButton.PerformClick ();
  }
  void receiptModifyButton_Click (object o, EventArgs e) {
    ListViewItem selectedReceipt = receiptListView.SelectedItems [0];
    Database.ReceiptConcept concept = selectedReceipt.Tag as Database.ReceiptConcept;
    int [] usedNumbers = new int [receiptListView.Items.Count];
    for (int i = 0; i < receiptListView.Items.Count; i++) usedNumbers [i] = (receiptListView.Items [i].Tag as Database.ReceiptConcept).Number;
    Form form = new ReceiptConceptForm (concept as Database.ReceiptConcept, 0, usedNumbers, 0);
    if (form.ShowDialog () == DialogResult.Cancel) return;
    concept = form.Tag as Database.ReceiptConcept;
    Interface.ListViewItem_Modify (selectedReceipt, new string [] { concept.Number.ToString (), Data.Double_Format (concept.Price), Data.Date_Format (concept.DueDate), concept.PayDate == null ? "" : Data.Date_Format ((DateTime) concept.PayDate), concept.AccountName }, concept);
    receiptListView.Sort ();
  }
  void receiptRemoveButton_Click (object o, EventArgs e) {
    receiptListView.SelectedItems [0].Remove ();
    headerPanel.Focus ();
  }
  void sectionTabControl_SelectedIndexChanged (object o, EventArgs e) {
    subitemListView.Visible = sectionTabControl.SelectedIndex == 0;
    subitemAddButton.Visible = sectionTabControl.SelectedIndex == 0;
    subitemModifyButton.Visible = sectionTabControl.SelectedIndex == 0;
    subitemRemoveButton.Visible = sectionTabControl.SelectedIndex == 0;
    receiptListView.Visible = sectionTabControl.SelectedIndex == 1;
    receiptAddButton.Visible = sectionTabControl.SelectedIndex == 1;
    receiptModifyButton.Visible = sectionTabControl.SelectedIndex == 1;
    receiptRemoveButton.Visible = sectionTabControl.SelectedIndex == 1;
  }
  void setModifying (bool value) {
    modifying = value;
    if (!modifying) errorManager.Clear ();
    Name = "invoiceDetail_" + (item == null ? 0 : item.Id);
    Text = "Factura: " + (item == null ? "" : item.Number + "/" + item.Date.Year);
    Interface.HeaderPanel_SetText (headerPanel, Text, modifying ? item == null ? "Crear" : "Modificar" : "Detalle");
    printButton.Enabled = !modifying;
    numberTextBox.ReadOnly = !modifying;
    numberTextBox.Text = (item == null ? Database.Invoice.CalculateNumber (DateTime.Now) : item.Number).ToString ();
    clientTextBox.Visible = !modifying;
    clientTextBox.Text = item == null ? "" : item.ClientName;
    clientLinkButton.Visible = !modifying;
    clientComboBox.Visible = modifying;
    if (modifying) foreach (Database.Client client in Database.Client.List ()) Interface.ComboBox_AddItem (clientComboBox, client, item != null && client.Id == item.ClientId);
    else clientComboBox.Items.Clear ();
    dateTextBox.ReadOnly = !modifying;
    dateTextBox.Text = item == null ? Data.Date_Format (DateTime.Now) : Data.Date_Format (item.Date);
    vatTextBox.ReadOnly = !modifying;
    vatTextBox.Text = item == null ? "16" : item.VAT.ToString ();
    retentionTextBox.ReadOnly = !modifying;
    retentionTextBox.Text = item == null ? "0" : item.Retention.ToString ();
    subitemListView.Enabled = modifying;
    if (item != null && !modifying) {
      Interface.ListView_Clear (subitemListView);
      foreach (Database.Concept concept in Database.BudgetConcept.GetForInvoice (item.Id)) Interface.ListView_AddListViewItem (subitemListView, new string [] { "Presupuesto", "Presupuesto nº " + concept.Name, Data.Quantity_Format (concept.Quantity), Data.Double_Format (concept.Cost), Data.Double_Format (concept.Price), Data.Double_Format (concept.Quantity * concept.Cost), Data.Double_Format (concept.Quantity * concept.Price) } , concept);
      foreach (Database.Concept concept in Database.ArbitraryConcept.GetForInvoice (item.Id)) Interface.ListView_AddListViewItem (subitemListView, new string [] { "Otro", concept.Name, Data.Quantity_Format (concept.Quantity), Data.Double_Format (concept.Cost), Data.Double_Format (concept.Price), Data.Double_Format (concept.Quantity * concept.Cost), Data.Double_Format (concept.Quantity * concept.Price) } , concept);
    }
    subitemAddButton.Enabled = modifying;
    subitemModifyButton.Enabled = false;
    subitemRemoveButton.Enabled = false;
    receiptListView.Enabled = modifying;
    if (item != null && !modifying) {
      Interface.ListView_Clear (receiptListView);
      foreach (Database.ReceiptConcept concept in Database.ReceiptConcept.GetForInvoice (item.Id)) Interface.ListView_AddListViewItem (receiptListView, new string [] { concept.Name, Data.Double_Format (concept.Price), Data.Date_Format (concept.DueDate), concept.PayDate == null ? "" : Data.Date_Format ((DateTime) concept.PayDate), concept.AccountName }, concept);
    }
    receiptAddButton.Enabled = modifying;
    receiptModifyButton.Enabled = false;
    receiptRemoveButton.Enabled = false;
    recalculate ();
    modifyButton.Text = modifying ? "Aceptar" : "Modificar";
    deleteButton.Enabled = item != null;
    cancelButton.Text = modifying ? "Cancelar" : "Cerrar";
    (modifying ? numberTextBox as Control : headerPanel).Focus ();
  }
  void subitemAddButton_Click (object o, EventArgs e) {
    ContextMenu contextMenu = new ContextMenu ();
    contextMenu.MenuItems.Add ("Presupuesto", delegate (object o2, EventArgs e2) { subitemAddForm_Show (new BudgetConceptForm (null), "Presupuesto"); });
    contextMenu.MenuItems.Add ("Otro", delegate (object o2, EventArgs e2) { subitemAddForm_Show (new ArbitraryConceptForm (null), "Otro"); });
    contextMenu.Show (subitemAddButton, new Point (0, 24));
  }
  void subitemAddForm_Show (Form form, string type) {
    if (form.ShowDialog () == DialogResult.Cancel) return;
    Database.Concept concept = form.Tag as Database.Concept;
    foreach (ListViewItem item2 in subitemListView.Items) if (concept.Id != 0 && (item2.Tag as Database.Concept).Id == concept.Id) return;
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
    if (concept is Database.BudgetConcept) form = new BudgetConceptForm (concept as Database.BudgetConcept);
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
  void receiptPrint () {
    ReceiptPrintDocument receiptPrintDocument = new ReceiptPrintDocument ();
    Database.Company.Get ();
    receiptPrintDocument.CompanyName = Database.Company.Name;
    receiptPrintDocument.CompanyDetails = Database.Company.FIC + "\n" + Database.Company.Address + "\n" + Database.Company.ZIP + " " + Database.Company.City + "\n" + Database.Company.Province;
    receiptPrintDocument.LogoType = Database.Company.LogoType;
    try { receiptPrintDocument.LogoImage = new Bitmap (Database.Company.LogoImage); } catch { receiptPrintDocument.LogoImage = null; }
    receiptPrintDocument.Number = item.Number;
    receiptPrintDocument.Date = item.Date;
    receiptPrintDocument.ClientName = item.ClientName;
    Database.Client client = Database.Client.Get (item.ClientId);
    receiptPrintDocument.ClientDetails = client.FIC + "\n" + client.Address + "\n" + client.ZIP + " " + client.City + "\n" + client.Province;
    foreach (ListViewItem subitem in receiptListView.Items) {
      Database.ReceiptConcept concept = subitem.Tag as Database.ReceiptConcept;
      receiptPrintDocument.AddConcept (concept.Number, concept.Price, concept.DueDate);
    }
    PrintDialog printDialog = new PrintDialog ();
    printDialog.Document = receiptPrintDocument;
    if (printDialog.ShowDialog () == DialogResult.OK) receiptPrintDocument.Print ();
  }
}

