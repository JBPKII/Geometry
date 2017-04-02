using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones
{
    interface ITriangulador
    {
        IList<Triangulo> Triangular(Poligono Perimetro, IList<Linea> LineasRuptura, IList<Punto3D> Puntos);
    } 
}
