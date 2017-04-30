using System;
using System.Xml.Serialization;

namespace Geometry.Geometrias
{
    [Serializable]
    /// <summary>
    /// Punto con terna de coordenadas
    /// </summary>
    public class Punto3D
    {
        private ulong _id;
        /// <summary>
        /// ID del punto
        /// </summary>
        public ulong ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                _SinID = false; ;
            }
        }
        private bool _SinID = true;

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

        private string _descripcion;
        /// <summary>
        /// ID del punto
        /// </summary>
        public string Descripcion
        {
            get
            {
                return _descripcion;
            }
            set
            {
                _descripcion = value;
                _SinDescrip = false; ;
            }
        }
        private bool _SinDescrip = true;

        private int _precision = 3;

        /// <summary>
        /// Inicializa un punto en el origen cartesiano de coordenadas
        /// </summary>
        public Punto3D()
        {
            _id = 0;
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
            _SinID = true;
        }

        /// <summary>
        /// Inicializa un punto con las coorenadas X, Y y Z
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Punto3D(double x, double y, double z)
        {
            _id = 0;
            X = x;
            Y = y;
            Z = z;
            _SinID = true;
        }

        public Punto3D(ulong id, double x, double y, double z)
        {
            _id = id;
            X = x;
            Y = y;
            Z = z;
            _SinID = false;
        }

        /// <summary>
        /// Proporciona las coorenadas en formato X, Y, Z
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string PreId = _SinID ? "" : string.Format("{0}, ", ID.ToString());
            string PosrDesc = _SinDescrip ? "" : string.Format(", {0}", Descripcion);

            return string.Format("{0}{1}, {2}, {3}{4}",
                                PreId,
                                Math.Round(X, _precision, MidpointRounding.ToEven).ToString(),
                                Math.Round(Y, _precision, MidpointRounding.ToEven).ToString(),
                                Math.Round(Z, _precision, MidpointRounding.ToEven).ToString(),
                                PosrDesc);
        }
    }
}