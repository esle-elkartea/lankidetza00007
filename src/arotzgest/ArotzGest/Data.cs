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
using System.Text.RegularExpressions;

static class Data {
  public static string Date_Format (DateTime value) {
    return value.ToString ("dd-MM-yyyy");
  }
  public static DateTime? Date_Parse (string value) {
    try { return DateTime.Parse (value); }
    catch { return null; }
  }
  public static string Double_Format (double value) {
    return value.ToString ("0.00");
  }
  public static double? Double_Parse (string value) {
    try { return Math.Round (double.Parse (value), 2); }
    catch { return null; }
  }
  public static int? Int_Parse (string value) {
    try { return int.Parse (value); }
    catch { return null; }
  }
  public static string Quantity_Format (double value) {
    return value == (int) value ? value.ToString () : value.ToString ("0.00");
  }
}

