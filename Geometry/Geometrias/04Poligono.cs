using System;
using System.Collections.Generic;

namespace Geometry.Geometrias
{
    /// <summary>
    /// Polígono irregular definido por sus vértices
    /// </summary>
    public class Poligono
    {
        /// <summary>
        /// Colección con los vértices que definen el contorno del polígono
        /// </summary>
        public IList<Punto3D> Vertices;

        IList<Triangulo> _Triangulacion = new List<Triangulo>();
        Operaciones.Triangulaciones.TipoTriangulado _TipoTriangulacion = Operaciones.Triangulaciones.TipoTriangulado.Ninguna;

        /// <summary>
        /// Inicializa un polígono sin vértices
        /// </summary>
        public Poligono()
        {
            Vertices = new List<Punto3D>();
        }

        /// <summary>
        /// Retorna si el punto indicado está dentro del triángulo
        /// </summary>
        /// <param name="PTest"></param>
        /// <returns></returns>
        public bool Contiene(Punto3D PTest)
        {
            bool Res = false;

            if(_Triangular(Operaciones.Triangulaciones.TipoTriangulado.Delaunay))
            {
                foreach (Triangulo TestTriangulo in _Triangulacion)
                {
                    if(TestTriangulo.Contiene(PTest))
                    {
                        Res = true;
                        break;
                    }
                }
            }

            return Res;
        }

        /// <summary>
        /// Triangula el polígono
        /// </summary>
        /// <param name="TipoTriangulacion"></param>
        /// <param name="ForzarActualizacion"></param>
        /// <returns></returns>
        private bool _Triangular (Operaciones.Triangulaciones.TipoTriangulado TipoTriangulacion, bool ForzarActualizacion = false)
        {
            bool Res = false;

            if (ForzarActualizacion || TipoTriangulacion != _TipoTriangulacion)
            {
                _Triangulacion = new List<Triangulo>();
            }

            if (_Triangulacion.Count == 0)
            {
                //Recalcula la triangualación
                _Triangulacion = Operaciones.Triangulaciones.Triangulacion.TriangularPoligono(this, _TipoTriangulacion);
            }

            Res = (_Triangulacion.Count > 0);

            return Res;
        }
    }
}
