using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Algoritmos
{
    public class FormCirculoPoli : Form
    {
        private float xc = 0, yc = 0, radio = 0;
        private bool drawItem = false;
        private TextBox txtXC, txtYC, txtRadio;
        private Button btnDibujar;
        private Panel panelDibujo;
        private DataGridView gridResultados;
        private List<Point> puntosCirculo = new List<Point>();

        public FormCirculoPoli()
        {
            this.Text = "Círculo - Ecuación Polinomial (y = sqrt(r² - x²))";
            this.Size = new Size(950, 550);
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            Label lblXC = new Label() { Text = "Xc:", Location = new Point(10, 20), Width = 30 };
            txtXC = new TextBox() { Location = new Point(40, 15), Width = 50, Text = "225" };

            Label lblYC = new Label() { Text = "Yc:", Location = new Point(100, 20), Width = 30 };
            txtYC = new TextBox() { Location = new Point(130, 15), Width = 50, Text = "215" };

            Label lblRadio = new Label() { Text = "R:", Location = new Point(200, 20), Width = 20 };
            txtRadio = new TextBox() { Location = new Point(220, 15), Width = 50, Text = "100" };

            btnDibujar = new Button() { 
                Text = "Trazar (Polin.)", 
                Location = new Point(290, 13), 
                Width = 100, Height = 30,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(233, 30, 99), // Pink
                ForeColor = Color.White, Cursor = Cursors.Hand
            };
            btnDibujar.FlatAppearance.BorderSize = 0;
            btnDibujar.Click += BtnDibujar_Click;

            panelDibujo = new Panel()
            {
                Location = new Point(10, 60), Size = new Size(450, 430),
                BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle
            };
            panelDibujo.Paint += PanelDibujo_Paint;

            gridResultados = new DataGridView()
            {
                Location = new Point(470, 60), Size = new Size(450, 430),
                ReadOnly = true, AllowUserToAddRows = false,
                RowHeadersVisible = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false
            };
            gridResultados.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(233, 30, 99);
            gridResultados.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridResultados.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            gridResultados.Columns.Add("XRel", "X Relativo");
            gridResultados.Columns.Add("YRel", "Y Relativo (±sqrt)");
            gridResultados.Columns.Add("XAbs", "X Absoluto");
            gridResultados.Columns.Add("YAbs1", "Y Absoluto (Top)");
            gridResultados.Columns.Add("YAbs2", "Y Absoluto (Bottom)");

            this.Controls.Add(lblXC); this.Controls.Add(txtXC);
            this.Controls.Add(lblYC); this.Controls.Add(txtYC);
            this.Controls.Add(lblRadio); this.Controls.Add(txtRadio);
            this.Controls.Add(btnDibujar); this.Controls.Add(panelDibujo);
            this.Controls.Add(gridResultados);
        }

        private void BtnDibujar_Click(object? sender, EventArgs e)
        {
            if (float.TryParse(txtXC.Text, out xc) && float.TryParse(txtYC.Text, out yc) && float.TryParse(txtRadio.Text, out radio))
            {
                CalcularPolinomial();
                drawItem = true;
                panelDibujo.Invalidate(); 
            }
        }

        private void CalcularPolinomial()
        {
            gridResultados.Rows.Clear();
            puntosCirculo.Clear();

            // En la ecuacion polinomial calculamos Y para cada X de 0 a R (4º cuadrante) y luego espejamos
            double r2 = radio * radio;

            // Para hacer una linea continua es mejor ir desde X = -R hasta X = R
            // Si el radio es grande, es posible que queden huecos donde la pendiente es > 1 (lados derecho e izq)
            // pero es el comportamiento nautral de iterar solo sobre X.
            for (float x = -radio; x <= radio; x++)
            {
                double y = Math.Sqrt(r2 - (x * x));

                int xAbs = (int)Math.Round(xc + x);
                int yTop = (int)Math.Round(yc - y); // Superior
                int yBot = (int)Math.Round(yc + y); // Inferior

                puntosCirculo.Add(new Point(xAbs, yTop));
                puntosCirculo.Add(new Point(xAbs, yBot));

                gridResultados.Rows.Add(x.ToString(), y.ToString("F2"), xAbs, yTop, yBot);
            }
        }

        private void PanelDibujo_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int gridSize = 10; 

            Pen gridPen = new Pen(Color.FromArgb(220, 220, 220), 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
            for (int i = 0; i < panelDibujo.Width; i += gridSize) g.DrawLine(gridPen, i, 0, i, panelDibujo.Height);
            for (int i = 0; i < panelDibujo.Height; i += gridSize) g.DrawLine(gridPen, 0, i, panelDibujo.Width, i);

            if (!drawItem || puntosCirculo.Count == 0) return;
            Brush brush = new SolidBrush(Color.FromArgb(233, 30, 99)); 
            foreach (var p in puntosCirculo) g.FillRectangle(brush, p.X, p.Y, 3, 3);
        }
    }
}