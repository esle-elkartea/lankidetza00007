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

class BudgetSearchTabPage : TabPage {
  Button acceptButton, cancelButton, createButton, detailButton, searchButton;
  ComboBox clientComboBox, monthComboBox, yearComboBox, stateComboBox;
  ListView itemListView;
  public BudgetSearchTabPage () {
    Name = "budgetSearch";
    Text = "Presupuestos";
    acceptButton = Interface.Button_Create (acceptButton_Click);
    Interface.HeaderPanel_Create (this, 0, 8, "Budget", Text, "Buscar");
    Interface.Label_Create (this, 0, 48, "Cliente");
    clientComboBox = Interface.ComboBox_Create (this, 128, 48, 128);
    Interface.ComboBox_AddItem (clientComboBox, "", true);
    foreach (Database.Client client in Database.Client.List ()) Interface.ComboBox_AddItem (clientComboBox, client, false);
    Interface.Label_Create (this, 0, 76, "Fecha");
    monthComboBox = Interface.ComboBox_Create (this, 128, 76, 64);
    Interface.ComboBox_AddItem (monthComboBox, "", true);
    foreach (string month in Database.Months) Interface.ComboBox_AddItem (monthComboBox, month, false);
    yearComboBox = Interface.ComboBox_Create (this, 224, 76, 24);
    Interface.ComboBox_AddItem (yearComboBox, "", true);
    while (yearComboBox.Items.Count < 50) Interface.ComboBox_AddItem (yearComboBox, (2000 + yearComboBox.Items.Count).ToString (), false);
    Interface.Label_Create (this, 0, 104, "Estado");
    stateComboBox = Interface.ComboBox_Create (this, 128, 104, 128);
    Interface.ComboBox_AddItem (stateComboBox, "", true);
    foreach (string state in Database.BudgetStates) Interface.ComboBox_AddItem (stateComboBox, state, false);
    searchButton = Interface.Button_Create (this, 0, 132, "Buscar", searchButton_Click);
    itemListView = Interface.ListView_Create (this, 0, 172, 0, 0, itemListView_SelectedIndexChanged, itemListView_DoubleClick);
    Interface.ListView_AddColumnHeader (itemListView, 53, "Número");
    Interface.ListView_AddColumnHeader (itemListView, 192, "Cliente");
    Interface.ListView_AddColumnHeader (itemListView, 54, "Fecha");
    Interface.ListView_AddColumnHeader (itemListView, 64, "Estado");
    createButton = Interface.Button_Create (this, 0, 0, "Crear", createButton_Click);
    detailButton = Interface.Button_Create (this, 88, 0, "Detalle", detailButton_Click);
    detailButton.Enabled = false;
    cancelButton = Interface.Button_Create (this, 0, 0, "Cerrar", cancelButton_Click);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    (itemListView.Items.Count == 0 ? clientComboBox as Control : itemListView).Focus ();
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
    (TopLevelControl as MainForm).BudgetDetailTabPage_Open (0);
  }
  void detailButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).BudgetDetailTabPage_Open ((int) itemListView.SelectedItems [0].Tag);
  }
  void itemListView_DoubleClick (object o, EventArgs e) {
    detailButton.PerformClick ();
  }
  void itemListView_SelectedIndexChanged (object o, EventArgs e) {
    detailButton.Enabled = itemListView.SelectedItems.Count > 0;
  }
  void searchButton_Click (object o, EventArgs e) {
    Interface.ListView_Clear (itemListView);
    int clientId = 0;
    if (clientComboBox.SelectedIndex > 0) clientId = (clientComboBox.SelectedItem as Database.Client).Id;
    int year = yearComboBox.SelectedIndex;
    if (year > 0) year += 2000;
    foreach (Database.Budget item in Database.Budget.Search (clientId, monthComboBox.SelectedIndex, year, stateComboBox.SelectedIndex - 1)) Interface.ListView_AddListViewItem (itemListView, new string [] { item.Number + "/" + item.Date.Year, item.ClientName, Data.Date_Format (item.Date), Database.BudgetStates [item.State] }, item.Id);
    if (itemListView.Items.Count > 0) itemListView.Focus ();
  }
}

