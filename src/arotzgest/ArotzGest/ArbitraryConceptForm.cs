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

class ArbitraryConceptForm : Form {
  Button acceptButton, cancelButton;
  Interface.ErrorManager errorManager;
  TextBox costTextBox, nameTextBox, priceTextBox, quantityTextBox;
  public ArbitraryConceptForm (Database.ArbitraryConcept concept) {
    Interface.Form_Prepare (this, 360, 200);
    errorManager = new Interface.ErrorManager ();
    Interface.HeaderPanel_Create (this, 8, 8, "Arbitrary", "Otro", concept == null ? "A�adir" : "Modificar");
    Interface.Label_Create (this, 8, 48, "Nombre");
    nameTextBox = Interface.TextBox_Create (this, 136, 48, 192, 1, 255);
    nameTextBox.Text = concept == null ? "" : concept.Name;
    Interface.Label_Create (this, 8, 76, "Cantidad");
    quantityTextBox = Interface.TextBox_Create (this, 136, 76, 51, 1, 9);
    quantityTextBox.TextAlign = HorizontalAlignment.Right;
    quantityTextBox.Text = concept == null ? "" : Data.Quantity_Format (concept.Quantity);
    Interface.Label_Create (this, 8, 104, "Coste");
    costTextBox = Interface.TextBox_Create (this, 136, 104, 51, 1, 9);
    costTextBox.TextAlign = HorizontalAlignment.Right;
    costTextBox.Text = concept == null ? "" : Data.Double_Format (concept.Cost);
    Interface.Label_Create (this, 8, 132, "Precio");
    priceTextBox = Interface.TextBox_Create (this, 136, 132, 51, 1, 9);
    priceTextBox.TextAlign = HorizontalAlignment.Right;
    priceTextBox.Text = concept == null ? "" : Data.Double_Format (concept.Price);
    acceptButton = Interface.Button_Create (this, 8, ClientSize.Height - 32, "Aceptar", acceptButton_Click);
    AcceptButton = acceptButton;
    cancelButton = Interface.Button_Create (this, ClientSize.Width - 88, ClientSize.Height - 32, "Cancelar", null);
    CancelButton = cancelButton;
  }
  void acceptButton_Click (object o, EventArgs e) {
    errorManager.Clear ();
    if (nameTextBox.Text == "") errorManager.Add (nameTextBox, "El nombre ha de especificarse");
    double? quantity = Data.Double_Parse (quantityTextBox.Text);
    if (quantityTextBox.Text == "") errorManager.Add (quantityTextBox, "La cantidad ha de especificarse");
    else if (quantity == null || quantity < 0 || quantity >= 1000000) errorManager.Add (quantityTextBox, "La cantidad especificada no es v�lida");
    double? cost = Data.Double_Parse (costTextBox.Text);
    if (costTextBox.Text == "") errorManager.Add (costTextBox, "El coste ha de especificarse");
    else if (cost == null || cost < 0 || cost >= 1000000) errorManager.Add (costTextBox, "El coste especificado no es v�lido");
    double? price = Data.Double_Parse (priceTextBox.Text);
    if (priceTextBox.Text == "") errorManager.Add (priceTextBox, "El precio ha de especificarse");
    else if (price == null || price < 0 || price >= 1000000) errorManager.Add (priceTextBox, "El precio especificado no es v�lido");
    if (errorManager.Controls.Count > 0) {
      errorManager.Controls [0].Focus ();
      return;
    }
    Tag = new Database.ArbitraryConcept (nameTextBox.Text, (double) quantity, (double) cost, (double) price);
    DialogResult = DialogResult.OK;
    Close ();
  }
}

