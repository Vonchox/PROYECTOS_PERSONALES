using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Algoritmos
{
    public class FormBoundaryFill : Form
    {
        private Bitmap lienzo;
        private Graphics g;

        private Queue<Point> colaPixeles;
        private Color colorReemplazo = Color.Green;
        private Color colorFrontera = Color.Black;
        private bool animando = false;
        private int pixelesPorIteracion = 1500;

        private Panel panelDibujo;
        private ComboBox cmbFiguras;
        private TextBox txtTamano;
        private Button btnDibujarFigura;
        private Button btnLimpiar;
        private Timer timerAnimacion;
        private Label lblEstado;

        public FormBoundaryFill()
        {
            this.Text = "Algoritmo Boundary Fill";
            this.Size = new Size(950, 550);
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            Label lblFigura = new Label() { Text = "Forma:", Location = new Point(10, 20), Width = 50 };
            cmbFiguras = new ComboBox() { Location = new Point(60, 15), Width = 90, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbFiguras.Items.AddRange(new object[] { "Círculo", "Rectángulo", "Triángulo" });
            cmbFiguras.SelectedIndex = 0;

            Label lblTamano = new Label() { Text = "Tamaño:", Location = new Point(160, 20), Width = 55 };
            txtTamano = new TextBox() { Location = new Point(220, 15), Width = 50, Text = "100" };

            btnDibujarFigura = new Button() { 
                Text = "Trazar Borde", 
                Location = new Point(280, 13), 
                Width = 90, Height = 30,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White, Cursor = Cursors.Hand
            };
            btnDibujarFigura.FlatAppearance.BorderSize = 0;
            btnDibujarFigura.Click += BtnDibujarFigura_Click;

            btnLimpiar = new Button() { 
                Text = "Limpiar Lienzo", 
                Location = new Point(380, 13), 
                Width = 110, Height = 30,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Gray,
                ForeColor = Color.White, Cursor = Cursors.Hand
            };
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.Click += BtnLimpiar_Click;

            lblEstado = new Label() { Text = "Estado: Listo. ¡Dibuje forma y haga clic adentro para iniciar Boundary Fill (verde)!", Location = new Point(500, 20), Width = 430, ForeColor = Color.Blue };

            panelDibujo = new Panel()
            {
                Location = new Point(10, 60),
                Size = new Size(910, 430),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Cross
            };
            panelDibujo.MouseClick += PanelDibujo_MouseClick;
            panelDibujo.Paint += PanelDibujo_Paint;

            timerAnimacion = new Timer();
            timerAnimacion.Interval = 5; 
            timerAnimacion.Tick += TimerAnimacion_Tick;

            this.Controls.Add(lblFigura); this.Controls.Add(cmbFiguras);
            this.Controls.Add(lblTamano); this.Controls.Add(txtTamano);
            this.Controls.Add(btnDibujarFigura); this.Controls.Add(btnLimpiar);
            this.Controls.Add(lblEstado); this.Controls.Add(panelDibujo);

            this.Load += FormBoundaryFill_Load;
        }

        private void FormBoundaryFill_Load(object? sender, EventArgs e)
        {
            InicializarLienzo();
        }

        private void InicializarLienzo()
        {
            if (panelDibujo.Width > 0 && panelDibujo.Height > 0)
            {
                lienzo = new Bitmap(panelDibujo.Width, panelDibujo.Height);
                g = Graphics.FromImage(lienzo);
                g.Clear(Color.White);
                colaPixeles = new Queue<Point>();
                lblEstado.Text = "Estado: Listo";
                animando = false;
                timerAnimacion.Stop();
                panelDibujo.Invalidate();
            }
        }

        private void BtnLimpiar_Click(object? sender, EventArgs e)
        {
            if (animando) return;
            InicializarLienzo();
        }

        private void BtnDibujarFigura_Click(object? sender, EventArgs e)
        {
            if (animando) return;
            g.Clear(Color.White);

            int tamano = 100;
            int.TryParse(txtTamano.Text, out tamano);
            int centroX = panelDibujo.Width / 2;
            int centroY = panelDibujo.Height / 2;

            // Boundary exige color exacto de choque. Usaremos Negro absoluto.
            Pen lapiz = new Pen(Color.Black, 2);
            string figura = cmbFiguras.SelectedItem.ToString();

            if (figura == "Círculo") g.DrawEllipse(lapiz, centroX - tamano, centroY - tamano, tamano * 2, tamano * 2);
            else if (figura == "Rectángulo") g.DrawRectangle(lapiz, centroX - tamano, centroY - (tamano / 2), tamano * 2, tamano);
            else if (figura == "Triángulo")
            {
                Point[] puntos = new Point[] { new Point(centroX, centroY - tamano), new Point(centroX - tamano, centroY + tamano), new Point(centroX + tamano, centroY + tamano) };
                g.DrawPolygon(lapiz, puntos);
            }
            panelDibujo.Invalidate();
        }

        private void PanelDibujo_MouseClick(object? sender, MouseEventArgs e)
        {
            if (animando || lienzo == null) return; 
            Point clickPos = e.Location;
            Color colorActual = lienzo.GetPixel(clickPos.X, clickPos.Y);

            // Impedimos clics donde ya esta del color de reemplazo o donde es frontera.
            if (colorActual.ToArgb() == colorReemplazo.ToArgb() || colorActual.ToArgb() == colorFrontera.ToArgb()) 
                return;

            colaPixeles.Clear();
            colaPixeles.Enqueue(clickPos);
            animando = true;
            lblEstado.Text = "Estado: Rellenando contra Frontera Negra...";
            timerAnimacion.Start();
        }

        private void TimerAnimacion_Tick(object? sender, EventArgs e)
        {
            int count = 0;
            while (colaPixeles.Count > 0 && count < pixelesPorIteracion)
            {
                Point p = colaPixeles.Dequeue();

                if (p.X < 0 || p.X >= lienzo.Width || p.Y < 0 || p.Y >= lienzo.Height) continue;

                Color colorActual = lienzo.GetPixel(p.X, p.Y);

                // Diff con Flood: Boundary pinta SI NO ES el color de reemplazo Y NO ES el color frontera
                if (colorActual.ToArgb() != colorFrontera.ToArgb() && colorActual.ToArgb() != colorReemplazo.ToArgb())
                {
                    lienzo.SetPixel(p.X, p.Y, colorReemplazo);
                    colaPixeles.Enqueue(new Point(p.X + 1, p.Y)); // Este
                    colaPixeles.Enqueue(new Point(p.X - 1, p.Y)); // Oeste
                    colaPixeles.Enqueue(new Point(p.X, p.Y + 1)); // Sur
                    colaPixeles.Enqueue(new Point(p.X, p.Y - 1)); // Norte
                    count++;
                }
            }

            panelDibujo.Invalidate();

            if (colaPixeles.Count == 0)
            {
                timerAnimacion.Stop();
                animando = false;
                lblEstado.Text = "Estado: Boundary Fill Completado";
            }
        }

        private void PanelDibujo_Paint(object? sender, PaintEventArgs e)
        {
            if (lienzo != null) e.Graphics.DrawImage(lienzo, Point.Empty);
        }
    }
}