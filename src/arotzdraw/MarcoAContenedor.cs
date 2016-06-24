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
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
namespace ArotzDraw {
    class MarcoAContenedorForm : Form {
        Button BotonAceptar, BotonCancelar;
        DesignerMockup DesignerParentForm;
        TextBox AnchoMarco;

        TextBox TexturaInterior, TexturaMarco;
        Button BotonSeleccionarTexturaInterior, BotonSeleccionarTexturaMarco;

        OpenFileDialog SeleccionarTextura;

        ArotzDraw.arElement OriginalElement;
        public MarcoAContenedorForm (ArotzDraw.arElement selectedelement, DesignerMockup pf) {
            if (selectedelement==null) {
                MessageBox.Show ("Debe de Seleccionar un elemento sobre el que crear los Paneles y Japonesas");
                Dispose ();
                return;
            }
            DesignerParentForm=pf;
            WindowState=FormWindowState.Maximized;
            FormBorderStyle=FormBorderStyle.SizableToolWindow;
            BackColor=Color.White;
            Text="Marco a Contenedor";
            Resize+=new EventHandler (MarcosyJaponesasForm_Resize);
            Icon=new System.Drawing.Icon (Interface.icon ("nueva_ventana"));
            BotonAceptar=Interface.create_button (10, 210, "Aceptar", aceptar_click);
            BotonCancelar=Interface.create_button (90, 210, "Cancelar", cancelar_click);
            Controls.Add (BotonAceptar);
            Controls.Add (BotonCancelar);
            Controls.Add (Interface.create_title (10, 10, "Marco a Contenedor"));
            ArotzDraw.arElement e=new ArotzDraw.arElement (0, 0);
            OriginalElement=selectedelement;
            e.Depth=selectedelement.Depth;
            e.Width=selectedelement.Width;
            e.Height=selectedelement.Height;
            e.Textura=selectedelement.Textura;
            e.Medidas=selectedelement.Medidas;
            e.Primitiva=selectedelement.Primitiva;
            e.Accesorio=selectedelement.Accesorio;
            e.allowEvents=false;
            e.Post_Construct ();
            e.Left=500;
            e.Top=0;
            e.Visible=true;
            Controls.Add (e);

            AnchoMarco=Interface.create_textbox (140, 56, 100);
            AnchoMarco.Text=10.ToString ();
            AnchoMarco.TextAlign=HorizontalAlignment.Right;

            TexturaInterior=Interface.create_textbox (140, 80, 240);
            TexturaMarco=Interface.create_textbox (140, 104, 240);

            TexturaInterior.Text=selectedelement.Textura;
            TexturaMarco.Text=selectedelement.Textura;

            BotonSeleccionarTexturaInterior=Interface.create_button (390, 78, "Examinar", BotonSeleccionarTexturaInterior_Click);
            BotonSeleccionarTexturaMarco=Interface.create_button (390, 102, "Examinar", BotonSeleccionarTexturaMarco_Click);

            TexturaInterior.TextChanged+=new EventHandler (TextChanged);
            TexturaMarco.TextChanged+=new EventHandler (TextChanged);
            AnchoMarco.TextChanged+=new EventHandler (AnchoMarco_TextChanged);

            Controls.Add (Interface.create_label (10, 58, "Ancho del Marco"));
            Controls.Add (Interface.create_label (10, 82, "Textura del Panel"));
            Controls.Add (Interface.create_label (10, 106, "Textura del Marco"));

            Controls.Add (AnchoMarco);
            Controls.Add (TexturaMarco);
            Controls.Add (TexturaInterior);
            Controls.Add (BotonSeleccionarTexturaInterior);
            Controls.Add (BotonSeleccionarTexturaMarco);
            UpdatePreview ();
            Show ();
        }

        void AnchoMarco_TextChanged (object sender, EventArgs e) {
            UpdatePreview ();
        }
        void MarcosyJaponesasForm_Resize (object sender, EventArgs e) {
            BotonAceptar.Top=Height-64;
            BotonCancelar.Top=Height-64;
        }

