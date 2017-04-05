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
        IList<Geometrias.Triangulo> Resultado();

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
            IList<Triangulo> Secciones = _CalcularSecciones(Perimetro, NumeroProcesos);

            foreach (Triangulo item in Secciones)
            {
                _PoolProcesos.Add(new ProcesoTriangulacion(_ContenidoEnSeccion(item, Puntos),
                                                           _ContenidoEnSeccion(item, LineasRuptura),
                                                           item));
            }

            _EstadoProceso = Estado.EnEspera;
        }

        private IList<Triangulo> _CalcularSecciones(Poligono Perimetro, int NumeroSecciones)
        {
            IList<Triangulo> ResSecciones = new List<Triangulo>(NumeroSecciones);

            try
            {
                //TODO: Calcula las secciones que envuelven al polígono


            }
            catch (Exception sysEx)
            {
                sysEx.Data.Clear();
                ResSecciones = new List<Triangulo>();
            }

            return ResSecciones;
        }

        private IList<Linea> _ContenidoEnSeccion(Triangulo Seccion, IList<Linea> LineasRuptura)
        {
            IList<Linea> Res = new List<Linea>();
            //TODO: Evalua si la línea de ruptura está dentro
            return Res;
        }
        private IList<Punto3D> _ContenidoEnSeccion(Triangulo Seccion, IList<Punto3D> Puntos)
        {
            IList<Punto3D> Res = new List<Punto3D>();
            //TODO: Evalua si el punto está dentro
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
                while (_EstadoProceso == Estado.EnEjecucion)
                {
                    System.Threading.Thread.sleep(300);
                }
                return _PoolResultados[0];
            }
        }
    }
}
