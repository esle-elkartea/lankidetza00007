/*  ArotzPrint
 *  (c) 2006 Webalianza T.I S.L.
 *  Licenciado bajo la GNU General Public License
 *  
 *  Programación: Jorge Moreiro
 *                Iker Estébanez
 * 
 *  Este fichero forma parte de ArotzPrint
 *
 * ArotzPrint es software libre; puede redistribuirlo y/o modificarlo
 * bajo los términos de la GNU General Public License, tal y como se haya
 * publicada por la Free Software Foundation, en la versión 2 de la licencia o
 * (según su elección) cualquier versión posterior.
 * 
 * ArotzPrint es redistribuido con la intención que sea útil, pero SIN NINGUNA
 * GARANTÍA, ni tan solo las garantías implícitas de MERCANTABILIDA o ADECUACIÓN 
 * A UN DETERMINADO MOTIVO. Lea la GNU General Public License para más detalles.
 * 
 * Debería haber recibido una copia de la GNU General Public License acompañando a 
 * ArotzPrint.
 * 
 * ÉSTE PROYECTO HA SIDO SUBVENCIONADO POR SPRI S.A. DENTRO DEL MARCO DEL PROGRAMA
 * KZ LANKIDETZA - más información en http://www.spri.es
 * 
 *
 * */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;

public class ArotzPrintDocument : PrintDocument {
  static string name = "ArotzPrint";
  static string version = "1.0";
  protected float scale = 1 / 0.0254f;
  protected float width = 0;
  protected float height = 0;
  protected double margin = 1.5f;
  protected void drawLine (Graphics graphics, Pen pen, double left, double top, double width, double height) {
    graphics.DrawLine (pen, (float) left * scale, (float) top * scale, (float) width * scale, (float) height * scale);
  }
  protected void drawString (Graphics graphics, double left, double top, double width, double height, Font font, string text, bool wordWrap, bool rightAlign) {
    StringFormat stringFormat = new StringFormat ();
    if (!wordWrap) stringFormat.FormatFlags = StringFormatFlags.NoWrap;
    if (rightAlign) stringFormat.Alignment = StringAlignment.Far;
    graphics.DrawString (text, font, Brushes.Black, new RectangleF ((float) left * scale, (float) top * scale, (float) width * scale, (float) height * scale), stringFormat);
  }
  protected double measureString (Graphics graphics, double width, double height, Font font, string text, bool wordWrap, bool rightAlign) {
    StringFormat stringFormat = new StringFormat ();
    if (!wordWrap) stringFormat.FormatFlags = StringFormatFlags.NoWrap;
    if (rightAlign) stringFormat.Alignment = StringAlignment.Far;
    return graphics.MeasureString (text, font, new SizeF ((float) width * scale, (float) height * scale), stringFormat).Height / scale;
  }
  protected void drawImage (Graphics graphics, double left, double top, double width, double height, Image image) {
    if (image != null) graphics.DrawImage (image, (float) left * scale, (float) top * scale, (float) width * scale, (float) height * scale);
  }
  protected string formatDouble (double value) {
    return value.ToString ("0.00");
  }
  protected string formatQuantity (double value) {
    return value == (int) value ? value.ToString () : value.ToString ("0.00");
  }
  public static string formatDate (DateTime value) {
    return value.ToString ("dd-MM-yyyy");
  }
  protected class Concept {
    public double Quantity;
    public string Text;
    public double Price;
  }
  protected class ReceiptConcept {
    public int Number;
    public double Price;
    public DateTime DueDate;
  }
}

