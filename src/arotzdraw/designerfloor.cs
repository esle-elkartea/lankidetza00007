/*  ArotzDraw
 *  (c) 2006 Webalianza T.I S.L.
 *  Licenciado bajo la GNU General Public License
 *  
 *  Programación: Luis Martín-Santos García
 * 
 *  Este fichero forma parte de Arotzdraw.
 *
 * Arotzdraw es software libre; puede redistribuirlo y/o modificarlo
 * bajo los términos de la GNU General Public License, tal y como se haya
 * publicada por la Free Software Foundation, en la versión 2 de la licencia o
 * (según su elección) cualquier versión posterior.
 * 
 * Arotzdraw es redistribuido con la intención que sea útil, pero SIN NINGUNA
 * GARANTÍA, ni tan solo las garantías implícitas de MERCANTABILIDA o ADECUACIÓN 
 * A UN DETERMINADO MOTIVO. Lea la GNU General Public License para más detalles.
 * 
 * Debería haber recibido una copia de la GNU General Public License acompañando a 
 * Arotzdraw.
 * 
 * ÉSTE PROYECTO HA SIDO SUBVENCIONADO POR SPRI S.A. DENTRO DEL MARCO DEL PROGRAMA
 * KZ LANKIDETZA - más información en http://www.spri.es
 * 
 *
 * */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Drawing.Printing;

namespace ArotzDraw {
    class FloorElement : Control {
        ArrayList Elementos;
        private Font CotasFuente;
        public FloorElement (ArrayList e) {
            Elementos=e;
            SetStyle (ControlStyles.SupportsTransparentBackColor, true);
            BackColor=Color.Transparent;
            Location=new Point (0, 0);
            CotasFuente=new Font ("Tahoma", 7, FontStyle.Bold);
        }

        protected override void OnPaint (PaintEventArgs args) {
            base.OnPaint (args);
            Graphics gfx=args.Graphics;
            gfx.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfx.CompositingQuality=System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            Pen DrawingPen=new Pen (Color.Black);
            DrawingPen.Width=2;
            int Cdist=15;
            float dx;
            int counter=1;
            foreach (ArotzDraw.arElement e in Elementos) {
                DrawingPen.StartCap=LineCap.Flat;
                DrawingPen.EndCap=LineCap.Flat;
                gfx.DrawLine (DrawingPen, e.Left, e.Distancia, e.Left, e.Distancia+e.Depth); // | i
                gfx.DrawLine (DrawingPen, e.Left, e.Distancia, e.Left+e.Width, e.Distancia); // - a
                gfx.DrawLine (DrawingPen, e.Left+e.Width, e.Distancia, e.Left+e.Width, e.Distancia+e.Depth); // | d
                gfx.DrawLine (DrawingPen, e.Left, e.Distancia+e.Depth, e.Left+e.Width, e.Distancia+e.Depth); // - ab
                if (e.TipoPuerta=="Batiente") {
                    if (e.HojasPuerta==1) {
                        gfx.DrawLine (DrawingPen, e.Left, e.Distancia+e.Depth, e.Left+(int)(e.Width/2), e.Distancia+e.Depth+(int)(e.Depth/2));
                        DrawingPen.Width=1;
                        DrawingPen.DashStyle=DashStyle.Dash;
                        gfx.DrawArc (DrawingPen, new Rectangle (e.Left, (int)(e.Distancia+(e.Depth/2)), e.Width, (int)e.Depth), 0, 90);
                        DrawingPen.Width=2;
                        DrawingPen.DashStyle=DashStyle.Solid;
                    }
                    else {
                        gfx.DrawLine (DrawingPen, e.Left, e.Distancia+e.Depth, e.Left+(int)(e.Width/4), e.Distancia+e.Depth+(int)(e.Depth/2));
                        gfx.DrawLine (DrawingPen, e.Left+e.Width, e.Distancia+e.Depth, e.Left+(int)(e.Width/2)+(int)(e.Width/4), e.Distancia+e.Depth+(int)(e.Depth/2));
                        DrawingPen.Width=1;
                        DrawingPen.DashStyle=DashStyle.Dash;
                        gfx.DrawArc (DrawingPen, new Rectangle (e.Left, (int)(e.Distancia+(e.Depth/2)), (int)e.Width/2, (int)e.Depth), 0, 90);
                        gfx.DrawArc (DrawingPen, new Rectangle (e.Left+e.Width/2, (int)(e.Distancia+(e.Depth/2)), (int)e.Width/2, (int)e.Depth), 90, 90);
                        DrawingPen.Width=2;
                        DrawingPen.DashStyle=DashStyle.Solid;
                    }
                }
                if (e.TipoPuerta=="Corredera") {
                    DrawingPen.Width=1;
                    DrawingPen.DashStyle=DashStyle.Dash;

                    switch (e.HojasPuerta) {
                        case 2:
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left, (int)(e.Distancia+e.Depth), (int)e.Width/2, 5));
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left+(int)e.Width/2, (int)(e.Distancia+e.Depth+12), (int)e.Width/2, 5));
                            break;

