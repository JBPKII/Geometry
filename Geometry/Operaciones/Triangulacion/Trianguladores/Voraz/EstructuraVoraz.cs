using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulacion.Trianguladores.Voraz
{
    public class IndicesSeccionVoraz
    {
        public int Anterior = -1;
        public int Siguiente = -1;
    }
    public class SeccionVoraz
    {
        public Triangulo Seccion = new Triangulo();
        public IndicesSeccionVoraz ParAristas = new IndicesSeccionVoraz();
    }
    public class ResultadoVoraz : IResultadoTriangulacion
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

    public class EstructuraVoraz
    {
        public EstructuraVoraz()
        {
        }
    }
}
