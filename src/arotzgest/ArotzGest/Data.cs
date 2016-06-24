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

