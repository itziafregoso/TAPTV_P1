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

namespace TAPTV_P1
{
    public partial class Form1 : Form
    {
        Panel panelNumerosLinea;
        Color colorPalabrasReservadas = Color.Blue;
        Color colorCadenas = Color.Red;
        Color colorComentarios = Color.Green;
        Color colorTexto = Color.Black;

        private string[] palabrasReservadas = new string[]
        {
            "abstract", "add", "alias", "as", "ascending", "async", "await",
            "base", "bool", "break", "by",
            "byte", "case", "catch", "char", "checked",
            "class", "const", "continue",
            "decimal", "default", "delegate", "descending", "do", "double",
            "dynamic", "else", "enum", "equals", "event",
            "explicit", "extern", "false", "finally", "fixed", "float",
            "for", "foreach", "from", "get", "goto",
            "global", "if", "implicit", "in", "int", "interface", "internal",
            "into", "is", "join", "let", "lock", "long",
            "namespace", "new", "null",
            "object", "operator", "orderby", "out", "override",
            "params", "partial", "private", "protected", "public",
            "readonly", "ref", "remove", "return",
            "sbyte", "sealed", "select", "set", "short",
            "sizeof", "stackalloc", "static", "string", "struct",
            "switch", "this", "throw", "true", "try",
            "typeof", "uint", "ulong", "unchecked", "unsafe",
            "ushort", "using", "value", "var", "virtual", "void",
            "volatile", "when", "where", "while", "yield",

            "SELECT", "FROM", "WHERE", "INSERT", "UPDATE", "DELETE", "CREATE", "DROP", "ALTER", "TABLE",
            "INDEX", "VIEW", "JOIN", "INNER", "OUTER", "LEFT", "RIGHT", "FULL", "AND", "OR", "NOT", "IN",
            "BETWEEN", "LIKE", "IS", "NULL", "TRUE", "FALSE", "GROUP", "HAVING", "ORDER", "BY", "ASC",
            "DESC", "LIMIT", "OFFSET", "DISTINCT", "AS", "INTO", "VALUES", "SET", "UNION", "EXCEPT",
            "INTERSECT", "WITH", "TRANSACTION", "COMMIT", "ROLLBACK", "SAVEPOINT", "CAST", "CONVERT",
            "INNER JOIN", "OUTER JOIN", "CROSS JOIN", "NATURAL JOIN", "UNION ALL", "NULLIF", "COALESCE",
            "CASE", "WHEN", "THEN", "ELSE", "END", "CHECK", "DEFAULT", "PRIMARY", "KEY", "FOREIGN",
            "REFERENCES", "UNIQUE", "AUTO_INCREMENT", "NOT NULL", "ENUM", "TEXT", "BLOB", "DATE", "TIME",
            "DATETIME", "YEAR", "BOOLEAN", "INTEGER", "FLOAT", "DECIMAL", "CHAR", "VARCHAR", "BINARY",
            "VARBINARY", "TINYINT", "SMALLINT", "BIGINT", "NUMERIC", "UUID", "STRING", "TIMESTAMP",
            "CURRENT_TIMESTAMP", "EXISTS", "ALL", "ANY"
        };

        public Form1()
        {
            InitializeComponent();
            toolStripStatusLabel5.Text = "";
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            AbrirArchivo();
        }

        private void toolStripAbrirArchivo_Click(object sender, EventArgs e)
        {
            AbrirArchivo();
        }

        private async void toolStripCerrarArchivo_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                toolStripProgressBar1.Visible = true; 
                toolStripProgressBar1.Style = ProgressBarStyle.Continuous; 
                toolStripProgressBar1.Value = 0; 
                toolStripStatusLabel5.Text = "Cerrando archivo..."; 
                await Task.Run(() =>
                {
                    for (int i = 0; i <= 100; i += 20) 
                    {
                        Invoke(new Action(() => toolStripProgressBar1.Value = i));
                        Task.Delay(100).Wait();
                    }
                });

                tabControl1.TabPages.Remove(tabControl1.SelectedTab);

