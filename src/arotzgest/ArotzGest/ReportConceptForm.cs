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
    else if (quantity == null || quantity >= 1000000) errorManager.Add (quantityTextBox, "Las horas especificadas no son v�lidas");
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

