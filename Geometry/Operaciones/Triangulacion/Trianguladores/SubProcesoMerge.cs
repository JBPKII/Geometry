using System;
using System.Collections.Generic;
using System.Text;
using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    class SubProcesoMerge : ISubProceso
    {
        private TriangulacionMultiProceso.Estado _Estado = TriangulacionMultiProceso.Estado.Vacio;
        private System.Exception _Error = new Exception("NoError");

        private IList<Triangulo> _Triangulacion1 = new List<Triangulo>();
        private IList<Triangulo> _Triangulacion2 = new List<Triangulo>();

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

        public SubProcesoMerge (IResultadoTriangulacion Triang1, IResultadoTriangulacion Triang2)
        { 
            _Triangulacion1 = Triang1.Resultado;
            _Triangulacion2 = Triang2.Resultado;

            if (Triang1.Seccion.MallaAnteriorSiguiente.MallaAnterior == Triang2.Seccion.MallaAnteriorSiguiente.MallaSiguiente)
            {
                // 1,2 - 0,1
                _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaAnterior = 
                    Triang1.Seccion.MallaAnteriorSiguiente.MallaSiguiente;
                _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaSiguiente =
                    Triang2.Seccion.MallaAnteriorSiguiente.MallaAnterior;
            }
            else
            {
                if (Triang1.Seccion.MallaAnteriorSiguiente.MallaSiguiente == Triang2.Seccion.MallaAnteriorSiguiente.MallaAnterior)
                {
                    // 0,1 - 1,2
                    _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaAnterior =
                        Triang1.Seccion.MallaAnteriorSiguiente.MallaAnterior;
                    _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaSiguiente =
                        Triang2.Seccion.MallaAnteriorSiguiente.MallaSiguiente;
                }
                else
                {
                    //No son consecutivas
                }
            }

            _Estado = TriangulacionMultiProceso.Estado.EnEspera;
        }

        public void Ejecutar()
        {
            //Ejecuta el merge de las dos triangulaciones
            try
            {
                _Estado = TriangulacionMultiProceso.Estado.EnEjecucion;

                //TODO: Ejecutar Merge



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
                //Lanza evento
                Filanlizado?.Invoke(this, new EventArgs());
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

        public virtual void AlFinalizar(EventArgs e)
        {
            Filanlizado?.Invoke(this, e);
        }
    }
}
