using System;

namespace Geometry.Geometrias
{
    /// <summary>
    /// 
    /// </summary>
    public class BBox
    {
        private Punto3D _Max = null;
        private Punto3D _Min = null;

        /// <summary>
        /// 
        /// </summary>
        public BBox()
        {
            //Determinar cual es el mayor y cual el menor, por eje
            _Max = null;
            _Min = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="P"></param>
        public BBox(Punto3D P)
        {
            //Determinar cual es el mayor y cual el menor, por eje
            _Max = P;
            _Min = P;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        public BBox(Punto3D P1, Punto3D P2)
	    {
            //Determinar cual es el mayor y cual el menor, por eje
            _Max = new Punto3D( Math.Max(P1.X, P2.X),
                                Math.Max(P1.Y, P2.X),
                                Math.Max(P1.Z, P2.Z));
            _Min = new Punto3D( Math.Min(P1.X, P2.X),
                                Math.Min(P1.Y, P2.X),
                                Math.Min(P1.Z, P2.Z));
        }

        /// <summary>
        /// Añade el punto al BBox, ampliandolo si es necesario
        /// </summary>
        /// <param name="P"></param>
        public void Añadir(Punto3D P)
        {
            if (_Max == null || _Min == null)
            {
                _Max = P;
                _Min = P;
            }
            else
            {
                //Determinar cual es el mayor y cual el menor, por eje
                _Max = new Punto3D(Math.Max(_Max.X, P.X),
                                    Math.Max(_Max.Y, P.X),
                                    Math.Max(_Max.Z, P.Z));
                _Min = new Punto3D(Math.Min(_Min.X, P.X),
                                    Math.Min(_Min.Y, P.X),
                                    Math.Min(_Min.Z, P.Z));
            }
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
            //Evaluar si está dentro del BBox
            if (PTest.X >= _Min.X && PTest.X <= _Max.X)
            {
                if (PTest.Y >= _Min.Y && PTest.Y <= _Max.Y)
                {
                    if (Plano)
                    {
                        Res = true;
                    }
                    else
                    { 
                        if (PTest.Z >= _Min.Z && PTest.Z <= _Max.Z)
                        {
                            Res = true;
                        }
                    }
                }
            }

            return Res;
        }

        public Poligono ToPoligono()
        {
            Poligono ResPol = new Poligono();

            ResPol.Vertices.Add(_Min);
            ResPol.Vertices.Add(new Punto3D(_Min.X, _Max.Y, (_Min.Z + _Max.Z) / 2.0));
            ResPol.Vertices.Add(_Max);
            ResPol.Vertices.Add(new Punto3D(_Max.X, _Min.Y, (_Min.Z + _Max.Z) / 2.0));

            return ResPol;
        }
    }
}
