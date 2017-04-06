using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Analisis
{
    class AnalisisGeometrico
    {
        //Return -1 o 1 en función del sentido del triángulo
        public static int SentidoTriangulo(Punto3D P1, Punto3D P2, Punto3D P3)
        {
            int ResSentido = 0;
            const int _sentidoPositivo = 1;
            const int _sentidoNegativo = -1;

            try
            {
                if ((P1.X - P3.X) * (P2.Y - P3.Y) - (P1.Y - P3.Y) * (P2.X - P3.X) >= 0)
                {
                    ResSentido = _sentidoPositivo;
                }
                else
                {
                    ResSentido = _sentidoNegativo;
                }

            }
            catch (System.Exception)
            {
                ResSentido = 0;
            }

            return ResSentido;
        }

        public static bool PuntoEnTriangulo(Punto3D PTest, Triangulo T)
        {
            return PuntoEnTriangulo(PTest, T.P1, T.P2, T.P3);
        }
        public static bool PuntoEnTriangulo(Punto3D PTest, Punto3D T1, Punto3D T2, Punto3D T3)
        {
            bool ResPeretenece = false;
            const int _sentidoPositivo = 1;
            const int _sentidoNegativo = -1;

            //T1,T2,T3
            int SentidoOriginal = SentidoTriangulo(T1, T2, T3);

            //T1,T2,PTest
            int T1T2P = SentidoTriangulo(T1, T2, PTest);

            //T2,T3,PTest
            int T2T3P = SentidoTriangulo(T2, T3, PTest);

            //T3,T1,PTest
            int T3T1P = SentidoTriangulo(T3, T1, PTest);

            if (SentidoOriginal == _sentidoPositivo)
            {
                if (T1T2P == _sentidoPositivo &&
                    T2T3P == _sentidoPositivo &&
                    T3T1P == _sentidoPositivo)
                {
                    ResPeretenece = true;
                }
            }
            else if (SentidoOriginal == _sentidoNegativo)
            {
                if (T1T2P == _sentidoNegativo &&
                    T2T3P == _sentidoNegativo &&
                    T3T1P == _sentidoNegativo)
                {
                    ResPeretenece = true;
                }
            }

            return ResPeretenece;
        }

        public static Triangulo OrdenarVertices(Triangulo TrianguloOriginal,bool SentidoHorario = false)
        {
            Triangulo ResTriangulo = TrianguloOriginal;

            if(SentidoHorario)
            {
                if(ResTriangulo.Sentido != -1)
                {
                    ResTriangulo = new Triangulo(TrianguloOriginal.P2, TrianguloOriginal.P1, TrianguloOriginal.P3);
                }
            }
            else
            {
                if (TrianguloOriginal.Sentido != 1)
                {
                    ResTriangulo = new Triangulo(TrianguloOriginal.P2, TrianguloOriginal.P1, TrianguloOriginal.P3);
                }
            }

            return ResTriangulo;
        }

        public static bool PuntoCircunscritoDelaunay(Triangulo Triangulo, Punto3D PTest)
        {
            //Triángulo en orden antihorario
            Triangulo = OrdenarVertices(Triangulo, false);

            Matrices.Matriz MatCircunscrito = new Matrices.Matriz(new double[4, 4] {
                {Triangulo.P1.X, Triangulo.P1.Y, Math.Pow(Triangulo.P1.X, 2.0) + Math.Pow(Triangulo.P1.Y, 2.0), 1.0 },
                {Triangulo.P2.X, Triangulo.P2.Y, Math.Pow(Triangulo.P2.X, 2.0) + Math.Pow(Triangulo.P2.Y, 2.0), 1.0 },
                {Triangulo.P3.X, Triangulo.P3.Y, Math.Pow(Triangulo.P3.X, 2.0) + Math.Pow(Triangulo.P3.Y, 2.0), 1.0 },
                {PTest.X,        PTest.Y,        Math.Pow(PTest.X, 2.0) +        Math.Pow(PTest.Y,        2.0), 1.0}
            });

            return MatCircunscrito.Determinate>0;
        }

        private static double AreaLazadaGauss(IList<Punto3D> P3dColl)
        {
            double PrimeraLazada = 0.0;
            double SegundaLazada = 0.0;

            for (int i = 0; i < P3dColl.Count - 1; i++)
            {
                PrimeraLazada += (P3dColl[i].X * P3dColl[i + 1].Y);
            }

            for (int i = 0; i < P3dColl.Count - 1; i++)
            {
                SegundaLazada += (P3dColl[i].Y * P3dColl[i + 1].X);
            }

            return Math.Abs(PrimeraLazada - SegundaLazada) / 2.0;
        }

    }
}
