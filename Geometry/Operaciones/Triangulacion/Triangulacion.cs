using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulacion
{
    /// <summary>
    /// 
    /// </summary>
    public enum TipoTriangulado
    {
        /// <summary>
        /// Valor por defecto o que no se ha realizado ninguna triangulación
        /// </summary>
        Ninguna,
        /// <summary>
        /// https://es.wikipedia.org/wiki/Triangulaci%C3%B3n_en_abanico
        /// </summary>
        Abanico,
        /// <summary>
        /// https://es.wikipedia.org/wiki/Triangulaci%C3%B3n_de_Delaunay
        /// </summary>
        Delaunay,
        /// <summary>
        /// https://es.wikipedia.org/wiki/Triangulaci%C3%B3n_de_peso_m%C3%ADnimo
        /// </summary>
        MinimoPeso,
        /// <summary>
        /// https://es.wikipedia.org/wiki/Algoritmo_de_triangulaci%C3%B3n_voraz
        /// </summary>
        Voraz
    }

    public interface IResultadoTriangulacion
    {
        IList<Triangulo> Resultado { get; set; }
        SeccionTriangulacion Seccion { get; }
    }

    public class IndicesSeccionTriangulacion
    {
        public int Anterior = -1;
        public int Siguiente = -1;

        public bool EsConsecutiva (IndicesSeccionTriangulacion SeccionTest)
        {
            bool Res = false;

            if(Anterior != -1 && Siguiente != -1 &&
                SeccionTest.Anterior != -1 && SeccionTest.Siguiente != -1)
            {
                if (this.Anterior == SeccionTest.Siguiente)
                {
                    // 1,2 - 0,1
                    Res = true;
                }
                else if (this.Siguiente == SeccionTest.Anterior)
                {
                    // 0,1 - 1,2
                    Res = true;
                }
                //else
                //{
                //    //no consecutivas
                //    Res = false;
                //}
            }

            return Res;
        }
    }
    public class SeccionTriangulacion
    {
        public Triangulo TrianguloSeccion = new Triangulo();
        public IndicesSeccionTriangulacion ParAristas = new IndicesSeccionTriangulacion();
    }

    /// <summary>
    /// 
    /// </summary>
    public class Triangulacion
    {
        private IList<Triangulo> _Resultado = new List<Triangulo>();

        /// <summary>
        /// 
        /// </summary>
        public Triangulacion()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private Log.Log _logTriangulacion = new Log.Log();
        public Log.Log LogTriangulacion
        {
            get { return _logTriangulacion; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<Triangulo> Resultado
        {
            get
            {
                return _Resultado;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PerimetroPoligono"></param>
        /// <param name="Metodo"></param>
        /// <returns></returns>
        public bool TriangularPoligono(Poligono PerimetroPoligono, TipoTriangulado Metodo = TipoTriangulado.Delaunay)
        {
            ITriangulador Triangulador = new Trianguladores.Delaunay.Delaunay();

            if (Triangulador != null)
            {
                _Resultado = Triangulador.Triangular(PerimetroPoligono, new List<Linea>(), PerimetroPoligono.Vertices);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PerimetroExclusion"></param>
        /// <param name="Metodo"></param>
        /// <returns></returns>
        public bool TriangularMalla(Poligono PerimetroExclusion, List<Linea> LineasRuptura, List<Punto3D> Puntos, TipoTriangulado Metodo = TipoTriangulado.Delaunay)
        {
            IList<Triangulo> ResTriang = new List<Triangulo>();

            int currentManagedThread = Environment.CurrentManagedThreadId;
            int processorCount = Environment.ProcessorCount;

            TriangulacionMultiProceso TrianguladorMultiProceso = new TriangulacionMultiProceso(processorCount)
            {
                TipoTriangulado = Metodo,
                Perimetro = PerimetroExclusion,
                LineasRuptura = LineasRuptura,
                Puntos3D = Puntos
            };

            TrianguladorMultiProceso.IniciarProceso();

            while (TrianguladorMultiProceso.EstadoProceso == TriangulacionMultiProceso.Estado.EnEjecucion)
            {
                System.Threading.Thread.Sleep(300);
            }

            if (TrianguladorMultiProceso.EstadoProceso == TriangulacionMultiProceso.Estado.Terminado)
            {
                _Resultado = TrianguladorMultiProceso.Resultado.Resultado;
                _logTriangulacion = TrianguladorMultiProceso.LogMultiProceso;
                return true;
            }
            else
            {
                //Informar de cada uno de los errores que an detenido cada uno de los procesos
                _logTriangulacion = TrianguladorMultiProceso.LogMultiProceso;
                return false;
            }
        }

        public static ITriangulador GetNewTriangulador(TipoTriangulado Metodo)
        {
            switch (Metodo)
            {
                case TipoTriangulado.Abanico:
                    return new Trianguladores.Abanico.Abanico();
                //break;
                case TipoTriangulado.Delaunay:
                    return new Trianguladores.Delaunay.Delaunay();
                //break;
                case TipoTriangulado.MinimoPeso:
                    return new Trianguladores.MinimoPeso.MinimoPeso();
                //break;
                case TipoTriangulado.Voraz:
                    return new Trianguladores.Voraz.Voraz();
                //break;
                case TipoTriangulado.Ninguna:
                default:
                    return null;
                    //break;
            }
        }

        public static ITriangulador GetNewMerge(TipoTriangulado Metodo)
        {
            switch (Metodo)
            {
                case TipoTriangulado.Abanico:
                    return new Trianguladores.Abanico.Abanico();
                //break;
                case TipoTriangulado.Delaunay:
                    return new Trianguladores.Delaunay.Delaunay();
                //break;
                case TipoTriangulado.MinimoPeso:
                    return new Trianguladores.MinimoPeso.MinimoPeso();
                //break;
                case TipoTriangulado.Voraz:
                    return new Trianguladores.Voraz.Voraz();
                //break;
                case TipoTriangulado.Ninguna:
                default:
                    return null;
                    //break;
            }
        }

        public static IResultadoTriangulacion GetNewResultadoTriangulacion(TipoTriangulado Metodo)
        {
            switch (Metodo)
            {
                case TipoTriangulado.Abanico:
                    return new Trianguladores.Abanico.ResultadoAbanico();
                //break;
                case TipoTriangulado.Delaunay:
                    return new Trianguladores.Delaunay.ResultadoDelaunay();
                //break;
                case TipoTriangulado.MinimoPeso:
                    return new Trianguladores.MinimoPeso.ResultadoMinimoPeso();
                //break;
                case TipoTriangulado.Voraz:
                    return new Trianguladores.Voraz.ResultadoVoraz();
                //break;
                case TipoTriangulado.Ninguna:
                default:
                    return null;
                    //break;
            }
        }
    }
}
