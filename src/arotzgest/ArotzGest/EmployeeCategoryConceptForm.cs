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

class EmployeeCategoryConceptForm : Form {
  Button acceptButton, cancelButton;
  CheckBox modifyCheckBox;
  ComboBox itemComboBox;
  Interface.ErrorManager errorManager;
  TextBox costTextBox, priceTextBox, quantityTextBox;
  public EmployeeCategoryConceptForm (Database.EmployeeCategoryConcept concept) {
    Interface.Form_Prepare (this, 316, 228);
    errorManager = new Interface.ErrorManager ();
    Interface.HeaderPanel_Create (this, 8, 8, "EmployeeCategory", "Categoría de empleado", concept == null ? "Añadir" : "Modificar");
    Interface.Label_Create (this, 8, 48, "Categoría de empleado");
    itemComboBox = Interface.ComboBox_Create (this, 136, 48, 128);
    foreach (Database.EmployeeCategory item in Database.EmployeeCategory.List ()) Interface.ComboBox_AddItem (itemComboBox, item, concept != null && item.Id == concept.Id);
    itemComboBox.SelectedIndexChanged += itemComboBox_SelectedIndexChanged;
    Interface.Label_Create (this, 8, 76, "Horas");
    quantityTextBox = Interface.TextBox_Create (this, 136, 76, 51, 1, 9);
    quantityTextBox.TextAlign = HorizontalAlignment.Right;
    quantityTextBox.Text = concept == null ? "" : Data.Quantity_Format (concept.Quantity);
    Interface.Label_Create (this, 8, 104, "Coste / hora");
    costTextBox = Interface.TextBox_Create (this, 136, 104, 51, 1, 9);
    costTextBox.TextAlign = HorizontalAlignment.Right;
    costTextBox.Text = concept == null ? "" : Data.Double_Format (concept.Cost);
    Interface.Label_Create (this, 8, 132, "Precio / hora");
    priceTextBox = Interface.TextBox_Create (this, 136, 132, 51, 1, 9);
    priceTextBox.TextAlign = HorizontalAlignment.Right;
    priceTextBox.Text = concept == null ? "" : Data.Double_Format (concept.Price);
    Interface.Label_Create (this, 8, 160, "Modificar");
    modifyCheckBox = Interface.CheckBox_Create (this, 136, 160, "", false, null);
    acceptButton = Interface.Button_Create (this, 8, ClientSize.Height - 32, "Aceptar", acceptButton_Click);
    AcceptButton = acceptButton;
    cancelButton = Interface.Button_Create (this, ClientSize.Width - 88, ClientSize.Height - 32, "Cancelar", null);
    CancelButton = cancelButton;
  }
  void acceptButton_Click (object o, EventArgs e) {
    errorManager.Clear ();
    Database.EmployeeCategory item = itemComboBox.SelectedItem as Database.EmployeeCategory;
    if (itemComboBox.SelectedItem == null) errorManager.Add (itemComboBox, "La categoría ha de especificarse");
    double? quantity = Data.Double_Parse (quantityTextBox.Text);
    if (quantityTextBox.Text == "") errorManager.Add (quantityTextBox, "Las horas han de especificarse");
    else if (quantity == null || quantity >= 1000000) errorManager.Add (quantityTextBox, "Las horas especificadas no son válidas");
    double? cost = Data.Double_Parse (costTextBox.Text);
    if (costTextBox.Text == "") errorManager.Add (costTextBox, "El coste/hora ha de especificarse");
    else if (cost == null || cost < 0 || cost >= 1000000) errorManager.Add (costTextBox, "El coste / hora especificado no es válido");
    double? price = Data.Double_Parse (priceTextBox.Text);
    if (priceTextBox.Text == "") errorManager.Add (priceTextBox, "El precio/hora ha de especificarse");
    else if (price == null || price < 0 || price >= 1000000) errorManager.Add (priceTextBox, "El precio / hora especificado no es válido");
    if (errorManager.Controls.Count > 0) {
      errorManager.Controls [0].Focus ();
      return;
    }
    if (modifyCheckBox.Checked) item.UpdatePrices ((double) cost, (double) price);
    Tag = new Database.EmployeeCategoryConcept (item.Id, item.Name, (double) quantity, (double) cost, (double) price);
    DialogResult = DialogResult.OK;
    Close ();
  }
  void itemComboBox_SelectedIndexChanged (object o, EventArgs e) {
    Database.EmployeeCategory concept = itemComboBox.SelectedItem as Database.EmployeeCategory;
    costTextBox.Text = Data.Double_Format (concept.Cost);
    priceTextBox.Text = Data.Double_Format (concept.Price);
  }
}

