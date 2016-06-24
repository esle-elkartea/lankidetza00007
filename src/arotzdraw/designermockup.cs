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
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Printing;
using System.Threading;
using System.Globalization;
using System.Reflection;
using System.Drawing.Design;
using ArotzDraw;

namespace ArotzDraw {
    /*
     * Ésta clase nos sirve de ayuda a la hora de serializar los ArotzDraw.arElements para
     * guardarlos a fichero. Lo que hacemos es copiar todos los ArotzDraw.arElements desde el 
     * formulario para luego serializarlos en el save.
     * Esta clase implementa ISerializable para poder ser serializada.
     * */
    public class MetricsTypeConverter : StringConverter {
        public override bool GetStandardValuesSupported (ITypeDescriptorContext context) {
            return true;
        }
        public override StandardValuesCollection GetStandardValues (ITypeDescriptorContext context) {
            return new StandardValuesCollection (new string[] { "m", "dm", "cm", "mm" });
        }
    }

    public class JPEGFileChooser : UITypeEditor {
        public override Object EditValue (System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, Object value) {
            System.Windows.Forms.OpenFileDialog openf=new System.Windows.Forms.OpenFileDialog ();
            openf.InitialDirectory=Application.StartupPath+"\\Libreria\\";
            openf.Filter="Archivos de Imagen Digital JPEG|*.jpg";
            openf.ShowReadOnly=false;
            openf.CheckFileExists=true;
            System.Windows.Forms.DialogResult r=openf.ShowDialog ();
            if (r==DialogResult.OK) return openf.FileName;
            return "";
        }
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle (System.ComponentModel.ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.Modal;
        }
    }

    public class BackgroundPropertiesObject : Control {
        float escala=1;
        public int iancho=0, ialto=0;
        public string image=Application.StartupPath+"\\Libreria\\madera3.png";
        [CategoryAttribute ("Imagen de Fondo"), DescriptionAttribute ("Textura"), ArotzDraw.viewParameter (true), Editor (typeof (JPEGFileChooser), typeof (UITypeEditor))]
        public string Imagen {
            get {
                return image;
            }

            set {
                image=value;
                DesignerMockup DesignerMockupParent=(DesignerMockup)Parent;
                DesignerMockupParent.CambiarFondo ();
            }

        }
        private String medidas="cm";
        [CategoryAttribute ("Escala"), DescriptionAttribute (""), ArotzDraw.viewParameter (true), TypeConverter (typeof (MetricsTypeConverter))]
        public String Medidas {
            get {
                return medidas;
            }

            set {
                medidas=value;
                DesignerMockup DesignerMockupParent=(DesignerMockup)Parent;
                foreach (Control c in DesignerMockupParent.Controls) {
                    ArotzDraw.arElement el=c as ArotzDraw.arElement;
                    if (el!=null) {
                        el.Medidas=value;
                        el.Refresh ();
                    }
                }
                DesignerMockupParent.Refresh ();
            }

        }
        [CategoryAttribute ("Escala"), DescriptionAttribute (""), ArotzDraw.viewParameter (true)]
        public float EscalaGlobal {
            get {
                return escala;
            }
            set {
                escala=value;
                DesignerMockup p=(DesignerMockup)Parent;
                foreach (Control c in p.Controls) {
                    ArotzDraw.arElement el=c as ArotzDraw.arElement;
                    if (el!=null) {
                        el.Escala=value;
                        el.Refresh ();
                    }
                }
                p.Refresh ();
            }
        }
    }

    public class DesignerMockup : Form {
      
