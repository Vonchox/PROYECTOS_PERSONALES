using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Algoritmos
{
    public class FormCirculoTrig : Form
    {
        private float xc = 0, yc = 0, radio = 0;
        private bool drawItem = false;
        private TextBox txtXC, txtYC, txtRadio;
        private Button btnDibujar;
        private Panel panelDibujo;
        private DataGridView gridResultados;
        private List<Point> puntosCirculo = new List<Point>();

        public FormCirculoTrig()
        {
            this.Text = "Círculo - Ecuación Trigonométrica";
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
                Text = "Trazar (Trig.)", 
                Location = new Point(290, 13), 
                Width = 100, Height = 30,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(156, 39, 176), // Deep Purple
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
            gridResultados.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(156, 39, 176);
            gridResultados.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridResultados.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            gridResultados.Columns.Add("Angulo", "Θ (rad)");
            gridResultados.Columns.Add("X", "X");
            gridResultados.Columns.Add("Y", "Y");
            gridResultados.Columns.Add("XRedondeado", "X Red.");
            gridResultados.Columns.Add("YRedondeado", "Y Red.");

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
                CalcularTrigonometrica();
                drawItem = true;
                panelDibujo.Invalidate(); 
            }
        }

        private void CalcularTrigonometrica()
        {
            gridResultados.Rows.Clear();
            puntosCirculo.Clear();

            // Paso base: 1/R (una heuristica para evitar dejar huecos)
            double step = (radio > 0) ? (1.0 / radio) : 0.01;

            for (double theta = 0; theta <= 2 * Math.PI; theta += step)
            {
                double x = xc + radio * Math.Cos(theta);
                double y = yc + radio * Math.Sin(theta);

                int xR = (int)Math.Round(x);
                int yR = (int)Math.Round(y);

                Point pt = new Point(xR, yR);
                if (!puntosCirculo.Contains(pt)) // Evitar duplicados
                {
                    puntosCirculo.Add(pt);
                    gridResultados.Rows.Add(theta.ToString("F4"), x.ToString("F2"), y.ToString("F2"), xR, yR);
                }
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
            Brush brush = new SolidBrush(Color.FromArgb(156, 39, 176)); 
            foreach (var p in puntosCirculo) g.FillRectangle(brush, p.X, p.Y, 3, 3);
        }
    }
}