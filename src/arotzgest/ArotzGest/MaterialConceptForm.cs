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
using System.Text.RegularExpressions;
using System.Windows.Forms;

class MaterialConceptForm : Form {
  Button acceptButton, cancelButton;
  CheckBox modifyCheckBox;
  Interface.ErrorManager errorManager;
  TextBox costTextBox, dimension1TextBox, dimension2TextBox, dimension3TextBox, priceTextBox, quantityTextBox;
  TreeView itemTreeView;
  public MaterialConceptForm (Database.MaterialConcept concept) {
    Interface.Form_Prepare (this, 420, 304);
    errorManager = new Interface.ErrorManager ();
    Interface.HeaderPanel_Create (this, 8, 8, "Material", "Material", concept == null ? "Añadir" : "Modificar");
    Interface.Label_Create (this, 8, 48, "Material");
    itemTreeView = Interface.TreeView_Create (this, 136, 48, 260, 68, itemTreeView_AfterSelect, null);
    foreach (Database.MaterialCategory category in Database.MaterialCategory.List ()) {
      TreeNode treeNode = itemTreeView.Nodes.Add (category.Name);
      foreach (Database.Material item in Database.Material.List (category.Id)) Interface.TreeNode_Create (treeNode, item.Name, item, concept != null && item.Id == concept.Id);
    }
    Interface.Label_Create (this, 8, 124, "Dimensiones");
    dimension1TextBox = Interface.TextBox_Create (this, 136, 124, 51, 1, 9);
    dimension1TextBox.TextAlign = HorizontalAlignment.Right;
    dimension1TextBox.ReadOnly = concept == null || concept.Type < 1;
    dimension1TextBox.TabStop = concept != null && concept.Type >= 1;
    dimension1TextBox.Text = dimension1TextBox.ReadOnly ? "" : Data.Double_Format (concept.Dimension1);
    Interface.Label_Create (this, 202, 124, "x");
    dimension2TextBox = Interface.TextBox_Create (this, 219, 124, 51, 1, 9);
    dimension2TextBox.TextAlign = HorizontalAlignment.Right;
    dimension2TextBox.ReadOnly = concept == null || concept.Type < 2;
    dimension2TextBox.TabStop = concept != null && concept.Type >= 2;
    dimension2TextBox.Text = dimension2TextBox.ReadOnly ? "" : Data.Double_Format (concept.Dimension2);
    Interface.Label_Create (this, 285, 124, "x");
    dimension3TextBox = Interface.TextBox_Create (this, 302, 124, 51, 1, 9);
    dimension3TextBox.TextAlign = HorizontalAlignment.Right;
    dimension3TextBox.ReadOnly = concept == null || concept.Type < 3;
    dimension3TextBox.TabStop = concept != null && concept.Type >= 3;
    dimension3TextBox.Text = dimension3TextBox.ReadOnly ? "" : Data.Double_Format (concept.Dimension3);
    Interface.Label_Create (this, 8, 152, "Cantidad");
    quantityTextBox = Interface.TextBox_Create (this, 136, 152, 51, 1, 9);
    quantityTextBox.TextAlign = HorizontalAlignment.Right;
    quantityTextBox.Text = concept == null ? "" : Data.Quantity_Format (concept.Quantity);
    Interface.Label_Create (this, 8, 180, "Coste");
    costTextBox = Interface.TextBox_Create (this, 136, 180, 51, 1, 9);
    costTextBox.TextAlign = HorizontalAlignment.Right;
    costTextBox.Text = concept == null ? "" : Data.Double_Format (concept.Cost);
    Interface.Label_Create (this, 8, 208, "Precio");
    priceTextBox = Interface.TextBox_Create (this, 136, 208, 51, 1, 9);
    priceTextBox.TextAlign = HorizontalAlignment.Right;
    priceTextBox.Text = concept == null ? "" : Data.Double_Format (concept.Price);
    Interface.Label_Create (this, 8, 236, "Modificar");
    modifyCheckBox = Interface.CheckBox_Create (this, 136, 236, "", false, null);
    acceptButton = Interface.Button_Create (this, 8, ClientSize.Height - 32, "Aceptar", acceptButton_Click);
    AcceptButton = acceptButton;
    cancelButton = Interface.Button_Create (this, ClientSize.Width - 88, ClientSize.Height - 32, "Cancelar", null);
    CancelButton = cancelButton;
  }
  void acceptButton_Click (object o, EventArgs e) {
    errorManager.Clear ();
    Database.Material item = itemTreeView.SelectedNode == null ? null : itemTreeView.SelectedNode.Tag as Database.Material;
    if (item == null) errorManager.Add (itemTreeView, "El material ha de especificarse");
    double? dimension1 = Data.Double_Parse (dimension1TextBox.Text);
    if (item != null && item.Type >= 1) {
      if (dimension1TextBox.Text == "") errorManager.Add (dimension1TextBox, "La dimensión ha de especificarse");
      else if (dimension1 == null || dimension1 < 0 || dimension1 >= 1000000) errorManager.Add (dimension1TextBox, "La dimensión especificada no es válida");
    } else dimension1 = 0;
    double? dimension2 = Data.Double_Parse (dimension2TextBox.Text);
    if (item != null && item.Type >= 2) {
      if (dimension2TextBox.Text == "") errorManager.Add (dimension2TextBox, "La dimensión ha de especificarse");
      else if (dimension2 == null || dimension2 < 0 || dimension2 >= 1000000) errorManager.Add (dimension2TextBox, "La dimensión especificada no es válida");
    } else dimension2 = 0;
    double? dimension3 = Data.Double_Parse (dimension3TextBox.Text);
    if (item != null && item.Type >= 3) {
      if (dimension3TextBox.Text == "") errorManager.Add (dimension3TextBox, "La dimensión ha de especificarse");
      else if (dimension3 == null || dimension3 < 0 || dimension3 >= 1000000) errorManager.Add (dimension3TextBox, "La dimensión especificada no es válida");
    } else dimension3 = 0;
    double? quantity = Data.Double_Parse (quantityTextBox.Text);
    if (quantityTextBox.Text == "") errorManager.Add (quantityTextBox, "La cantidad ha de especificarse");
    else if (quantity == null || quantity >= 1000000) errorManager.Add (quantityTextBox, "La cantidad especificada no es válida");
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
    if (modifyCheckBox.Checked) item.UpdatePrices ((double) cost, (double) price);
    Tag = new Database.MaterialConcept (item.Id, item.Name, item.Type, (double) dimension1, (double) dimension2, (double) dimension3, (double) quantity, (double) cost, (double) price);
    DialogResult = DialogResult.OK;
    Close ();
  }
  void itemTreeView_AfterSelect (object o, TreeViewEventArgs e) {
    if (e.Action != TreeViewAction.Unknown) {
      Database.Material concept = itemTreeView.SelectedNode.Tag as Database.Material;
      if (concept != null) {
        dimension1TextBox.ReadOnly = concept == null || concept.Type < 1;
        dimension1TextBox.TabStop = concept != null && concept.Type >= 1;
        dimension1TextBox.Text = dimension1TextBox.ReadOnly ? "" : Data.Double_Format (concept.Dimension1);
        if (dimension1TextBox.ReadOnly) errorManager.Remove (dimension1TextBox);
        dimension2TextBox.ReadOnly = concept == null || concept.Type < 2;
        dimension2TextBox.TabStop = concept != null && concept.Type >= 2;
        dimension2TextBox.Text = dimension2TextBox.ReadOnly ? "" : Data.Double_Format (concept.Dimension2);
        if (dimension2TextBox.ReadOnly) errorManager.Remove (dimension2TextBox);
        dimension3TextBox.ReadOnly = concept == null || concept.Type < 3;
        dimension3TextBox.TabStop = concept != null && concept.Type >= 3;
        dimension3TextBox.Text = dimension3TextBox.ReadOnly ? "" : Data.Double_Format (concept.Dimension3);
        if (dimension3TextBox.ReadOnly) errorManager.Remove (dimension3TextBox);
        costTextBox.Text = Data.Double_Format (concept.Cost);
        priceTextBox.Text = Data.Double_Format (concept.Price);
      }
    }
  }
}

