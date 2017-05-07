using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulacion.Trianguladores.Abanico
{
    public class IndicesSeccionAbanico
    {
        public int Anterior = -1;
        public int Siguiente = -1;
    }
    public class SeccionAbanico
    {
        public Triangulo Seccion = new Triangulo();
        public IndicesSeccionAbanico ParAristas = new IndicesSeccionAbanico();
    }
    public class ResultadoAbanico : IResultadoTriangulacion
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

    public class EstructuraAbanico
    {
        public EstructuraAbanico()
        {
        }
    }
}
