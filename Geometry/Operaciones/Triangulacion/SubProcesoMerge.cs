using System;
using System.Collections.Generic;
using System.Text;
using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulacion
{
    class SubProcesoMerge : ISubProceso
    {
        private TipoTriangulado _tipoTriangulado = TipoTriangulado.Delaunay;
        private IList<Triangulo> _triangulacion1 = new List<Triangulo>();
        private IList<Triangulo> _triangulacion2 = new List<Triangulo>();

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

        private IResultadoTriangulacion _ResTriangulacion;

        private TriangulacionMultiProceso.Estado _Estado = TriangulacionMultiProceso.Estado.Vacio;
        public TriangulacionMultiProceso.Estado Estado
        {
            get
            {
                return _Estado;
            }
        }

        private System.Exception _Error = new Exception("NoError");
        public System.Exception Error
        {
            get
            {
                return _Error;
            }
        }


        private Log.ProcessLog _logProceso = new Log.ProcessLog("Merge", Log.TypeProceso.Triangulacion);
        public Log.ProcessLog LogProceso
        {
            get
            {
                return _logProceso;
            }
        }

        public SubProcesoMerge (TipoTriangulado tipoTriangulado, IResultadoTriangulacion Triang1, IResultadoTriangulacion Triang2)
        {
            _logProceso.Add(new Log.EventoLog(Log.TypeEvento.Inicio, "Inicialización el Proceso."));

            _tipoTriangulado = tipoTriangulado;
            _triangulacion1 = Triang1.Resultado;
            _triangulacion2 = Triang2.Resultado;

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

            _logProceso.Add(new Log.EventoLog(Log.TypeEvento.Fin, "Inicialización el Proceso."));

            _Estado = TriangulacionMultiProceso.Estado.EnEspera;
        }

        public void Ejecutar()
        {
            //Ejecuta el merge de las dos triangulaciones
            try
            {
                _Estado = TriangulacionMultiProceso.Estado.EnEjecucion;

                _logProceso.ActualizaProcessLog(OnThread);

                _logProceso.Add(new Log.EventoLog(Log.TypeEvento.Inicio, "Procesa el Merge."));

                ITriangulador Triangulador = Triangulacion.GetNewMerge(_tipoTriangulado);

                IList<Triangulo> ResTriang = Triangulador.Merge(_triangulacion1, _triangulacion2);

                //TODO: Ejecutar Merge
                _ResTriangulacion = null;


                _Estado = TriangulacionMultiProceso.Estado.Terminado;
            }
            catch(System.Threading.ThreadAbortException)
            {
                _logProceso.Add(new Log.EventoLog(Log.TypeEvento.Informacion, "Proceso abortado."));
                _Error = new Exception("Subproceso de merge abortado.");
                _Estado = TriangulacionMultiProceso.Estado.Detenido;
                System.Threading.Thread.ResetAbort();
            }
            catch (Exception sysEx)
            {
                _logProceso.Add(new Log.EventoLog(Log.TypeEvento.Error, sysEx.ToString()));
                _Error = sysEx;
                _Estado = TriangulacionMultiProceso.Estado.ConErrores;
            }
            finally
            {
                _logProceso.Add(new Log.EventoLog(Log.TypeEvento.Fin, "Procesa el Merge."));
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
