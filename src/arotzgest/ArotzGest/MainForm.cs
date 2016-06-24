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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

class MainForm : Form {
  TabControl mainTabControl;
  public MainForm () {
    StartPosition = FormStartPosition.CenterScreen;
    Text = ArotzGest.Name;
    ToolStrip toolStrip = new ToolStrip ();
    toolStrip.ShowItemToolTips = false;
    ToolStripDropDownButton dropDownButton = new ToolStripDropDownButton ("Gestión", Interface.Resources.GetImage16 ("Management"));
    dropDownButton.DropDownItems.Add ("Empresa", Interface.Resources.GetImage16 ("Company"), delegate (object o, EventArgs e) { CompanyDetailTabPage_Open (); });
    dropDownButton.DropDownItems.Add ("Clientes", Interface.Resources.GetImage16 ("Client"), delegate (object o, EventArgs e) { ClientSearchTabPage_Open (); });
    dropDownButton.DropDownItems.Add ("Categorías de empleado", Interface.Resources.GetImage16 ("EmployeeCategory"), delegate (object o, EventArgs e) { EmployeeCategorySearchTabPage_Open (); });
    dropDownButton.DropDownItems.Add ("Empleados", Interface.Resources.GetImage16 ("Employee"), delegate (object o, EventArgs e) { EmployeeSearchTabPage_Open (); });
    dropDownButton.DropDownItems.Add ("Proveedores", Interface.Resources.GetImage16 ("Provider"), delegate (object o, EventArgs e) { ProviderSearchTabPage_Open (); });
    dropDownButton.DropDownItems.Add ("Categorías de material", Interface.Resources.GetImage16 ("MaterialCategory"), delegate (object o, EventArgs e) { MaterialCategorySearchTabPage_Open (); });
    dropDownButton.DropDownItems.Add ("Materiales", Interface.Resources.GetImage16 ("Material"), delegate (object o, EventArgs e) { MaterialSearchTabPage_Open (); });
    dropDownButton.DropDownItems.Add ("Categorías de plantilla", Interface.Resources.GetImage16 ("TemplateCategory"), delegate (object o, EventArgs e) { TemplateCategorySearchTabPage_Open (); });
    dropDownButton.DropDownItems.Add ("Cuentas", Interface.Resources.GetImage16 ("Account"), delegate (object o, EventArgs e) { AccountSearchTabPage_Open (); });
    toolStrip.Items.Add (dropDownButton);
    dropDownButton = new ToolStripDropDownButton ("Operaciones", Interface.Resources.GetImage16 ("Operations"));
    dropDownButton.DropDownItems.Add ("Plantillas", Interface.Resources.GetImage16 ("Template"), delegate (object o, EventArgs e) { TemplateSearchTabPage_Open (); });
    dropDownButton.DropDownItems.Add ("Presupuestos", Interface.Resources.GetImage16 ("Budget"), delegate (object o, EventArgs e) { BudgetSearchTabPage_Open (); });
    dropDownButton.DropDownItems.Add ("Facturas", Interface.Resources.GetImage16 ("Invoice"), delegate (object o, EventArgs e) { InvoiceSearchTabPage_Open (); });
    dropDownButton.DropDownItems.Add ("Movimientos", Interface.Resources.GetImage16 ("AccountEntry"), delegate (object o, EventArgs e) { AccountEntrySearchTabPage_Open (); });
    toolStrip.Items.Add (dropDownButton);
    dropDownButton = new ToolStripDropDownButton ("Informes", Interface.Resources.GetImage16 ("Report"));
    dropDownButton.DropDownItems.Add ("Balance", Interface.Resources.GetImage16 ("Balance"), delegate (object o, EventArgs e) { BalanceDetailTabPage_Open (); });
    toolStrip.Items.Add (dropDownButton);
    Controls.Add (toolStrip);
    mainTabControl = Interface.TabControl_Create (this, 4, 28);
    TabPage homeTabPage = Interface.TabControl_AddTabPage (mainTabControl, new HomeTabPage ());
    ActiveControl = homeTabPage;
    Size = new Size (800, 600);
    MinimumSize = Size;
  }
  protected override void OnResize (EventArgs e) {
    base.OnResize (e);
    mainTabControl.Size = new Size (ClientSize.Width - 8, ClientSize.Height - mainTabControl.Top - 4);
    GraphicsPath graphicsPath = new GraphicsPath ();
    graphicsPath.AddRectangle (new Rectangle (0, 0, mainTabControl.Width, 21));
    graphicsPath.AddRectangle (new Rectangle (4, 22, mainTabControl.Width - 8, mainTabControl.Height - 26));
    mainTabControl.Region = new Region (graphicsPath);
  }
  public void AccountDetailTabPage_Open (int id) {
    TabPage tabPage = mainTabControl.TabPages ["accountDetail_" + id];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else {
      tabPage = new AccountDetailTabPage (id);
      if (id > 0 && tabPage.Name == "accountDetail_0") {
        Interface.ErrorDialog_Show ("La cuenta especificada no existe");
        return;
      }
      Interface.TabControl_AddTabPage (mainTabControl, tabPage);
    }
  }
  public void AccountSearchTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["accountSearch"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new AccountSearchTabPage ());
  }
  public void AccountEntrySearchTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["accountEntrySearch"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new AccountEntrySearchTabPage ());
  }
  public void AccountEntryDetailTabPage_Open (int id) {
    TabPage tabPage = mainTabControl.TabPages ["accountEntryDetail_" + id];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else {
      tabPage = new AccountEntryDetailTabPage (id);
      if (id > 0 && tabPage.Name == "accountEntryDetail_0") {
        Interface.ErrorDialog_Show ("El movimiento especificado no existe");
        return;
      }
      Interface.TabControl_AddTabPage (mainTabControl, tabPage);
    }
  }
  public void BalanceDetailTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["balanceDetail"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new BalanceDetailTabPage ());
  }
  public void BudgetDetailTabPage_Open (int id) {
    TabPage tabPage = mainTabControl.TabPages ["budgetDetail_" + id];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else {
      tabPage = new BudgetDetailTabPage (id);
      if (id > 0 && tabPage.Name == "budgetDetail_0") {
        Interface.ErrorDialog_Show ("El presupuesto especificado no existe");
        return;
      }
      Interface.TabControl_AddTabPage (mainTabControl, tabPage);
    }
  }
  public void BudgetSearchTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["budgetSearch"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new BudgetSearchTabPage ());
  }
  public void ClientDetailTabPage_Open (int id) {
    TabPage tabPage = mainTabControl.TabPages ["clientDetail_" + id];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else {
      tabPage = new ClientDetailTabPage (id);
      if (id > 0 && tabPage.Name == "clientDetail_0") {
        Interface.ErrorDialog_Show ("El cliente especificado no existe");
        return;
      }
      Interface.TabControl_AddTabPage (mainTabControl, tabPage);
    }
  }
  public void ClientSearchTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["clientSearch"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new ClientSearchTabPage ());
  }
  public void DefaultButtons_Set (Button acceptButton, Button cancelButton) {
    AcceptButton = acceptButton;
    CancelButton = cancelButton;
  }
  public void EmployeeCategoryDetailTabPage_Open (int id) {
    TabPage tabPage = mainTabControl.TabPages ["employeeCategoryDetail_" + id];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else {
      tabPage = new EmployeeCategoryDetailTabPage (id);
      if (id > 0 && tabPage.Name == "employeeCategoryDetail_0") {
        Interface.ErrorDialog_Show ("La categoría de empleado especificada no existe");
        return;
      }
      Interface.TabControl_AddTabPage (mainTabControl, tabPage);
    }
  }
  public void CompanyDetailTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["companyDetail"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new CompanyDetailTabPage ());
  }
  public void EmployeeCategorySearchTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["employeeCategorySearch"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new EmployeeCategorySearchTabPage ());
  }
  public void EmployeeDetailTabPage_Open (int id) {
    TabPage tabPage = mainTabControl.TabPages ["employeeDetail_" + id];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else {
      tabPage = new EmployeeDetailTabPage (id);
      if (id > 0 && tabPage.Name == "employeeDetail_0") {
        Interface.ErrorDialog_Show ("El empleado especificado no existe");
        return;
      }
      Interface.TabControl_AddTabPage (mainTabControl, tabPage);
    }
  }
  public void EmployeeSearchTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["employeeSearch"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new EmployeeSearchTabPage ());
  }
  public void InvoiceDetailTabPage_Open (int id) {
    TabPage tabPage = mainTabControl.TabPages ["invoiceDetail_" + id];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else {
      tabPage = new InvoiceDetailTabPage (id);
      if (id > 0 && tabPage.Name == "invoiceDetail_0") {
        Interface.ErrorDialog_Show ("La factura especificada no existe");
        return;
      }
      Interface.TabControl_AddTabPage (mainTabControl, tabPage);
    }
  }
  public void InvoiceSearchTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["invoiceSearch"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new InvoiceSearchTabPage ());
  }
  public void MaterialCategoryDetailTabPage_Open (int id) {
    TabPage tabPage = mainTabControl.TabPages ["materialCategoryDetail_" + id];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else {
      tabPage = new MaterialCategoryDetailTabPage (id);
      if (id > 0 && tabPage.Name == "materialCategoryDetail_0") {
        Interface.ErrorDialog_Show ("La categoría de material especificada no existe");
        return;
      }
      Interface.TabControl_AddTabPage (mainTabControl, tabPage);
    }
  }
  public void MaterialCategorySearchTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["materialCategorySearch"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new MaterialCategorySearchTabPage ());
  }
  public void MaterialDetailTabPage_Open (int id) {
    TabPage tabPage = mainTabControl.TabPages ["materialDetail_" + id];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else {
      tabPage = new MaterialDetailTabPage (id);
      if (id > 0 && tabPage.Name == "materialDetail_0") {
        Interface.ErrorDialog_Show ("El material especificado no existe");
        return;
      }
      Interface.TabControl_AddTabPage (mainTabControl, tabPage);
    }
  }
  public void MaterialSearchTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["materialSearch"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new MaterialSearchTabPage ());
  }
  public void ProviderDetailTabPage_Open (int id) {
    TabPage tabPage = mainTabControl.TabPages ["providerDetail_" + id];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else {
      tabPage = new ProviderDetailTabPage (id);
      if (id > 0 && tabPage.Name == "providerDetail_0") {
        Interface.ErrorDialog_Show ("El proveedor especificado no existe");
        return;
      }
      Interface.TabControl_AddTabPage (mainTabControl, tabPage);
    }
  }
  public void ProviderSearchTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["providerSearch"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new ProviderSearchTabPage ());
  }
  public void TemplateCategoryDetailTabPage_Open (int id) {
    TabPage tabPage = mainTabControl.TabPages ["templateCategoryDetail_" + id];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else {
      tabPage = new TemplateCategoryDetailTabPage (id);
      if (id > 0 && tabPage.Name == "templateCategoryDetail_0") {
        Interface.ErrorDialog_Show ("La categoría de plantilla especificada no existe");
        return;
      }
      Interface.TabControl_AddTabPage (mainTabControl, tabPage);
    }
  }
  public void TemplateCategorySearchTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["templateCategorySearch"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new TemplateCategorySearchTabPage ());
  }
  public void TemplateDetailTabPage_Open (int id) {
    TabPage tabPage = mainTabControl.TabPages ["templateDetail_" + id];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else {
      tabPage = new TemplateDetailTabPage (id);
      if (id > 0 && tabPage.Name == "templateDetail_0") {
        Interface.ErrorDialog_Show ("La plantilla especificada no existe");
        return;
      }
      Interface.TabControl_AddTabPage (mainTabControl, tabPage);
    }
  }
  public void TemplateSearchTabPage_Open () {
    TabPage tabPage = mainTabControl.TabPages ["templateSearch"];
    if (tabPage != null) mainTabControl.SelectedTab = tabPage;
    else Interface.TabControl_AddTabPage (mainTabControl, new TemplateSearchTabPage ());
  }
}