public class BudgetPrintDocument : ArotzPrintDocument {
  int page = 0;
  int line = 0;
  public int LogoType, Number, ValidDays;
  public string ClientName, ClientDetails, CompanyName, CompanyDetails;
  public DateTime Date;
  public Image LogoImage;
  List<Concept> concepts = new List<Concept> ();
  public void addConcept (double quantity, string text, double price) {
    Concept concept = new Concept ();
    concept.Quantity = quantity;
    concept.Text = text;
    concept.Price = price;
    concepts.Add (concept);
  }
  protected override void OnPrintPage (PrintPageEventArgs e) {
    base.OnPrintPage (e);
    width = e.PageBounds.Width / scale;
    height = e.PageBounds.Height / scale;
    if (page == 0) {
      if (LogoType == 0) {
        drawString (e.Graphics, margin, margin, width - 2 * margin - 5, 0, new Font ("tahoma", 16, FontStyle.Bold), CompanyName, false, false);
        drawString (e.Graphics, margin, margin + 0.75, width - 2 * margin - 5, 0, new Font ("tahoma", 12), CompanyDetails, false, false);
      } else if (LogoType == 1) {
        drawImage (e.Graphics, margin, margin, 8, 2, LogoImage);
        drawString (e.Graphics, margin, margin + 2, width - 2 * margin - 5, 0, new Font ("tahoma", 12), CompanyDetails, false, false);
      } else if (LogoType == 2) {
        drawImage (e.Graphics, margin, margin, 8, 4, LogoImage);
      }
      drawString (e.Graphics, width - margin - 6, margin + 0.15, 3.5, 0, new Font ("tahoma", 12, FontStyle.Bold), "Presupuesto:\nFecha:", false, true);
      drawString (e.Graphics, width - margin - 2.5, margin + 0.15, 2.5, 0, new Font ("tahoma", 12), Number + "\n" + formatDate (Date), false, true);
      drawString (e.Graphics, margin, margin + 2.5, width - 2 * margin, 0, new Font ("tahoma", 16, FontStyle.Bold), ClientName, false, true);
      drawString (e.Graphics, margin, margin + 3.25, width - 2 * margin, 0, new Font ("tahoma", 12), ClientDetails, false, true);
    }
    double top = margin + (page == 0 ? 6 : 0);
    double position = top + 1;
    double bottom = height - margin;
    Font font = new Font ("tahoma", 12, FontStyle.Bold);
    Pen grayPen = new Pen (Color.Gray, 0.01f * scale);
    Pen blackPen = new Pen (Color.Black, 0.025f * scale);
    drawString (e.Graphics, margin + 0.1, top + 0.25, 2.3, 0, font, "Cantidad", false, false);
    drawString (e.Graphics, margin + 2.6, top + 0.25, width - 2 * margin - 8.7, 0, font, "Concepto", false, false);
    drawString (e.Graphics, width - margin - 5.9, top + 0.25, 2.8, 0, font, "Precio", false, false);
    drawString (e.Graphics, width - margin - 2.9, top + 0.25, 2.8, 0, font, "Importe", false, false);
    font = new Font ("tahoma", 12);
    for (; line < concepts.Count; line++) {
      double height2 = measureString (e.Graphics, width - 2 * margin - 8.7, 0, font, concepts [line].Text, true, false);
      if (position + height2 <= bottom) {
        if (position + 1 > top) drawLine (e.Graphics, grayPen, margin, position, width - margin, position);
        drawString (e.Graphics, margin + 0.1, position + 0.1, 2.3, 0, font, formatQuantity (concepts [line].Quantity), false, true);
        drawString (e.Graphics, margin + 2.6, position + 0.1, width - 2 * margin - 8.7, 0, font, concepts [line].Text, true, false);
        drawString (e.Graphics, width - margin - 5.9, position + 0.1, 2.8, 0, font, formatDouble (concepts [line].Price) + " €", false, true);
        drawString (e.Graphics, width - margin - 2.9, position + 0.1, 2.8, 0, font, formatDouble (concepts [line].Quantity * concepts [line].Price) + " €", false, true);
        position += height2 + 0.1;
      } else {
        e.HasMorePages = true;
        page++;
        break;
      }
    }
    if (line == concepts.Count) {
      if (position < bottom - 1) {
        bottom -= 1;
        drawLine (e.Graphics, grayPen, width - margin - 3, bottom, width - margin - 3, bottom + 1);
        drawLine (e.Graphics, blackPen, width - margin, bottom, width - margin, bottom + 1);
        drawLine (e.Graphics, blackPen, width - margin, bottom + 1, width - margin - 6, bottom + 1);
        drawLine (e.Graphics, blackPen, width - margin - 6, bottom + 1, width - margin - 6, bottom);
        drawString (e.Graphics, width - margin - 5.9, bottom + 0.25, 2.8, 0, font, "Total", false, false);
        double total = 0;
        foreach (Concept concept in concepts) total += concept.Quantity * concept.Price;
        drawString (e.Graphics, width - margin - 2.9, bottom + 0.25, 2.8, 0, font, formatDouble (total) + " €", false, true);
        drawString (e.Graphics, margin, bottom + 0.25, width - 2 * margin, 0, font, "IVA no incluído. Validez de " + ValidDays + " días.", false, false);
      } else {
        e.HasMorePages = true;
        page++;
      }
    }
    drawLine (e.Graphics, grayPen, margin + 2.5, top, margin + 2.5, bottom);
    drawLine (e.Graphics, grayPen, width - margin - 6, top, width - margin - 6, bottom);
    drawLine (e.Graphics, grayPen, width - margin - 3, top, width - margin - 3, bottom);
    drawLine (e.Graphics, blackPen, margin, top + 1, width - margin, top + 1);
    drawLine (e.Graphics, blackPen, margin, top, width - margin, top);
    drawLine (e.Graphics, blackPen, width - margin, top, width - margin, bottom);
    drawLine (e.Graphics, blackPen, width - margin, bottom, margin, bottom);
    drawLine (e.Graphics, blackPen, margin, bottom, margin, top);
  }
}

