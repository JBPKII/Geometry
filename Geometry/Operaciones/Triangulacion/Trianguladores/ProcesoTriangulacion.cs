using System;
using System.Collections.Generic;
using System.Text;
using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    class ProcesoTriangulacion : IProceso
    {
        private TriangulacionMultiProceso.Estado _Estado = TriangulacionMultiProceso.Estado.Vacio;
        private System.Exception _Error = new Exception("NoError");

        private IList<Punto3D> _Vertices = new List<Punto3D>();
        private IList<Linea> _Rupturas = new List<Linea>();
        private Triangulo _Envolvente = new Triangulo();

        private Triangulaciones.Delaunay.ResultadoDelaunay _ResTriangulacion = new Triangulaciones.Delaunay.ResultadoDelaunay();

        public TriangulacionMultiProceso.Estado Estado
        {
            get
            {
                return _Estado;
            }
        }
        public System.Exception Error
        {
            get
            {
                return _Error;
            }
        }

        public ProcesoTriangulacion(IList<Punto3D> Vertices, IList<Linea> Rupturas, Triangulo Envolvente,
            int MallaAnterior,int MallaSiguiente)
        {
            _Vertices = Vertices;
            _Rupturas = Rupturas;
            _Envolvente = Envolvente;

            //Añade a la triangulación los índices de la malla anterior y la siguiente
            _ResTriangulacion.Seccion.Seccion = Envolvente;
            _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaAnterior = MallaAnterior;
            _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaAnterior = MallaSiguiente;

            _Estado = TriangulacionMultiProceso.Estado.EnEspera;
        }

        public void Ejecutar()
        {
            //Ejecuta procesa _Vértices y _Rupturas para obtener _ResTriangulación
            try
            {
                //TODO: Ejecutar Triangulación

                //TODO: Lanzar evento

                _Estado = TriangulacionMultiProceso.Estado.Terminado;
            }
            catch (Exception sysEx)
            {
                _Error = sysEx;
                _Estado = TriangulacionMultiProceso.Estado.ConErrores;
            }
            finally
            {
                FinProcesoEvent?.Invoke(this, new EventArgs());
            }
        }

        public Triangulaciones.Delaunay.ResultadoDelaunay Resultado()
        {
            return _ResTriangulacion;
        }

        public EventHandler FinProcesoEvent;
        public event EventHandler OnFinProceso
        {
            add
            {
                lock (this)
                {
                    FinProcesoEvent += value;
                }
            }
            remove
            {
                lock (this)
                {
                    FinProcesoEvent -= value;
                }
            }
        }
    }
}
