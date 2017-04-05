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
        IList<Triangulo> _Resultado = new List<Triangulo>();

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PerimetroPoligono"></param>
        /// <param name="Metodo"></param>
        /// <returns></returns>
        public bool TriangularPoligono(Poligono PerimetroPoligono, TipoTriangulado Metodo = TipoTriangulado.Delaunay)
        {
            ITriangulador Triangulador = null; ;

            switch (Metodo)
            {
                case TipoTriangulado.Abanico:
                    Triangulador = new Trianguladores.Abanico();
                    break;
                case TipoTriangulado.Delaunay:
                    Triangulador = new Trianguladores.Delaunay();
                    break;
                case TipoTriangulado.MinimoPeso:
                    Triangulador = new Trianguladores.Abanico();
                    break;
                case TipoTriangulado.Voraz:
                    Triangulador = new Trianguladores.Abanico();
                    break;
                case TipoTriangulado.Ninguna:
                default:
                    Triangulador = null;
                    break;
            }

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
    }
}
