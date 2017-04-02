using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Analisis
{
    /// <summary>
    /// 
    /// </summary>
    public class Triangulacion
    {
        /// <summary>
        /// 
        /// </summary>
        public enum TipoTriangulado
        {
            /// <summary>
            /// https://es.wikipedia.org/wiki/Triangulaci%C3%B3n_en_abanico
            /// </summary>
            Avanico,
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
        public Triangulacion()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PerimetroPoligono"></param>
        /// <param name="Metodo"></param>
        /// <returns></returns>
        public static IList<Triangulo> TriangularPoligono(Poligono PerimetroPoligono, TipoTriangulado Metodo = TipoTriangulado.Delaunay)
        {
            IList<Triangulo> ResTriangulacion = new List<Triangulo>();



            return ResTriangulacion;
        }
    }
}
