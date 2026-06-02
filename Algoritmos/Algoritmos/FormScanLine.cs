using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Algoritmos
{
    public class FormScanLine : Form
    {
        private Bitmap lienzo;
        private Graphics g;

        private List<Point> poligonoActual = new List<Point>();
        private bool poligonocerrado = false;
        private Color colorReemplazo = Color.Purple;

        private Panel panelDibujo;
        private Button btnLimpiar;
        private Button btnCerrarPoligono;
        private Button btnScan;
        private Label lblEstado;

        public FormScanLine()
        {
            this.Text = "Algoritmo ScanLine Fill";
            this.Size = new Size(950, 550);
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            btnCerrarPoligono = new Button() { 
                Text = "Cerrar Polígono", Location = new Point(10, 13), 
                Width = 120, Height = 30, FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, 87, 34), ForeColor = Color.White, Cursor = Cursors.Hand
            };
            btnCerrarPoligono.FlatAppearance.BorderSize = 0;
            btnCerrarPoligono.Click += BtnCerrarPoligono_Click;

            btnScan = new Button() { 
                Text = "Ejecutar ScanLine", Location = new Point(140, 13), 
                Width = 150, Height = 30, FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(156, 39, 176), ForeColor = Color.White, Cursor = Cursors.Hand
            };
            btnScan.FlatAppearance.BorderSize = 0;
            btnScan.Click += BtnScan_Click;

            btnLimpiar = new Button() { 
                Text = "Limpiar Lienzo", Location = new Point(300, 13), 
                Width = 110, Height = 30, FlatStyle = FlatStyle.Flat,
                BackColor = Color.Gray, ForeColor = Color.White, Cursor = Cursors.Hand
            };
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.Click += BtnLimpiar_Click;

            lblEstado = new Label() { Text = "Estado: 1. Haga clics para armar polígono. 2. Cerrar. 3. ScanLine.", Location = new Point(430, 20), Width = 450, ForeColor = Color.Blue };

            panelDibujo = new Panel()
            {
                Location = new Point(10, 60), Size = new Size(910, 430),
                BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Cursor = Cursors.Cross
            };
            panelDibujo.MouseClick += PanelDibujo_MouseClick;
            panelDibujo.Paint += PanelDibujo_Paint;

            this.Controls.Add(btnCerrarPoligono); this.Controls.Add(btnScan);
            this.Controls.Add(btnLimpiar); this.Controls.Add(lblEstado);
            this.Controls.Add(panelDibujo);

            this.Load += FormScanLine_Load;
        }

        private void FormBoundaryFill_Load(object? sender, EventArgs e) => InicializarLienzo();
        private void FormScanLine_Load(object? sender, EventArgs e) => InicializarLienzo();

        private void InicializarLienzo()
        {
            if (panelDibujo.Width > 0 && panelDibujo.Height > 0)
            {
                lienzo = new Bitmap(panelDibujo.Width, panelDibujo.Height);
                g = Graphics.FromImage(lienzo);
                g.Clear(Color.White);
                poligonoActual.Clear();
                poligonocerrado = false;
                lblEstado.Text = "Haga clics en el lienzo para crear un polígono.";
                panelDibujo.Invalidate();
            }
        }

        private void BtnLimpiar_Click(object? sender, EventArgs e) => InicializarLienzo();

        private void PanelDibujo_MouseClick(object? sender, MouseEventArgs e)
        {
            if (poligonocerrado) return;
            poligonoActual.Add(e.Location);
            RedibujarPoligono(false);
        }

        private void BtnCerrarPoligono_Click(object? sender, EventArgs e)
        {
            if (poligonoActual.Count > 2 && !poligonocerrado)
            {
                poligonocerrado = true;
                RedibujarPoligono(false);
                lblEstado.Text = "Polígono cerrado. Presione Ejecutar ScanLine.";
            }
            else
            {
                MessageBox.Show("Se necesitan al menos 3 puntos para cerrar un polígono.");
            }
        }

        private void RedibujarPoligono(bool fill)
        {
            g.Clear(Color.White);
            Pen pen = new Pen(Color.Black, 2);

            for (int i = 0; i < poligonoActual.Count - 1; i++)
            {
                g.DrawLine(pen, poligonoActual[i], poligonoActual[i + 1]);
                g.FillEllipse(Brushes.Red, poligonoActual[i].X - 3, poligonoActual[i].Y - 3, 6, 6);
            }
            if (poligonoActual.Count > 0) 
                g.FillEllipse(Brushes.Red, poligonoActual[poligonoActual.Count-1].X - 3, poligonoActual[poligonoActual.Count-1].Y - 3, 6, 6);

            if (poligonocerrado && poligonoActual.Count > 2)
            {
                g.DrawLine(pen, poligonoActual[poligonoActual.Count - 1], poligonoActual[0]);
                if (fill) 
                {
                    // Scanline nativo usando el Graphics de C#
                    // Internamente Windows GDI+ usa scan-line conversion polygon fill
                    g.FillPolygon(new SolidBrush(colorReemplazo), poligonoActual.ToArray());
                }
            }
            panelDibujo.Invalidate();
        }

        private void BtnScan_Click(object? sender, EventArgs e)
        {
            if (!poligonocerrado)
            {
                MessageBox.Show("Primero cierre el polígono.");
                return;
            }
            RedibujarPoligono(true);
            lblEstado.Text = "Polígono rellenado con ScanLine.";
        }

        private void PanelDibujo_Paint(object? sender, PaintEventArgs e)
        {
            if (lienzo != null) e.Graphics.DrawImage(lienzo, Point.Empty);
        }
    }
}