using System;
using System.Collections.Generic;

namespace Geometry.Geometrias
{
    [Serializable]
    /// <summary>
    /// Polígono irregular definido por sus vértices
    /// </summary>
    public class Poligono
    {
        /// <summary>
        /// Colección con los vértices que definen el contorno del polígono
        /// </summary>
        public List<Punto3D> Vertices;

        private IList<Triangulo> _Triangulacion = new List<Triangulo>();
        Operaciones.Triangulacion.TipoTriangulado _TipoTriangulacion = Operaciones.Triangulacion.TipoTriangulado.Ninguna;

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

            if(_Triangular(Operaciones.Triangulacion.TipoTriangulado.Delaunay))
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
        private bool _Triangular (Operaciones.Triangulacion.TipoTriangulado TipoTriangulacion, bool ForzarActualizacion = false)
        {
            bool Res = false;

            if (ForzarActualizacion || TipoTriangulacion != _TipoTriangulacion)
            {
                _Triangulacion = new List<Triangulo>();
            }

            if (_Triangulacion.Count == 0)
            {
                //Recalcula la triangualación
                Operaciones.Triangulacion.Triangulacion Triang = new Operaciones.Triangulacion.Triangulacion();
                Triang.TriangularPoligono(this, _TipoTriangulacion);
                _Triangulacion = Triang.Resultado;
            }

            Res = (_Triangulacion.Count > 0);

            return Res;
        }
    }
}
