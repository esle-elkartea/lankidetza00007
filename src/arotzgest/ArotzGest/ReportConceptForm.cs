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

class ReportConceptForm : Form {
  Button acceptButton, cancelButton;
  ComboBox employeeComboBox;
  Database.ReportConcept reportConcept;
  Interface.ErrorManager errorManager;
  TextBox quantityTextBox;
  public ReportConceptForm (Database.ReportConcept concept) {
    reportConcept = concept;
    Interface.Form_Prepare (this, 316, 228);
    errorManager = new Interface.ErrorManager ();
    Interface.HeaderPanel_Create (this, 8, 8, "AccountEntry", "Parte", "Modificar");
    Interface.Label_Create (this, 8, 48, "Empleado");
    employeeComboBox = Interface.ComboBox_Create (this, 136, 48, 128);
    foreach (Database.Employee employee in Database.Employee.Search ("", reportConcept.Id)) Interface.ComboBox_AddItem (employeeComboBox, employee, employee.Id == reportConcept.EmployeeId);
    Interface.Label_Create (this, 8, 76, "Horas");
    quantityTextBox = Interface.TextBox_Create (this, 136, 76, 51, 1, 9);
    quantityTextBox.TextAlign = HorizontalAlignment.Right;
    quantityTextBox.Text = concept == null ? "" : reportConcept.Quantity.ToString ();
    acceptButton = Interface.Button_Create (this, 8, ClientSize.Height - 32, "Aceptar", acceptButton_Click);
    AcceptButton = acceptButton;
    cancelButton = Interface.Button_Create (this, ClientSize.Width - 88, ClientSize.Height - 32, "Cancelar", null);
    CancelButton = cancelButton;
  }
  void acceptButton_Click (object o, EventArgs e) {
    errorManager.Clear ();
    Database.Employee employee = employeeComboBox.SelectedItem as Database.Employee;
    if (employeeComboBox.SelectedItem == null) errorManager.Add (employeeComboBox, "El empleado ha de especificarse");
    double? quantity = Data.Double_Parse (quantityTextBox.Text);
    if (quantityTextBox.Text == "") errorManager.Add (quantityTextBox, "Las horas han de especificarse");
    else if (quantity == null || quantity >= 1000000) errorManager.Add (quantityTextBox, "Las horas especificadas no son válidas");
    if (errorManager.Controls.Count > 0) {
      errorManager.Controls [0].Focus ();
      return;
    }
    reportConcept.EmployeeId = employee.Id;
    reportConcept.EmployeeName = employee.Name;
    reportConcept.Quantity = quantity;
    Tag = reportConcept;
    DialogResult = DialogResult.OK;
    Close ();
  }
}

