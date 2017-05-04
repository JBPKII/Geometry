using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulacion.Trianguladores.MinimoPeso
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

        public IList<Triangulo> Merge(IList<Triangulo> triangulacion1, IList<Triangulo> triangulacion2)
        {
            //TODO: Desarrollar Merge
            IList<Triangulo> ResTriang = new List<Triangulo>();


            return ResTriang;
        }
    }
}
