using System;

namespace Geometry.Geometrias
{
    /// <summary>
    /// Punto con terna de coordenadas
    /// </summary>
    public class Punto3D
    {
        /// <summary>
        /// Coordena X del punto
        /// </summary>
        public double X;

        /// <summary>
        /// Coordena Y del punto
        /// </summary>
        public double Y;

        /// <summary>
        /// Coordena Z del punto
        /// </summary>
        public double Z;

        private int _precision = 3;

        /// <summary>
        /// Inicializa un punto en el origen cartesiano de coordenadas
        /// </summary>
        public Punto3D()
        {
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
        }

        /// <summary>
        /// Inicializa un punto con las coorenadas X, Y y Z
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Punto3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Proporciona las coorenadas en formato X, Y, Z
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", 
                                Math.Round(X, _precision, MidpointRounding.ToEven).ToString(),
                                Math.Round(Y, _precision, MidpointRounding.ToEven).ToString(),
                                Math.Round(Z, _precision, MidpointRounding.ToEven).ToString());
        }
    }
}