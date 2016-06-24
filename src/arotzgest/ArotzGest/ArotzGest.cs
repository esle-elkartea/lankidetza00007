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
using System.Windows.Forms;

static class ArotzGest {
  public static string Name = "ArotzGest";
  public static string Version = "2.0";
  static Form splashForm;
  static Timer alphaTimer, closeTimer;
  [STAThread]
  public static void Main () {
    if (!Database.Connect ()) {
      MessageBox.Show ("No se pudo conectar con la base de datos", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
      return;
    }
    Application.EnableVisualStyles ();
    /*splashForm = new Form ();
    Interface.Form_Prepare (splashForm, 636, 342);
    splashForm.StartPosition = FormStartPosition.CenterScreen;
    splashForm.FormBorderStyle = FormBorderStyle.None;
    splashForm.BackgroundImage = Interface.Resources.GetImage ("Splash.png");
    splashForm.Opacity = 0;
    alphaTimer = new Timer ();
    alphaTimer.Interval = 50;
    alphaTimer.Tick += alphaTimer_Tick;
    alphaTimer.Enabled = true;
    closeTimer = new Timer ();
    closeTimer.Interval = 1000;
    closeTimer.Tick += closeTimer_Tick;
    Application.Run (splashForm);*/
    Application.Run (new MainForm ());
  }
  static void alphaTimer_Tick (object o, EventArgs e) {
    splashForm.Opacity += 0.1;
    if (splashForm.Opacity == 1) {
      alphaTimer.Enabled = false;
      closeTimer.Enabled = true;
    }
  }
  static void closeTimer_Tick (object o, EventArgs e) {
    splashForm.Close ();
  }
}

