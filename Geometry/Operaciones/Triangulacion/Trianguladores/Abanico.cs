using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    /// <summary>
    /// TODO: https://es.wikipedia.org/wiki/Triangulaci%C3%B3n_en_abanico
    /// </summary>
    class Abanico : ITriangulador
    {
        public IList<Triangulo> Triangular(Poligono Perimetro, List<Linea> LineasRuptura, List<Punto3D> Puntos)
        {
            IList<Triangulo> ResTriang = new List<Triangulo>();


            return ResTriang;
        }
    }
}
