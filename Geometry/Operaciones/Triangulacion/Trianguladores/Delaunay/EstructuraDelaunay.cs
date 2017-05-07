using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulacion.Trianguladores.Delaunay
{
    public class IndicesSeccionDelaunay
    {
        public int Anterior = -1;
        public int Siguiente = -1;
    }
    public class SeccionDelaunay
    {
        public Triangulo Seccion = new Triangulo();
        public IndicesSeccionDelaunay ParAristas = new IndicesSeccionDelaunay();
    }
    public class ResultadoDelaunay : IResultadoTriangulacion
    {
        private IList<Triangulo> _Resultado = new List<Triangulo>();
        public IList<Triangulo> Resultado
        {
            set
            {
                _Resultado = value;
            }
            get
            {
                return _Resultado;
            }
        }
        private SeccionTriangulacion _Seccion = new SeccionTriangulacion();
        public SeccionTriangulacion Seccion
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
