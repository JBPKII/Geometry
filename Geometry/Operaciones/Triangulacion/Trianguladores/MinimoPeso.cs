using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    /// <summary>
    /// TODO: https://es.wikipedia.org/wiki/Triangulaci%C3%B3n_de_peso_m%C3%ADnimo
    /// </summary>
    class MinimoPeso : ITriangulador
    {
        public IList<Triangulo> Triangular(Poligono Perimetro, List<Linea> LineasRuptura, List<Punto3D> Puntos)
        {
            IList<Triangulo> ResTriang = new List<Triangulo>();
            


            return ResTriang;
        }
    }
}
