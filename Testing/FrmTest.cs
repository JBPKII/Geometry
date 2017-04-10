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
    /// <summary>
    /// 
    /// </summary>
    public partial class FrmTest : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public FrmTest()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Poligono Pol = new Poligono()
            {
                Vertices = new List<Punto3D>()
            };
            Pol.Vertices.Add(new Punto3D(13.9617, 0.8461, 0.0));
            Pol.Vertices.Add(new Punto3D(7.4365, 7.9198, 0.0));
            Pol.Vertices.Add(new Punto3D(10.3867, 11.7687, 0.0));
            Pol.Vertices.Add(new Punto3D(23.3678, 13.2597, 0.0));
            Pol.Vertices.Add(new Punto3D(24.0542, 7.2840, 0.0));
            Pol.Vertices.Add(new Punto3D(33.7109, 4.7990, 0.0));
            Pol.Vertices.Add(new Punto3D(32.0390, -1.2920, 0.0));
            Pol.Vertices.Add(new Punto3D(20.7450, -2.4724, 0.0));

            Triangulacion Triang = new Triangulacion();
            if (Triang.TriangularPoligono(Pol, TipoTriangulado.Delaunay))
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
