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
using System.Windows.Forms;

class ReceiptConceptForm : Form {
  int [] usedNumbers;
  Button acceptButton, cancelButton;
  CheckBox payedCheckBox;
  ComboBox accountComboBox;
  Database.ReceiptConcept concept;
  Interface.ErrorManager errorManager;
  TextBox numberTextBox, dueDateTextBox, payDateTextBox, priceTextBox;
  public ReceiptConceptForm (Database.ReceiptConcept concept, int suggestedNumber, int [] usedNumbers, double price) {
    Interface.Form_Prepare (this, 423, 256);
    errorManager = new Interface.ErrorManager ();
    Interface.HeaderPanel_Create (this, 8, 8, "Receipt", "Recibo", concept == null ? "A�adir" : "Modificar");
    Interface.Label_Create (this, 8, 48, "N�mero");
    numberTextBox = Interface.TextBox_Create (this, 136, 48, 24, 1, 4);
    numberTextBox.TextAlign = HorizontalAlignment.Right;
    numberTextBox.Text = concept == null ? suggestedNumber.ToString () : concept.Number.ToString ();
    Interface.Label_Create (this, 8, 76, "Importe");
    priceTextBox = Interface.TextBox_Create (this, 136, 76, 51, 1, 9);
    priceTextBox.TextAlign = HorizontalAlignment.Right;
    priceTextBox.Text = concept == null ? Data.Double_Format (price) : Data.Double_Format (concept.Price);
    Interface.Label_Create (this, 8, 104, "Fecha vencimiento");
    dueDateTextBox = Interface.TextBox_Create (this, 136, 104, 54, 1, 10);
    dueDateTextBox.Text = concept == null ? "" : Data.Date_Format (concept.DueDate);
    Interface.Label_Create (this, 8, 132, "Cobrado");
    payedCheckBox = Interface.CheckBox_Create (this, 136, 132, "", concept != null && concept.PayDate != null, payedCheckBox_CheckedChanged);
    Interface.Label_Create (this, 8, 160, "Fecha cobro");
    payDateTextBox = Interface.TextBox_Create (this, 136, 160, 54, 1, 10);
    payDateTextBox.Enabled = concept != null && concept.PayDate != null;
    payDateTextBox.Text = concept == null || concept.PayDate == null ? Data.Date_Format (DateTime.Now) : Data.Date_Format ((DateTime) concept.PayDate);
    Interface.Label_Create (this, 8, 188, "Cuenta");
    accountComboBox = Interface.ComboBox_Create (this, 136, 188, 255);
    accountComboBox.Enabled = concept != null && concept.PayDate != null;
    Interface.ComboBox_AddItem (accountComboBox, "", true);
    foreach (Database.Account account in Database.Account.List ()) Interface.ComboBox_AddItem (accountComboBox, account, concept != null && account.Id == concept.AccountId);
    acceptButton = Interface.Button_Create (this, 8, ClientSize.Height - 32, "Aceptar", acceptButton_Click);
    AcceptButton = acceptButton;
    cancelButton = Interface.Button_Create (this, ClientSize.Width - 88, ClientSize.Height - 32, "Cancelar", null);
    CancelButton = cancelButton;
    this.concept = concept;
    this.usedNumbers = usedNumbers;
  }
  void acceptButton_Click (object o, EventArgs e) {
    errorManager.Clear ();
    int? number = Data.Int_Parse (numberTextBox.Text);
    if (numberTextBox.Text == "") errorManager.Add (numberTextBox, "El n�mero ha de especificarse");
    else if (number == null || number <= 0 || number >= 10000) errorManager.Add (numberTextBox, "El n�mero especificado no es v�lido");
    else if (concept == null || number != concept.Number) foreach (int i in usedNumbers) if (number == i) errorManager.Add (numberTextBox, "Ya existe un recibo con el n�mero especificado");
    double? price = Data.Double_Parse (priceTextBox.Text);
    if (priceTextBox.Text == "") errorManager.Add (priceTextBox, "El importe ha de especificarse");
    else if (price == null || price < 0 || price >= 1000000) errorManager.Add (priceTextBox, "El importe especificado no es v�lido");
    DateTime? dueDate = Data.Date_Parse (dueDateTextBox.Text);
    if (dueDateTextBox.Text == "") errorManager.Add (dueDateTextBox, "La fecha de vencimiento ha de especificarse");
    else if (dueDate == null) errorManager.Add (dueDateTextBox, "La fecha de vencimiento especificada no es v�lida");
    DateTime? payDate = !payedCheckBox.Checked ? null : Data.Date_Parse (payDateTextBox.Text);
    if (payedCheckBox.Checked) {
      if (payDateTextBox.Text == "") errorManager.Add (payDateTextBox, "La fecha de cobro ha de especificarse");
      else if (payDateTextBox.Text != "" && payDate == null) errorManager.Add (payDateTextBox, "La fecha de cobro especificada no es v�lida");
    }
    Database.Account account = accountComboBox.SelectedItem as Database.Account;
    if (errorManager.Controls.Count > 0) {
      errorManager.Controls [0].Focus ();
      return;
    }
    Tag = new Database.ReceiptConcept ((int) number, (double) price, (DateTime) dueDate, payDate, payDate == null || account == null ? (int?) null : account.Id, payDate == null || account == null ? (string) null : account.Code + " - " + account.Name);
    DialogResult = DialogResult.OK;
    Close ();
  }
  void payedCheckBox_CheckedChanged (object o, EventArgs e) {
    bool payed = payedCheckBox.Checked;
    payDateTextBox.Enabled = payed;
    accountComboBox.Enabled = payed;
  }
}