        private String CheckPoint;
        private int MaxWidth;
        private int MaxHeight;
        private int MaxDepth;
        private BackgroundPropertiesObject FormBackgroundSelector;
        private bool MarcoMoving=false;
        public bool MoreControlsAdded=false;
        private int DrawingOriginX=20, DrawingOriginY=40;
        private PropertyGrid ObjectPropertyGrid;
        PrintDocument Print;
        Image PrintImage;
        Boolean DesactivarTodasCotasToggle=false;
        /*
       * Como serializar todos los controles no es una práctica buena (y tampoco
       * creo que funcione muy bien), hemos hecho éste wrapper para recuperar y
       * inferir los controles del tipo ArotzDraw.arElement en el formulario. En concreto el
       * setter borra todos los del tipo ArotzDraw.arElement y los crea desde el valor que
       * se le pasa.
       * */
        public ArrayList myControls {
            get {
                ArrayList cc=new ArrayList ();
                foreach (Control c in this.Controls) {
                    if (c.GetType ()==new ArotzDraw.arElement (0, 0).GetType ()) {
                        ArotzDraw.arElement el=new ArotzDraw.arElement (0, 0);
                        el=(ArotzDraw.arElement)c;
                        cc.Add (el);
                    }
                }
                return cc;
            }
            set {
                ArrayList al=(ArrayList)value;
                foreach (Control c in this.Controls) {
                    if (c.GetType ()==new ArotzDraw.arElement (0, 0).GetType ()) {
                        this.Controls.Remove (c);
                        c.Dispose ();
                    }
                }


                foreach (Control c in value) {
                    if (c.GetType ()==new ArotzDraw.arElement (0, 0).GetType ()) {
                        ArotzDraw.arElement el=(ArotzDraw.arElement)c;
                        this.Controls.Add (el);
                    }
                }
            }
        }   

        public DesignerMockup () {
            WindowState=FormWindowState.Maximized;
            Icon = new Icon(Application.StartupPath + "\\shared\\arotzdraw.ico");
            this.CreateNewMockup ();
        }
        void CreateNewMockup () {
            
            MoreControlsAdded=false;
            Thread.CurrentThread.CurrentCulture=new CultureInfo ("es-ES");
            Thread.CurrentThread.CurrentUICulture=new CultureInfo ("es-ES");
            this.BackColor=Color.White;
            this.Text="ArotzDraw - Diseñador de Armarios";
            /* La Barra lateral derecha de propiedades. Ésto fue macanudo de implementar.
             * Tuvimos que crear un atributo nuevo para las propiedades de ArotzDraw.arElement que
             * nos dijese si se tendrían que ver o no. Ese atributo es viewParameter y está
             * especificado abajo como que debe de ser true */
            ObjectPropertyGrid=new System.Windows.Forms.PropertyGrid ();
            ObjectPropertyGrid.Dock=System.Windows.Forms.DockStyle.Right;
            ObjectPropertyGrid.Location=new System.Drawing.Point (411, 0);
            ObjectPropertyGrid.Name="Propiedades del Marco";
            ObjectPropertyGrid.Size=new System.Drawing.Size (230, 607);
            ObjectPropertyGrid.TabIndex=0;
            ObjectPropertyGrid.ToolbarVisible=false;
            ObjectPropertyGrid.SelectedObject=null; // Ésto hace que el objeto que se inspeccione sea el marco inicial
            ObjectPropertyGrid.BrowsableAttributes=new AttributeCollection (new ArotzDraw.viewParameter (true)); // Aqui definimos qué propiedades se ven
            FormBackgroundSelector=new BackgroundPropertiesObject ();
            Controls.Add (FormBackgroundSelector);
            Controls.Add (this.ObjectPropertyGrid);
            Click+=new EventHandler (DesignerMockupClickEvent);
            MaxWidth=500;
            MaxHeight=500;
            MaxDepth=100;
            Click+=new EventHandler (DesignerMockupClickEvent);
            MouseMove+=new MouseEventHandler (DesignerMockupMouseMoveEvent);
            CreateMenu ();
            CreateLayer ();
            SaveCheckPoint();
        }
        void CreateMenu () {
            FormClosed+=new FormClosedEventHandler (ClosedEvent);
            ToolStrip too=Interface.Toolstrip.Create ();
            ToolStripDropDownButton archivo=Interface.Toolstrip.CreateDropDown ("Archivo", Interface.icon ("abrir"), null);
            ToolStripDropDownButton dibujo=Interface.Toolstrip.CreateDropDown ("Dibujo", Interface.icon ("brocha"), null);
            ToolStripDropDownButton ayuda=Interface.Toolstrip.CreateDropDown ("Ayuda", Interface.icon ("ayuda"), null);
            ToolStripDropDownButton planta=Interface.Toolstrip.CreateDropDown ("Vistas", Interface.icon ("empaquetar"), null);
            ToolStripDropDownButton exportado=Interface.Toolstrip.CreateDropDown ("Exportado", Interface.icon ("mover"), null);
            Interface.Toolstrip.AddToToolStrip (too, archivo);
            Interface.Toolstrip.AddToToolStrip (too, dibujo);
            Interface.Toolstrip.AddToToolStrip (too, planta);
            Interface.Toolstrip.AddToToolStrip (too, exportado);
            Interface.Toolstrip.AddToToolStrip (too, ayuda);
            Interface.Toolstrip.CreateButton (archivo, "Nuevo Dibujo", Interface.icon ("nuevo_documento"), NuevoClickEvent);
            Interface.Toolstrip.CreateButton (archivo, "Abrir Dibujo", Interface.icon ("abrir"), AbrirClickEvent);
            Interface.Toolstrip.CreateButton (archivo, "Guardar Dibujo", Interface.icon ("guardar"), GuardarClickEvent);
            Interface.Toolstrip.CreateButton (archivo, "Salir", Interface.icon ("cancelar"), SalirClickEvent);
            Interface.Toolstrip.CreateButton (dibujo, "Mover Marco", Interface.icon ("mover"), MoveMarcoClickEvent);
            Interface.Toolstrip.CreateButton(dibujo, "Deshacer", Interface.icon("atras"), DeshacerClickEvent);
            Interface.Toolstrip.CreateButton (dibujo, "Redimensionar Marco", Interface.icon ("mover"), ResizeMarcoClickEvent);
            Interface.Toolstrip.CreateButton (dibujo, "(Des)activar Cotas", Interface.icon ("enlace"), DesactivarTodasCotasClickEvent);
            Interface.Toolstrip.CreateButton (dibujo, "Paneles y Japonesas", Interface.icon ("nueva_ventana"), PanelesYJaponesasClickEvent);
            Interface.Toolstrip.CreateButton (dibujo, "Baldas Horizontales", Interface.icon ("nueva_ventana"), BaldasHorizontalesClickEvent);
            Interface.Toolstrip.CreateButton (dibujo, "Baldas Verticales", Interface.icon ("nueva_ventana"), BaldasVerticalesClickEvent);
            Interface.Toolstrip.CreateButton (dibujo, "Elemento a Contenedor", Interface.icon ("brocha_ventana"), MarcoAContenedorClickEvent);
            Interface.Toolstrip.CreateButton (exportado, "Imprimir", Interface.icon ("imprimir"), ImprimirClickEvent);
            Interface.Toolstrip.CreateButton (exportado, "Exportar a Imagen", Interface.icon ("guardar"), ExportarClickEvent);
            Interface.Toolstrip.CreateButton (planta, "Ver Planta", Interface.icon ("empaquetar"), VerPlantaClickEvent);
            ayuda.Click += new EventHandler(ayudaClickEvent);
            Controls.Add (too);
        }

