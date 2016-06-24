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

class BudgetConceptForm : Form {
  Button acceptButton, cancelButton;
  ComboBox itemComboBox;
  Interface.ErrorManager errorManager;
  public BudgetConceptForm (Database.BudgetConcept concept) {
    Interface.Form_Prepare (this, 316, 116);
    errorManager = new Interface.ErrorManager ();
    Interface.HeaderPanel_Create (this, 8, 8, "Budget", "Presupuesto", concept == null ? "Añadir" : "Modificar");
    Interface.Label_Create (this, 8, 48, "Presupuesto");
    itemComboBox = Interface.ComboBox_Create (this, 136, 48, 36);
    foreach (Database.Budget item in Database.Budget.ListAccepted ()) Interface.ComboBox_AddItem (itemComboBox, item, concept != null && item.Id == concept.Id);
    acceptButton = Interface.Button_Create (this, 8, ClientSize.Height - 32, "Aceptar", acceptButton_Click);
    AcceptButton = acceptButton;
    cancelButton = Interface.Button_Create (this, ClientSize.Width - 88, ClientSize.Height - 32, "Cancelar", null);
    CancelButton = cancelButton;
  }
  void acceptButton_Click (object o, EventArgs e) {
    errorManager.Clear ();
    Database.Budget item = itemComboBox.SelectedItem as Database.Budget;
    if (itemComboBox.SelectedItem == null) errorManager.Add (itemComboBox, "El presupuesto ha de especificarse");
    if (errorManager.Controls.Count > 0) {
      errorManager.Controls [0].Focus ();
      return;
    }
    item.Calculate ();
    Tag = new Database.BudgetConcept (item.Id, "Presupuesto nº " + item.Number, 1, item.Cost, item.Price);
    DialogResult = DialogResult.OK;
    Close ();
  }
}

