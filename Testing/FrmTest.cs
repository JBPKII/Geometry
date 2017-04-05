using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Geometry.Geometrias;
using Geometry.Operaciones.Triangulaciones;

namespace Testing
{
    public partial class FrmTest : Form
    {
        public FrmTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Poligono Pol = new Poligono();
            Pol.Vertices = new List<Punto3D>();
            Pol.Vertices.Add(new Punto3D());

            Triangulacion Triang = new Triangulacion();
            if(Triang.TriangularPoligono(Pol, TipoTriangulado.Delaunay))
            {
                //OK
                IList<Triangulo> LstResultado = Triang.Resultado;
            }
            else
            {
                //KO
            }
        }
    }
}
