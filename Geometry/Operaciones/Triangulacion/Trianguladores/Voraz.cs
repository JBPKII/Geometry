﻿using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    class Voraz : ITriangulador
    {
        public IList<Geometrias.Triangulo> Triangular(Poligono Perimetro, IList<Linea> LineasRuptura, IList<Punto3D> Puntos)
        {
            IList<Triangulo> ResTriang = new List<Triangulo>();
            //TODO: https://es.wikipedia.org/wiki/Algoritmo_de_triangulaci%C3%B3n_voraz


            return ResTriang;
        }
    }
}