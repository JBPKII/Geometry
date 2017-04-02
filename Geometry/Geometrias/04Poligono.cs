using System;
using System.Collections.Generic;

namespace Geometry.Geometrias
{
    /// <summary>
    /// Polígono irregular definido por sus vértices
    /// </summary>
    public class Poligono
    {
        IList<Punto3D> Vertices;
        /// <summary>
        /// Inicializa un polígono sin vértices
        /// </summary>
        public Poligono()
        {
            Vertices = new List<Punto3D>();
        }
    }
}
