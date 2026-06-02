using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Algoritmos
{
    public class FormLineaBasica : Form
    {
        private float x1 = 0, y1 = 0, x2 = 0, y2 = 0;
        private bool drawLine = false;

        private TextBox txtX1, txtY1, txtX2, txtY2;
        private Button btnDibujar;
        private Panel panelDibujo;
        private DataGridView gridResultados;
        private List<Point> puntosLinea = new List<Point>();

        public FormLineaBasica()
        {
            this.Text = "Línea - Ecuación Básica (y = mx + b)";
            this.Size = new Size(950, 550);
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            Label lblX1 = new Label() { Text = "X1:", Location = new Point(10, 20), Width = 30 };
            txtX1 = new TextBox() { Location = new Point(40, 15), Width = 50, Text = "50" };

            Label lblY1 = new Label() { Text = "Y1:", Location = new Point(100, 20), Width = 30 };
            txtY1 = new TextBox() { Location = new Point(130, 15), Width = 50, Text = "50" };

            Label lblX2 = new Label() { Text = "X2:", Location = new Point(200, 20), Width = 30 };
            txtX2 = new TextBox() { Location = new Point(230, 15), Width = 50, Text = "300" };

            Label lblY2 = new Label() { Text = "Y2:", Location = new Point(300, 20), Width = 30 };
            txtY2 = new TextBox() { Location = new Point(330, 15), Width = 50, Text = "200" };

            btnDibujar = new Button() { 
                Text = "Dibujar (y=mx+b)", 
                Location = new Point(400, 13), 
                Width = 120, Height = 30,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(63, 81, 181), // Indigo
                ForeColor = Color.White, Cursor = Cursors.Hand
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

            gridResultados = new DataGridView()
            {
                Location = new Point(470, 60),
                Size = new Size(450, 430),
                ReadOnly = true, AllowUserToAddRows = false,
                RowHeadersVisible = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false
            };
            gridResultados.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(63, 81, 181);
            gridResultados.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridResultados.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            gridResultados.Columns.Add("Paso", "#");
            gridResultados.Columns.Add("X", "X");
            gridResultados.Columns.Add("Y", "Y");
            gridResultados.Columns.Add("XRedondeado", "X Redondeado");
            gridResultados.Columns.Add("YRedondeado", "Y Redondeado");

            this.Controls.Add(lblX1); this.Controls.Add(txtX1);
            this.Controls.Add(lblY1); this.Controls.Add(txtY1);
            this.Controls.Add(lblX2); this.Controls.Add(txtX2);
            this.Controls.Add(lblY2); this.Controls.Add(txtY2);
            this.Controls.Add(btnDibujar); this.Controls.Add(panelDibujo);
            this.Controls.Add(gridResultados);
        }

        private void BtnDibujar_Click(object? sender, EventArgs e)
        {
            if (float.TryParse(txtX1.Text, out x1) && float.TryParse(txtY1.Text, out y1) &&
                float.TryParse(txtX2.Text, out x2) && float.TryParse(txtY2.Text, out y2))
            {
                CalcularEcuacionBasica();
                drawLine = true;
                panelDibujo.Invalidate(); 
            }
            else
            {
                MessageBox.Show("Ingrese coordenadas numéricas válidas.");
            }
        }

        private void CalcularEcuacionBasica()
        {
            gridResultados.Rows.Clear();
            puntosLinea.Clear();

            int step = 0;
            float dx = x2 - x1;
            float dy = y2 - y1;

            if (dx == 0) // Linea vertical
            {
                float minY = Math.Min(y1, y2);
                float maxY = Math.Max(y1, y2);
                for (float currY = minY; currY <= maxY; currY++)
                {
                    puntosLinea.Add(new Point((int)Math.Round(x1), (int)Math.Round(currY)));
                    gridResultados.Rows.Add(step++, x1.ToString("F2"), currY.ToString("F2"), (int)Math.Round(x1), (int)Math.Round(currY));
                }
            }
            else
            {
                float m = dy / dx; // Pendiente
                float b = y1 - m * x1; // Intercepto y = mx + b -> b = y - mx

                if (Math.Abs(m) <= 1) // Avanza en X
                {
                    float minX = Math.Min(x1, x2);
                    float maxX = Math.Max(x1, x2);
                    for (float currX = minX; currX <= maxX; currX++)
                    {
                        float currY = m * currX + b; // y = mx + b
                        puntosLinea.Add(new Point((int)Math.Round(currX), (int)Math.Round(currY)));
                        gridResultados.Rows.Add(step++, currX.ToString("F2"), currY.ToString("F2"), (int)Math.Round(currX), (int)Math.Round(currY));
                    }
                }
                else  // Avanza en Y
                {
                    float minY = Math.Min(y1, y2);
                    float maxY = Math.Max(y1, y2);
                    for (float currY = minY; currY <= maxY; currY++)
                    {
                        float currX = (currY - b) / m; // x = (y - b) / m
                        puntosLinea.Add(new Point((int)Math.Round(currX), (int)Math.Round(currY)));
                        gridResultados.Rows.Add(step++, currX.ToString("F2"), currY.ToString("F2"), (int)Math.Round(currX), (int)Math.Round(currY));
                    }
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

            if (!drawLine || puntosLinea.Count == 0) return;

            Brush brush = new SolidBrush(Color.FromArgb(63, 81, 181)); 
            foreach (var point in puntosLinea)
            {
                g.FillRectangle(brush, point.X, point.Y, 3, 3);
            }
        }
    }
}