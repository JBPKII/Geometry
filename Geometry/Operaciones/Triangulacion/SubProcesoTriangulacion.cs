using System;
using System.Collections.Generic;
using System.Text;
using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulacion
{
    class SubProcesoTriangulacion : ISubProceso
    {
        private TipoTriangulado _tipoTriangulado = TipoTriangulado.Delaunay;
        private List<Punto3D> _vertices = new List<Punto3D>();
        private List<Linea> _rupturas = new List<Linea>();
        private Triangulo _envolvente = new Triangulo();

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

        private Log.ProcessLog _logProceso = new Log.ProcessLog("Triangulacion", Log.TypeProceso.Triangulacion);
        public Log.ProcessLog LogProceso
        {
            get
            {
                return _logProceso;
            }
        }


        public SubProcesoTriangulacion(TipoTriangulado tipoTriangulado, List<Punto3D> Vertices, List<Linea> Rupturas, Triangulo Envolvente,
            int MallaAnterior,int MallaSiguiente)
        {
            _logProceso.Add(new Log.EventoLog(Log.TypeEvento.Inicio, "Inicialización del Proceso."));

            _tipoTriangulado = tipoTriangulado;
            _vertices = Vertices;
            _rupturas = Rupturas;
            _envolvente = Envolvente;

            //Añade a la triangulación los índices de la malla anterior y la siguiente
            _ResTriangulacion.Seccion.TrianguloSeccion = Envolvente;
            _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaAnterior = MallaAnterior;
            _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaAnterior = MallaSiguiente;

            _logProceso.Add(new Log.EventoLog(Log.TypeEvento.Fin, "Inicialización del Proceso."));

            _Estado = TriangulacionMultiProceso.Estado.EnEspera;
        }

        public void Ejecutar()
        {
            //Ejecuta procesa _Vértices y _Rupturas para obtener _ResTriangulación
            try
            {
                _Estado = TriangulacionMultiProceso.Estado.EnEjecucion;

                _logProceso.ActualizaProcessLog(OnThread);

                _logProceso.Add(new Log.EventoLog(Log.TypeEvento.Inicio, "Procesa la Triangulación."));

                ITriangulador Triangulador = Triangulacion.GetNewTriangulador(_tipoTriangulado);

                IList<Triangulo> ResTriang = Triangulador.Triangular(_envolvente.ToPoligono(), _rupturas, _vertices);

                //TODO: Ejecutar Triangulación
                _ResTriangulacion = null;


                _Estado = TriangulacionMultiProceso.Estado.Terminado;
            }
            catch (System.Threading.ThreadAbortException)
            {
                _logProceso.Add(new Log.EventoLog(Log.TypeEvento.Informacion, "Proceso abortado."));
                _Error = new Exception("Subproceso de triangulación abortado.");
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
                _logProceso.Add(new Log.EventoLog(Log.TypeEvento.Fin, "Procesa la Triangulación."));
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