        void ayudaClickEvent(object sender, EventArgs e) {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = "iexplore";
            proc.StartInfo.Arguments = "http://arotzgi.webalianza.com";
            proc.Start();
        }
        void CreateLayer () {

            Graphics gfx=this.CreateGraphics ();
            Bitmap MyImage=new Bitmap (this.ClientRectangle.Width, this.ClientRectangle.Height, gfx);
            ArotzDraw.arElement TempElement=new ArotzDraw.arElement (MaxDepth, MaxDepth);
            TempElement.Width=MaxWidth;
            TempElement.Height=MaxHeight;
            TempElement.Depth=MaxDepth;
            TempElement.Location=new Point (DrawingOriginX, DrawingOriginY);
            TempElement.MouseDown+=new MouseEventHandler (ArElementSelectedEvent);
            TempElement.hdivev+=new ArotzDraw.HDivEventHandler (ArElementHorizontalDivisionClickEvent);
            TempElement.vdivev+=new ArotzDraw.VDivEventHandler (ArElementVerticalDivisionClickEvent);
            TempElement.Post_Construct ();
            Controls.Add (TempElement);
        }
        void DesignerMockupMouseMoveEvent (object sender, MouseEventArgs e) {
            if (!MarcoMoving) return;
            foreach (Control c in this.Controls) {
                ArotzDraw.arElement el=c as ArotzDraw.arElement;
                if (el!=null) {
                    el.Left=e.X+12;
                    el.Top=e.Y+12;
                    DrawingOriginX=el.Left;
                    DrawingOriginY=el.Top;
                }
            }
        }
        void DesignerMockupClickEvent (object sender, EventArgs e) {
            ObjectPropertyGrid.SelectedObject=FormBackgroundSelector;
            MarcoMoving=false;
        }
        void DesactivarTodasCotasClickEvent (object sender, EventArgs e) {
            DesactivarTodasCotasToggle=!DesactivarTodasCotasToggle;
            foreach (Control c in Controls) {
                ArotzDraw.arElement el=c as ArotzDraw.arElement;
                if (el!=null) {
                    el.Cotas=!DesactivarTodasCotasToggle;
                }
            }
        }
        void PanelesYJaponesasClickEvent (object sender, EventArgs e) {
            SaveCheckPoint();
            MarcosyJaponesasForm f=new MarcosyJaponesasForm (getSelectedElement (), this);
        }
        void BaldasHorizontalesClickEvent (object sender, EventArgs e) {
            SaveCheckPoint();
            BaldasHorizontalesForm f=new BaldasHorizontalesForm (getSelectedElement (), this);
        }
        void BaldasVerticalesClickEvent (object sender, EventArgs e) {
            SaveCheckPoint();
            BaldasVerticalesForm f=new BaldasVerticalesForm (getSelectedElement (), this);
        }
        void MarcoAContenedorClickEvent (object sender, EventArgs e) {
            SaveCheckPoint();
            MarcoAContenedorForm f=new MarcoAContenedorForm (getSelectedElement (), this);
        }
        void GuardarClickEvent (object sender, EventArgs e) {
            /*
             * Guardamos los controles ArotzDraw.arElements a partir del ControlContainer que nos hemos hecho
             * utilizando la infraestructura de serialización 
             */
           
            SaveFileDialog dialog=new SaveFileDialog ();
            dialog.Filter="Esquemas ArotzDraw|*.arg";
            try {
                if (dialog.ShowDialog ()==DialogResult.OK) {
                    ControlContainer cc=new ControlContainer ();
                    cc.controls=this.myControls; // Aquí copiamos los controles usando la propiedad self-currada myControls a ControlContainer
                    foreach (ArotzDraw.arElement el in cc.controls) {
                        el.Selected=false;
                    }
                    BinaryFormatter formatter=new BinaryFormatter ();
                    FileStream fs=new FileStream (dialog.FileName, FileMode.Create);
                    formatter.Serialize (fs, cc);
                    fs.Close ();
                }
            }
            catch (Exception el) {
                MessageBox.Show (el.Message);
                MessageBox.Show (el.StackTrace);
            }
        }
        void NuevoClickEvent (object sender, EventArgs e) {
            /* Hack-kluge para restaurar la form */
            this.Controls.Clear ();
            this.CreateNewMockup ();
        }
        void AbrirClickEvent (object sender, EventArgs e) {
            
            OpenFileDialog dialog=new OpenFileDialog ();

            dialog.Filter="Esquemas ArotzDraw|*.arg";
            try {
                if (dialog.ShowDialog ()==DialogResult.OK) {
                    for (int i=0; i<Controls.Count; i++) {
                        ArotzDraw.arElement el=Controls[i] as ArotzDraw.arElement;
                        if (el!=null) {
                            Controls.Remove (el);
                            i--;
                        }
                    }
                    FileStream fs;
                    ControlContainer cc=new ControlContainer ();
                    BinaryFormatter formatter=new BinaryFormatter ();
                    fs=new FileStream (dialog.FileName, FileMode.Open);
                    cc=(ControlContainer)formatter.Deserialize (fs);
                    this.myControls=cc.controls;
                    foreach (Control c in Controls) {
                        ArotzDraw.arElement el=c as ArotzDraw.arElement;
                        if (el!=null) {
                            el.Click+=new EventHandler (ArElementSelectedEvent);
                            el.Fix ();
                        }
                    }
                    fs.Close ();
                }
            }
            catch (Exception el) {
                MessageBox.Show (el.Message);
                MessageBox.Show (el.InnerException.Message);
                //MessageBox.Show(el.StackTrace);
                Application.Exit ();
            }
            ObjectPropertyGrid.SelectedObject=null;
        }
        void SalirClickEvent (object o, EventArgs e) {
            Application.Exit ();
            Dispose ();

        }
        void DeshacerClickEvent(object o, EventArgs e)
        {
            RestoreCheckPoint();
         

        }
        void ExportarClickEvent (object sender, EventArgs e) {
            for (int i=0; i<16; i++) {
                Refresh ();
                Graphics graphic=this.CreateGraphics ();
                Size s=this.Size;
                PrintImage=new Bitmap (s.Width, s.Height, graphic);
                Graphics memGraphic=Graphics.FromImage (PrintImage);
                IntPtr dc1=graphic.GetHdc ();
                IntPtr dc2=memGraphic.GetHdc ();
                NativeMethods.BitBlt (dc2, 0, 0, this.Width-ObjectPropertyGrid.Width-32, this.Height-70, dc1, 00, 25, 13369376);
                graphic.ReleaseHdc (dc1);
                memGraphic.ReleaseHdc (dc2);
            }
            SaveFileDialog dialog=new SaveFileDialog ();
            dialog.Filter="Imagenes JPEG|*.jpeg";
            try {
                if (dialog.ShowDialog ()==DialogResult.OK) {
                    PrintImage.Save (dialog.FileName);
                }
            }
            catch (Exception el) {
                MessageBox.Show (el.Message);
                MessageBox.Show (el.StackTrace);
            }
        }
        void VerPlantaClickEvent (object sender, EventArgs e) {
            designerfloor d=new designerfloor (myControls);
            d.Show ();
        }
        void MoveMarcoClickEvent (object sender, EventArgs e) {
            SaveCheckPoint();
            if (MoreControlsAdded) {
                MessageBox.Show ("Solo es posible mover el marco cuando no se han realizado todavia divisiones");
                return;
            }
            MarcoMoving=true;
        }
        void ResizeMarcoClickEvent (object sender, EventArgs e) {
            SaveCheckPoint();
            if (MoreControlsAdded) {
                MessageBox.Show ("Solo es posible redimensionar el marco cuando no se han realizado todavia divisiones");
                return;
            }

            InputBoxDialog InputBox=new InputBoxDialog ();
            string s;
            int ibw, ibh;
            InputBox.FormPrompt="Redefinir Anchura del Marco";
            InputBox.FormCaption="Redefinir Marco Inicial";
            InputBox.DefaultValue="500";
            InputBox.ShowDialog ();
            s=InputBox.InputResponse;
            if (InputBox.Canceled) return;
            try {
                ibw=int.Parse (InputBox.InputResponse);
                MaxWidth=ibw;
            }
            catch {
                return;
            }
            InputBox=new InputBoxDialog ();
            InputBox.FormPrompt="Redefinir Altura del Marco";
            InputBox.FormCaption="Redefinir Marco Inicial";
            InputBox.DefaultValue="500";
            InputBox.ShowDialog ();
            s=InputBox.InputResponse;
            if (InputBox.Canceled) return;
            try {
                ibh=int.Parse (InputBox.InputResponse);
                MaxHeight=ibh;
            }
            catch {
                return;
            }
            foreach (Control c in this.Controls) {
                ArotzDraw.arElement el=c as ArotzDraw.arElement;
                if (el!=null) {
                    el.Width=ibw;
                    el.Height=ibh;
                }
            }
        }
        void ImprimirClickEvent (object o, EventArgs e) {
            for (int i=0; i<16; i++) {
                Refresh ();
                Graphics graphic=this.CreateGraphics ();
                Size s=this.Size;
                PrintImage=new Bitmap (s.Width, s.Height, graphic);
                Graphics memGraphic=Graphics.FromImage (PrintImage);
                IntPtr dc1=graphic.GetHdc ();
                IntPtr dc2=memGraphic.GetHdc ();
                NativeMethods.BitBlt (dc2, 0, 0, this.Width-ObjectPropertyGrid.Width-32, this.Height-70, dc1, 00, 25, 13369376);
                graphic.ReleaseHdc (dc1);
                memGraphic.ReleaseHdc (dc2);
            }
            Print=new PrintDocument ();
            PrintDialog SWFPrintDialog=new PrintDialog ();
            SWFPrintDialog.Document=Print;
            DialogResult result=SWFPrintDialog.ShowDialog ();
            if (result==DialogResult.OK) {
                Print.PrintPage+=new PrintPageEventHandler (PrintPageEvent);
                Print.Print ();
            }
        }
        void PrintPageEvent (object sender, System.Drawing.Printing.PrintPageEventArgs e) {
            e.Graphics.DrawImage (PrintImage, 0, 0);
        }  
        void ClosedEvent (object o, EventArgs e) {
            Application.Exit ();
            Dispose ();

        }
        void DisposedEvent (object sender, EventArgs e) {
            Application.Exit ();
        }
        void SaveCheckPoint()
        {
            try
            {
                ControlContainer cc = new ControlContainer();
                cc.controls = this.myControls; // Aquí copiamos los controles usando la propiedad self-currada myControls a ControlContainer
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fs = new FileStream(Application.StartupPath + "\\SaveCheckpoint.sav", FileMode.Create);
                formatter.Serialize(fs, cc);
                fs.Close();
            }
            catch (Exception el)
            {
                MessageBox.Show(el.Message);
                MessageBox.Show(el.StackTrace);
            }
        }
        void RestoreCheckPoint()
        {
            try
            {
                FileStream fs;
                ControlContainer cc = new ControlContainer();
                BinaryFormatter formatter = new BinaryFormatter();
                fs = new FileStream(Application.StartupPath + "\\SaveCheckpoint.sav", FileMode.Open);
                cc = (ControlContainer)formatter.Deserialize(fs);
                for (int i = 0; i < Controls.Count; i++)
                {
                    ArotzDraw.arElement el = Controls[i] as ArotzDraw.arElement;
                    if (el != null)
                    {
                        Controls.Remove(el);
                        i--;
                    }
                }
               
                foreach (Control c in cc.controls)
                {
                    ArotzDraw.arElement el = c as ArotzDraw.arElement;
                    if (el != null)
                    {
                        
                        el.MouseDown += new MouseEventHandler(ArElementSelectedEvent);
                        el.hdivev += new ArotzDraw.HDivEventHandler(ArElementHorizontalDivisionClickEvent);
                        el.vdivev += new ArotzDraw.VDivEventHandler(ArElementVerticalDivisionClickEvent);
                        el.Fix();
                        Controls.Add(el);
                    }
                    
                }
                fs.Close();
            }
            catch (Exception el)
            {
                MessageBox.Show(el.Message);
                Application.Exit();
                return;
            }
            ObjectPropertyGrid.SelectedObject = FormBackgroundSelector;
        }
        public void ArElementVerticalDivisionClickEvent(object sender) {
            /*
             * Hacemos una nueva división vertical. Ésto en realidad se traduce a
             * reducir el tamaño vertical del objeto seleccionado y crear un nuevo
             * ArotzDraw.arElement que reemplace dicho espacio
             */
            SaveCheckPoint();
            InputBoxDialog InputBox = new InputBoxDialog();
            InputBox.FormPrompt = "Altura de la División";
            InputBox.FormCaption = "División Horizontal";
            InputBox.DefaultValue = "100";
            InputBox.ShowDialog();
            string StringResponse = InputBox.InputResponse;
            if (InputBox.Canceled) return;
            int divval = Int16.Parse(StringResponse);
            if (StringResponse == "") return;
            InputBox.Close();
            InputBox = new InputBoxDialog();
            InputBox.FormPrompt = "Fondo de la División";
            InputBox.FormCaption = "División Horizontal";
            InputBox.DefaultValue = "100";
            InputBox.ShowDialog();
            StringResponse = InputBox.InputResponse;
            if (InputBox.Canceled) return;
            InputBox.Close();
            if (StringResponse == "") return;
            ArotzDraw.arElement AffectedElement = this.getSelectedElement();
            AffectedElement.Height = AffectedElement.Height - divval;
            ArotzDraw.arElement NewElement = new ArotzDraw.arElement(0, 0);
            NewElement.Location = new Point(AffectedElement.Left, AffectedElement.Top + AffectedElement.Height);
            NewElement.Width = AffectedElement.Width;
            NewElement.Depth = int.Parse(StringResponse);
            NewElement.Height = divval;
            NewElement.Post_Construct();
            NewElement.Textura = AffectedElement.Textura;
            this.Controls.Add(NewElement);
            NewElement.MouseDown += new MouseEventHandler(ArElementSelectedEvent);
            NewElement.hdivev += new ArotzDraw.HDivEventHandler(ArElementHorizontalDivisionClickEvent);
            NewElement.vdivev += new ArotzDraw.VDivEventHandler(ArElementVerticalDivisionClickEvent);
            this.Refresh();
            MoreControlsAdded = true;
            AffectedElement.Escala = FormBackgroundSelector.EscalaGlobal;
            AffectedElement.Refresh();

        }
        public void ArElementHorizontalDivisionClickEvent(object sender) {
            /*
             * Lo mismo que ivdiv_Click pero en vertical
             */
            SaveCheckPoint();
            InputBoxDialog InputBox = new InputBoxDialog();
            InputBox.FormPrompt = "Anchura de la División";
            InputBox.FormCaption = "División Vertical";
            InputBox.DefaultValue = "100";
            InputBox.ShowDialog();
            string StringResponse = InputBox.InputResponse;
            if (InputBox.Canceled) return;
            int divval = (Int16.Parse(StringResponse));
            
            InputBox.Close();
            if (StringResponse == "") return;
            InputBox = new InputBoxDialog();
            InputBox.FormPrompt = "Fondo de la División";
            InputBox.FormCaption = "División Vertical";
            InputBox.DefaultValue = "100";
            InputBox.ShowDialog();
            StringResponse = InputBox.InputResponse;
            if (InputBox.Canceled) return;
            InputBox.Close();
            if (StringResponse == "") return;
            ArotzDraw.arElement AffectedElement = this.getSelectedElement();
            AffectedElement.Width = AffectedElement.Width - divval;
            ArotzDraw.arElement NewElement = new ArotzDraw.arElement(0, 0);
            NewElement.Location = new Point(AffectedElement.Left + AffectedElement.Width, AffectedElement.Top);
            NewElement.Width = divval;
            NewElement.Height = AffectedElement.Height;
            NewElement.Depth = int.Parse(StringResponse);
          
            NewElement.Post_Construct();
            NewElement.Textura = AffectedElement.Textura;
            this.Controls.Add(NewElement);
            NewElement.MouseDown += new MouseEventHandler(ArElementSelectedEvent);
            NewElement.hdivev += new ArotzDraw.HDivEventHandler(ArElementHorizontalDivisionClickEvent);
            NewElement.vdivev += new ArotzDraw.VDivEventHandler(ArElementVerticalDivisionClickEvent);
            this.Refresh();
            MoreControlsAdded = true;
            AffectedElement.Escala = FormBackgroundSelector.EscalaGlobal;
            AffectedElement.Refresh();
        }
        public void ArElementSelectedEvent(object o, EventArgs e) {
            /* Inferimos el objeto seleccionado a la barra lateral y deseleccionamos el
             * resto de objetos.
             * ArotzDraw.arElement se encarga de auto-seleccionarse.
             */

            foreach (Control c in this.Controls) {
                ArotzDraw.arElement ela = c as ArotzDraw.arElement;
                if (ela != null) ela.deSelect();
            }
            this.ObjectPropertyGrid.SelectedObject = Controls[Controls.IndexOf((Control)o)];
            ArotzDraw.arElement el = (ArotzDraw.arElement)Controls[Controls.IndexOf((Control)o)];
            el.Select();

        }
        public void CambiarFondo() {
            Bitmap b = new Bitmap(FormBackgroundSelector.Imagen);
            b = NativeMethods.ResizeBitmap(b, Width - ObjectPropertyGrid.Width, Height - 40);
            BackgroundImage = b;
        }
        ArotzDraw.arElement getSelectedElement() {
            /* Obtiene el elemento seleccionado a partir de la propia información de los ArotzDraw.arElements */
            foreach (Control c in this.Controls) {
                ArotzDraw.arElement el = c as ArotzDraw.arElement;
                if (el != null && el.Selected == true) return el;
            }
            return null;
        }      
    }
}
