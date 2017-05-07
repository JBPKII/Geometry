using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulacion.Trianguladores.Delaunay
{
    /// <summary>
    /// https://es.wikipedia.org/wiki/Triangulaci%C3%B3n_de_Delaunay
    /// </summary>
    class Delaunay : ITriangulador
    {
        public IList<Triangulo> Triangular(Poligono Perimetro, List<Linea> LineasRuptura, List<Punto3D> Puntos)
        {
            //TODO: Algoritmo triangulación Delaunay
            IList<Triangulo> ResTriang = new List<Triangulo>();

            //Condición de Delaunay
            //Analisis.AnalisisGeometrico.PuntoCircunscrito(Triangulo,Ptest) 

            //el espacio se divide en el número de procesadores de la máquina y se lanzan sucesivos thread para resolver cada una de las zonas
            //Según terminan se van uniendo y resolviendo cada una de las divisiones en función de cuando terminen



            return ResTriang;
        }

        public IList<Triangulo> Merge(IList<Triangulo> triangulacion1, IList<Triangulo> triangulacion2)
        {
            MergeDelaunay MD = new MergeDelaunay(triangulacion1, triangulacion2);
            return MD.DoMerge();
        }

        private IList<Triangulo> _Split(Triangulo T, Punto3D P)
        {
            IList<Triangulo> Res = new List<Triangulo>();

            Triangulo res = new Triangulo(T.P1, T.P2, P);
            res.OrdenarVertices(false);//CCW
            Res.Add(res);

            res = new Triangulo(T.P2, T.P3, P);
            res.OrdenarVertices(false);//CCW
            Res.Add(res);

            res = new Triangulo(T.P3, T.P1, P);
            res.OrdenarVertices(false);//CCW
            Res.Add(res);

            return Res;
        }

        private IList<Triangulo> _Flip(Triangulo T1,Triangulo T2)
        {
            IList<Triangulo> Res = new List<Triangulo> { T1, T2 };

            IList<Punto3D> _aristaComun = new List<Punto3D>();
            Punto3D _puntoSoloT1 = null;
            Punto3D _puntoSoloT2 = null;

            #region Obtiene la arista común
            if (T1.P1 == T2.P1 || T1.P1 == T2.P2 || T1.P1 == T2.P3)
            {
                _aristaComun.Add(T1.P1);
            }
            else
            {
                _puntoSoloT1 = T1.P1;
            }

            if (T1.P2 == T2.P1 || T1.P2 == T2.P2 || T1.P2 == T2.P3)
            {
                _aristaComun.Add(T1.P2);
            }
            else
            {
                _puntoSoloT1 = T1.P2;
            }

            if (T1.P3 == T2.P1 || T1.P3 == T2.P2 || T1.P3 == T2.P3)
            {
                _aristaComun.Add(T1.P1);
            }
            else
            {
                _puntoSoloT1 = T1.P1;
            }

            if (!_aristaComun.Contains(T2.P1))
            {
                _puntoSoloT2 = T2.P1;
            }
            else if (!_aristaComun.Contains(T2.P2))
            {
                _puntoSoloT2 = T2.P2;
            }
            else
            {
                _puntoSoloT2 = T2.P3;
            }
            #endregion

            if (_aristaComun.Count == 2 && _puntoSoloT1 != null && _puntoSoloT2 != null)
            {
                //Pasa la condición de Delaunay
                if (!_CondicionDelaunay(T1, _puntoSoloT2))
                {
                    Res.Clear();

                    //Hace el Flip
                    Triangulo temp = new Triangulo(_puntoSoloT1, _puntoSoloT2, _aristaComun[0]);
                    temp.OrdenarVertices(false);
                    Res.Add(temp);

                    temp = new Triangulo(_puntoSoloT1, _puntoSoloT2, _aristaComun[1]);
                    temp.OrdenarVertices(false);
                    Res.Add(temp);
                }
            }
            else
            {
                //No comparten solo una arista
            }

            return Res;
        }

        private bool _CondicionDelaunay(Triangulo T, Punto3D P)
        {
            //TODO: determinar cual de las dos operaciones matriciales es más rápida

            T.OrdenarVertices(false);

            Matrices.Matriz MatDelaunay = new Matrices.Matriz(new double[4,4]
                {
                    {T.P1.X, T.P1.Y, Math.Pow(T.P1.X, 2.0) + Math.Pow(T.P1.Y, 2.0), 1.0},
                    {T.P2.X, T.P2.Y, Math.Pow(T.P2.X, 2.0) + Math.Pow(T.P2.Y, 2.0), 1.0},
                    {T.P3.X, T.P3.Y, Math.Pow(T.P3.X, 2.0) + Math.Pow(T.P3.Y, 2.0), 1.0},
                    {   P.X,    P.Y, Math.Pow(   P.X, 2.0) + Math.Pow(   P.Y, 2.0), 1.0}
                });

            /*Matrices.Matriz MatDelaunayCompacta = new Matrices.Matriz(new double[3,3]
                {
                    {T.P1.X - P.X, T.P1.Y - P.Y, Math.Pow(T.P1.X - P.X, 2.0) + Math.Pow(T.P1.Y - P.Y, 2.0)},
                    {T.P2.X - P.X, T.P2.Y - P.Y, Math.Pow(T.P2.X - P.X, 2.0) + Math.Pow(T.P2.Y - P.Y, 2.0)},
                    {T.P3.X - P.X, T.P3.Y - P.Y, Math.Pow(T.P3.X - P.X, 2.0) + Math.Pow(T.P3.Y - P.Y, 2.0)},
                });*/

            return MatDelaunay.Determinate > 0.0;
            //return MatDelaunayCompacta.Determinate > 0.0;
        }
    }
}
