using System;
using System.Collections.Generic;
using System.Text;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    interface ISubProceso
    {
        TriangulacionMultiProceso.Estado Estado { get; }
        System.Exception Error { get; }
        System.Threading.Thread OnThread { get; set; }
        void Ejecutar();
        IResultadoTriangulacion Resultado { get; }

        //eventos al terminar
        event EventHandler Filanlizado;
        //void AlFinalizar(EventArgs e);
        //event EventHandler OnFinProceso;  
    }

    interface IResultadoTriangulacion
    {
        IList<Triangulo> Resultado { get; }
        SeccionTriangulacion Seccion { get; }
    }

    public class IndicesSeccionTriangulacion
    {
        public int MallaAnterior = -1;
        public int MallaSiguiente = -1;
    }
    public class SeccionTriangulacion
    {
        public Triangulo TrianguloSeccion = new Triangulo();
        public IndicesSeccionTriangulacion MallaAnteriorSiguiente = new IndicesSeccionTriangulacion();
    }

    class TriangulacionMultiProceso
    {
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

        private List<ISubProceso> _PoolProcesos = new List<ISubProceso>();
        private List<IResultadoTriangulacion> _PoolResultados = new List<IResultadoTriangulacion>();

        public enum Estado
        {
            Vacio,
            EnEspera,
            EnEjecucion,
            Terminado,
            ConErrores,
            Detenido
        }

        public TriangulacionMultiProceso(int ProcesosDedicados)
        {
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
                //_error = new Exception("Subproceso de triangulación abortado.");
                _estadoProceso = Estado.Detenido;
                System.Threading.Thread.ResetAbort();
            }
            finally
            {
                //Lanzar el evento que se dispara al terminar un proceso
                _AlFinalizar(EventArgs.Empty);
            }
        }

        public void DetenerProcesos()
        {
            foreach (ISubProceso itemSubProceso in _PoolProcesos)
            {
                itemSubProceso.Filanlizado -= SubProceso_Filanlizado;
                itemSubProceso.OnThread.Abort();
                itemSubProceso.OnThread.Join();
            }

            _PoolProcesos = new List<ISubProceso>();
        }

        private void SubProceso_Filanlizado(object sender, EventArgs e)
        {
            //mientras que haya procesos disponibles arranca el siguiente
            lock (_PoolProcesos)
            {
                lock (_PoolResultados)
                {
                    if (sender is ISubProceso CurrentSubproceso)
                    {
                        //libera el que ha terminado
                        _PoolResultados.Add(CurrentSubproceso.Resultado);
                        CurrentSubproceso.Filanlizado -= SubProceso_Filanlizado;
                        _PoolProcesos.Remove(CurrentSubproceso);
                        _procesosEnEjecucion--;

                        if(CurrentSubproceso.Estado== Estado.ConErrores)
                        {
                            DetenerProcesos();
                        }
                    }

                    _GenerarSubResultado();

                    _ArrancarSubProcesos();

                    //condiciones de parada
                    if (_PoolProcesos.Count == 0)
                    {
                        _estadoProceso = Estado.Terminado;
                        //TODO: Transmitir errores o proceso mediante un log en el resultado o similar
                    }
                }
            }

        }

        private void _GenerarSubResultado()
        {
            if (_PoolResultados.Count >= 2)
            {
                //elimino los resultados y creo un nuevo proceso
                int Indice1=-1, Indice2 = -1;

                //TODO: Obtener dos índices consecutivos dentro de la malla



                ISubProceso ProcesoMerge = new SubProcesoMerge(_PoolResultados[Indice1], _PoolResultados[Indice2]);

                //Elimina por orden para evitar que los índices cambien
                _PoolResultados.RemoveAt(Math.Max(Indice1, Indice2));
                _PoolResultados.RemoveAt(Math.Min(Indice1, Indice2));

                _PoolProcesos.Add(ProcesoMerge);
            }
        }

        private void _ArrancarSubProcesos()
        {
            if (_procesosEnEjecucion < _procesosDedicados)
            {
                foreach (ISubProceso itemSubProceso in _PoolProcesos)
                {
                    if (itemSubProceso.Estado == Estado.EnEspera)
                    {
                        itemSubProceso.Filanlizado += SubProceso_Filanlizado;

                        //itemSubProceso.Ejecutar();
                        System.Threading.ThreadStart sTh = new System.Threading.ThreadStart(itemSubProceso.Ejecutar);
                        System.Threading.Thread Th = new System.Threading.Thread(sTh);
                        itemSubProceso.OnThread = Th;

                        _procesosEnEjecucion++;

                        Th.Start();

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
            //parte la triangulación en una serie de procesos
            _PoolProcesos = new List<ISubProceso>();

            //Secciones
            List<Triangulaciones.Delaunay.SeccionDelaunay> secciones = _CalcularSecciones(_perimetro, _procesosDedicados);

            foreach (Triangulaciones.Delaunay.SeccionDelaunay item in secciones)
            {
                _PoolProcesos.Add(new SubProcesoTriangulacion(_ContenidoEnSeccion(item.Seccion, _puntos3D),
                                                              _ContenidoEnSeccion(item.Seccion, _lineasRuptura),
                                                              item.Seccion,
                                                              item.MallaAnteriorSiguiente.MallaAnterior,
                                                              item.MallaAnteriorSiguiente.MallaSiguiente));
            }

            _estadoProceso = Estado.EnEspera;
        }

        private List<Triangulaciones.Delaunay.SeccionDelaunay> _CalcularSecciones(Poligono Perimetro, int NumeroSecciones)
        {
            List<Triangulaciones.Delaunay.SeccionDelaunay> ResSecciones = new List<Triangulaciones.Delaunay.SeccionDelaunay>();

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
                    ResSecciones.Add(ResSubSec);
                }
            }
            catch (Exception sysEx)
            {
                sysEx.Data.Clear();
                ResSecciones = new List<Triangulaciones.Delaunay.SeccionDelaunay>();
            }

            return ResSecciones;
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
                return _PoolResultados[0];
            }
        }
    }
}
