using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

using Geometry.Geometrias;
using Geometry.Operaciones.Triangulacion;

namespace Testing
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FrmTest : Form
    {
        List<Punto3D> lstPuntos3d = new List<Punto3D>();

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

        #region Test Triangulación de Vértices
        private void CmBCargar_Click(object sender, EventArgs e)
        {
            if (TxtRuta.Text == "")
            {
                if (OFD.ShowDialog(this) == DialogResult.OK)
                {
                    TxtRuta.Text = OFD.FileName;
                }
            }

            try
            {
                //Des-serializa la colección de vértices
                lstPuntos3d = GeomSerialize.DesSerializaPuntos3D(TxtRuta.Text);
            }
            catch (Exception sysEx)
            {
                System.Windows.Forms.MessageBox.Show(this,
                                                     sysEx.ToString(),
                                                     this.Text,
                                                     MessageBoxButtons.OK,
                                                     MessageBoxIcon.Error);
                lstPuntos3d = new List<Punto3D>();
            }

            CargarDGV();
        }

        private void CmBGuardar_Click(object sender, EventArgs e)
        {
            if (TxtRuta.Text == "")
            {
                if (SFD.ShowDialog(this) == DialogResult.OK)
                {
                    TxtRuta.Text = SFD.FileName;
                }
            }

            try
            {
                if (lstPuntos3d.Count == 0)
                {
                    //Auto genera 2000 puntos
                    Random Rndm = new Random();
                    for (ulong i = 0; i < 5000; i++)
                    {
                        lstPuntos3d.Add(new Punto3D(i ,Rndm.NextDouble() * 1000.0, Rndm.NextDouble() * 1000.0, Rndm.NextDouble() * 100.0));
                    }

                }

                //Serializa la colección de vértices
                GeomSerialize.SerializaPuntos3D(TxtRuta.Text, lstPuntos3d);
            }
            catch (Exception sysEx)
            {
                System.Windows.Forms.MessageBox.Show(this,
                                                     sysEx.ToString(),
                                                     this.Text,
                                                     MessageBoxButtons.OK,
                                                     MessageBoxIcon.Error);
            }

            CargarDGV();
        }

        private void CargarDGV()
        {
            try
            {
                //Formatea y carga el DGV
                DGV.Rows.Clear();
                DGV.Columns.Clear();

                DGV.AllowUserToAddRows = false;
                DGV.AllowUserToDeleteRows = false;
                DGV.AllowUserToOrderColumns = true;
                DGV.AllowUserToResizeColumns = true;
                DGV.AllowUserToResizeRows = false;
                DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                DGV.Columns.Add("id", "ID:");
                DGV.Columns.Add("x", "X:");
                DGV.Columns.Add("y", "Y:");
                DGV.Columns.Add("z", "Z:");
                DGV.Columns.Add("descrip", "Descripción:");
                DGV.Columns[0].ValueType = typeof(long);
                DGV.Columns[1].ValueType = typeof(double);
                DGV.Columns[2].ValueType = typeof(double);
                DGV.Columns[3].ValueType = typeof(double);
                DGV.Columns[4].ValueType = typeof(string);

                foreach (Punto3D itemP3D in lstPuntos3d)
                {
                    DataGridViewRow Dgvr = new DataGridViewRow();
                    Dgvr.CreateCells(DGV);

                    Dgvr.Cells[0].Value = itemP3D.ID;
                    Dgvr.Cells[1].Value = itemP3D.X;
                    Dgvr.Cells[2].Value = itemP3D.Y;
                    Dgvr.Cells[3].Value = itemP3D.Z;
                    Dgvr.Cells[4].Value = itemP3D.Descripcion;

                    DGV.Rows.Add(Dgvr);
                }
            }
            catch (Exception dgvEx)
            {
                dgvEx.Data.Clear();
            }
        }

        private void CmBTriangularPuntos_Click(object sender, EventArgs e)
        {
            //TODO: Triangular los puntos autogenerados
            Geometry.Operaciones.Triangulacion.Triangulacion DelaTriang = new Geometry.Operaciones.Triangulacion.Triangulacion();
            DelaTriang.TriangularMalla(new Poligono(), new List<Linea>(), lstPuntos3d, TipoTriangulado.Delaunay);

        }
        #endregion

        
    }
}
