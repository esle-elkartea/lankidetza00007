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
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;

namespace ArotzDraw {

    public class InputBoxDialog : System.Windows.Forms.Form {
       /*   InputBox.FormPrompt = "Altura de la División";
            InputBox.FormCaption = "División Vertical";
            InputBox.DefaultValue = "100";
            InputBox.ShowDialog();
      */
        Label Caption;
        Label SecondCaption;
        TextBox Value;
        public bool Canceled = false;
        public String InputResponse;

        public string FormPrompt {
            get {
                return SecondCaption.Text;

            }

            set {
                SecondCaption.Text = value;

            }
        }

        public string FormCaption {

            get {
                return Caption.Text;
                
            }

            set {
                Caption.Text = value;
                Text = value;
            }
        }

        public string DefaultValue {
            get {
                return Value.Text;

            }

            set {
                Value.Text = value;

            }

        }

        public InputBoxDialog () {
            Value = new TextBox();
            Caption = new Label();
            SecondCaption = new Label();
            Value = Interface.create_textbox(10, 70, 290);
            WindowState= FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
            Icon = new Icon(Application.StartupPath + "\\shared\\arotzdraw.ico");
            Caption = Interface.create_label(10, 10, "");
            Caption.Width = Width;
            Caption.Height = 30;
            Caption.Font = new Font("Tahoma", 14);
            SecondCaption = Interface.create_label(10, 40, SecondCaption.Text);
            SecondCaption.Width = Width;
            Width = 320;
            Height = 180;
            Button Aceptar = Interface.create_button(Width - 176,Height -74,"Aceptar",new EventHandler(AceptarClickEvent));
            Button Cancelar= Interface.create_button(Width - 94,Height -74,"Cancelar",new EventHandler(CancelarClickEvent));
            
            Controls.Add(Value);
            Controls.Add(SecondCaption);
            Controls.Add(Caption);
            Controls.Add(Aceptar);
            Controls.Add(Cancelar);
            InputResponse = "";
            KeyDown += new KeyEventHandler(doKeyDown);
            Value.KeyDown += new KeyEventHandler(doKeyDown);
            
        }

        void doKeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter)
            {
                InputResponse = Value.Text;
                Close();
            }
        }

        public void AceptarClickEvent (object o , EventArgs e)
        {
            InputResponse = Value.Text;
            Close();

        }

        public void CancelarClickEvent (object o , EventArgs e)
        {
            Canceled = true;
            InputResponse = "";
            Close();
        }

      
            
      
        private void InitializeComponent () {
      

        }
        
      
     
    }

    class appMain {
        static AboutDialog a;
        static System.Windows.Forms.Timer t;
        static DesignerMockup d;
        [STAThread]
        public static void Main () {
            try {
                Application.EnableVisualStyles();
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
                a = new AboutDialog();
                a.Show();
                t = new System.Windows.Forms.Timer();
                t.Interval = 1800;
                t.Tick += new EventHandler(t_Tick);
                t.Start();
                d = new DesignerMockup();
                Application.Run();
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
                MessageBox.Show(e.StackTrace);

            }
        }

        static void t_Tick (object sender, EventArgs e) {
            t.Stop ();
            a.Close ();
            d.Show ();
            t.Enabled=false;
        }
    }
}
