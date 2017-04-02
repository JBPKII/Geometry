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






    }
}
