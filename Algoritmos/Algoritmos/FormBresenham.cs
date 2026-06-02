using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Algoritmos
{
    public class FormBresenham : Form
    {
        private TextBox txtX1, txtY1, txtX2, txtY2;
        private Button btnDibujar;
        private Panel panelDibujo;
        private DataGridView grid;

        private List<Point> puntosLinea = new List<Point>();
        private bool drawLine = false;

        public FormBresenham()
        {
            this.Text = "Algoritmo de Bresenham (Línea)";
            this.Size = new Size(950, 550);
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            Label lblX1 = new Label() { Text = "X1:", Location = new Point(10, 20), Width = 30 };
            txtX1 = new TextBox() { Location = new Point(40, 15), Width = 50, Text = "20" };

            Label lblY1 = new Label() { Text = "Y1:", Location = new Point(100, 20), Width = 30 };
            txtY1 = new TextBox() { Location = new Point(130, 15), Width = 50, Text = "20" };

            Label lblX2 = new Label() { Text = "X2:", Location = new Point(200, 20), Width = 30 };
            txtX2 = new TextBox() { Location = new Point(230, 15), Width = 50, Text = "300" };

            Label lblY2 = new Label() { Text = "Y2:", Location = new Point(300, 20), Width = 30 };
            txtY2 = new TextBox() { Location = new Point(330, 15), Width = 50, Text = "150" };

            btnDibujar = new Button()
            {
                Text = "Dibujar Bresenham",
                Location = new Point(400, 13),
                Width = 150,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 120, 215),
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
                Size = new Size(450, 430),
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            grid.Columns.Add("k", "k");
            grid.Columns.Add("pk", "Pk");
            grid.Columns.Add("x", "X+1");
            grid.Columns.Add("y", "Y+1");

            this.Controls.Add(lblX1); this.Controls.Add(txtX1);
            this.Controls.Add(lblY1); this.Controls.Add(txtY1);
            this.Controls.Add(lblX2); this.Controls.Add(txtX2);
            this.Controls.Add(lblY2); this.Controls.Add(txtY2);
            this.Controls.Add(btnDibujar);
            this.Controls.Add(panelDibujo);
            this.Controls.Add(grid);
        }

        private void BtnDibujar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtX1.Text, out int x1) && int.TryParse(txtY1.Text, out int y1) &&
                int.TryParse(txtX2.Text, out int x2) && int.TryParse(txtY2.Text, out int y2))
            {
                CalcularBresenham(x1, y1, x2, y2);
                drawLine = true;
                panelDibujo.Invalidate();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese valores enteros.");
            }
        }

        private void CalcularBresenham(int x1, int y1, int x2, int y2)
        {
            grid.Rows.Clear();
            puntosLinea.Clear();

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int err = dx - dy;

            int x = x1;
            int y = y1;
            int k = 0;

            while (true)
            {
                puntosLinea.Add(new Point(x, y));
                grid.Rows.Add(k, err, x, y);

                if (x == x2 && y == y2) break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y += sy;
                }
                k++;
            }
        }

        private void PanelDibujo_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen gridPen = new Pen(Color.FromArgb(220, 220, 220), 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
            for (int i = 0; i < panelDibujo.Width; i += 10) g.DrawLine(gridPen, i, 0, i, panelDibujo.Height);
            for (int i = 0; i < panelDibujo.Height; i += 10) g.DrawLine(gridPen, 0, i, panelDibujo.Width, i);

            if (!drawLine || puntosLinea.Count == 0) return;

            Brush brush = new SolidBrush(Color.FromArgb(0, 150, 136)); 
            foreach (var pt in puntosLinea)
            {
                g.FillRectangle(brush, pt.X, pt.Y, 3, 3);
            }
        }
    }
}