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
using System.Windows.Forms;
using System.Drawing;
namespace ArotzDraw
{
    class Interface
    {
        public static TextBox create_textbox(int left, int top, int width)
        {
            TextBox t = new TextBox();
            t.Left = left;
            t.Top = top;
            t.Width = width;
            t.Height = 24;
            return t;
        }
        public static Label create_label(int left, int top, string str)
        {
            Label l = new Label();
            l.Left = left;
            l.Top = top;
            l.Width = (int)(str.Length * 7.5);
            if (l.Width < 100) l.Width = 100;
            l.Height = 16;
            l.Text = str;
            return l;
        }
        public static Label create_title(int left, int top, string str)
        {
            Label l = new Label();
            l.Left = left;
            l.Top = top;
            l.Width = (int)(str.Length * 17.5);
            if (l.Width < 100) l.Width = 100;
            l.Height = 32;
            l.Text = str;
            l.Font = new Font("Tahoma", 18);
            return l;

        }
        public static Button create_button(int left, int top, string str, EventHandler d)
        {
            Button b = new Button();
            b.Height = 24;
            b.Width = str.Length * 8 + 16;
            b.Left = left;
            b.Top = top;
            b.Text = str;
            b.Click += d;
            return b;
        }
        public static string icon(string t)
        {
            return (Application.StartupPath + "\\shared\\icons\\" + t + ".ico");
        }
        public class Toolstrip
        {
            public static ToolStrip Create()
            {
                ToolStrip t = new ToolStrip();
                return t;
            }
            public static ToolStripDropDownButton CreateDropDown(string text, string img, EventHandler d)
            {
                ToolStripDropDownButton t;
                if (img != "")
                {
                    Bitmap b = new Icon(img).ToBitmap();
                    t = new ToolStripDropDownButton(text, b, d);
                }
                else
                {
                    t = new ToolStripDropDownButton(text);
                    t.Click += d;
                }
                return t;

            }
            public static void CreateButton(ToolStripDropDownButton t, string text, string img, EventHandler d)
            {
                try
                {
                    if (img != "")
                    {
                        Bitmap b = new Icon(img).ToBitmap();
                        t.DropDownItems.Add(text, b, d);
                    }
                    else
                    {
                        t.DropDownItems.Add(text, null, d);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("No se encontró algún medio necesario para ejecutar ArotzDraw.\n Por favor, compruebe que ArotzDraw está instalado correctamente");
                    Application.ExitThread();
                    Application.Exit();
                    return;
                }
            }
            public static void AddToToolStrip(ToolStrip t, ToolStripDropDownButton b)
            {
                t.Items.Add((ToolStripItem)b);
            }

        }
    }
}