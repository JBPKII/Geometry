using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulacion
{
    public interface ITriangulador
    {
        IList<Triangulo> Triangular(Poligono Perimetro, List<Linea> LineasRuptura, List<Punto3D> Puntos);
        IList<Triangulo> Merge(IList<Triangulo> triangulacion1, IList<Triangulo> triangulacion2);
    } 
}
