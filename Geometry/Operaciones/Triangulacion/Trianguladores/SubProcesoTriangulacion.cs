using System;
using System.Collections.Generic;
using System.Text;
using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    class SubProcesoTriangulacion : ISubProceso
    {
        private TriangulacionMultiProceso.Estado _Estado = TriangulacionMultiProceso.Estado.Vacio;
        private System.Exception _Error = new Exception("NoError");

        private List<Punto3D> _Vertices = new List<Punto3D>();
        private List<Linea> _Rupturas = new List<Linea>();
        private Triangulo _Envolvente = new Triangulo();

        private IResultadoTriangulacion _ResTriangulacion = new Triangulaciones.Delaunay.ResultadoDelaunay();

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

        private System.Threading.Thread _OnThread;
        public System.Threading.Thread OnThread
        {
            get
            {
                return _OnThread;
            }
            set
            {
                _OnThread = value;
            }
        }


        public SubProcesoTriangulacion(List<Punto3D> Vertices, List<Linea> Rupturas, Triangulo Envolvente,
            int MallaAnterior,int MallaSiguiente)
        {
            _Vertices = Vertices;
            _Rupturas = Rupturas;
            _Envolvente = Envolvente;

            //Añade a la triangulación los índices de la malla anterior y la siguiente
            _ResTriangulacion.Seccion.TrianguloSeccion = Envolvente;
            _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaAnterior = MallaAnterior;
            _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaAnterior = MallaSiguiente;

            _Estado = TriangulacionMultiProceso.Estado.EnEspera;
        }

        public void Ejecutar()
        {
            //Ejecuta procesa _Vértices y _Rupturas para obtener _ResTriangulación
            try
            {
                _Estado = TriangulacionMultiProceso.Estado.EnEjecucion;

                //TODO: Ejecutar Triangulación



                _Estado = TriangulacionMultiProceso.Estado.Terminado;
            }
            catch(System.Threading.ThreadAbortException)
            {
                _Error = new Exception("Subproceso de triangulación abortado.");
                _Estado = TriangulacionMultiProceso.Estado.Detenido;
                System.Threading.Thread.ResetAbort();
            }
            catch (Exception sysEx)
            {
                _Error = sysEx;
                _Estado = TriangulacionMultiProceso.Estado.ConErrores;
            }
            finally
            {
                //Lanzar evento
                Filanlizado?.Invoke(this, EventArgs.Empty);
            }
        }

        public IResultadoTriangulacion Resultado
        {
            get
            {
                return _ResTriangulacion;
            }
        }

        //Event handler que se dispara al terminar un proceso
        public event EventHandler Filanlizado;

        private void _AlFinalizar(EventArgs e)
        {
            Filanlizado?.Invoke(this, e);
        }
    }
}
