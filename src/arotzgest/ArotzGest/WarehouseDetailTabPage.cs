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

class WarehouseDetailTabPage : TabPage {
  bool modifying;
  Button acceptButton, budgetDetailButton, cancelButton, deleteButton, modifyButton;
  Database.Warehouse item;
  Interface.ErrorManager errorManager;
  ListView itemListView;
  Panel headerPanel;
  TabControl statusTabControl;
  TextBox nameTextBox;
  public WarehouseDetailTabPage (int id) {
    errorManager = new Interface.ErrorManager ();
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "Warehouse", "", "");
    Interface.Label_Create (this, 0, 48, "Nombre");
    nameTextBox = Interface.TextBox_Create (this, 128, 48, 128, 1, 255);
    statusTabControl = Interface.TabControl_Create (this, 0, 84, 130, new string [] { "Terminados", "Todos" }, statusTabControl_SelectedIndexChanged);
    itemListView = Interface.ListView_Create (this, 0, 116, 0, 0, itemListView_SelectedIndexChanged, itemListView_DoubleClick);
    Interface.ListView_AddColumnHeader (itemListView, 41, "Número");
    Interface.ListView_AddColumnHeader (itemListView, 192, "Cliente");
    Interface.ListView_AddColumnHeader (itemListView, 54, "Fecha");
    Interface.ListView_AddColumnHeader (itemListView, 64, "Estado");
    budgetDetailButton = Interface.Button_Create (this, 0, 116, "Detalle", budgetDetailButton_Click);
    modifyButton = Interface.Button_Create (this, 0, 0, "", modifyButton_Click);
    deleteButton = Interface.Button_Create (this, 88, 0, "Borrar", deleteButton_Click);
    cancelButton = Interface.Button_Create (this, 0, 0, "", cancelButton_Click);
    if (id > 0) item = Database.Warehouse.Get (id);
    setModifying (item == null);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    (modifying ? nameTextBox as Control : headerPanel).Focus ();
  }
  protected override void OnResize (EventArgs e) {
    base.OnResize (e);
    statusTabControl.Left = Width - statusTabControl.Width - 86;
    itemListView.Size = new Size (Width - 88, Height - itemListView.Top - 96);
    budgetDetailButton.Left = Width - 80;
    modifyButton.Top = Height - 24;
    deleteButton.Top = Height - 24;
    cancelButton.Location = new Point (Width - 80, Height - 24);
  }
  void statusTabControl_SelectedIndexChanged (object o, EventArgs e) {
    setModifying (modifying);
  }
  void itemListView_SelectedIndexChanged (object o, EventArgs e) {
    bool selected = itemListView.SelectedItems.Count > 0;
    budgetDetailButton.Enabled = selected;
  }
  void itemListView_DoubleClick (object o, EventArgs e) {
    budgetDetailButton.PerformClick ();
  }
  void budgetDetailButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).BudgetDetailTabPage_Open ((int) itemListView.SelectedItems [0].Tag);
  }
  void acceptButton_Click (object o, EventArgs e) {
    if (modifying) modifyButton.PerformClick ();
  }
  void cancelButton_Click (object o, EventArgs e) {
    if (modifying && item != null) setModifying (false);
    else Parent.Controls.Remove (this);
  }
  void deleteButton_Click (object o, EventArgs e) {
    if (!Interface.ConfirmationDialog_Show ("Está seguro de que desea borrar la categoría de material actual?")) return;
    if (item.CheckUsed ()) {
      Interface.ErrorDialog_Show ("La categoría de material actual no se puede borrar por que tiene entidades relacionadas");
      return;
    }
    Database.Warehouse.Delete (item.Id);
    Parent.Controls.Remove (this);
  }
  void modifyButton_Click (object o, EventArgs e) {
    if (modifying) {
      errorManager.Clear ();
      if (nameTextBox.Text == "") errorManager.Add (nameTextBox, "El nombre ha de especificarse");
      else if (!Database.Warehouse.CheckUnique (item, nameTextBox.Text)) errorManager.Add (nameTextBox, "Ya existe una categoría de material con el nombre especificado");
      if (errorManager.Controls.Count > 0) {
        errorManager.Controls [0].Focus ();
        return;
      }
      Database.Warehouse criteria = new Database.Warehouse (nameTextBox.Text);
      item = item == null ? Database.Warehouse.Create (criteria) : item.Update (criteria);
    }
    setModifying (!modifying);
  }
  void setModifying (bool value) {
    modifying = value;
    if (!modifying) errorManager.Clear ();
    Name = "WarehouseDetail_" + (item == null ? 0 : item.Id);
    Text = "Almacén: " + (item == null ? "" : item.Name);
    Interface.HeaderPanel_SetText (headerPanel, Text, modifying ? item == null ? "Crear" : "Modificar" : "Detalle");
    nameTextBox.ReadOnly = !modifying;
    nameTextBox.Text = item == null ? "" : item.Name;
    itemListView.Enabled = !modifying;
    if (item != null && !modifying) {
      Interface.ListView_Clear (itemListView);
      foreach (Database.Budget budget in Database.Budget.Search (item.Id, statusTabControl.SelectedIndex <= 0 ? 3 : (int?)null)) Interface.ListView_AddListViewItem (itemListView, new string [] { budget.Number.ToString (), budget.ClientName, Data.Date_Format (budget.Date), Database.BudgetStates [budget.State] }, budget.Id);
    }
    budgetDetailButton.Enabled = false;
    modifyButton.Text = modifying ? "Aceptar" : "Modificar";
    deleteButton.Enabled = item != null;
    cancelButton.Text = modifying ? "Cancelar" : "Cerrar";
    (modifying ? nameTextBox as Control : headerPanel).Focus ();
  }
}

