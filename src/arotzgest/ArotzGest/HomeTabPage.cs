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

class HomeTabPage : TabPage {
  Button acceptButton, itemDetailButton, accountDetailButton;
  Label balanceLabel;
  ListView accountListView, itemListView;
  Panel headerPanel;
  public HomeTabPage () {
    Text = "Principal";
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "Home", Text, ArotzGest.Name + " " + ArotzGest.Version);
    Interface.Label_Create (this, 0, 48, "Pendientes");
    itemListView = Interface.ListView_Create (this, 0, 76, 0, 128, itemListView_SelectedIndexChanged, itemListView_DoubleClick);
    Interface.ListView_AddColumnHeader (itemListView, 64, "Tipo");
    ColumnHeader columnHeader = Interface.ListView_AddColumnHeader (itemListView, 41, "Número");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    Interface.ListView_AddColumnHeader (itemListView, 54, "Fecha");
    Interface.ListView_AddColumnHeader (itemListView, 192, "Cliente");
    columnHeader = Interface.ListView_AddColumnHeader (itemListView, 45, "Importe");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    itemDetailButton = Interface.Button_Create (this, 0, 76, "Detalle", itemDetailButton_Click);
    itemDetailButton.Enabled = false;
    accountListView = Interface.ListView_Create (this, 0, 0, 0, 52, accountListView_SelectedIndexChanged, accountListView_DoubleClick);
    balanceLabel = Interface.Label_Create (this, 0, 0, "Balance mensual");
    Interface.ListView_AddColumnHeader (accountListView, 128, "Mes");
    columnHeader = Interface.ListView_AddColumnHeader (accountListView, 51, "Ingresos");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    columnHeader = Interface.ListView_AddColumnHeader (accountListView, 51, "Gastos");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    columnHeader = Interface.ListView_AddColumnHeader (accountListView, 51, "Balance");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    accountDetailButton = Interface.Button_Create (this, 0, 0, "Buscar", accountDetailButton_Click);
    accountDetailButton.Enabled = false;
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, null);
    Interface.ListView_Clear (itemListView);
    foreach (Database.Budget budget in Database.Budget.ListPendant ()) {
      budget.Calculate ();
      Interface.ListView_AddListViewItem (itemListView, new string [] { "Presupuesto", budget.Number.ToString (), Data.Date_Format (budget.Date), budget.ClientName, Data.Double_Format (budget.Price) }, budget);
    }
    foreach (Database.Invoice invoice in Database.Invoice.ListPendant ()) {
      invoice.Calculate ();
      Interface.ListView_AddListViewItem (itemListView, new string [] { "Factura", invoice.Number.ToString (), Data.Date_Format (invoice.Date), invoice.ClientName, Data.Double_Format (invoice.Price) }, invoice);
    }
    Interface.ListView_Clear (accountListView);
    for (int i = -1; i <= 0; i++) {
      DateTime dateTime = new DateTime (DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths (i);
      double income = 0;
      double outcome = 0;
      foreach (Database.AccountEntry accountEntry in Database.AccountEntry.Search (0, dateTime.Month, dateTime.Year)) {
        if (accountEntry.Price > 0) income += accountEntry.Price;
        if (accountEntry.Price < 0) outcome += accountEntry.Price;
      }
      Interface.ListView_AddListViewItem (accountListView, new string [] { Database.Months [dateTime.Month - 1] + " " + dateTime.Year, Data.Double_Format (income), Data.Double_Format (outcome), Data.Double_Format (income + outcome) }, dateTime);
    }
    itemListView.Focus ();
  }
  protected override void OnResize (EventArgs e) {
    base.OnResize (e);
    itemListView.Size = new Size (Width - 88, Height - itemListView.Top - 116);
    itemDetailButton.Left = Width - 80;
    balanceLabel.Top = Height - accountListView.Height - 56;
    accountListView.Bounds = new Rectangle (accountListView.Left, Height - accountListView.Height - 32, Width - 88, accountListView.Height);
    accountDetailButton.Location = new Point (Width - 80, Height - accountListView.Height - 32);
  }
  void acceptButton_Click (object o, EventArgs e) {
    if (itemListView.ContainsFocus) itemDetailButton.PerformClick ();
    else if (accountListView.ContainsFocus) accountDetailButton.PerformClick ();
  }
  void accountDetailButton_Click (object o, EventArgs e) {
    DateTime dateTime = (DateTime) (accountListView.SelectedItems [0]).Tag;
    (TopLevelControl as MainForm).AccountEntrySearchTabPage_Open ();
    AccountEntrySearchTabPage tabPage = ((TopLevelControl as MainForm).Controls [1] as TabControl).TabPages ["accountEntrySearch"] as AccountEntrySearchTabPage;
    (tabPage.Controls [2] as ComboBox).SelectedIndex = 0;
    (tabPage.Controls [4] as ComboBox).SelectedIndex = dateTime.Month;
    (tabPage.Controls [5] as ComboBox).SelectedIndex = dateTime.Year - 2000;
    (tabPage.Controls [6] as Button).PerformClick ();
  }
  void accountListView_DoubleClick (object o, EventArgs e) {
    accountDetailButton.PerformClick ();
  }
  void accountListView_SelectedIndexChanged (object o, EventArgs e) {
    accountDetailButton.Enabled = accountListView.SelectedItems.Count > 0;
  }
  void itemDetailButton_Click (object o, EventArgs e) {
    object concept = itemListView.SelectedItems [0].Tag;
    if (concept is Database.Budget) (TopLevelControl as MainForm).BudgetDetailTabPage_Open ((concept as Database.Budget).Id);
    else if (concept is Database.Invoice) (TopLevelControl as MainForm).InvoiceDetailTabPage_Open ((concept as Database.Invoice).Id);
  }
  void itemListView_DoubleClick (object o, EventArgs e) {
    itemDetailButton.PerformClick ();
  }
  void itemListView_SelectedIndexChanged (object o, EventArgs e) {
    itemDetailButton.Enabled = itemListView.SelectedItems.Count > 0;
  }
}

