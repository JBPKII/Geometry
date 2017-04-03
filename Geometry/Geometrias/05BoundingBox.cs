using System;

namespace Geometry.Geometrias
{
    /// <summary>
    /// 
    /// </summary>
    public class BBox
    {
        private Punto3D _Max;
        private Punto3D _Min;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        public BBox(Punto3D P1, Punto3D P2)
	    {
            //TODO: Determinar cual es el mayor y cual el menor, por eje

        }

        /// <summary>
        /// Añade el punto al BBox, ampliandolo si es necesario
        /// </summary>
        /// <param name="P"></param>
        public void Añadir(Punto3D P)
        {
            //TODO: Determinar cual es el mayor y cual el menor, por eje
        }

        /// <summary>
        /// Anchura del BBox (eje X)
        /// </summary>
        public double Ancho
        {
            get
            {
                return _Max.X  - _Min.X;
            }
        }
        /// <summary>
        /// Altura del BBox (eje Y)
        /// </summary>
        public double Alto
        {
            get
            {
                return _Max.Y - _Min.Y;
            }
        }
        /// <summary>
        /// Profundidad del BBox (eje Z)
        /// </summary>
        public double Profundidad
        {
            get
            {
                return _Max.Z - _Min.Z;
            }
        }
        /// <summary>
        /// Punto máximo
        /// </summary>
        public Punto3D Maximo
        {
            get
            {
                return _Max;
            }
        }
        /// <summary>
        /// Punto mínimo
        /// </summary>
        public Punto3D Minimo
        {
            get
            {
                return _Min;
            }
        }

        /// <summary>
        /// Evalua si el punto está dentro del BBox
        /// </summary>
        /// <param name="PTest"></param>
        /// <param name="Plano"></param>
        /// <returns></returns>
        public bool Contiene(Punto3D PTest, bool Plano = true)
        {
            bool Res = false;
            //TODO: Evaluar si está dentro del BBox

            return Res;
        }
    }
}