                        case 3:
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left, (int)(e.Distancia+e.Depth), (int)e.Width/3, 5));
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left+(int)e.Width/3, (int)(e.Distancia+e.Depth+12), (int)e.Width/3, 5));
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left+2*((int)e.Width/3), (int)(e.Distancia+e.Depth), (int)e.Width/3, 5));

                            break;

                        case 4:
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left, (int)(e.Distancia+e.Depth), (int)e.Width/4, 5));
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left+(int)e.Width/4, (int)(e.Distancia+e.Depth+12), (int)e.Width/4, 5));
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left+2*((int)e.Width/4), (int)(e.Distancia+e.Depth+12), (int)e.Width/4, 5));
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left+3*((int)e.Width/4), (int)(e.Distancia+e.Depth), (int)e.Width/4, 5));

                            break;

                        case 5:
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left, (int)(e.Distancia+e.Depth), (int)e.Width/5, 5));
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left+(int)e.Width/5, (int)(e.Distancia+e.Depth+12), (int)e.Width/5, 5));
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left+2*((int)e.Width/5), (int)(e.Distancia+e.Depth), (int)e.Width/5, 5));
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left+3*((int)e.Width/5), (int)(e.Distancia+e.Depth+12), (int)e.Width/5, 5));
                            gfx.DrawRectangle (DrawingPen, new Rectangle (e.Left+4*((int)e.Width/5), (int)(e.Distancia+e.Depth), (int)e.Width/5, 5));

                            break;
                    }
                    DrawingPen.Width=2;
                    DrawingPen.DashStyle=DashStyle.Solid;
                }

                if (e.TipoPuerta=="Plegable") {
                    DrawingPen.Width=1;
                    DrawingPen.DashStyle=DashStyle.Dash;
                    dx=e.Width/e.HojasPuerta;
                    DrawingPen.Width=3;
                    DrawingPen.DashStyle=DashStyle.Solid;
                    for (int k=0; k<e.Width-dx; k+=(int)dx) {
                        counter++;
                        if (counter%2==0) {
                            gfx.DrawLine (DrawingPen, k-5, e.distance+e.Depth, k+dx-5, e.distance+e.Depth+15);
                        }
                        else {
                            gfx.DrawLine (DrawingPen, k-5, e.distance+e.Depth+15, k+dx-5, e.distance+e.Depth);
                        }
                    }
                    DrawingPen.EndCap=LineCap.ArrowAnchor;
                    gfx.DrawLine (DrawingPen, e.Left+e.Width, e.Depth+e.distance+8, e.Left+e.Width-20, e.distance+e.Depth+8);
                    DrawingPen.EndCap=LineCap.Flat;
                }

                if (e.TipoPuerta=="Plegable-Corredera") {
                    DrawingPen.Width=1;
                    DrawingPen.DashStyle=DashStyle.Dash;
                    dx=e.Width/e.HojasPuerta;
                    dx=dx-4;
                    DrawingPen.Width=3;
                    DrawingPen.DashStyle=DashStyle.Solid;
                    for (int k=20; k<e.Width-20; k+=(int)dx) {
                        counter++;
                        if (counter%2==0) {
                            gfx.DrawLine (DrawingPen, k-5, e.distance+e.Depth, k+dx-5, e.distance+e.Depth+15);
                        }
                        else {
                            gfx.DrawLine (DrawingPen, k-5, e.distance+e.Depth+15, k+dx-5, e.distance+e.Depth);
                        }
                    }
                    DrawingPen.EndCap=LineCap.ArrowAnchor;
                    gfx.DrawLine (DrawingPen, e.Left+e.Width, e.Depth+e.distance+8, e.Left+e.Width-20, e.distance+e.Depth+8);
                    gfx.DrawLine (DrawingPen, e.Left+2, e.Depth+e.distance+8, e.Left+20, e.distance+e.Depth+8);
                    DrawingPen.EndCap=LineCap.Flat;
                }


                /*  if (e.Cotas)
                  {
                      DrawingPen.Width = 2;
                      DrawingPen.StartCap = LineCap.ArrowAnchor;
                      DrawingPen.EndCap = LineCap.Square;
                      gfx.DrawLine(DrawingPen, e.Left, e.distance + Cdist, (int)e.Left + e.Width / 2 - (e.Width.ToString().Length * 6), e.distance + Cdist);
                      DrawingPen.StartCap = LineCap.Square;
                      DrawingPen.EndCap = LineCap.ArrowAnchor;
                      gfx.DrawLine(DrawingPen, (int)e.Left + e.Width / 2 + (e.Width.ToString().Length * 9), e.distance + Cdist, e.Left + e.Width, e.distance + Cdist);
                      gfx.DrawString(e.Width.ToString() + " " + e.Medidas, CotasFuente, Brushes.Black, new PointF(e.Left - (int)(e.Width.ToString().Length * 5) + (int)(e.Width / 2), e.distance + Cdist - 5));
                      DrawingPen.StartCap = LineCap.ArrowAnchor;
                      DrawingPen.EndCap = LineCap.Square;
                      gfx.DrawLine(DrawingPen, e.Left + Cdist, e.distance, e.Left + Cdist, (int)e.distance + e.Depth / 2 - 8);
                      DrawingPen.StartCap = LineCap.Square;
                      DrawingPen.EndCap = LineCap.ArrowAnchor;
                      gfx.DrawLine(DrawingPen, (int)e.Left + Cdist, (int)e.distance + e.Depth / 2 + 8, e.Left + Cdist, e.distance + e.Depth);
                      gfx.DrawString(e.Depth.ToString() + " " + e.Medidas, CotasFuente, Brushes.Black, new PointF(e.Left + Cdist - 15, e.distance + e.Depth / 2 - 5));
                      DrawingPen.Width = 2;
                      Cdist += 10;
                  }*/
            }
        }
    }

    class designerfloor : Form {
        Button Imprimir;
        FloorElement floorelement;
        public designerfloor (ArrayList Elements) {
            FormBorderStyle=FormBorderStyle.SizableToolWindow;
            Text="Planta del Dibujo";
            BackColor=Color.White;
            WindowState=FormWindowState.Maximized;
            floorelement=new FloorElement (Elements);
            floorelement.Width=Width;
            floorelement.Height=Height;
            Controls.Add (floorelement);
            Imprimir=new Button ();
            Imprimir.Text="Imprimir";
            Imprimir.Click+=new EventHandler (Imprimir_Click);
            Controls.Add (Imprimir);
            Resize+=new EventHandler (designerfloor_Resize);
            Width=600;
            Height=600;
            Imprimir.BringToFront ();

        }


        protected override void OnResize (EventArgs e) {
            base.OnResize (e);
            floorelement.Width=Width;
            floorelement.Height=Height;
            Imprimir.Location=new Point (ClientSize.Width-80, ClientSize.Height-24);
        }
        void designerfloor_Resize (object sender, EventArgs e) {
            Imprimir.Location=new Point (Width-80, Height-24);
        }


        PrintDocument Print;
        Image PrintImage;
        public void Imprimir_Click (object o, EventArgs e) {
            Imprimir.Visible=false;
            Print=new PrintDocument ();
            PrintDialog SWFPrintDialog=new PrintDialog ();
            SWFPrintDialog.Document=Print;
            DialogResult result=SWFPrintDialog.ShowDialog ();
            if (result==DialogResult.OK) {
                Print.PrintPage+=new PrintPageEventHandler (Print_PrintPage);
                Graphics graphic=this.CreateGraphics ();
                Size s=this.Size;
                PrintImage=new Bitmap (s.Width, s.Height, graphic);
                Graphics memGraphic=Graphics.FromImage (PrintImage);
                IntPtr dc1=graphic.GetHdc ();
                IntPtr dc2=memGraphic.GetHdc ();
                NativeMethods.BitBlt (dc2, 0, 0, Width, Height, dc1, 20, 20, 13369376);
                graphic.ReleaseHdc (dc1);
                memGraphic.ReleaseHdc (dc2);
                Print.Print ();
            }
            Imprimir.Visible=true;
        }
        private void Print_PrintPage (object sender, System.Drawing.Printing.PrintPageEventArgs e) {
            e.Graphics.DrawImage (PrintImage, 0, 0);
        }


    }
}
