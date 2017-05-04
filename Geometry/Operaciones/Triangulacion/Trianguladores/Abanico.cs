using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulacion.Trianguladores.Abanico
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

        public IList<Triangulo> Merge(IList<Triangulo> triangulacion1, IList<Triangulo> triangulacion2)
        {
            //TODO: Desarrollar Merge
            IList<Triangulo> ResTriang = new List<Triangulo>();


            return ResTriang;
        }
    }
}
