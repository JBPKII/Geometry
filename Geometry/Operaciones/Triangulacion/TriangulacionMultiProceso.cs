using System;
using System.Collections.Generic;
using System.Text;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulacion
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


        private Poligono _perimetro = new Poligono();
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
        private List<Linea> _lineasRuptura = new List<Linea>();
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
        private List<Punto3D> _puntos3D = new List<Punto3D>();
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

        private void _DetenerProcesos()
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
                    if (sender is ISubProceso CurrSubproceso)
                    {
                        //libera el que ha terminado
                        _poolResultados.Add(CurrSubproceso.Resultado);
                        CurrSubproceso.Filanlizado -= SubProceso_Filanlizado;
                        _poolProcesos.Remove(CurrSubproceso);

                        _log.LogProcesos.Add(CurrSubproceso.LogProceso);

                        _procesosEnEjecucion--;

                        if(CurrSubproceso.Estado == Estado.ConErrores)
                        {
                            _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Advertencia, "SubProceso Finalizado con errores."));

                            _DetenerProcesos();
                            Finalizar = true;
                        }
                    }

                    if (!Finalizar)
                    {
                        _GenerarSubResultado();

                        _ArrancarSubProcesos();

                        //condiciones de parada
                        if (_poolProcesos.Count == 0 && _poolResultados.Count == 1)
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

                //Obtiene dos índices consecutivos dentro de la malla
                //compara todos con todos para encontrar los consercutivos
                for (Indice1 = 0; Indice1 < _poolResultados.Count; Indice1++)
                {
                    for (Indice2 = 0; Indice2 < _poolResultados.Count; Indice2++)
                    {
                        if (Indice1 != Indice2 && 
                            _poolResultados[Indice1].Seccion.MallaAnteriorSiguiente.EsConsecutiva(_poolResultados[Indice2].Seccion.MallaAnteriorSiguiente))
                        {
                            //Encuentra las posibles mallas consecutivas
                            ProcesoMerge = new SubProcesoMerge(_tipoTriangulado, _poolResultados[Indice1], _poolResultados[Indice2]);
                            break;
                        }
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
                foreach (ISubProceso itemSubProceso in _poolProcesos)
                {
                    if (itemSubProceso.Estado == Estado.EnEspera)
                    {
                        _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Inicio, "Iniciar SubProceso"));

                        itemSubProceso.Filanlizado += SubProceso_Filanlizado;

                        //itemSubProceso.Ejecutar();
                        System.Threading.ThreadStart sTh = new System.Threading.ThreadStart(itemSubProceso.Ejecutar);
                        itemSubProceso.OnThread = new System.Threading.Thread(sTh);

                        _procesosEnEjecucion++;

                        itemSubProceso.OnThread.Start();

                        _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Fin, "Iniciar SubProceso"));
                    }

                    if (_procesosEnEjecucion >= _procesosDedicados)
                    {
                        break;
                    }
                }
            }
        }

        #region Repartidor
        private void _Repartir()
        {
            _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Inicio, "Repartir datos en cada sector."));

            //parte la triangulación en una serie de procesos
            _poolProcesos = new List<ISubProceso>();

            //Secciones
            IList<SeccionTriangulacion> secciones = _CalcularSecciones(_perimetro, _procesosDedicados);
            if(secciones.Count==0)
            {
                secciones = _CalcularSecciones(_puntos3D, _procesosDedicados);
            }

            foreach (SeccionTriangulacion item in secciones)
            {
                _poolProcesos.Add(new SubProcesoTriangulacion(_tipoTriangulado,
                                                              _ContenidoEnSeccion(item.TrianguloSeccion, _puntos3D),
                                                              _ContenidoEnSeccion(item.TrianguloSeccion, _lineasRuptura),
                                                              item.TrianguloSeccion,
                                                              item.MallaAnteriorSiguiente.MallaAnterior,
                                                              item.MallaAnteriorSiguiente.MallaSiguiente));
            }

            _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Fin, "Repartir datos en cada sector."));
        }

        private List<SeccionTriangulacion> _CalcularSecciones(Poligono Perimetro, int NumeroSecciones)
        {
            List<SeccionTriangulacion> resSecciones = new List<SeccionTriangulacion>();

            try
            {
                //Calcula las secciones que envuelven al polígono
                if (Perimetro.Vertices.Count > 0)
                {
                    BBox BBoxPoligono = new BBox();
                    foreach (Punto3D P3D in Perimetro.Vertices)
                    {
                        BBoxPoligono.Añadir(P3D);
                    }

                    double RadioCircinscrito = Math.Sqrt(Math.Pow(BBoxPoligono.Ancho, 2.0) + Math.Pow(BBoxPoligono.Alto, 2.0)) / 2.0;
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

                        SeccionTriangulacion ResSubSec = new SeccionTriangulacion()
                        {
                            TrianguloSeccion = Seccion
                        };
                        ResSubSec.MallaAnteriorSiguiente.MallaAnterior = i;
                        ResSubSec.MallaAnteriorSiguiente.MallaSiguiente = (i + 1) > NumeroSecciones ? 1 : (i + 1);//Revisar
                        resSecciones.Add(ResSubSec);
                    }

                    _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Informacion, "Generados " + resSecciones.Count + " sectores."));
                }
                else
                {
                    _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Informacion, "No se han encontrado vértices en el perímetro."));
                    resSecciones = new List<SeccionTriangulacion>();
                }
            }
            catch (Exception sysEx)
            {
                _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Error, sysEx.ToString()));
                sysEx.Data.Clear();
                resSecciones = new List<SeccionTriangulacion>();
            }

            return resSecciones;
        }

        private List<SeccionTriangulacion> _CalcularSecciones(IList<Punto3D> Puntos, int NumeroSecciones)
        {
            List<SeccionTriangulacion> resSecciones = new List<SeccionTriangulacion>();

            try
            {
                if (Puntos.Count != 0)
                {
                    //calcula el cierre convexo
                    BBox BB = new BBox();
                    foreach (Punto3D itemP3D in Puntos)
                    {
                        BB.Añadir(itemP3D);
                    }
                    Poligono cierreConvexo = BB.ToPoligono();
                    resSecciones = _CalcularSecciones(cierreConvexo, NumeroSecciones);
                }
                else
                {
                    _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Informacion, "No se han encontrado puntos."));
                    resSecciones = new List<SeccionTriangulacion>();
                }
            }
            catch (Exception sysEx)
            {
                _logProcesoMain.Add(new Log.EventoLog(Log.TypeEvento.Error, sysEx.ToString()));
                sysEx.Data.Clear();
                resSecciones = new List<SeccionTriangulacion>();
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
