using System;

namespace Geometrias
{
    public class Punto3D
    {
        public double X;
        public double Y;
        public double Z;

        private int _precision = 3;

        public Punto3D()
        {
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
        }

        public Punto3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", 
                                Math.Round(X, _precision, MidpointRounding.ToEven),
                                Math.Round(Y, _precision, MidpointRounding.ToEven),
                                Math.Round(Z, _precision, MidpointRounding.ToEven));
        }
    }
}