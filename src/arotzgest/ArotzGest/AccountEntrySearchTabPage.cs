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

class AccountEntrySearchTabPage : TabPage {
  Button acceptButton, cancelButton, createButton, detailButton, searchButton;
  ComboBox accountComboBox, monthComboBox, yearComboBox;
  ListView itemListView;
  public AccountEntrySearchTabPage () {
    Name = "accountEntrySearch";
    Text = "Movimientos";
    acceptButton = Interface.Button_Create (acceptButton_Click);
    Interface.HeaderPanel_Create (this, 0, 8, "AccountEntry", Text, "Buscar");
    Interface.Label_Create (this, 0, 48, "Cuenta");
    accountComboBox = Interface.ComboBox_Create (this, 128, 48, 255);
    Interface.ComboBox_AddItem (accountComboBox, "", true);
    foreach (Database.Account account in Database.Account.List ()) Interface.ComboBox_AddItem (accountComboBox, account, false);
    Interface.Label_Create (this, 0, 76, "Fecha");
    monthComboBox = Interface.ComboBox_Create (this, 128, 76, 64);
    Interface.ComboBox_AddItem (monthComboBox, "", true);
    foreach (string month in Database.Months) Interface.ComboBox_AddItem (monthComboBox, month, false);
    yearComboBox = Interface.ComboBox_Create (this, 224, 76, 24);
    Interface.ComboBox_AddItem (yearComboBox, "", true);
    while (yearComboBox.Items.Count < 50) Interface.ComboBox_AddItem (yearComboBox, (2000 + yearComboBox.Items.Count).ToString (), false);
    searchButton = Interface.Button_Create (this, 0, 104, "Buscar", searchButton_Click);
    itemListView = Interface.ListView_Create (this, 0, 144, 0, 0, itemListView_SelectedIndexChanged, itemListView_DoubleClick);
    Interface.ListView_AddColumnHeader (itemListView, 255, "Cuenta");
    Interface.ListView_AddColumnHeader (itemListView, 54, "Fecha");
    Interface.ListView_AddColumnHeader (itemListView, 192, "Concepto");
    ColumnHeader columnHeader = Interface.ListView_AddColumnHeader (itemListView, 54, "Importe");
    columnHeader.TextAlign = HorizontalAlignment.Right;
    createButton = Interface.Button_Create (this, 0, 0, "Crear", createButton_Click);
    detailButton = Interface.Button_Create (this, 88, 0, "Detalle", detailButton_Click);
    detailButton.Enabled = false;
    cancelButton = Interface.Button_Create (this, 0, 0, "Cerrar", cancelButton_Click);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    (itemListView.Items.Count == 0 ? accountComboBox as Control : itemListView).Focus ();
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
    (TopLevelControl as MainForm).AccountEntryDetailTabPage_Open (0);
  }
  void detailButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).AccountEntryDetailTabPage_Open ((int) itemListView.SelectedItems [0].Tag);
  }
  void itemListView_DoubleClick (object o, EventArgs e) {
    detailButton.PerformClick ();
  }
  void itemListView_SelectedIndexChanged (object o, EventArgs e) {
    detailButton.Enabled = itemListView.SelectedItems.Count > 0;
  }
  void searchButton_Click (object o, EventArgs e) {
    Interface.ListView_Clear (itemListView);
    int accountId = 0;
    if (accountComboBox.SelectedIndex > 0) accountId = (accountComboBox.SelectedItem as Database.Account).Id;
    int year = yearComboBox.SelectedIndex;
    if (year > 0) year += 2000;    foreach (Database.AccountEntry item in Database.AccountEntry.Search (accountId, monthComboBox.SelectedIndex, year)) Interface.ListView_AddListViewItem (itemListView, new string [] { item.AccountCode + " - " + item.AccountName, Data.Date_Format (item.Date), item.Name, Data.Double_Format (item.Price) }, item.Id);
    if (itemListView.Items.Count > 0) itemListView.Focus ();
  }
}

