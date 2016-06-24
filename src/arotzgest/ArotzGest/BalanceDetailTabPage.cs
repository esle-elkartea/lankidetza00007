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

class BalanceDetailTabPage : TabPage {
  Button acceptButton, cancelButton, searchButton;
  ComboBox yearComboBox;
  Label outcomeLabel;
  ListView incomeListView, outcomeListView;
  Panel headerPanel;
  public BalanceDetailTabPage () {
    Name = "balanceDetail";
    Text = "Balance";
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "Balance", "Balance", "Detalle");
    Interface.Label_Create (this, 0, 48, "Año");
    yearComboBox = Interface.ComboBox_Create (this, 128, 48, 24);
    Interface.ComboBox_AddItem (yearComboBox, "", true);
    for (int i = 2001; i < 2050; i ++) Interface.ComboBox_AddItem (yearComboBox, i.ToString (), i == DateTime.Now.Year);
    searchButton = Interface.Button_Create (this, 0, 76, "Buscar", searchButton_Click);
    Interface.Label_Create (this, 0, 104, "Ingresos");
    incomeListView = Interface.ListView_Create (this, 0, 128, 0, 0, null, listView_DoubleClick);
    Interface.ListView_AddColumnHeader (incomeListView, 255, "Cuenta");
    ColumnHeader columnHeader = Interface.ListView_AddColumnHeader (incomeListView, 54, "Importe");
    outcomeLabel = Interface.Label_Create (this, 0, 104, "Gastos");
    outcomeListView = Interface.ListView_Create (this, 0, 128, 0, 0, null, listView_DoubleClick);
    Interface.ListView_AddColumnHeader (outcomeListView, 255, "Cuenta");
    columnHeader = Interface.ListView_AddColumnHeader (outcomeListView, 54, "Importe");
    cancelButton = Interface.Button_Create (this, 0, 0, "Cerrar", cancelButton_Click);
    searchButton.PerformClick ();
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    incomeListView.Focus ();
  }
  protected override void OnResize (EventArgs e) {
    base.OnResize (e);
    incomeListView.Size = new Size ((Width - 16) / 2, Height - incomeListView.Top - 32);
    outcomeLabel.Left = (Width - 16) / 2 + 16;
    outcomeListView.Bounds = new Rectangle ((Width - 16) / 2 + 16, outcomeListView.Top, (Width - 16) / 2, Height - incomeListView.Top - 32);
    cancelButton.Location = new Point (Width - 80, Height - 24);
  }
  void acceptButton_Click (object o, EventArgs e) {
    if (incomeListView.ContainsFocus) {
      if (incomeListView.SelectedItems.Count > 0) listView_DoubleClick (incomeListView, EventArgs.Empty);
    } else if (outcomeListView.ContainsFocus) {
      if (outcomeListView.SelectedItems.Count > 0) listView_DoubleClick (outcomeListView, EventArgs.Empty);
    }
  }
  void cancelButton_Click (object o, EventArgs e) {
    Parent.Controls.Remove (this);
  }
  void listView_DoubleClick (object o, EventArgs e) {
    int id = (int) (o as ListView).SelectedItems [0].Tag;
    (TopLevelControl as MainForm).AccountEntrySearchTabPage_Open ();
    AccountEntrySearchTabPage tabPage = ((TopLevelControl as MainForm).Controls [1] as TabControl).TabPages ["accountEntrySearch"] as AccountEntrySearchTabPage;
    foreach (object item in (tabPage.Controls [2] as ComboBox).Items) {
      Database.Account account = item as Database.Account;
      if (account != null && account.Id == id) (tabPage.Controls [2] as ComboBox).SelectedItem = item;
    }
    (tabPage.Controls [4] as ComboBox).SelectedIndex = 0;
    (tabPage.Controls [5] as ComboBox).SelectedIndex = 0;
    (tabPage.Controls [6] as Button).PerformClick ();
  }
  void searchButton_Click (object o, EventArgs e) {
    int year = yearComboBox.SelectedIndex == 0 ? 0 : yearComboBox.SelectedIndex + 2000;
    Interface.ListView_Clear (incomeListView);
    foreach (Database.Concept concept in Database.Account.Balance ("1", year)) Interface.ListView_AddListViewItem (incomeListView, new string [] { concept.Name, Data.Double_Format (concept.Price) }, concept.Id);
    Interface.ListView_Clear (outcomeListView);
    foreach (Database.Concept concept in Database.Account.Balance ("2", year)) Interface.ListView_AddListViewItem (outcomeListView, new string [] { concept.Name, Data.Double_Format (concept.Price) }, concept.Id);
  }
}

