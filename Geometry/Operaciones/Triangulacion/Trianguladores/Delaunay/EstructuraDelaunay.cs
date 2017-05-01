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
    public class ResultadoDelaunay : Trianguladores.IResultadoTriangulacion
    {
        private IList<Triangulo> _Resultado = new List<Triangulo>();
        public IList<Triangulo> Resultado
        {
            private set
            {
                _Resultado = value;
            }
            get
            {
                return _Resultado;
            }
        }
        private Trianguladores.SeccionTriangulacion _Seccion = new Trianguladores.SeccionTriangulacion();
        public Trianguladores.SeccionTriangulacion Seccion
        {
            private set
            {
                _Seccion = value;
            }
            get
            {
                return _Seccion;
            }
        }
    }

    public class EstructuraDelaunay
    {
        public EstructuraDelaunay()
        {
        }
    }
}
