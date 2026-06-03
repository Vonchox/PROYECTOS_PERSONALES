using System;
using System.Drawing;
using System.Windows.Forms;

namespace Algoritmos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Opcional: Cambiar el color de fondo del MDI container
            foreach (Control control in this.Controls)
            {
                if (control is MdiClient)
                {
                    control.BackColor = Color.FromArgb(245, 246, 250);
                    break;
                }
            }
        }

        private void btnDDA_Click(object sender, EventArgs e)
        {
            FormDDA hijoDDA = new FormDDA();
            hijoDDA.MdiParent = this;
            hijoDDA.Show();
        }

        private void btnBresenham_Click(object sender, EventArgs e)
        {
            FormBresenham hijoBresenham = new FormBresenham();
            hijoBresenham.MdiParent = this;
            hijoBresenham.Show();
        }

        private void btnCirculo_Click(object sender, EventArgs e)
        {
            Elipse hijoElipse = new Elipse();
            hijoElipse.MdiParent = this;
            hijoElipse.Show();
        }

        private void btnElipseReal_Click(object sender, EventArgs e)
        {
            FormElipseReal hijoElipseReal = new FormElipseReal();
            hijoElipseReal.MdiParent = this;
            hijoElipseReal.Show();
        }

        private void btnFloodFill_Click(object sender, EventArgs e)
        {
            FormFloodFill hijoFloodFill = new FormFloodFill();
            hijoFloodFill.MdiParent = this;
            hijoFloodFill.Show();
        }
        private void btnLineaBasica_Click(object sender, EventArgs e)
        {
            FormLineaBasica hijoLineaBasica = new FormLineaBasica();
            hijoLineaBasica.MdiParent = this;
            hijoLineaBasica.Show();
        }

        private void btnCirculoTrig_Click(object sender, EventArgs e)
        {
            FormCirculoTrig hijoCirculoTrig = new FormCirculoTrig();
            hijoCirculoTrig.MdiParent = this;
            hijoCirculoTrig.Show();
        }

        private void btnCirculoPoli_Click(object sender, EventArgs e)
        {
            FormCirculoPoli hijoCirculoPoli = new FormCirculoPoli();
            hijoCirculoPoli.MdiParent = this;
            hijoCirculoPoli.Show();
        }

        private void btnBoundaryFill_Click(object sender, EventArgs e)
        {
            FormBoundaryFill hjoBoundaryFill = new FormBoundaryFill();
            hjoBoundaryFill.MdiParent = this;
            hjoBoundaryFill.Show();
        }

        private void btnScanLine_Click(object sender, EventArgs e)
        {
            FormScanLine hijoScanLine = new FormScanLine();
            hijoScanLine.MdiParent = this;
            hijoScanLine.Show();
        }
    }
}
