using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
namespace ArotzDraw {
    class BaldasHorizontalesForm : Form {
        TextBox Cantidad, AnchuraMarcos, TexturaInterior, TexturaMarco;
        Button BotonSeleccionarTexturaInterior, BotonSeleccionarTexturaMarco;
        Label AnchoMarco, AlturaMarco;
        OpenFileDialog SeleccionarTextura;
        Button BotonAceptar, BotonCancelar;
        ArrayList PreviewElements;
        DesignerMockup DesignerParentForm;
        ArotzDraw.arElement OriginalElement;
        Label NotEqualAlert;
        public BaldasHorizontalesForm (ArotzDraw.arElement selectedelement, DesignerMockup pf) {
            if (selectedelement==null) {
                MessageBox.Show ("Debe de Seleccionar un elemento sobre el que crear los Paneles y Japonesas");
                Dispose ();
                return;
            }
            DesignerParentForm=pf;
            WindowState=FormWindowState.Maximized;
            FormBorderStyle=FormBorderStyle.SizableToolWindow;
            BackColor=Color.White;
            Text="Paneles y Japonesas";
            Resize+=new EventHandler (BaldasHorizontalesForm_Resize);
            Icon=new System.Drawing.Icon (Interface.icon ("nueva_ventana"));
            Cantidad=Interface.create_textbox (140, 104, 40);
            Cantidad.Text=3.ToString ();
            Cantidad.TextAlign=HorizontalAlignment.Right;
            AnchuraMarcos=Interface.create_textbox (140, 128, 40);
            AnchuraMarcos.Text=10.ToString ();
            AnchuraMarcos.TextAlign=HorizontalAlignment.Right;
            TexturaMarco=Interface.create_textbox (140, 188, 240);
            TexturaMarco.Text=selectedelement.Textura;
            BotonSeleccionarTexturaInterior=Interface.create_button (390, 152, "Examinar", BotonSeleccionarTexturaInterior_Click);
            BotonSeleccionarTexturaMarco=Interface.create_button (390, 186, "Examinar", BotonSeleccionarTexturaMarco_Click);
            AnchoMarco=Interface.create_label (140, 56, selectedelement.Width.ToString ());
            AlturaMarco=Interface.create_label (140, 80, selectedelement.Height.ToString ());
            BotonAceptar=Interface.create_button (10, 210, "Aceptar", aceptar_click);
            BotonCancelar=Interface.create_button (90, 210, "Cancelar", cancelar_click);
            Cantidad.TextChanged+=new EventHandler (Cantidad_TextChanged);
            AnchuraMarcos.TextChanged+=new EventHandler (Cantidad_TextChanged);
            TexturaMarco.TextChanged+=new EventHandler (Cantidad_TextChanged);
            Controls.Add (BotonAceptar);
            Controls.Add (BotonCancelar);
            Controls.Add (Interface.create_title (10, 10, "Definir Baldas Horizontales"));
            Controls.Add (Interface.create_label (10, 56, "Ancho del Marco"));
            Controls.Add (Interface.create_label (10, 80, "Altura del Marco"));
            Controls.Add (Interface.create_label (10, 104, "Nº de Divisiones"));
            Controls.Add (Interface.create_label (10, 128, "Anchura Baldas"));

            Controls.Add (Interface.create_label (10, 188, "Textura del Marco"));
            PreviewElements=new ArrayList ();
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
            Controls.Add (Cantidad);
            Controls.Add (AnchuraMarcos);
            Controls.Add (TexturaMarco);
            Controls.Add (AnchoMarco);
            Controls.Add (AlturaMarco);
            Controls.Add (BotonSeleccionarTexturaMarco);
            NotEqualAlert=Interface.create_label (120, 400, "El resultado no es exacto");
            NotEqualAlert.ForeColor=Color.Red;
            NotEqualAlert.Width=280;
            NotEqualAlert.Height=32;
            NotEqualAlert.Font=new Font ("Tahoma", 16);
            NotEqualAlert.Visible=false;
            Controls.Add (NotEqualAlert);
            Show ();
            UpdatePreview ();
        }
        void BaldasHorizontalesForm_Resize (object sender, EventArgs e) {
            BotonAceptar.Top=Height-64;
            BotonCancelar.Top=Height-64;
        }
        void Cantidad_TextChanged (object sender, EventArgs e) {
            UpdatePreview ();
        }
        public void aceptar_click (object o, EventArgs er) {
            try {
                DesignerParentForm.Controls.Remove (OriginalElement);
                int numpaneles=int.Parse (Cantidad.Text);
                int Anchura=int.Parse (AnchuraMarcos.Text);
                int altura_panel=(OriginalElement.Height/numpaneles)+(int)(Anchura/numpaneles);
                ArotzDraw.arElement e;

                for (int i=1; i<numpaneles; i++) {
                    e=new ArotzDraw.arElement (0, 0);
                    e.Width=OriginalElement.Width;
                    e.Height=Anchura;
                    e.Top=OriginalElement.Top+(i*altura_panel)-Anchura;
                    e.Left=OriginalElement.Left;
                    e.allowEvents=false;
                    e.Depth=OriginalElement.Depth;
                    e.Distancia=OriginalElement.Distancia;
                    e.Post_Construct ();
                    e.Textura=TexturaMarco.Text;
                    e.Cotas=false;
                    DesignerParentForm.Controls.Add (e);
                    e.MouseDown+=new MouseEventHandler (DesignerParentForm.ArElementSelectedEvent);
                    e.hdivev+=new ArotzDraw.HDivEventHandler (DesignerParentForm.ArElementHorizontalDivisionClickEvent);
                    e.vdivev+=new ArotzDraw.VDivEventHandler (DesignerParentForm.ArElementVerticalDivisionClickEvent);

                }
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

                int numpaneles=int.Parse (Cantidad.Text);
                int japonesas=int.Parse (AnchuraMarcos.Text);
                int altura_panel=(int)(OriginalElement.Height/numpaneles)+(japonesas/numpaneles);
                NotEqualAlert.Visible=false;
                if (((altura_panel*numpaneles)-japonesas)<OriginalElement.Height-2||((altura_panel*numpaneles)-japonesas)>OriginalElement.Height+2) {
                    NotEqualAlert.Visible=true;
                }
                ArotzDraw.arElement e;


                for (int i=1; i<numpaneles; i++) {
                    e=new ArotzDraw.arElement (0, 0);
                    e.Width=OriginalElement.Width;
                    e.Height=japonesas;
                    e.Top=(i*altura_panel)-japonesas;
                    e.Left=500;
                    e.allowEvents=false;
                    e.Post_Construct ();
                    e.Textura=TexturaMarco.Text;
                    e.Cotas=false;
                    Controls.Add (e);
                }

            }
            catch (Exception e) {

            }

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