public class InvoicePrintDocument : ArotzPrintDocument {
  int page = 0;
  int line = 0;
  public double VAT;
  public int LogoType, Number;
  public string ClientName, ClientDetails, CompanyName, CompanyDetails;
  public DateTime Date;
  public Image LogoImage;
  List<Concept> concepts = new List<Concept> ();
  public void addConcept (double quantity, string text, double price) {
    Concept concept = new Concept ();
    concept.Quantity = quantity;
    concept.Text = text;
    concept.Price = price;
    concepts.Add (concept);
  }
  protected override void OnPrintPage (PrintPageEventArgs e) {
    base.OnPrintPage (e);
    width = e.PageBounds.Width / scale;
    height = e.PageBounds.Height / scale;
    if (page == 0) {
      if (LogoType == 0) {
        drawString (e.Graphics, margin, margin, width - 2 * margin - 5, 0, new Font ("tahoma", 16, FontStyle.Bold), CompanyName, false, false);
        drawString (e.Graphics, margin, margin + 0.75, width - 2 * margin - 5, 0, new Font ("tahoma", 12), CompanyDetails, false, false);
      } else if (LogoType == 1) {
        drawImage (e.Graphics, margin, margin, 8, 2, LogoImage);
        drawString (e.Graphics, margin, margin + 2, width - 2 * margin - 5, 0, new Font ("tahoma", 12), CompanyDetails, false, false);
      } else if (LogoType == 2) {
        drawImage (e.Graphics, margin, margin, 8, 4, LogoImage);
      }
      drawString (e.Graphics, width - margin - 6, margin + 0.15, 3.5, 0, new Font ("tahoma", 12, FontStyle.Bold), "Factura:\nFecha:", false, true);
      drawString (e.Graphics, width - margin - 2.5, margin + 0.15, 2.5, 0, new Font ("tahoma", 12), Number + "\n" + formatDate (Date), false, true);
      drawString (e.Graphics, margin, margin + 2.5, width - 2 * margin, 0, new Font ("tahoma", 16, FontStyle.Bold), ClientName, false, true);
      drawString (e.Graphics, margin, margin + 3.25, width - 2 * margin, 0, new Font ("tahoma", 12), ClientDetails, false, true);
    }
    double top = margin + (page == 0 ? 6 : 0);
    double position = top + 1;
    double bottom = height - margin;
    Font font = new Font ("tahoma", 12, FontStyle.Bold);
    Pen grayPen = new Pen (Color.Gray, 0.01f * scale);
    Pen blackPen = new Pen (Color.Black, 0.025f * scale);
    drawString (e.Graphics, margin + 0.1, top + 0.25, 2.3, 0, font, "Cantidad", false, false);
    drawString (e.Graphics, margin + 2.6, top + 0.25, width - 2 * margin - 8.7, 0, font, "Concepto", false, false);
    drawString (e.Graphics, width - margin - 5.9, top + 0.25, 2.8, 0, font, "Precio", false, false);
    drawString (e.Graphics, width - margin - 2.9, top + 0.25, 2.8, 0, font, "Importe", false, false);
    font = new Font ("tahoma", 12);
    for (; line < concepts.Count; line++) {
      double height2 = measureString (e.Graphics, width - 2 * margin - 8.7, 0, font, concepts [line].Text, true, false);
      if (position + height2 <= bottom) {
        if (position + 1 > top) drawLine (e.Graphics, grayPen, margin, position, width - margin, position);
        drawString (e.Graphics, margin + 0.1, position + 0.1, 2.3, 0, font, formatQuantity (concepts [line].Quantity), false, true);
        drawString (e.Graphics, margin + 2.6, position + 0.1, width - 2 * margin - 8.7, 0, font, concepts [line].Text, true, false);
        drawString (e.Graphics, width - margin - 5.9, position + 0.1, 2.8, 0, font, formatDouble (concepts [line].Price) + " €", false, true);
        drawString (e.Graphics, width - margin - 2.9, position + 0.1, 2.8, 0, font, formatDouble (concepts [line].Quantity * concepts [line].Price) + " €", false, true);
        position += height2 + 0.1;
      } else {
        e.HasMorePages = true;
        page++;
        break;
      }
    }
    if (line == concepts.Count) {
      if (position < bottom - 3) {
        bottom -= 3;
        drawLine (e.Graphics, grayPen, width - margin - 3, bottom, width - margin - 3, bottom + 3);
        drawLine (e.Graphics, blackPen, width - margin, bottom, width - margin, bottom + 3);
        drawLine (e.Graphics, blackPen, width - margin, bottom + 3, width - margin - 6, bottom + 3);
        drawLine (e.Graphics, blackPen, width - margin - 6, bottom + 3, width - margin - 6, bottom);
        drawString (e.Graphics, width - margin - 5.9, bottom + 0.25, 2.8, 0, font, "Subtotal", false, false);
        double subtotal = 0;
        foreach (Concept concept in concepts) subtotal += concept.Quantity * concept.Price;
        drawString (e.Graphics, width - margin - 2.9, bottom + 0.25, 2.8, 0, font, formatDouble (subtotal) + " €", false, true);
        drawLine (e.Graphics, grayPen, width - margin, bottom + 1, width - margin - 6, bottom + 1);
        drawString (e.Graphics, width - margin - 5.9, bottom + 1.25, 2.8, 0, font, "IVA " + VAT + "%", false, false);
        double vat = subtotal * VAT / 100;
        drawString (e.Graphics, width - margin - 2.9, bottom + 1.25, 2.8, 0, font, formatDouble (vat) + " €", false, true);
        drawLine (e.Graphics, grayPen, width - margin, bottom + 2, width - margin - 6, bottom + 2);
        drawString (e.Graphics, width - margin - 5.9, bottom + 2.25, 2.8, 0, font, "Total", false, false);
        double total = subtotal + vat;
        drawString (e.Graphics, width - margin - 2.9, bottom + 2.25, 2.8, 0, font, formatDouble (total) + " €", false, true);
      } else {
        e.HasMorePages = true;
        page++;
      }
    }
    drawLine (e.Graphics, grayPen, margin + 2.5, top, margin + 2.5, bottom);
    drawLine (e.Graphics, grayPen, width - margin - 6, top, width - margin - 6, bottom);
    drawLine (e.Graphics, grayPen, width - margin - 3, top, width - margin - 3, bottom);
    drawLine (e.Graphics, blackPen, margin, top + 1, width - margin, top + 1);
    drawLine (e.Graphics, blackPen, margin, top, width - margin, top);
    drawLine (e.Graphics, blackPen, width - margin, top, width - margin, bottom);
    drawLine (e.Graphics, blackPen, width - margin, bottom, margin, bottom);
    drawLine (e.Graphics, blackPen, margin, bottom, margin, top);
  }
}