                toolStripProgressBar1.Value = 100; 
                await Task.Delay(500); 
                toolStripProgressBar1.Visible = false; 
                toolStripStatusLabel5.Text = "Archivo cerrado."; 
                MessageBox.Show("Archivo cerrado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No hay ninguna pestaña abierta para cerrar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private async void toolStripGuardarArchivo_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                SaveFileDialog dialogoGuardarArchivo = new SaveFileDialog();
                dialogoGuardarArchivo.Filter = "Archivos C#|*.cs|Archivos SQL|*.sql";

                if (dialogoGuardarArchivo.ShowDialog() == DialogResult.OK)
                {
                    toolStripProgressBar1.Visible = true;
                    toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
                    toolStripProgressBar1.Value = 0;
                    toolStripStatusLabel5.Text = "Guardando archivo...";

                    await Task.Run(() =>
                    {
                        for (int i = 0; i <= 100; i += 20)
                        {
                            Invoke(new Action(() => toolStripProgressBar1.Value = i));
                            Task.Delay(100).Wait();
                        }
                        File.WriteAllText(dialogoGuardarArchivo.FileName, richTextBox.Text);
                    });

                    toolStripProgressBar1.Value = 100;
                    Task.Delay(500).Wait();
                    toolStripProgressBar1.Visible = false;
                    toolStripStatusLabel5.Text = "Archivo guardado exitosamente.";
                    MessageBox.Show("Archivo guardado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para guardar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void AbrirArchivo()
        {
            OpenFileDialog dialogoAbrirArchivo = new OpenFileDialog();
            dialogoAbrirArchivo.Filter = "Archivos C#|*.cs|Archivos SQL|*.sql";

            if (dialogoAbrirArchivo.ShowDialog() == DialogResult.OK)
            {
                // Reiniciar los colores a sus valores por defecto
                colorPalabrasReservadas = Color.Blue;
                colorCadenas = Color.Red;
                colorComentarios = Color.Green;
                colorTexto = Color.Black;

                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
                toolStripProgressBar1.Value = 0;
                toolStripStatusLabel5.Text = "Cargando archivo...";

                string contenidoArchivo = await Task.Run(() =>
                {
                    for (int i = 0; i <= 100; i += 20)
                    {
                        Invoke(new Action(() => toolStripProgressBar1.Value = i));
                        Task.Delay(100).Wait();
                    }
                    return File.ReadAllText(dialogoAbrirArchivo.FileName);
                });

                string contenidoFormateado = FormatearTextoConGuia(contenidoArchivo);

                TabPage nuevaPestana = new TabPage(Path.GetFileName(dialogoAbrirArchivo.FileName));
                tabControl1.TabPages.Add(nuevaPestana);

                RichTextBox richTextBox = new RichTextBox
                {
                    Dock = DockStyle.Fill,
                    Font = new Font("Consolas", 10),
                    WordWrap = false,
                    ScrollBars = RichTextBoxScrollBars.ForcedBoth,
                    Text = contenidoFormateado
                };

                richTextBox.VScroll += (s, args) => panelNumerosLinea.Invalidate();
                richTextBox.TextChanged += (s, args) => panelNumerosLinea.Invalidate();
                richTextBox.Resize += (s, args) => panelNumerosLinea.Invalidate();
                richTextBox.SelectionChanged += (s, args) => MostrarInformacionArchivo(richTextBox);
                richTextBox.TextChanged += (s, args) => MostrarInformacionArchivo(richTextBox);
                richTextBox.MouseWheel += (s, args) => RichTextBox_MouseWheel(s, args, richTextBox);
                richTextBox.TextChanged += (s, e) => richTextBox.Invalidate();

                panelNumerosLinea = new Panel
                {
                    Dock = DockStyle.Left,
                    Width = 35
                };
                panelNumerosLinea.Paint += (s, pe) => DibujarNumerosLinea(pe, richTextBox);

                nuevaPestana.Controls.Add(richTextBox);
                nuevaPestana.Controls.Add(panelNumerosLinea);

                tabControl1.SelectedTab = nuevaPestana;

                toolStripProgressBar1.Value = 100;
                await Task.Delay(500);
                toolStripProgressBar1.Visible = false;
                toolStripStatusLabel5.Text = "Archivo cargado correctamente.";

                ResaltarPalabrasReservadasCadenasYComentarios(richTextBox);
                richTextBox.Invalidate();
            }
        }




        private string FormatearTextoConGuia(string contenido)
        {
            StringBuilder textoConGuia = new StringBuilder();
            int nivelIndentacion = 0;

            string[] lineas = contenido.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string linea in lineas)
            {
                string lineaActual = linea.Trim();

                if (lineaActual.StartsWith("}"))
                {
                    nivelIndentacion = Math.Max(0, nivelIndentacion - 1);
                }

                string guiaIndentacion = string.Concat(Enumerable.Repeat("|    ", nivelIndentacion));
                textoConGuia.AppendLine($"{guiaIndentacion}{lineaActual}");

                if (lineaActual.EndsWith("{"))
                {
                    nivelIndentacion++;
                }
            }

            return textoConGuia.ToString();
        }




        private void RichTextBox_MouseWheel(object sender, MouseEventArgs e, RichTextBox richTextBox)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (e.Delta > 0)
                {
                    richTextBox.ZoomFactor += 0.1f;
                }
                else if (e.Delta < 0)
                {
                    richTextBox.ZoomFactor = Math.Max(0.1f, richTextBox.ZoomFactor - 0.1f);
                }
                panelNumerosLinea.Invalidate();
            }
        }

