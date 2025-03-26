using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Drawing.Printing;

namespace TAPTV_P1
{
    public partial class Form1 : Form
    {
        StreamReader TR;
        StreamWriter TW;
        string archivo;
        private Color colorCSharp = Color.Blue;
        private Color colorSQL = Color.DarkRed;
        private Color colorCopiadoCSharp = Color.Black;
        private Color colorCopiadoSQL = Color.Black;
        private Stack<Color> pilaDeshacerCSharp = new Stack<Color>();
        private Stack<Color> pilaDeshacerSQL = new Stack<Color>();
        private Stack<Color> pilaRehacerCSharp = new Stack<Color>();
        private Stack<Color> pilaRehacerSQL = new Stack<Color>();
        private Color colorCSharpDefault = Color.Blue;
        private Color colorSQLDefault = Color.DarkRed;
        private ToolStripStatusLabel lblLineas;
        private ToolStripStatusLabel lblCaracteres;
        private ToolStripStatusLabel lblPosicion;

        public Form1()
        {
            InitializeComponent();
 
            if (tabControl1.TabPages.Count > 0 && tabControl1.TabPages[0].Text == "Nuevo")
            {
                tabControl1.TabPages.RemoveAt(0);
            }
            ConfigurarStatusStrip();
            toolStripStatusLabel1.Dock = DockStyle.Right;
            toolStripStatusLabel2.Dock = DockStyle.Right;

        }
        private void ConfigurarStatusStrip()
        {
            lblLineas = new ToolStripStatusLabel("Total de líneas: 0");
            lblCaracteres = new ToolStripStatusLabel("Caracteres: 0");
            lblPosicion = new ToolStripStatusLabel("Línea: 0, Columna: 0");

            statusStrip1.Items.Insert(0, lblPosicion);
            statusStrip1.Items.Insert(0, lblCaracteres);
            statusStrip1.Items.Insert(0, lblLineas);
        }
        private void ActualizarEstado(RichTextBox rtb)
        {
            lblLineas.Text = $"Líneas: {rtb.Lines.Length}";
            lblCaracteres.Text = $"Caracteres: {rtb.Text.Length}";
            int linea = rtb.GetLineFromCharIndex(rtb.SelectionStart) + 1;
            int columna = rtb.SelectionStart - rtb.GetFirstCharIndexOfCurrentLine() + 1;
            lblPosicion.Text = $"Línea: {linea}, Columna: {columna}";
        }

