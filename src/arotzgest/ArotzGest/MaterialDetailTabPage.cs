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

class MaterialDetailTabPage : TabPage {
  bool modifying;
  Button acceptButton, cancelButton, categoryLinkButton, deleteButton, modifyButton, providerLinkButton;
  ComboBox categoryComboBox, providerComboBox, typeComboBox;
  Database.Material item;
  Interface.ErrorManager errorManager;
  Panel headerPanel;
  TextBox costTextBox, nameTextBox, categoryTextBox, dimension1TextBox, dimension2TextBox, dimension3TextBox, priceTextBox, providerTextBox, typeTextBox;
  public MaterialDetailTabPage (int id) {
    errorManager = new Interface.ErrorManager ();
    acceptButton = Interface.Button_Create (acceptButton_Click);
    headerPanel = Interface.HeaderPanel_Create (this, 0, 8, "Material", "", "");
    Interface.Label_Create (this, 0, 48, "Categoría");
    categoryTextBox = Interface.TextBox_Create (this, 128, 48, 128, 1, 255);
    categoryTextBox.ReadOnly = true;
    categoryLinkButton = Interface.Button_CreateSmall (this, 272, 48, ">", categoryLinkButton_Click);
    categoryComboBox = Interface.ComboBox_Create (this, 128, 48, 128);
    Interface.Label_Create (this, 0, 76, "Nombre");
    nameTextBox = Interface.TextBox_Create (this, 128, 76, 192, 1, 255);
    Interface.Label_Create (this, 0, 104, "Proveedor");
    providerTextBox = Interface.TextBox_Create (this, 128, 104, 128, 1, 255);
    providerTextBox.ReadOnly = true;
    providerLinkButton = Interface.Button_CreateSmall (this, 272, 104, ">", providerLinkButton_Click);
    providerComboBox = Interface.ComboBox_Create (this, 128, 104, 128);
    Interface.Label_Create (this, 0, 132, "Tipo");
    typeTextBox = Interface.TextBox_Create (this, 128, 132, 128, 1, 255);
    typeTextBox.ReadOnly = true;
    typeComboBox = Interface.ComboBox_Create (this, 128, 132, 128);
    typeComboBox.SelectedIndexChanged += typeComboBox_SelectedIndexChanged;
    Interface.Label_Create (this, 0, 160, "Dimensiones");
    dimension1TextBox = Interface.TextBox_Create (this, 128, 160, 51, 1, 9);
    dimension1TextBox.TextAlign = HorizontalAlignment.Right;
    Interface.Label_Create (this, 194, 160, "x");
    dimension2TextBox = Interface.TextBox_Create (this, 211, 160, 51, 1, 9);
    dimension2TextBox.TextAlign = HorizontalAlignment.Right;
    Interface.Label_Create (this, 277, 160, "x");
    dimension3TextBox = Interface.TextBox_Create (this, 294, 160, 51, 1, 9);
    dimension3TextBox.TextAlign = HorizontalAlignment.Right;
    Interface.Label_Create (this, 0, 188, "Coste");
    costTextBox = Interface.TextBox_Create (this, 128, 188, 51, 1, 9);
    costTextBox.TextAlign = HorizontalAlignment.Right;
    Interface.Label_Create (this, 0, 216, "Precio");
    priceTextBox = Interface.TextBox_Create (this, 128, 216, 51, 1, 9);
    priceTextBox.TextAlign = HorizontalAlignment.Right;
    modifyButton = Interface.Button_Create (this, 0, 0, "", modifyButton_Click);
    deleteButton = Interface.Button_Create (this, 88, 0, "Borrar", deleteButton_Click);
    cancelButton = Interface.Button_Create (this, 0, 0, "", cancelButton_Click);
    if (id > 0) item = Database.Material.Get (id);
    setModifying (item == null);
  }
  protected override void OnGotFocus (EventArgs e) {
    base.OnGotFocus (e);
    (TopLevelControl as MainForm).DefaultButtons_Set (acceptButton, cancelButton);
    (modifying ? categoryComboBox as Control : headerPanel).Focus ();
  }
  protected override void OnResize (EventArgs e) {
    base.OnResize (e);
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
    (TopLevelControl as MainForm).MaterialCategoryDetailTabPage_Open (item.CategoryId);
  }
  void deleteButton_Click (object o, EventArgs e) {
    if (!Interface.ConfirmationDialog_Show ("Está seguro de que desea borrar el material actual?")) return;
    if (item.CheckUsed ()) {
      Interface.ErrorDialog_Show ("El material actual no se puede borrar por que tiene entidades relacionadas");
      return;
    }
    Database.Material.Delete (item.Id);
    Parent.Controls.Remove (this);
  }
  void modifyButton_Click (object o, EventArgs e) {
    if (modifying) {
      errorManager.Clear ();
      Database.MaterialCategory category = categoryComboBox.SelectedItem as Database.MaterialCategory;
      if (categoryComboBox.SelectedItem == null) errorManager.Add (categoryComboBox, "La categoría ha de especificarse");
      else if (Database.MaterialCategory.Get (category.Id) == null) errorManager.Add (categoryComboBox, "La categoría especificada no existe");
      if (nameTextBox.Text == "") errorManager.Add (nameTextBox, "El nombre ha de especificarse");
      else if (!Database.Material.CheckUnique (item, nameTextBox.Text)) errorManager.Add (nameTextBox, "Ya existe un material con el nombre especificado");
      Database.Provider provider = providerComboBox.SelectedItem as Database.Provider;
      if (providerComboBox.SelectedItem == null) errorManager.Add (providerComboBox, "El proveedor ha de especificarse");
      else if (Database.Provider.Get (provider.Id) == null) errorManager.Add (providerComboBox, "El proveedor especificado no existe");
      if (typeComboBox.SelectedItem == null) errorManager.Add (typeComboBox, "El tipo ha de especificarse");
      double? dimension1 = Data.Double_Parse (dimension1TextBox.Text);
      if (typeComboBox.SelectedIndex >= 1) {
        if (dimension1TextBox.Text == "") errorManager.Add (dimension1TextBox, "La dimensión ha de especificarse");
        else if (dimension1 == null || dimension1 < 0 || dimension1 >= 1000000) errorManager.Add (dimension1TextBox, "La dimensión especificada no es válida");
      } else dimension1 = 0;
      double? dimension2 = Data.Double_Parse (dimension2TextBox.Text);
      if (typeComboBox.SelectedIndex >= 2) {
        if (dimension2TextBox.Text == "") errorManager.Add (dimension2TextBox, "La dimensión ha de especificarse");
        else if (dimension2 == null || dimension2 < 0 || dimension2 >= 1000000) errorManager.Add (dimension2TextBox, "La dimensión especificada no es válida");
      } else dimension2 = 0;
      double? dimension3 = Data.Double_Parse (dimension3TextBox.Text);
      if (typeComboBox.SelectedIndex >= 3) {
        if (dimension3TextBox.Text == "") errorManager.Add (dimension3TextBox, "La dimensión ha de especificarse");
        else if (dimension3 == null || dimension3 < 0 || dimension3 >= 1000000) errorManager.Add (dimension3TextBox, "La dimensión especificada no es válida");
      } else dimension3 = 0;
      double? cost = Data.Double_Parse (costTextBox.Text);
      if (costTextBox.Text == "") errorManager.Add (costTextBox, "El coste ha de especificarse");
      else if (cost == null || cost < 0 || cost >= 1000000) errorManager.Add (costTextBox, "El coste especificado no es válido");
      double? price = Data.Double_Parse (priceTextBox.Text);
      if (priceTextBox.Text == "") errorManager.Add (priceTextBox, "El precio ha de especificarse");
      else if (price == null || price < 0 || price >= 1000000) errorManager.Add (priceTextBox, "El precio especificado no es válido");
      if (errorManager.Controls.Count > 0) {
        errorManager.Controls [0].Focus ();
        return;
      }
      Database.Material criteria = new Database.Material (nameTextBox.Text, category.Id, provider.Id, typeComboBox.SelectedIndex, (double) dimension1, (double) dimension2, (double) dimension3, (double) cost, (double) price);
      item = item == null ? Database.Material.Create (criteria) : item.Update (criteria);
    }
    setModifying (!modifying);
  }
  void providerLinkButton_Click (object o, EventArgs e) {
    (TopLevelControl as MainForm).ProviderDetailTabPage_Open (item.ProviderId);
  }
  void typeComboBox_SelectedIndexChanged (object o, EventArgs e) {
    dimension1TextBox.ReadOnly = typeComboBox.SelectedIndex < 1;
    dimension1TextBox.TabStop = typeComboBox.SelectedIndex >= 1;
    dimension2TextBox.ReadOnly = typeComboBox.SelectedIndex < 2;
    dimension2TextBox.TabStop = typeComboBox.SelectedIndex >= 2;
    dimension3TextBox.ReadOnly = typeComboBox.SelectedIndex < 3;
    dimension3TextBox.TabStop = typeComboBox.SelectedIndex >= 3;
  }
  void setModifying (bool value) {
    modifying = value;
    if (!modifying) errorManager.Clear ();
    Name = "materialDetail_" + (item == null ? 0 : item.Id);
    Text = "Material: " + (item == null ? "" : item.Name);
    Interface.HeaderPanel_SetText (headerPanel, Text, modifying ? item == null ? "Crear" : "Modificar" : "Detalle");
    categoryTextBox.Visible = !modifying;
    categoryTextBox.Text = item == null ? "" : item.CategoryName;
    categoryLinkButton.Visible = !modifying;
    categoryComboBox.Visible = modifying;
    if (modifying) foreach (Database.MaterialCategory category in Database.MaterialCategory.List ()) Interface.ComboBox_AddItem (categoryComboBox, category, item != null && category.Id == item.CategoryId);
    else categoryComboBox.Items.Clear ();
    nameTextBox.ReadOnly = !modifying;
    nameTextBox.Text = item == null ? "" : item.Name;
    providerTextBox.Visible = !modifying;
    providerTextBox.Text = item == null ? "" : item.ProviderName;
    providerLinkButton.Visible = !modifying;
    providerComboBox.Visible = modifying;
    if (modifying) foreach (Database.Provider provider in Database.Provider.List ()) Interface.ComboBox_AddItem (providerComboBox, provider, item != null && provider.Id == item.ProviderId);
    else providerComboBox.Items.Clear ();
    typeTextBox.Visible = !modifying;
    typeTextBox.Text = item == null ? "" : Database.MaterialTypes [item.Type];
    typeComboBox.Visible = modifying;
    if (modifying) for (int i = 0; i < Database.MaterialTypes.Length; i++) Interface.ComboBox_AddItem (typeComboBox, Database.MaterialTypes [i], i == 0 || item != null && i == item.Type);
    else typeComboBox.Items.Clear ();
    dimension1TextBox.ReadOnly = typeComboBox.SelectedIndex < 1;
    dimension1TextBox.TabStop = item != null && item.Type >= 1;
    dimension1TextBox.Text = item == null || item.Type < 1 ? "" : Data.Double_Format (item.Dimension1);
    dimension2TextBox.ReadOnly = typeComboBox.SelectedIndex < 2;
    dimension2TextBox.TabStop = item != null && item.Type >= 2;
    dimension2TextBox.Text = item == null || item.Type < 2 ? "" : Data.Double_Format (item.Dimension2);
    dimension3TextBox.ReadOnly = typeComboBox.SelectedIndex < 3;
    dimension3TextBox.TabStop = item != null && item.Type >= 3;
    dimension3TextBox.Text = item == null || item.Type < 3 ? "" : Data.Double_Format (item.Dimension3);
    costTextBox.ReadOnly = !modifying;
    costTextBox.Text = item == null ? "" : Data.Double_Format (item.Cost);
    priceTextBox.ReadOnly = !modifying;
    priceTextBox.Text = item == null ? "" : Data.Double_Format (item.Price);
    modifyButton.Text = modifying ? "Aceptar" : "Modificar";
    deleteButton.Enabled = item != null;
    cancelButton.Text = modifying ? "Cancelar" : "Cerrar";
    (modifying ? categoryComboBox as Control : headerPanel).Focus ();
  }
}
