using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones
{
    interface ITriangulador
    {
        IList<Triangulo> Triangular(Poligono Perimetro, List<Linea> LineasRuptura, List<Punto3D> Puntos);
    } 
}
