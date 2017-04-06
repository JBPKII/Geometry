using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Delaunay
{
    public class IndicesSeccionDelaunay
    {
        public int MallaAnterior = -1;
        public int MallaSiguiente = -1;
    }
    public class SeccionDelaunay
    {
        public Triangulo Seccion = new Triangulo();
        public IndicesSeccionDelaunay MallaAnteriorSiguiente = new IndicesSeccionDelaunay();
    }
    public class ResultadoDelaunay
    {
        public IList<Triangulo> Resultado = new List<Triangulo>();
        public SeccionDelaunay Seccion = new SeccionDelaunay();
    }

    public class EstructuraDelaunay
    {
        public EstructuraDelaunay()
        {
        }
    }
}