        private void DibujarNumerosLinea(PaintEventArgs e, RichTextBox richTextBox)
        {
            float zoom = richTextBox.ZoomFactor;
            Font fontOriginal = richTextBox.Font;
            Font fontZoom = new Font(fontOriginal.FontFamily, fontOriginal.Size * zoom);

            int primeraLinea = richTextBox.GetLineFromCharIndex(richTextBox.GetCharIndexFromPosition(new Point(0, 0)));
            int ultimaLinea = richTextBox.GetLineFromCharIndex(richTextBox.GetCharIndexFromPosition(new Point(0, richTextBox.ClientRectangle.Height)));

            for (int i = primeraLinea; i <= ultimaLinea + 1; i++)
            {
                int lineaNumero = i + 1;
                int charIndice = richTextBox.GetFirstCharIndexFromLine(i);
                Point charPosicion = richTextBox.GetPositionFromCharIndex(charIndice);
                e.Graphics.DrawString(lineaNumero.ToString(), fontZoom, Brushes.Gray, new PointF(0, charPosicion.Y * zoom));
            }
        }


        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripFormato_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                using (FontDialog fontDialog = new FontDialog())
                {
                    fontDialog.Font = richTextBox.SelectionFont;
                    if (fontDialog.ShowDialog() == DialogResult.OK)
                    {
                        richTextBox.SelectAll();
                        richTextBox.SelectionFont = fontDialog.Font;
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para cambiar el formato.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripColorFondo_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = richTextBox.BackColor;
                    colorDialog.FullOpen = true; 
                    colorDialog.ShowHelp = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        richTextBox.BackColor = colorDialog.Color; 
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para cambiar el color de fondo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ResaltarPalabrasReservadas(RichTextBox richTextBox)
        {
            string texto = richTextBox.Text;

            foreach (string palabra in palabrasReservadas)
            {
                int indice = 0;
                while ((indice = texto.IndexOf(palabra, indice)) != -1)
                {
                    bool esPalabraCompleta =
                        (indice == 0 || !char.IsLetterOrDigit(texto[indice - 1])) &&
                        (indice + palabra.Length == texto.Length || !char.IsLetterOrDigit(texto[indice + palabra.Length]));

                    if (esPalabraCompleta)
                    {
                        richTextBox.Select(indice, palabra.Length);
                        richTextBox.SelectionColor = colorPalabrasReservadas;
                    }
                    indice += palabra.Length;
                }
            }
            richTextBox.Select(0, 0);
        }

        private void ColorTextoBase(RichTextBox richTextBox)
        {
            string texto = richTextBox.Text;
            richTextBox.SelectionColor = colorTexto;
        }

        private void ResaltarPalabrasReservadasCadenasYComentarios(RichTextBox richTextBox)
        {

            richTextBox.SelectAll();
            richTextBox.SelectionColor = colorTexto; 

            Dictionary<int, Color> coloresOriginales = new Dictionary<int, Color>();

            string texto = richTextBox.Text;
            int inicioCadena = -1;
            bool dentroDeCadena = false;

            for (int i = 0; i < texto.Length; i++)
            {
                if (texto[i] == '\"')
                {
                    if (dentroDeCadena)
                    {
                        for (int j = inicioCadena; j <= i; j++)
                        {
                            coloresOriginales[j] = colorCadenas; 
                        }
                        richTextBox.Select(inicioCadena, i - inicioCadena + 1);
                        richTextBox.SelectionColor = colorCadenas;
                        dentroDeCadena = false;
                    }
                    else
                    {
                        inicioCadena = i;
                        dentroDeCadena = true;
                    }
                }
            }

            for (int i = 0; i < texto.Length - 1; i++)
            {
                if (texto[i] == '/' && texto[i + 1] == '/') 
                {
                    int inicioComentario = i;
                    int finComentario = texto.IndexOf('\n', i);
                    if (finComentario == -1) finComentario = texto.Length;

                    for (int j = inicioComentario; j < finComentario; j++)
                    {
                        coloresOriginales[j] = colorComentarios;
                    }

                    richTextBox.Select(inicioComentario, finComentario - inicioComentario);
                    richTextBox.SelectionColor = colorComentarios;
                    i = finComentario;
                }
                else if (texto[i] == '/' && texto[i + 1] == '*') 
                {
                    int inicioComentario = i;
                    int finComentario = texto.IndexOf("*/", i + 2);
                    if (finComentario == -1) finComentario = texto.Length - 1; 

                    for (int j = inicioComentario; j <= finComentario + 1; j++)
                    {
                        coloresOriginales[j] = colorComentarios;
                    }

                    richTextBox.Select(inicioComentario, finComentario - inicioComentario + 2);
                    richTextBox.SelectionColor = colorComentarios;
                    i = finComentario + 1;
                }
            }

            foreach (string palabra in palabrasReservadas)
            {
                int indice = 0;
                while ((indice = texto.IndexOf(palabra, indice)) != -1)
                {
                    bool esPalabraCompleta =
                        (indice == 0 || !char.IsLetterOrDigit(texto[indice - 1])) &&
                        (indice + palabra.Length == texto.Length || !char.IsLetterOrDigit(texto[indice + palabra.Length]));

                    if (esPalabraCompleta)
                    {
                        bool palabraDentroDeCadenaOComentario = false;

                        for (int j = indice; j < indice + palabra.Length; j++)
                        {
                            if (coloresOriginales.ContainsKey(j))
                            {
                                palabraDentroDeCadenaOComentario = true;
                                break;
                            }
                        }

                        if (!palabraDentroDeCadenaOComentario)
                        {
                            richTextBox.Select(indice, palabra.Length);
                            richTextBox.SelectionColor = colorPalabrasReservadas;
                        }
                    }
                    indice += palabra.Length;
                }
            }

            richTextBox.Select(0, 0); 
        }



        private void ResaltarCadenas(RichTextBox richTextBox)
        {
            string texto = richTextBox.Text;
            int inicioCadena = -1;
            bool dentroDeCadena = false;
            for (int i = 0; i < texto.Length; i++)
            {
                if (texto[i] == '\"')
                {
                    if (dentroDeCadena)
                    {
                        richTextBox.Select(inicioCadena, i - inicioCadena + 1);
                        richTextBox.SelectionColor = colorCadenas;
                        dentroDeCadena = false;
                    }
                    else
                    {
                        inicioCadena = i;
                        dentroDeCadena = true;
                    }
                }
            }
            richTextBox.Select(0, 0);
        }


        private void toolStripColorCadenas_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = colorCadenas;
                    colorDialog.FullOpen = true;
                    colorDialog.ShowHelp = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        colorCadenas = colorDialog.Color;

                        ResaltarPalabrasReservadasCadenasYComentarios(richTextBox);
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para cambiar el color de las cadenas.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void toolStripPalabraReservada_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = colorPalabrasReservadas;
                    colorDialog.FullOpen = true;
                    colorDialog.ShowHelp = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        colorPalabrasReservadas = colorDialog.Color; 

                        ResaltarPalabrasReservadasCadenasYComentarios(richTextBox);
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para cambiar el color de las palabras reservadas.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void toolStripColorCometarios_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = colorComentarios;
                    colorDialog.FullOpen = true;
                    colorDialog.ShowHelp = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        colorComentarios = colorDialog.Color;

                        ResaltarPalabrasReservadasCadenasYComentarios(richTextBox);
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para cambiar el color de los comentarios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

       

        private void MostrarInformacionArchivo(RichTextBox richTextBox)
        {
            int totalLineas = richTextBox.Lines.Length;
            int lineaActual = richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart) + 1;
            int totalCaracteres = richTextBox.Text.Length;

            toolStripStatusLabel5.Text = $"Línea: {lineaActual} / {totalLineas} | Caracteres: {totalCaracteres}";
        }

        private void toolStripDeshacer_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) return;

            RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

            if (richTextBox == null) return;

            if (richTextBox.CanUndo)
            {
                richTextBox.Undo();

                if (richTextBox.CanRedo)
                {
                    toolStripRehacer.Enabled = true;
                }
            }
        }


        private void toolStripRehacer_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) return;

            RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

            if (richTextBox == null) return;

            if (richTextBox.CanRedo)
            {
                richTextBox.Redo();
            }
        }


        private void toolStripCopiar_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() is RichTextBox richTextBox)
            {
                if (!string.IsNullOrEmpty(richTextBox.SelectedRtf))
                {
                    Clipboard.SetText(richTextBox.SelectedRtf, TextDataFormat.Rtf); // Copia con formato
                }
            }
        }


        private void toolStripCortar_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                if (richTextBox.SelectedText != string.Empty)
                {
                    richTextBox.Cut(); 
                }
            }
        }

        private void toolStripPegar_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() is RichTextBox richTextBox)
            {
                if (Clipboard.ContainsText(TextDataFormat.Rtf)) // Pegar con formato si existe
                {
                    richTextBox.SelectedRtf = Clipboard.GetText(TextDataFormat.Rtf);
                }
                else if (Clipboard.ContainsText(TextDataFormat.Text)) // Si no hay formato, pegar como texto plano
                {
                    richTextBox.SelectedText = Clipboard.GetText(TextDataFormat.Text);
                }
            }
        }



        private void toolStripZoomMas_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = tabControl1.SelectedTab?.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (rtb != null && rtb.ZoomFactor < 5.0f)
            {
                rtb.ZoomFactor += 0.1f;
                panelNumerosLinea.Invalidate();
            }
        }

        private void toolStripZoomMenos_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = tabControl1.SelectedTab?.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (rtb != null && rtb.ZoomFactor > 0.5f)
            {
                rtb.ZoomFactor -= 0.1f;
                panelNumerosLinea.Invalidate();
            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirArchivo();
        }

        private async void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                SaveFileDialog dialogoGuardarArchivo = new SaveFileDialog();
                dialogoGuardarArchivo.Filter = "Archivos C#|*.cs|Archivos SQL|*.sql";

                if (dialogoGuardarArchivo.ShowDialog() == DialogResult.OK)
                {
                    toolStripProgressBar1.Visible = true;
                    toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
                    toolStripProgressBar1.Value = 0;
                    toolStripStatusLabel5.Text = "Guardando archivo...";

                    await Task.Run(() =>
                    {
                        for (int i = 0; i <= 100; i += 20)
                        {
                            Invoke(new Action(() => toolStripProgressBar1.Value = i));
                            Task.Delay(100).Wait();
                        }
                        File.WriteAllText(dialogoGuardarArchivo.FileName, richTextBox.Text);
                    });

                    toolStripProgressBar1.Value = 100;
                    Task.Delay(500).Wait();
                    toolStripProgressBar1.Visible = false;
                    toolStripStatusLabel5.Text = "Archivo guardado exitosamente.";
                    MessageBox.Show("Archivo guardado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para guardar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
                toolStripProgressBar1.Value = 0;
                toolStripStatusLabel5.Text = "Cerrando archivo...";
                await Task.Run(() =>
                {
                    for (int i = 0; i <= 100; i += 20)
                    {
                        Invoke(new Action(() => toolStripProgressBar1.Value = i));
                        Task.Delay(100).Wait();
                    }
                });

                tabControl1.TabPages.Remove(tabControl1.SelectedTab);

                toolStripProgressBar1.Value = 100;
                await Task.Delay(500);
                toolStripProgressBar1.Visible = false;
                toolStripStatusLabel5.Text = "Archivo cerrado.";
                MessageBox.Show("Archivo cerrado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No hay ninguna pestaña abierta para cerrar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                if (richTextBox.CanUndo)
                {
                    richTextBox.Undo();
                }
            }
        }

        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                if (richTextBox.CanRedo)
                {
                    richTextBox.Redo();
                }
            }
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                if (richTextBox.SelectedText != string.Empty)
                {
                    richTextBox.Copy();
                }
            }
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                if (richTextBox.SelectedText != string.Empty)
                {
                    richTextBox.Cut();
                }
            }
        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() is RichTextBox richTextBox)
            {
                richTextBox.Paste();

                richTextBox.SelectAll();
                richTextBox.SelectionColor = Color.Black;

                richTextBox.Select(0, 0);

                ResaltarPalabrasReservadasCadenasYComentarios(richTextBox);
            }
        }

        private void selecionarTodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = tabControl1.SelectedTab?.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (rtb != null)
            {
                rtb.SelectAll();
            }
        }

        private void eliminarTodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = tabControl1.SelectedTab?.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (rtb != null)
            {
                rtb.Clear(); 
            }
        }

        private void fuenteYTamañoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                using (FontDialog fontDialog = new FontDialog())
                {
                    fontDialog.Font = richTextBox.SelectionFont;
                    if (fontDialog.ShowDialog() == DialogResult.OK)
                    {
                        richTextBox.SelectAll();
                        richTextBox.SelectionFont = fontDialog.Font;
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para cambiar el formato.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void colorPalabraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = colorPalabrasReservadas;
                    colorDialog.FullOpen = true;
                    colorDialog.ShowHelp = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        colorPalabrasReservadas = colorDialog.Color;

                        ResaltarPalabrasReservadasCadenasYComentarios(richTextBox);
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para cambiar el color de las palabras reservadas.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void colorCadenasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = colorCadenas;
                    colorDialog.FullOpen = true;
                    colorDialog.ShowHelp = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        colorCadenas = colorDialog.Color;

                        ResaltarPalabrasReservadasCadenasYComentarios(richTextBox);
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para cambiar el color de las cadenas.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void colorComentariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = colorComentarios;
                    colorDialog.FullOpen = true;
                    colorDialog.ShowHelp = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        colorComentarios = colorDialog.Color;

                        ResaltarPalabrasReservadasCadenasYComentarios(richTextBox);
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para cambiar el color de los comentarios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void colorFondoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = richTextBox.BackColor;
                    colorDialog.FullOpen = true;
                    colorDialog.ShowHelp = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        richTextBox.BackColor = colorDialog.Color;
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para cambiar el color de fondo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripSeleccionarTodo_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = tabControl1.SelectedTab?.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (rtb != null)
            {
                rtb.SelectAll();
            }
        }


     
        private void toolStripBorrar_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) return;
            RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (richTextBox == null) return;

            richTextBox.SelectAll();
            richTextBox.SelectedText = "";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault() != null)
            {
                using (ColorDialog colorDialog = new ColorDialog())
                {
                    RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                    colorDialog.Color = colorTexto;
                    colorDialog.FullOpen = true;
                    colorDialog.ShowHelp = true;
                    richTextBox.SelectAll();
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        colorTexto = colorDialog.Color;
                        ColorTextoBase(richTextBox);
                        ResaltarCadenas(richTextBox);
                        ResaltarPalabrasReservadas(richTextBox);
                        ResaltarPalabrasReservadasCadenasYComentarios(richTextBox);
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay ningún archivo abierto para cambiar el formato.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}


