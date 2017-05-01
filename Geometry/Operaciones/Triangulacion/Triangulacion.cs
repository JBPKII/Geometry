using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones
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
        public IList<Triangulo> Resultado
        {
            get
            {
                return _Resultado;
            }
        }

        private ITriangulador GetNewTriangulador(TipoTriangulado Metodo)
        {
            switch (Metodo)
            {
                case TipoTriangulado.Abanico:
                    return new Trianguladores.Abanico();
                    //break;
                case TipoTriangulado.Delaunay:
                    return new Trianguladores.Delaunay();
                    //break;
                case TipoTriangulado.MinimoPeso:
                    return new Trianguladores.Abanico();
                    //break;
                case TipoTriangulado.Voraz:
                    return new Trianguladores.Abanico();
                    //break;
                case TipoTriangulado.Ninguna:
                default:
                    return null;
                    //break;
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
            ITriangulador Triangulador = GetNewTriangulador(Metodo);

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
            ITriangulador Triangulador = GetNewTriangulador(Metodo);

            if (Triangulador != null)
            {
                _Resultado = Triangulador.Triangular(PerimetroExclusion, new List<Linea>(), Puntos);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
