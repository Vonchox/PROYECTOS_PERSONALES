using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Algoritmos
{
    public class FormElipseReal : Form
    {
        private TextBox txtXC, txtYC, txtRX, txtRY;
        private Button btnDibujar;
        private Panel panelDibujo;
        private DataGridView grid;

        private List<Point> puntosCalculados = new List<Point>();
        private bool drawItem = false;
        private int centerX, centerY;

        public FormElipseReal()
        {
            this.Text = "Algoritmo de Elipse (Punto Medio)";
            this.Size = new Size(1100, 550);
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            Label lblXC = new Label() { Text = "Xc:", Location = new Point(10, 20), Width = 30 };
            txtXC = new TextBox() { Location = new Point(40, 15), Width = 50, Text = "225" };

            Label lblYC = new Label() { Text = "Yc:", Location = new Point(100, 20), Width = 30 };
            txtYC = new TextBox() { Location = new Point(130, 15), Width = 50, Text = "225" };

            Label lblRX = new Label() { Text = "Rx:", Location = new Point(190, 20), Width = 30 };
            txtRX = new TextBox() { Location = new Point(220, 15), Width = 50, Text = "150" };

            Label lblRY = new Label() { Text = "Ry:", Location = new Point(280, 20), Width = 30 };
            txtRY = new TextBox() { Location = new Point(310, 15), Width = 50, Text = "100" };

            btnDibujar = new Button()
            {
                Text = "Dibujar Elipse",
                Location = new Point(380, 13),
                Width = 120,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(233, 30, 99), // Pink Moderno
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnDibujar.FlatAppearance.BorderSize = 0;
            btnDibujar.Click += BtnDibujar_Click;

            panelDibujo = new Panel()
            {
                Location = new Point(10, 60),
                Size = new Size(450, 430),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panelDibujo.Paint += PanelDibujo_Paint;

            grid = new DataGridView()
            {
                Location = new Point(470, 60),
                Size = new Size(600, 430),
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(233, 30, 99);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            grid.Columns.Add("step", "Paso");
            grid.Columns.Add("p", "Pk");
            grid.Columns.Add("x", "X");
            grid.Columns.Add("y", "Y");
            grid.Columns.Add("q1", "(x,y)");
            grid.Columns.Add("q2", "(-x,y)");
            grid.Columns.Add("q3", "(x,-y)");
            grid.Columns.Add("q4", "(-x,-y)");

            this.Controls.Add(lblXC); this.Controls.Add(txtXC);
            this.Controls.Add(lblYC); this.Controls.Add(txtYC);
            this.Controls.Add(lblRX); this.Controls.Add(txtRX);
            this.Controls.Add(lblRY); this.Controls.Add(txtRY);
            this.Controls.Add(btnDibujar);
            this.Controls.Add(panelDibujo);
            this.Controls.Add(grid);
        }

        private void BtnDibujar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtXC.Text, out int xc) && int.TryParse(txtYC.Text, out int yc) &&
                int.TryParse(txtRX.Text, out int rx) && int.TryParse(txtRY.Text, out int ry))
            {
                centerX = xc;
                centerY = yc;
                puntosCalculados.Clear();
                grid.Rows.Clear();

                CalcularElipse(xc, yc, rx, ry);

                drawItem = true;
                panelDibujo.Invalidate();
            }
        }

        private void CalcularElipse(int xc, int yc, int rx, int ry)
        {
            float dx, dy, d1, d2;
            int x = 0;
            int y = ry;
            int step = 0;

            // Region 1
            d1 = (ry * ry) - (rx * rx * ry) + (0.25f * rx * rx);
            dx = 2 * ry * ry * x;
            dy = 2 * rx * rx * y;

            while (dx < dy)
            {
                AgregarPuntos(xc, yc, x, y, step++, d1);
                if (d1 < 0)
                {
                    x++;
                    dx = dx + (2 * ry * ry);
                    d1 = d1 + dx + (ry * ry);
                }
                else
                {
                    x++;
                    y--;
                    dx = dx + (2 * ry * ry);
                    dy = dy - (2 * rx * rx);
                    d1 = d1 + dx - dy + (ry * ry);
                }
            }

            // Region 2
            d2 = ((ry * ry) * ((x + 0.5f) * (x + 0.5f))) + ((rx * rx) * ((y - 1) * (y - 1))) - (rx * rx * ry * ry);

            while (y >= 0)
            {
                AgregarPuntos(xc, yc, x, y, step++, d2);
                if (d2 > 0)
                {
                    y--;
                    dy = dy - (2 * rx * rx);
                    d2 = d2 + (rx * rx) - dy;
                }
                else
                {
                    y--;
                    x++;
                    dx = dx + (2 * ry * ry);
                    dy = dy - (2 * rx * rx);
                    d2 = d2 + dx - dy + (rx * rx);
                }
            }
        }

        private void AgregarPuntos(int xc, int yc, int x, int y, int step, float p)
        {
            puntosCalculados.Add(new Point(xc + x, yc - y)); // Q1
            puntosCalculados.Add(new Point(xc - x, yc - y)); // Q2
            puntosCalculados.Add(new Point(xc + x, yc + y)); // Q4
            puntosCalculados.Add(new Point(xc - x, yc + y)); // Q3

            grid.Rows.Add(step, p.ToString("F1"), x, y,
                $"({x}, {y})", $"({-x}, {y})", $"({x}, {-y})", $"({-x}, {-y})");
        }

        private void PanelDibujo_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen gridPen = new Pen(Color.FromArgb(220, 220, 220), 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
            for (int i = 0; i < panelDibujo.Width; i += 10) g.DrawLine(gridPen, i, 0, i, panelDibujo.Height);
            for (int i = 0; i < panelDibujo.Height; i += 10) g.DrawLine(gridPen, 0, i, panelDibujo.Width, i);

            Pen axisPen = new Pen(Color.Gray, 1);
            if (centerX > 0 && centerX < panelDibujo.Width) g.DrawLine(axisPen, centerX, 0, centerX, panelDibujo.Height);
            if (centerY > 0 && centerY < panelDibujo.Height) g.DrawLine(axisPen, 0, centerY, panelDibujo.Width, centerY);

            if (!drawItem || puntosCalculados.Count == 0) return;

            Brush brush = new SolidBrush(Color.FromArgb(233, 30, 99)); // Pink Moderno
            foreach (var pt in puntosCalculados)
            {
                g.FillRectangle(brush, pt.X - 1, pt.Y - 1, 3, 3);
            }
        }
    }
}