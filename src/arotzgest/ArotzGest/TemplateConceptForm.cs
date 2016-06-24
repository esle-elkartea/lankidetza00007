/*  ArotzGest
 *  (c) 2006 Webalianza T.I S.L.
 *  Licenciado bajo la GNU General Public License
 *  
 *  Programaci�n: Jorge Moreiro
 *                Iker Est�banez
 * 
 *  Este fichero forma parte de ArotzGest
 *
 * ArotzGest es software libre; puede redistribuirlo y/o modificarlo
 * bajo los t�rminos de la GNU General Public License, tal y como se haya
 * publicada por la Free Software Foundation, en la versi�n 2 de la licencia o
 * (seg�n su elecci�n) cualquier versi�n posterior.
 * 
 * ArotzGest es redistribuido con la intenci�n que sea �til, pero SIN NINGUNA
 * GARANT�A, ni tan solo las garant�as impl�citas de MERCANTABILIDA o ADECUACI�N 
 * A UN DETERMINADO MOTIVO. Lea la GNU General Public License para m�s detalles.
 * 
 * Deber�a haber recibido una copia de la GNU General Public License acompa�ando a 
 * ArotzGest.
 * 
 * �STE PROYECTO HA SIDO SUBVENCIONADO POR SPRI S.A. DENTRO DEL MARCO DEL PROGRAMA
 * KZ LANKIDETZA - m�s informaci�n en http://www.spri.es
 * 
 *
 * */

using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

class TemplateConceptForm : Form {
  Button acceptButton, cancelButton;
  Interface.ErrorManager errorManager;
  TextBox costTextBox, descriptionTextBox, priceTextBox, quantityTextBox;
  TreeView itemTreeView;
  public TemplateConceptForm (Database.TemplateConcept concept) {
    Interface.Form_Prepare (this, 423, 315);
    errorManager = new Interface.ErrorManager ();
    Interface.HeaderPanel_Create (this, 8, 8, "Template", "Plantilla", concept == null ? "A�adir" : "Modificar");
    Interface.Label_Create (this, 8, 48, "Material");
    itemTreeView = Interface.TreeView_Create (this, 136, 48, 244, 68, itemTreeView_AfterSelect, null);
    foreach (Database.TemplateCategory category in Database.TemplateCategory.List ()) {
      TreeNode treeNode = itemTreeView.Nodes.Add (category.Name);
      foreach (Database.Template item in Database.Template.List (category.Id)) {
        item.Calculate ();
        Interface.TreeNode_Create (treeNode, item.Name, item, concept != null && item.Id == concept.Id);
      }
    }
    Interface.Label_Create (this, 8, 124, "Descripci�n");
    descriptionTextBox = Interface.TextBox_Create (this, 136, 124, 255, 4, 65535);
    descriptionTextBox.Text = concept == null ? "" : concept.Description;
    Interface.Label_Create (this, 8,191, "Cantidad");
    quantityTextBox = Interface.TextBox_Create (this, 136, 191, 27, 1, 5);
    quantityTextBox.TextAlign = HorizontalAlignment.Right;
    quantityTextBox.Text = concept == null ? "" : Data.Quantity_Format (concept.Quantity);
    Interface.Label_Create (this, 8, 219, "Coste");
    costTextBox = Interface.TextBox_Create (this, 136, 219, 33, 1, 6);
    costTextBox.TextAlign = HorizontalAlignment.Right;
    costTextBox.Text = concept == null ? "" : Data.Double_Format (concept.Cost);
    Interface.Label_Create (this, 8, 247, "Precio");
    priceTextBox = Interface.TextBox_Create (this, 136, 247, 33, 1, 6);
    priceTextBox.TextAlign = HorizontalAlignment.Right;
    priceTextBox.Text = concept == null ? "" : Data.Double_Format (concept.Price);
    acceptButton = Interface.Button_Create (this, 8, ClientSize.Height - 32, "Aceptar", acceptButton_Click);
    AcceptButton = acceptButton;
    cancelButton = Interface.Button_Create (this, ClientSize.Width - 88, ClientSize.Height - 32, "Cancelar", null);
    CancelButton = cancelButton;
  }
  void acceptButton_Click (object o, EventArgs e) {
    errorManager.Clear ();
    Database.Template item = itemTreeView.SelectedNode.Tag as Database.Template;
    if (item == null) errorManager.Add (itemTreeView, "La plantilla ha de especificarse");
    if (descriptionTextBox.Text == "") errorManager.Add (descriptionTextBox, "La descripci�n ha de especificarse");
    double? quantity = Data.Double_Parse (quantityTextBox.Text);
    if (quantityTextBox.Text == "") errorManager.Add (quantityTextBox, "La cantidad ha de especificarse");
    else if (quantity == null || quantity >= 1000) errorManager.Add (quantityTextBox, "La cantidad especificada no es v�lida");
    double? cost = Data.Double_Parse (costTextBox.Text);
    if (costTextBox.Text == "") errorManager.Add (costTextBox, "El coste ha de especificarse");
    else if (cost == null || cost < 0 || cost >= 1000) errorManager.Add (costTextBox, "El coste especificado no es v�lido");
    double? price = Data.Double_Parse (priceTextBox.Text);
    if (priceTextBox.Text == "") errorManager.Add (priceTextBox, "El precio ha de especificarse");
    else if (price == null || price < 0 || price >= 1000) errorManager.Add (priceTextBox, "El precio especificado no es v�lido");
    if (errorManager.Controls.Count > 0) {
      errorManager.Controls [0].Focus ();
      return;
    }
    Tag = new Database.TemplateConcept (item.Id, item.Name, descriptionTextBox.Text, (double) quantity, (double) cost, (double) price);
    DialogResult = DialogResult.OK;
    Close ();
  }
  void itemTreeView_AfterSelect (object o, TreeViewEventArgs e) {
    if (e.Action != TreeViewAction.Unknown) {
      Database.Template concept = itemTreeView.SelectedNode.Tag as Database.Template;
      if (concept != null) {
        descriptionTextBox.Text = concept.Description;
        costTextBox.Text = Data.Double_Format (concept.Cost);
        priceTextBox.Text = Data.Double_Format (concept.Price);
      }
    }
  }
}

