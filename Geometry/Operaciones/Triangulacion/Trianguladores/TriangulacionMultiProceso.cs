using System;
using System.Collections.Generic;
using System.Text;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    interface ISubProceso
    {
        System.Threading.Thread OnThread { get; set; }
        TriangulacionMultiProceso.Estado Estado { get; }
        System.Exception Error { get; }
        Log.ProcessLog LogProceso { get; }

        void Ejecutar();

        IResultadoTriangulacion Resultado { get; }

        //evento al terminar
        event EventHandler Filanlizado;

    }



    class TriangulacionMultiProceso
    {
        public enum Estado
        {
            Vacio,
            EnEspera,
            EnEjecucion,
            Terminado,
            ConErrores,
            Detenido
        }

        private Estado _estadoProceso = TriangulacionMultiProceso.Estado.Vacio;
        public Estado EstadoProceso
        {
            get
            {
                return _estadoProceso;
            }
        }

        private int _procesosDedicados = Environment.ProcessorCount;
        private int _procesosEnEjecucion = 0;


        private Poligono _perimetro;
        public Poligono Perimetro
        {
            private get
            {
                return _perimetro;
            }
            set
            {
                _perimetro = value;
            }
        }
        private List<Linea> _lineasRuptura;
        public List<Linea> LineasRuptura
        {
            private get
            {
                return _lineasRuptura;
            }
            set
            {
                _lineasRuptura = value;
            }
        }
        private List<Punto3D> _puntos3D;
        public List<Punto3D> Puntos3D
        {
            private get
            {
                return _puntos3D;
            }
            set
            {
                _puntos3D = value;
            }
        }

        private List<ISubProceso> _poolProcesos = new List<ISubProceso>();
        private List<IResultadoTriangulacion> _poolResultados = new List<IResultadoTriangulacion>();

        private TipoTriangulado _tipoTriangulado = TipoTriangulado.Delaunay;
        public TipoTriangulado TipoTriangulado
        {
            get
            {
                return _tipoTriangulado;
            }
            set
            {
                _tipoTriangulado = value;
            }
        }

        private Log.Log _log = new Log.Log();
        public Log.Log LogMultiProceso
        {
            get
            {
                Log.Log _resLog = new Log.Log();

                _resLog.LogProcesos.Add(_logProcesoMain);

                foreach (Log.ProcessLog itemPL in _log.LogProcesos)
                {
                    _resLog.LogProcesos.Add(itemPL);
                }

                return _resLog;
            }
        }

        private Log.ProcessLog _logProcesoMain = new Log.ProcessLog("Triangulación Multiproceso ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(),
                                                                    Log.TypeProceso.Multiproceso);

        public TriangulacionMultiProceso(int ProcesosDedicados)
        {
            _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Informacion, "Procesos dedicados = " + ProcesosDedicados.ToString()));

            _procesosDedicados = ProcesosDedicados;
            _procesosEnEjecucion = 0;
        }

        //Event handler que se dispara al terminar un proceso
        public event EventHandler Filanlizado;

        private void _AlFinalizar(EventArgs e)
        {
            Filanlizado?.Invoke(this, e);
        }

        public void IniciarProceso()
        {
            _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Inicio, "Inicialización de la Triangulación Multiproceso."));

            _procesosEnEjecucion = 0;
            _estadoProceso = Estado.EnEjecucion;

            try
            {
                //Realizar la multi-triangulación

                //reparte los datos y genera el pool de procesos
                _Repartir();

                //arranca los procesos
                SubProceso_Filanlizado(this, EventArgs.Empty);
            }
            catch (System.Threading.ThreadAbortException)
            {
                _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Informacion, "Inicialización de la Triangulación Multiproceso abortada."));
                //_error = new Exception("Subproceso de triangulación abortado.");
                _estadoProceso = Estado.Detenido;
                System.Threading.Thread.ResetAbort();
            }
            finally
            {
                _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Fin, "Inicialización de la Triangulación Multiproceso."));
                //Lanzar el evento que se dispara al terminar un proceso
                _AlFinalizar(EventArgs.Empty);
            }
        }

        public void DetenerProcesos()
        {
            _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Inicio, "Detener Subprocesos."));

            foreach (ISubProceso itemSubProceso in _poolProcesos)
            {
                itemSubProceso.Filanlizado -= SubProceso_Filanlizado;
                itemSubProceso.OnThread.Abort();
                itemSubProceso.OnThread.Join();
                itemSubProceso.OnThread = null;

                _log.LogProcesos.Add(itemSubProceso.LogProceso);

                _procesosEnEjecucion--;
            }

            _poolProcesos = new List<ISubProceso>();

            _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Fin, "Detener Subprocesos."));
        }

        private void SubProceso_Filanlizado(object sender, EventArgs e)
        {
            _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Inicio, "SubProceso Finalizado."));

            bool Finalizar = false;
            //mientras que haya procesos disponibles arranca el siguiente
            lock (_poolProcesos)
            {
                lock (_poolResultados)
                {
                    if (sender is ISubProceso CurrentSubproceso)
                    {
                        //libera el que ha terminado
                        _poolResultados.Add(CurrentSubproceso.Resultado);
                        CurrentSubproceso.Filanlizado -= SubProceso_Filanlizado;
                        _poolProcesos.Remove(CurrentSubproceso);

                        _log.LogProcesos.Add(CurrentSubproceso.LogProceso);

                        _procesosEnEjecucion--;

                        if(CurrentSubproceso.Estado == Estado.ConErrores)
                        {
                            _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Advertencia, "SubProceso Finalizado con errores."));

                            DetenerProcesos();
                            Finalizar = true;
                        }
                    }

                    if (!Finalizar)
                    {
                        _GenerarSubResultado();

                        _ArrancarSubProcesos();

                        //condiciones de parada
                        if (_poolProcesos.Count == 0)
                        {
                            _estadoProceso = Estado.Terminado;
                            //TODO: Transmitir errores o proceso mediante un log en el resultado o similar
                        }
                    }
                    else
                    {
                        _estadoProceso = Estado.ConErrores;
                    }
                }
            }

            _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Fin, "SubProceso Finalizado."));

        }

        private void _GenerarSubResultado()
        {
            if (_poolResultados.Count >= 2)
            {
                _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Inicio, "Generar proceso SubResultado"));

                //elimino los resultados y creo un nuevo proceso
                int Indice1 = -1, Indice2 = -1;
                ISubProceso ProcesoMerge = null;

                //TODO: Obtener dos índices consecutivos dentro de la malla
                //compara todos con todos para encontrar los consercutivos
                for (Indice1 = 0; Indice1 < _poolResultados.Count; Indice1++)
                {
                    for (Indice2 = 0; Indice2 < _poolResultados.Count; Indice2++)
                    {
                        if (Indice1 != Indice2 && 
                            _poolResultados[Indice1].Seccion.MallaAnteriorSiguiente.EsConsecutiva(_poolResultados[Indice2].Seccion.MallaAnteriorSiguiente))
                        {
                            ProcesoMerge = new SubProcesoMerge(_poolResultados[Indice1], _poolResultados[Indice2]);
                            break;
                        }
                        //else
                        //{
                        //    pasa a la siguiente combinación
                        //}
                    }

                    if (ProcesoMerge != null)
                    {
                        break;
                    }
                }

                _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Fin, "Generar proceso SubResultado"));

                if (ProcesoMerge != null)
                {
                    //Elimina por orden para evitar que los índices cambien
                    _poolResultados.RemoveAt(Math.Max(Indice1, Indice2));
                    _poolResultados.RemoveAt(Math.Min(Indice1, Indice2));

                    _poolProcesos.Add(ProcesoMerge);

                    //Rellama al proceso para buscar posibles coincidencias
                    _GenerarSubResultado();
                }
            }
        }

        private void _ArrancarSubProcesos()
        {
            if (_procesosEnEjecucion < _procesosDedicados)
            {
                _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Inicio, "Iniciar SubProcesos"));

                foreach (ISubProceso itemSubProceso in _poolProcesos)
                {
                    if (itemSubProceso.Estado == Estado.EnEspera)
                    {
                        itemSubProceso.Filanlizado += SubProceso_Filanlizado;

                        //itemSubProceso.Ejecutar();
                        System.Threading.ThreadStart sTh = new System.Threading.ThreadStart(itemSubProceso.Ejecutar);
                        itemSubProceso.OnThread = new System.Threading.Thread(sTh);

                        _procesosEnEjecucion++;

                        itemSubProceso.OnThread.Start();
                    }
                    if (_procesosEnEjecucion >= _procesosDedicados)
                    {
                        break;
                    }
                }

                _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Fin, "Iniciar SubProcesos"));
            }
        }

        #region Repartidor
        private void _Repartir()
        {
            _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Inicio, "Repartir datos en cada sector."));

            //parte la triangulación en una serie de procesos
            _poolProcesos = new List<ISubProceso>();

            //Secciones
            IList<Triangulaciones.Delaunay.SeccionDelaunay> secciones = _CalcularSecciones(_perimetro, _procesosDedicados);

            foreach (Triangulaciones.Delaunay.SeccionDelaunay item in secciones)
            {
                _poolProcesos.Add(new SubProcesoTriangulacion(_ContenidoEnSeccion(item.Seccion, _puntos3D),
                                                              _ContenidoEnSeccion(item.Seccion, _lineasRuptura),
                                                              item.Seccion,
                                                              item.MallaAnteriorSiguiente.MallaAnterior,
                                                              item.MallaAnteriorSiguiente.MallaSiguiente));
            }

            _estadoProceso = Estado.EnEspera;

            _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Fin, "Repartir datos en cada sector."));
        }

        private List<Triangulaciones.Delaunay.SeccionDelaunay> _CalcularSecciones(Poligono Perimetro, int NumeroSecciones)
        {
            List<Triangulaciones.Delaunay.SeccionDelaunay> resSecciones = new List<Triangulaciones.Delaunay.SeccionDelaunay>();

            try
            {
                //Calcula las secciones que envuelven al polígono
                BBox BBoxPoligono = new BBox();
                foreach (Punto3D P3D in Perimetro.Vertices)
                {
                    BBoxPoligono.Añadir(P3D);
                }

                double RadioCircinscrito = Math.Max(BBoxPoligono.Ancho, BBoxPoligono.Alto) / 2.0;
                Punto3D PMedio = new Punto3D((BBoxPoligono.Maximo.X + BBoxPoligono.Minimo.X) / 2.0,
                                             (BBoxPoligono.Maximo.Y + BBoxPoligono.Minimo.Y) / 2.0,
                                             0.0);
                Punto3D PAnterior = new Punto3D(PMedio.X + RadioCircinscrito, PMedio.Y, 0.0);
                for (int i = 1; i <= NumeroSecciones; i++)
                {
                    Triangulo Seccion = new Triangulo()
                    {
                        P1 = PMedio,
                        P2 = PAnterior
                    };
                    double Alfa = double.Parse(i.ToString()) * 2.0 * Math.PI / double.Parse(NumeroSecciones.ToString());
                    Punto3D PSiguiente = new Punto3D(PMedio.X + (RadioCircinscrito * Math.Cos(Alfa)),
                                                     PMedio.Y + (RadioCircinscrito * Math.Sin(Alfa)),
                                                     0.0);

                    Seccion.P3 = PSiguiente;
                    PAnterior = PSiguiente;

                    Triangulaciones.Delaunay.SeccionDelaunay ResSubSec = new Triangulaciones.Delaunay.SeccionDelaunay()
                    {
                        Seccion = Seccion
                    };
                    ResSubSec.MallaAnteriorSiguiente.MallaAnterior = i;
                    ResSubSec.MallaAnteriorSiguiente.MallaSiguiente = (i + 1) > NumeroSecciones ? 1 : (i + 1);//Revisar
                    resSecciones.Add(ResSubSec);
                }

                _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Informacion, "Generados " + resSecciones.Count + " sectores."));
            }
            catch (Exception sysEx)
            {
                _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Error, sysEx.ToString()));
                sysEx.Data.Clear();
                resSecciones = new List<Triangulaciones.Delaunay.SeccionDelaunay>();
            }

            return resSecciones;
        }

        private List<Linea> _ContenidoEnSeccion(Triangulo Seccion, List<Linea> LineasRuptura)
        {
            List<Linea> Res = new List<Linea>();
            //Evalua si la línea de ruptura está dentro
            foreach (Linea item in LineasRuptura)
            {
                if (Geometry.Analisis.AnalisisGeometrico.PuntoEnTriangulo(item.Inicio, Seccion) ||
                    Geometry.Analisis.AnalisisGeometrico.PuntoEnTriangulo(item.Fin, Seccion))
                {
                    Res.Add(item);
                }
            }
            return Res;
        }
        private List<Punto3D> _ContenidoEnSeccion(Triangulo Seccion, List<Punto3D> Puntos)
        {
            List<Punto3D> Res = new List<Punto3D>();
            //Evalua si el punto está dentro
            foreach (Punto3D item in Puntos)
            {
                if(Geometry.Analisis.AnalisisGeometrico.PuntoEnTriangulo(item, Seccion))
                {
                    Res.Add(item);
                }
            }
            return Res;
        }
        #endregion

        public IResultadoTriangulacion Resultado
        {
            get
            {
                if(_estadoProceso == Estado.EnEspera)
                {
                    IniciarProceso();
                }

                while (_estadoProceso == Estado.EnEjecucion)
                {
                    System.Threading.Thread.Sleep(300);
                }
                return _poolResultados[0];
            }
        }

        
    }
}
