using System;
using System.Collections.Generic;
using System.Text;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    interface IProceso
    {
        TriangulacionMultiProceso.Estado Estado { get; }
        System.Exception Error { get; }
        void Ejecutar();
        Triangulaciones.Delaunay.ResultadoDelaunay Resultado();

        //eventos al terminar
        event EventHandler OnFinProceso;  
    }

    class TriangulacionMultiProceso
    {
        private Estado _EstadoProceso = TriangulacionMultiProceso.Estado.Vacio;
        public Estado EstadoProceso
        {
            get
            {
                return _EstadoProceso;
            }
        }

        private int _ProcesosDedicados = Environment.ProcessorCount;

        private IList<IProceso> _PoolProcesos = new List<IProceso>();
        private IList<IList<Triangulo>> _PoolResultados = new List<IList<Triangulo>>();

        public enum Estado
        {
            Vacio,
            EnEspera,
            EnEjecucion,
            Terminado,
            ConErrores
        }

        public TriangulacionMultiProceso(int ProcesosDedicados)
        {
            _ProcesosDedicados = ProcesosDedicados;
        }

        #region Repartidor
        public void Repartir(Poligono Perimetro, IList<Linea> LineasRuptura, IList<Punto3D> Puntos,
                             int NumeroProcesos = 8)
        {
            //parte la triangulación en una serie de procesos
            _PoolProcesos = new List<IProceso>();

            //Secciones
            IList<Triangulaciones.Delaunay.SeccionDelaunay> Secciones = _CalcularSecciones(Perimetro, NumeroProcesos);

            foreach (Triangulaciones.Delaunay.SeccionDelaunay item in Secciones)
            {
                _PoolProcesos.Add(new ProcesoTriangulacion(_ContenidoEnSeccion(item.Seccion, Puntos),
                                                           _ContenidoEnSeccion(item.Seccion, LineasRuptura),
                                                           item.Seccion, 
                                                           item.MallaAnteriorSiguiente.MallaAnterior, 
                                                           item.MallaAnteriorSiguiente.MallaSiguiente));
            }

            _EstadoProceso = Estado.EnEspera;
        }

        private IList<Triangulaciones.Delaunay.SeccionDelaunay> _CalcularSecciones(Poligono Perimetro, int NumeroSecciones)
        {
            IList<Triangulaciones.Delaunay.SeccionDelaunay> ResSecciones = new List<Triangulaciones.Delaunay.SeccionDelaunay>();

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

        private IList<Linea> _ContenidoEnSeccion(Triangulo Seccion, IList<Linea> LineasRuptura)
        {
            IList<Linea> Res = new List<Linea>();
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
        private IList<Punto3D> _ContenidoEnSeccion(Triangulo Seccion, IList<Punto3D> Puntos)
        {
            IList<Punto3D> Res = new List<Punto3D>();
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

        public void IniciarProceso()
        {
            _EstadoProceso = Estado.EnEjecucion;
            //TODO: Lanzar el evento que se dispara al terminar un proceso


        }


        //TODO: crear el event handler que se dispara al terminar un proceso
        //mientras que haya procesos disponibles arranca el siguiente
        //cuando termine con errores o por la finalización de los merges _EstadoProceso = Terminado

        public IList<Triangulo> Resultado
        {
            get
            {
                if(_EstadoProceso== Estado.EnEspera)
                {
                    IniciarProceso();
                }

                while (_EstadoProceso == Estado.EnEjecucion)
                {
                    System.Threading.Thread.Sleep(300);
                }
                return _PoolResultados[0];
            }
        }
    }
}
