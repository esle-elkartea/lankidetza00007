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
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;

namespace ArotzDraw {
    class AboutDialog : Form {
        PictureBox AboutBitmap;
        public AboutDialog () {
            Text="Acerca de Arotzdraw";
            Opacity=0;
            StartPosition=FormStartPosition.CenterScreen;
            MaximizeBox=false;
            MinimizeBox=false;
            Width=638;
            Height=365;
            AboutBitmap=new PictureBox ();
            AboutBitmap.Width=640;
            AboutBitmap.Height=345;
            AboutBitmap.Left=-3;
            AboutBitmap.Top=-3;
            AboutBitmap.Image=new Bitmap (Application.StartupPath+"\\shared\\splash.png");
            Controls.Add (AboutBitmap);
            FormBorderStyle=FormBorderStyle.FixedToolWindow;
            Show ();
            anim ();
        }
        protected override void OnClosed (EventArgs e) {
            while (true) {
                if (Opacity>0) Opacity-=0.1;
                Refresh ();
                if (Opacity==0||Opacity<0) {
                    return;
                }
            }
        }

        public void anim () {
            while (true) {
                if (Opacity<1) Opacity+=0.1;
                Refresh ();
                if (Opacity==1) {
                    return;
                }
            }
        }
    }
}