        public void aceptar_click (object o, EventArgs er) {
            try {
                DesignerParentForm.Controls.Remove (OriginalElement);

                ArotzDraw.arElement marco_arriba=new ArotzDraw.arElement (0, 0);
                ArotzDraw.arElement marco_abajo=new ArotzDraw.arElement (0, 0);
                ArotzDraw.arElement marco_izq=new ArotzDraw.arElement (0, 0);
                ArotzDraw.arElement marco_dcha=new ArotzDraw.arElement (0, 0);
                ArotzDraw.arElement contenedor=new ArotzDraw.arElement (0, 0);

                int ancho=int.Parse (AnchoMarco.Text);


                marco_arriba.Top=OriginalElement.Top;
                marco_arriba.Left=OriginalElement.Left;
                marco_arriba.Width=OriginalElement.Width;
                marco_arriba.Height=ancho;
                marco_arriba.Post_Construct ();
                marco_arriba.Textura=TexturaMarco.Text;


                marco_arriba.Depth=OriginalElement.Depth;
                marco_arriba.MouseDown+=new MouseEventHandler (DesignerParentForm.ArElementSelectedEvent);
                marco_arriba.hdivev+=new ArotzDraw.HDivEventHandler (DesignerParentForm.ArElementHorizontalDivisionClickEvent);
                marco_arriba.vdivev+=new ArotzDraw.VDivEventHandler (DesignerParentForm.ArElementVerticalDivisionClickEvent);

                marco_izq.Left=OriginalElement.Left;
                marco_izq.Top=OriginalElement.Top+ancho;
                marco_izq.Height=OriginalElement.Height-ancho-ancho;
                marco_izq.Width=ancho;
                marco_izq.Post_Construct ();
                marco_izq.Textura=TexturaMarco.Text;


                marco_izq.Depth=OriginalElement.Depth;
                marco_izq.MouseDown+=new MouseEventHandler (DesignerParentForm.ArElementSelectedEvent);
                marco_izq.hdivev+=new ArotzDraw.HDivEventHandler (DesignerParentForm.ArElementHorizontalDivisionClickEvent);
                marco_izq.vdivev+=new ArotzDraw.VDivEventHandler (DesignerParentForm.ArElementVerticalDivisionClickEvent);

                marco_dcha.Top=OriginalElement.Top+ancho;
                marco_dcha.Height=OriginalElement.Height-ancho-ancho;
                marco_dcha.Width=ancho;
                marco_dcha.Left=OriginalElement.Left+OriginalElement.Width-ancho;
                marco_dcha.Post_Construct ();

                marco_dcha.Depth=OriginalElement.Depth;
                marco_dcha.MouseDown+=new MouseEventHandler (DesignerParentForm.ArElementSelectedEvent);
                marco_dcha.hdivev+=new ArotzDraw.HDivEventHandler (DesignerParentForm.ArElementHorizontalDivisionClickEvent);
                marco_dcha.vdivev+=new ArotzDraw.VDivEventHandler (DesignerParentForm.ArElementVerticalDivisionClickEvent);

                marco_dcha.Textura=TexturaMarco.Text;
                marco_abajo.Top=OriginalElement.Top+OriginalElement.Height-ancho;
                marco_abajo.Height=ancho;
                marco_abajo.Left=OriginalElement.Left;
                marco_abajo.Width=OriginalElement.Width;
                marco_abajo.Post_Construct ();
                marco_abajo.Textura=TexturaMarco.Text;


                marco_abajo.Depth=OriginalElement.Depth;
                marco_abajo.MouseDown+=new MouseEventHandler (DesignerParentForm.ArElementSelectedEvent);
                marco_abajo.hdivev+=new ArotzDraw.HDivEventHandler (DesignerParentForm.ArElementHorizontalDivisionClickEvent);
                marco_abajo.vdivev+=new ArotzDraw.VDivEventHandler (DesignerParentForm.ArElementVerticalDivisionClickEvent);

                contenedor.Top=OriginalElement.Top+ancho;
                contenedor.Height=OriginalElement.Height-ancho-ancho;
                contenedor.Left=OriginalElement.Left+ancho;
                contenedor.Width=OriginalElement.Width-ancho-ancho;
                contenedor.Post_Construct ();
                contenedor.Textura=TexturaInterior.Text;


                contenedor.Depth=OriginalElement.Depth;
                contenedor.MouseDown+=new MouseEventHandler (DesignerParentForm.ArElementSelectedEvent);
                contenedor.hdivev+=new ArotzDraw.HDivEventHandler (DesignerParentForm.ArElementHorizontalDivisionClickEvent);
                contenedor.vdivev+=new ArotzDraw.VDivEventHandler (DesignerParentForm.ArElementVerticalDivisionClickEvent);

                marco_izq.Cotas=marco_dcha.Cotas=marco_arriba.Cotas=marco_abajo.Cotas=false;

                DesignerParentForm.Controls.Add (marco_arriba);
                DesignerParentForm.Controls.Add (marco_abajo);
                DesignerParentForm.Controls.Add (marco_izq);
                DesignerParentForm.Controls.Add (marco_dcha);
                DesignerParentForm.Controls.Add (contenedor);
                Close ();
                DesignerParentForm.MoreControlsAdded=true;
            }
            catch (Exception e) {

            }

        }
        public void cancelar_click (object o, EventArgs e) {
            Close ();
        }
        public void UpdatePreview () {
            try {
                for (int i=0; i<Controls.Count; i++) {
                    ArotzDraw.arElement el=Controls[i] as ArotzDraw.arElement;
                    if (el!=null) {
                        Controls.Remove (el);
                        i--;
                    }
                }

                ArotzDraw.arElement marco_arriba=new ArotzDraw.arElement (0, 0);
                ArotzDraw.arElement marco_abajo=new ArotzDraw.arElement (0, 0);
                ArotzDraw.arElement marco_izq=new ArotzDraw.arElement (0, 0);
                ArotzDraw.arElement marco_dcha=new ArotzDraw.arElement (0, 0);
                ArotzDraw.arElement contenedor=new ArotzDraw.arElement (0, 0);

                int ancho=int.Parse (AnchoMarco.Text);


                marco_arriba.Top=0;
                marco_arriba.Left=0;
                marco_arriba.Width=OriginalElement.Width;
                marco_arriba.Height=ancho;
                marco_arriba.Post_Construct ();
                marco_arriba.Textura=TexturaMarco.Text;
                marco_izq.Top=ancho;
                marco_izq.Height=OriginalElement.Height-ancho-ancho;
                marco_izq.Width=ancho;
                marco_izq.Post_Construct ();
                marco_izq.Textura=TexturaMarco.Text;
                marco_dcha.Top=ancho;
                marco_dcha.Height=OriginalElement.Height-ancho-ancho;
                marco_dcha.Width=ancho;
                marco_dcha.Left=OriginalElement.Width-ancho;
                marco_dcha.Post_Construct ();

                marco_dcha.Textura=TexturaMarco.Text;
                marco_abajo.Top=OriginalElement.Height-ancho;
                marco_abajo.Height=ancho;
                marco_abajo.Left=0;
                marco_abajo.Width=OriginalElement.Width;
                marco_abajo.Post_Construct ();
                marco_abajo.Textura=TexturaMarco.Text;

                contenedor.Top=ancho;
                contenedor.Height=OriginalElement.Height-ancho-ancho;
                contenedor.Left=ancho;
                contenedor.Width=OriginalElement.Width-ancho-ancho;
                contenedor.Post_Construct ();
                contenedor.Textura=TexturaInterior.Text;

                Controls.Add (marco_arriba);
                Controls.Add (marco_abajo);
                Controls.Add (marco_izq);
                Controls.Add (marco_dcha);
                Controls.Add (contenedor);
                marco_arriba.Left+=500;
                marco_abajo.Left+=500;
                marco_izq.Left+=500;
                marco_dcha.Left+=500;
                contenedor.Left+=500;

            }
            catch (Exception e) {

            }
        }

        void TextChanged (object sender, EventArgs e) {
            UpdatePreview ();
        }

        public void BotonSeleccionarTexturaInterior_Click (object o, EventArgs e) {
            SeleccionarTextura=new OpenFileDialog ();
            SeleccionarTextura.ShowDialog ();
            TexturaInterior.Text=SeleccionarTextura.FileName;
            TexturaInterior.Enabled=false;
        }
        public void BotonSeleccionarTexturaMarco_Click (object o, EventArgs e) {
            SeleccionarTextura=new OpenFileDialog ();
            SeleccionarTextura.ShowDialog ();
            TexturaMarco.Text=SeleccionarTextura.FileName;
            TexturaMarco.Enabled=false;
        }

    }
}