public class ReceiptPrintDocument : ArotzPrintDocument {
  public int LogoType, Number;
  public string ClientName, ClientDetails, CompanyName, CompanyDetails;
  public Image LogoImage;
  public DateTime Date;
  List<ReceiptConcept> concepts = new List<ReceiptConcept> ();
  public void addConcept (int number, double price, DateTime dueDate) {
    ReceiptConcept concept = new ReceiptConcept ();
    concept.Number = number;
    concept.Price = price;
    concept.DueDate = dueDate;
    concepts.Add (concept);
  }
  int page = 0;
  protected override void OnPrintPage (PrintPageEventArgs e) {
    base.OnPrintPage (e);
    ReceiptConcept concept = concepts [page];
    width = e.PageBounds.Width / scale;
    height = e.PageBounds.Height / scale;
    for (int j = 0; j < 2; j++) {
      float offset = (height / 2) * j;
      if (LogoType == 0) {
        drawString (e.Graphics, margin, offset + margin, width - 2 * margin - 5, 0, new Font ("tahoma", 16, FontStyle.Bold), CompanyName, false, false);
        drawString (e.Graphics, margin, offset + margin + 0.75, width - 2 * margin - 5, 0, new Font ("tahoma", 12), CompanyDetails, false, false);
      } else if (LogoType == 1) {
        drawImage (e.Graphics, margin, offset + margin, 8, 2, LogoImage);
        drawString (e.Graphics, margin, offset + margin + 2, width - 2 * margin - 5, 0, new Font ("tahoma", 12), CompanyDetails, false, false);
      } else if (LogoType == 2) {
        drawImage (e.Graphics, margin, offset + margin, 8, 4, LogoImage);
      }
      drawString (e.Graphics, width - margin - 6, offset + margin + 0.15, 3.5, 0, new Font ("tahoma", 12, FontStyle.Bold), "Recibo:\nFecha:", false, true);
      drawString (e.Graphics, width - margin - 2.5, offset + margin + 0.15, 2.5, 0, new Font ("tahoma", 12), Number + "/" + concept.Number + "\n" + formatDate (Date), false, true);
      drawString (e.Graphics, margin, offset + margin + 2.5, width - 2 * margin, 0, new Font ("tahoma", 16, FontStyle.Bold), ClientName, false, true);
      drawString (e.Graphics, margin, offset + margin + 3.25, width - 2 * margin, 0, new Font ("tahoma", 12), ClientDetails, false, true);

      drawString (e.Graphics, width / 2 - 6, offset + margin + 8, 6, 0, new Font ("tahoma", 12, FontStyle.Bold), "Importe:\nFecha vencimiento:", false, true);
      drawString (e.Graphics, width / 2, offset + margin + 8, 0, 0, new Font ("tahoma", 12), formatDouble (concept.Price) + " €\n" + formatDate (concept.DueDate), false, false);

      drawLine (e.Graphics, new Pen (Color.Gray, 0.025f * scale), 0, height / 2, width, height / 2);
    }
    page ++;
    if (page < concepts.Count) e.HasMorePages = true;
  }
}