        private void aBRIRToolStripMenuItem_Click(object sender, EventArgs e)
        { AbrirArchivo(); }
        private void ToolAbrir_Click(object sender, EventArgs e)
        { AbrirArchivo(); }
        public void AbrirArchivo()
        {
            OpenFileDialog OPF = new OpenFileDialog();
            OPF.Filter = "Archivo de texto (*.txt)|*.txt|Todos los Archivos (*.*)|*.*|Archivo C# (*.cs)|*.cs|Archivo SQL(*.sql)|*.sql";
            OPF.Title = "Abrir archivo";
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                string archivo = OPF.FileName;
                string[] lineas = File.ReadAllLines(archivo);
                int totalLineas = lineas.Length;

                TabPage pestañaDestino = null;
                RichTextBox rtbEditor = null;

                foreach (TabPage pestaña in tabControl1.TabPages)
                {
                    if (pestaña.Text == "Nuevo Documento   ")
                    {
                        pestañaDestino = pestaña;
                        rtbEditor = pestaña.Controls.OfType<RichTextBox>().FirstOrDefault();
                        break;
                    }
                }

                if (pestañaDestino == null)
                {
                    pestañaDestino = new TabPage($"{Path.GetFileName(archivo)}   ");
                    rtbEditor = new RichTextBox
                    {
                        Font = new Font("Arial", 10),
                        Multiline = true,
                        ScrollBars = RichTextBoxScrollBars.Vertical,
                        Dock = DockStyle.Fill,
                        ReadOnly = true
                    };

                    pestañaDestino.Controls.Add(rtbEditor);
                    tabControl1.TabPages.Add(pestañaDestino);
                }
                else
                {
                    pestañaDestino.Text = $"{Path.GetFileName(archivo)}   ";
                }
                tabControl1.SelectedTab = pestañaDestino;

                rtbEditor.Text = string.Join("\n", lineas);
                ActualizarEstado(rtbEditor);
                rtbEditor.SelectionChanged += (s, e) => ActualizarEstado(rtbEditor);

                colorCSharp = colorCSharpDefault;
                colorSQL = colorSQLDefault;

                tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
                tabControl1.DrawItem += new DrawItemEventHandler(BotonX);
                tabControl1.MouseDown += new MouseEventHandler(CerrarPestaña);

                Task.Run(() =>
                {
                    try
                    {
                        StringBuilder textoConNumeros = new StringBuilder();
                        for (int i = 0; i < lineas.Length; i++)
                        {
                            textoConNumeros.AppendLine($"{i + 1}        {lineas[i]}");
                        }

                        this.Invoke(new Action(() =>
                        {
                            rtbEditor.Text = textoConNumeros.ToString();
                        
                            PalabrasClave(rtbEditor);
                        }));
                    }
                    catch (Exception ms)
                    {
                        MessageBox.Show("Error al abrir el archivo: " + ms.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });
            }
        }


        private void PalabrasClave(RichTextBox rtb)
        {
            string[] palabrasClaveCSharp =
 {
    "abstract", "add", "alias", "as", "ascending", "async", "await", "EventArgs", "bool", "break", "by",
    "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default",
    "delegate", "descending", "do", "double", "dynamic", "else", "enum", "equals", "event", "explicit",
    "extern", "false", "finally", "fixed", "float", "for", "foreach", "from", "get", "global", "goto",
    "group", "if", "implicit", "in", "init", "int", "interface", "internal", "into", "is", "join", "let",
    "lock", "long", "managed", "nameof", "namespace", "new", "nint", "not", "notnull", "null", "nuint",
    "object", "on", "operator", "or", "orderby", "out", "override", "params", "partial", "private",
    "protected", "public", "readonly", "record", "ref", "remove", "return", "sbyte", "sealed", "select",
    "set", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw",
    "true", "try", "typeof", "uint", "ulong", "unchecked", "unmanaged", "unsafe", "ushort", "using",
    "value", "var", "virtual", "void", "volatile", "when", "where", "while", "with", "yield", "length"
};

            string[] palabrasClaveSQL =
            {
    "ADD", "ALL", "ALTER", "AND", "ANY", "AS", "ASC", "AUTHORIZATION", "BACKUP", "BEGIN", "BETWEEN",
    "BREAK", "BROWSE", "BULK", "BY", "CASCADE", "CASE", "CHECK", "CHECKPOINT", "CLOSE", "CLUSTERED",
    "COALESCE", "COLLATE", "COLUMN", "COMMIT", "COMPUTE", "CONSTRAINT", "CONTAINS", "CONTAINSTABLE",
    "CONTINUE", "CONVERT", "CREATE", "CROSS", "CURRENT", "CURRENT_DATE", "CURRENT_TIME",
    "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", "DATABASE", "DBCC", "DEALLOCATE", "DECLARE",
    "DEFAULT", "DELETE", "DENEGAR", "DESC", "DISK", "DISTINCT", "DISTRIBUTED", "DOUBLE", "DROP",
    "DUMP", "ELSE", "END", "ERRLVL", "ESCAPE", "EXCEPT", "EXEC", "EXECUTE", "EXISTS", "EXIT", "EXTERNAL",
    "FETCH", "FILE", "FILLFACTOR", "FOR", "FOREIGN", "FREETEXT", "FREETEXTTABLE", "FROM", "FULL",
    "FUNCTION", "GOTO", "GRANT", "GROUP", "HAVING", "HOLDLOCK", "IDENTITY", "IDENTITY_INSERT",
    "IDENTITYCOL", "IF", "IN", "INDEX", "INNER", "INSERT", "INTERSECT", "INTO", "IS", "JOIN", "KEY",
    "KILL", "LEFT", "LIKE", "LINENO", "LOAD", "MERGE", "NATIONAL", "NOCHECK", "NONCLUSTERED", "NOT",
    "NULL", "NULLIF", "OF", "OFFSETS", "ON", "OPEN", "OPENDATASOURCE", "OPENQUERY", "OPENROWSET",
    "OPENXML", "OPTION", "OR", "ORDER", "OUTER", "OVER", "PERCENT", "PIVOT", "PLAN", "PRECISION",
    "PRIMARY", "PRINT", "PROC", "PROCEDURE", "PUBLIC", "RAISERROR", "READ", "READTEXT", "RECONFIGURE",
    "REFERENCES", "REPLICACIÓN", "RESTORE", "RESTRICT", "RETURN", "REVERT", "REVOKE", "RIGHT",
    "ROLLBACK", "ROWCOUNT", "ROWGUIDCOL", "RULE", "SAVE", "SCHEMA", "SECURITYAUDIT", "SELECT",
    "SEMANTICKEYPHRASETABLE", "SEMANTICSIMILARITYDETAILSTABLE", "SEMANTICSIMILARITYTABLE", "SESSION_USER",
    "SET", "SETUSER", "SHUTDOWN", "SOME", "STATISTICS", "SYSTEM_USER", "TABLE", "TABLESAMPLE", "TEXTSIZE",
    "THEN", "TO", "TOP", "TRAN", "TRANSACTION", "TRIGGER", "TRUNCATE", "TSEQUAL", "UNION", "UNIQUE",
    "UNPIVOT", "UPDATE", "UPDATETEXT", "USE", "USER", "VALUES", "VARYING", "VIEW", "WAITFOR", "WHEN",
    "WHERE", "WHILE", "WITH", "WITHIN GROUP", "WRITETEXT", "INT", "VARCHAR",
};
            rtb.SelectionStart = 0;
            rtb.SelectionLength = rtb.Text.Length;
            rtb.SelectionColor = Color.Black;

            foreach (string palabra in palabrasClaveCSharp)
            {
                ResaltarPalabra(rtb, palabra, colorCSharp, esMayuscula: false);
            }
            foreach (string palabra in palabrasClaveSQL)
            {
                ResaltarPalabra(rtb, palabra, colorSQL, esMayuscula: true);
            }
        }
        private void ResaltarPalabra(RichTextBox rtb, string palabra, Color color, bool esMayuscula)
        {
            string patron = $@"\b{palabra}\b";

            MatchCollection matches = Regex.Matches(rtb.Text, patron, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string palabraEnTexto = match.Value;
                if ((esMayuscula && palabraEnTexto == palabra.ToUpper()) || (!esMayuscula && palabraEnTexto == palabra.ToLower()))
                {
                    rtb.SelectionStart = match.Index;
                    rtb.SelectionLength = match.Length;
                    rtb.SelectionColor = color;
                }
            }
        }
        private void ToolCsharp_Click(object sender, EventArgs e)
        { CambiarColorCsharp(); }
        private void cambiarColoresDePalabrasClaveCToolStripMenuItem_Click(object sender, EventArgs e)
        { CambiarColorCsharp(); }
        public void CambiarColorCsharp()
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    pilaDeshacerCSharp.Push(colorCSharp);
                    pilaRehacerCSharp.Clear();

                    colorCSharp = colorDialog.Color;
                    ResaltarPestaña();
                }
            }
        }
        private void ToolSQL_Click(object sender, EventArgs e)
        { CambiarColorSQL(); }
        public void CambiarColorSQL()
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    pilaDeshacerSQL.Push(colorSQL);
                    pilaRehacerSQL.Clear();

                    colorSQL = colorDialog.Color;
                    ResaltarPestaña();
                }
            }
        }
        private void cambiaColorDePalabrasClaveSQLToolStripMenuItem_Click(object sender, EventArgs e)
        { CambiarColorSQL(); }

        private void ResaltarPestaña()
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox rtb = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (rtb != null)
                {
                    PalabrasClave(rtb);
                }
            }
        }
        private string configPath = "settings.json";
        private void guardarColoresToolStripMenuItem_Click(object sender, EventArgs e)
        { GuardarColor(); }
        private void ToolGuardar_Click(object sender, EventArgs e)
        { GuardarColor(); }
        public void GuardarColor()
        {
            var colores = new
            {
                CSharp = colorCSharp.ToArgb(),
                SQL = colorSQL.ToArgb()
            };
            string json = JsonConvert.SerializeObject(colores, Formatting.Indented);
            File.WriteAllText(configPath, json);
            colorCSharpDefault = colorCSharp;
            colorSQLDefault = colorSQL;

            MessageBox.Show("Color guardado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ToolCopiar_Click(object sender, EventArgs e)
        { CopiarColor(); }
        private void copiarColorToolStripMenuItem_Click(object sender, EventArgs e)
        { CopiarColor(); }
        public void CopiarColor()
        {
            if (tabControl1.SelectedTab != null)
            {
                colorCopiadoCSharp = colorCSharp;
                colorCopiadoSQL = colorSQL;

                MessageBox.Show("Color copiado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void pegarColorToolStripMenuItem_Click(object sender, EventArgs e)
        { PegarColor(); }
        private void ToolPegar_Click(object sender, EventArgs e)
        { PegarColor(); }
        public void PegarColor()
        {
            if (tabControl1.SelectedTab != null)
            {
                colorCSharp = colorCopiadoCSharp;
                colorSQL = colorCopiadoSQL;

                ResaltarPestaña();

                MessageBox.Show("Color aplicado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ToolDeshacer_Click(object sender, EventArgs e)
        { Deshacer(); }
        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        { Deshacer(); }
        public void Deshacer()
        {
            if (pilaDeshacerCSharp.Count > 0)
            {
                pilaRehacerCSharp.Push(colorCSharp);
                colorCSharp = pilaDeshacerCSharp.Pop();
            }
            if (pilaDeshacerSQL.Count > 0)
            {
                pilaRehacerSQL.Push(colorSQL);
                colorSQL = pilaDeshacerSQL.Pop();
            }
            ResaltarPestaña();
        }
        private void ToolRehacer_Click(object sender, EventArgs e)
        { Rehacer(); }
        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        { Rehacer(); }
        public void Rehacer()
        {
            if (pilaRehacerCSharp.Count > 0)
            {
                pilaDeshacerCSharp.Push(colorCSharp);
                colorCSharp = pilaRehacerCSharp.Pop();
            }
            if (pilaRehacerSQL.Count > 0)
            {
                pilaDeshacerSQL.Push(colorSQL);
                colorSQL = pilaRehacerSQL.Pop();
            }
            ResaltarPestaña();
        }
        private void cerrarPestañasToolStripMenuItem_Click(object sender, EventArgs e)
        { tabControl1.TabPages.Clear(); }
        private void ToolPestañas_Click(object sender, EventArgs e)
        { tabControl1.TabPages.Clear(); }
        private void cerrarTodasLasVentanasToolStripMenuItem_Click(object sender, EventArgs e)
        { tabControl1.TabPages.Clear(); }
        private void ToolZoom_Click(object sender, EventArgs e)
        { CambiarZoom(2); }
        private void ToolDisminuirZoom_Click(object sender, EventArgs e)
        { CambiarZoom(-2); }
        private void aumentarZoomToolStripMenuItem_Click(object sender, EventArgs e)
        { CambiarZoom(2); }
        private void disminuirZoomToolStripMenuItem_Click(object sender, EventArgs e)
        { CambiarZoom(-2); }
        private void CambiarZoom(int incremento)
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox rtb = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (rtb != null)
                {
                    float nuevoTamaño = rtb.Font.Size + incremento;

                    if (nuevoTamaño > 6 && nuevoTamaño < 15)
                    {
                        rtb.Font = new Font(rtb.Font.FontFamily, nuevoTamaño);
                        PalabrasClave(rtb);
                    }
                }
            }
        }
        private void ToolImprimir_Click(object sender, EventArgs e)
        { MandarImprimir(); }
        private void imprimirToolStripMenuItem_Click(object sender, EventArgs e)
        { MandarImprimir(); }
        public void MandarImprimir()
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox rtb = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (rtb != null)
                {
                    PrintDocument printDoc = new PrintDocument();
                    printDoc.PrintPage += (s, ev) =>
                    {
                        ev.Graphics.DrawString(rtb.Text, rtb.Font, Brushes.Black, ev.MarginBounds, StringFormat.GenericTypographic);
                        ev.HasMorePages = false;
                    };
                    PrintDialog printDialog = new PrintDialog
                    {
                        Document = printDoc
                    };
                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        printDoc.Print();
                    }
                }
                else
                {
                    MessageBox.Show("No hay texto para imprimir.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void ToolDocumento_Click(object sender, EventArgs e)
        { CrearNuevaPestaña(); }
        private void nuevoDocumentoToolStripMenuItem_Click(object sender, EventArgs e)
        { CrearNuevaPestaña(); }
        private void CrearNuevaPestaña()
        {
            TabPage nuevaPestaña = new TabPage("Nuevo Documento   ");
            RichTextBox rtbEditor = new RichTextBox
            {
                Font = new Font("Arial", 10),
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Dock = DockStyle.Fill,
                ReadOnly = true,
            };

            nuevaPestaña.Controls.Add(rtbEditor);
            tabControl1.TabPages.Add(nuevaPestaña);
            tabControl1.SelectedTab = nuevaPestaña;

            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += new DrawItemEventHandler(BotonX);
            tabControl1.MouseDown += new MouseEventHandler(CerrarPestaña);
        }
       
        private void BotonX(object sender, DrawItemEventArgs e)
        {
            Rectangle tabRect = tabControl1.GetTabRect(e.Index);
            tabRect.Inflate(-2, -2);

            string titulo = tabControl1.TabPages[e.Index].Text;

            using (Brush textBrush = new SolidBrush(tabControl1.ForeColor))
            {
                e.Graphics.DrawString(titulo, tabControl1.Font, textBrush, (float)(tabRect.X + 5), (float)(tabRect.Y + (tabRect.Height / 4)));
            }
            float tamañoBoton = 16; 
            float paddingDerecha = 0; 
            float paddingArriba = 0; 

            RectangleF closeButton = new RectangleF(
                (float)(tabRect.Right - tamañoBoton - paddingDerecha),
                (float)(tabRect.Top + paddingArriba),
                tamañoBoton,
                tamañoBoton
            );

            e.Graphics.FillEllipse(Brushes.Red, closeButton);
            e.Graphics.DrawEllipse(Pens.Black, closeButton);

            using (Font closeFont = new Font(tabControl1.Font.FontFamily, 9, FontStyle.Bold)) 
            {
                e.Graphics.DrawString("X", closeFont, Brushes.White,
                    closeButton.X + 3, closeButton.Y + 1); 
            }

            tabControl1.TabPages[e.Index].Tag = closeButton;
        }
        private void CerrarPestaña(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                if (tabControl1.TabPages[i].Tag is RectangleF closeButton && closeButton.Contains((float)e.Location.X, (float)e.Location.Y))
                {
                    tabControl1.TabPages.RemoveAt(i);
                    break;
                }
            }
        }
        private void ToolFondo_Click(object sender, EventArgs e)
        { CambiarFondo(); }
        private void cambiarElColorDelFondoToolStripMenuItem_Click(object sender, EventArgs e)
        { CambiarFondo(); }
        private Color colorFondo = Color.White;
        public void CambiarFondo()
        {
            using (ColorDialog colorDialogFondo = new ColorDialog())
            {
                if (colorDialogFondo.ShowDialog() == DialogResult.OK)
                {
                    colorFondo = colorDialogFondo.Color;
                    TabPage currentTab = tabControl1.SelectedTab;
                    currentTab.BackColor = colorFondo;

                    RichTextBox rtb = currentTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                    if (rtb != null)
                    {
                        rtb.BackColor = colorFondo;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el RichTextBox en la pestaña actual.");
                    }
                }
            }
            TabPage selectedTab = tabControl1.SelectedTab;
            RichTextBox rtbToHighlight = selectedTab?.Controls.OfType<RichTextBox>().FirstOrDefault();

            if (rtbToHighlight != null)
            {
                PalabrasClave(rtbToHighlight);
            }
        }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        { Application.Exit(); }
        private void ToolSalir_Click(object sender, EventArgs e)
        { Application.Exit(); }
        private void salirToolStripMenuItem1_Click(object sender, EventArgs e)
        { Application.Exit(); }
    }
}