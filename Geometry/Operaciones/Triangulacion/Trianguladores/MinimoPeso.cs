﻿using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    class MinimoPeso : ITriangulador
    {
        public IList<Geometrias.Triangulo> Triangular(Poligono Perimetro, IList<Linea> LineasRuptura, IList<Punto3D> Puntos)
        {
            IList<Triangulo> ResTriang = new List<Triangulo>();
            //TODO: https://es.wikipedia.org/wiki/Triangulaci%C3%B3n_de_peso_m%C3%ADnimo


            return ResTriang;
        }
    }
}