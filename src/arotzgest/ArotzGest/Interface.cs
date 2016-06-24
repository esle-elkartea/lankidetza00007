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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

static class Interface {
  public static Button Button_Create (EventHandler click) {
    Button button = new Button ();
    button.Click += click;
    return button;
  }
  public static Button Button_Create (Control container, int left, int top, string text, EventHandler click) {
    Button button = new Button ();
    button.Bounds = new Rectangle (left, top, 80, 24);
    button.Text = text;
    button.Click += click;
    container.Controls.Add (button);
    return button;
  }
  public static Button Button_CreateSmall (Control container, int left, int top, string text, EventHandler click) {
    Button button = new Button ();
    button.Bounds = new Rectangle (left, top, 20, 20);
    button.FlatStyle = FlatStyle.Popup;
    button.Text = text;
    button.Click += click;
    container.Controls.Add (button);
    return button;
  }
  public static CheckBox CheckBox_Create (Control container, int left, int top, string text, bool state, EventHandler checkedChanged) {
    CheckBox checkBox = new CheckBox ();
    checkBox.Location = new Point (left, top + 1);
    checkBox.AutoSize = true;
    checkBox.Text = text;
    checkBox.Checked = state;
    checkBox.CheckedChanged += checkedChanged;
    container.Controls.Add (checkBox);
    return checkBox;
  }
  public static object ComboBox_AddItem (ComboBox comboBox, object item, bool selected) {
    int index = comboBox.Items.Add (item);
    if (selected) comboBox.SelectedIndex = index;
    return item;
  }
  public static ComboBox ComboBox_Create (Control container, int left, int top, int width) {
    ComboBox comboBox = new ComboBox ();
    comboBox.Bounds = new Rectangle (left, top - 1, width + 24, 21);
    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
    container.Controls.Add (comboBox);
    return comboBox;
  }
  public static bool ConfirmationDialog_Show (string text) {
    Form form = new Form ();
    Form_Prepare (form, 320, 88);
    Interface.PictureBox_Create (form, 8, 8, 32, 32, Resources.GetImage32 ("Confirmation"));
    Label textLabel = Interface.Label_Create (form, 48, 8, text);
    textLabel.MaximumSize = new Size (form.ClientSize.Width - 56, form.ClientSize.Height - 56);
    Button acceptButton = Interface.Button_Create (form, 8, form.ClientSize.Height - 32, "Aceptar", null);
    acceptButton.DialogResult = DialogResult.Yes;
    Button cancelButton = Interface.Button_Create (form, form.ClientSize.Width - 88, form.ClientSize.Height - 32, "Cancelar", null);
    form.CancelButton = cancelButton;
    return form.ShowDialog () == DialogResult.Yes;
  }
  public static void ErrorDialog_Show (string text) {
    Form form = new Form ();
    Form_Prepare (form, 320, 88);
    Interface.PictureBox_Create (form, 8, 8, 32, 32, Resources.GetImage32 ("error"));
    Label textLabel = Interface.Label_Create (form, 48, 8, text);
    textLabel.MaximumSize = new Size (form.ClientSize.Width - 56, form.ClientSize.Height - 56);
    Button closeButton = Interface.Button_Create (form, 8, 56, "Aceptar", null);
    form.CancelButton = closeButton;
    form.ShowDialog ();
  }
  public static void Form_Prepare (Form form, int width, int height) {
    form.StartPosition = FormStartPosition.CenterParent;
    form.FormBorderStyle = FormBorderStyle.FixedDialog;
    form.ClientSize = new Size (width, height);
    form.Text = ArotzGest.Name;
    form.MinimizeBox = false;
    form.MaximizeBox = false;
    form.ShowInTaskbar = false;
  }
  public static Panel HeaderPanel_Create (Control container, int left, int top, string icon, string title, string subtitle) {
    Panel panel = Panel_Create (container, left, top, 616, 32);
    Interface.PictureBox_Create (panel, 0, 0, 32, 32, Resources.GetImage32 (icon));
    Label titleLabel = Interface.Label_Create (panel, 36, -8, title);
    titleLabel.Font = new Font ("trebuchet ms", 13.5f, FontStyle.Bold);
    Label subtitleLabel = Interface.Label_Create (panel, 38, 16, subtitle);
    subtitleLabel.BringToFront ();
    subtitleLabel.ForeColor = Color.FromArgb (0, 96, 96, 96);
    return panel;
  }
  public static void HeaderPanel_SetText (Panel panel, string title, string subtitle) {
    panel.Controls [2].Text = title;
    panel.Controls [0].Text = subtitle;
  }
  public static void InformationDialog_Show (string text) {
    Form form = new Form ();
    Form_Prepare (form, 320, 88);
    Interface.PictureBox_Create (form, 8, 8, 32, 32, Resources.GetImage32 ("Information"));
    Label textLabel = Interface.Label_Create (form, 48, 8, text);
    textLabel.MaximumSize = new Size (form.ClientSize.Width - 56, form.ClientSize.Height - 56);
    Button closeButton = Interface.Button_Create (form, 8, form.ClientSize.Height - 32, "Aceptar", null);
    form.CancelButton = closeButton;
    form.ShowDialog ();
  }
  public static Label Label_Create (Control container, int left, int top, string text) {
    Label label = new Label ();
    label.Location = new Point (left, top + 3);
    label.AutoSize = true;
    label.TextAlign = ContentAlignment.TopLeft;
    label.Text = text;
    container.Controls.Add (label);
    return label;
  }
  public static ColumnHeader ListView_AddColumnHeader (ListView listView, int width, string text) {
    ColumnHeader columnHeader = new ColumnHeader ();
    columnHeader.Width = width + (listView.Columns.Count == 0 ? 8 : 12);
    columnHeader.Text = text;
    listView.Columns.Add (columnHeader);
    return columnHeader;
  }
  public static ListViewItem ListView_AddListViewItem (ListView listView, string [] texts, object tag) {
    ListViewItem listViewItem = new ListViewItem (texts);
    listViewItem.Tag = tag;
    listView.Items.Add (listViewItem);
    return listViewItem;
  }
  public static void ListView_Clear (ListView listView) {
    listView.SelectedItems.Clear ();
    listView.Items.Clear ();
  }
  public static ListView ListView_Create (Control container, int left, int top, int width, int height, EventHandler selectedIndexChanged, EventHandler doubleClick) {
    ListView listView = new ListView ();
    listView.Bounds = new Rectangle (left, top, width, height);
    listView.View = View.Details;
    listView.FullRowSelect = true;
    listView.MultiSelect = false;
    listView.HideSelection = false;
    listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    listView.GotFocus += delegate (object o, EventArgs e) {
      if (listView.Items.Count == 0) return;
      if (listView.FocusedItem == null) listView.Items [0].Focused = true;
      listView.FocusedItem.Selected = true;
    };
    listView.EnabledChanged += delegate (object o, EventArgs e) {
      if (!listView.Enabled) listView.SelectedItems.Clear ();
    };
    listView.DoubleClick += doubleClick;
    listView.SelectedIndexChanged += selectedIndexChanged;
    container.Controls.Add (listView);
    return listView;
  }
  public static void ListViewItem_Modify (ListViewItem listViewItem, string [] texts, object tag) {
    for (int i = 0; i < listViewItem.SubItems.Count; i++) listViewItem.SubItems [i].Text = texts [i];
    listViewItem.Tag = tag;
  }
  public static Panel Panel_Create (Control container, int left, int top, int width, int height) {
    Panel panel = new Panel ();
    panel.Bounds = new Rectangle (left, top, width, height);
    container.Controls.Add (panel);
    return panel;
  }
  public static PictureBox PictureBox_Create (Control container, int left, int top, int width, int height, Image image) {
    PictureBox pictureBox = new PictureBox ();
    pictureBox.Bounds = new Rectangle (left, top, width, height);
    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
    pictureBox.Image = image;
    container.Controls.Add (pictureBox);
    return pictureBox;
  }
  public static TabPage TabControl_AddTabPage (TabControl tabControl, TabPage tabPage) {
    tabControl.TabPages.Add (tabPage);
    tabControl.SelectedTab = tabPage;
    return tabPage;
  }
  public static TabControl TabControl_Create (Control container, int left, int top) {
    TabControl tabControl = new TabControl ();
    tabControl.Bounds = new Rectangle (left, top, 0, 0);
    tabControl.ControlRemoved += delegate (object o, ControlEventArgs e) { tabControl.SelectedIndex --; };
    tabControl.SelectedIndexChanged += delegate (object o, EventArgs e) { tabControl.SelectedTab.Focus (); };
    container.Controls.Add (tabControl);
    return tabControl;
  }
  public static TabControl TabControl_Create (Control container, int left, int top, int width, string [] texts, EventHandler selectedIndexChanged) {
    TabControl tabControl = new TabControl ();
    tabControl.Bounds = new Rectangle (left, top, width, 24);
    tabControl.Appearance = TabAppearance.FlatButtons;
    tabControl.ItemSize = new Size (0, 24);
    tabControl.SelectedIndexChanged += selectedIndexChanged;
    foreach (string text in texts) tabControl.TabPages.Add (text);
    container.Controls.Add (tabControl);
    return tabControl;
  }
  public static TextBox TextBox_Create (Control container, int left, int top, int width, int lines, int maxLength) {
    TextBox textBox = new TextBox ();
    textBox.Bounds = new Rectangle (left, top, width + 8, 13 * lines + 7);
    if (lines > 1) {
      textBox.Multiline = true;
      textBox.AcceptsReturn = true;
    }
    textBox.MaxLength = maxLength;
    textBox.GotFocus += delegate (object o, EventArgs e) { textBox.SelectAll (); };
    container.Controls.Add (textBox);
    return textBox;
  }
  public static TreeNode TreeNode_Create (TreeNode treeNode, string text, object tag, bool selected) {
    TreeNode treeNode2 = new TreeNode ();
    treeNode2.Text = text;
    treeNode2.Tag = tag;
    if (selected) treeNode.TreeView.SelectedNode = treeNode2;
    treeNode.Nodes.Add (treeNode2);
    return treeNode2;
  }
  public static TreeView TreeView_Create (Control container, int left, int top, int width, int height, TreeViewEventHandler afterSelect, EventHandler doubleClick) {
    TreeView treeView = new TreeView ();
    treeView.Bounds = new Rectangle (left, top, width, height);
    treeView.HideSelection = false;
    treeView.AfterSelect += afterSelect;
    treeView.DoubleClick += doubleClick;
    container.Controls.Add (treeView);
    return treeView;
  }
  public class ErrorManager {
    ErrorProvider errorProvider;
    public List <Control> Controls;
    public ErrorManager () {
      errorProvider = new ErrorProvider ();
      errorProvider.Icon = Resources.GetIcon16 ("Warning");
      errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
      Controls = new List <Control> ();
    }
    public void Add (Control control, string message) {
      errorProvider.SetError (control, message);
      if (Controls.Contains (control)) return;
      Controls.Add (control);
      errorProvider.SetIconPadding (control, 3);
      if (control is TextBox) (control as TextBox).TextChanged += control_Changed;
      else if (control is ComboBox) (control as ComboBox).SelectedIndexChanged += control_Changed;
      else if (control is Button) (control as Button).Click += control_Changed;
    }
    public void Clear () {
      while (Controls.Count > 0) Remove (Controls [0]);
    }
    public void Remove (Control control) {
      errorProvider.SetError (control, "");
      Controls.Remove (control);
    }
    void control_Changed (object o, EventArgs e) {
      Remove (o as Control);
      if (o is TextBox) (o as TextBox).TextChanged -= control_Changed;
      else if (o is ComboBox) (o as ComboBox).SelectedIndexChanged -= control_Changed;
      else if (o is Button) (o as Button).Click -= control_Changed;
    }
  }
  public class ListViewComprarer : IComparer {
    public int Compare (object a, object b) {
      ListViewItem x = a as ListViewItem;
      ListViewItem y = b as ListViewItem;
      string xs = "";
      string ys = "";
      int j = x.ListView.Columns.Count;
      for (int i = 0; i < j; i++) {
        xs += x.SubItems [i];
        ys += y.SubItems [i];
      }
      return String.Compare (xs, ys);
    }
  }
  public static class Resources {
    public static Icon GetIcon16 (string name) {
#if DEBUG
      return new Icon (Application.StartupPath + "\\res\\" + name + ".ico", 16, 16);
#else
      return new Icon (System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream (name + ".ico"), 16, 16);
#endif
    }
    public static Image GetImage (string name) {
#if DEBUG
      return new Bitmap (Application.StartupPath + "\\res\\" + name);
#else
      return new Bitmap (System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream (name));
#endif
    }
    public static Image GetImage16 (string name) {
#if DEBUG
      return new Icon (Application.StartupPath + "\\res\\" + name + ".ico", 16, 16).ToBitmap ();
#else
      return new Icon (System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream (name + ".ico"), 16, 16).ToBitmap ();
#endif
    }
    public static Image GetImage32 (string name) {
#if DEBUG
      return new Icon (Application.StartupPath + "\\res\\" + name + ".ico", 32, 32).ToBitmap ();
#else
      return new Icon (System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream (name + ".ico"), 32, 32).ToBitmap ();
#endif
    }
  }
}

