using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Matrices
{
    class Matriz
    {
        double[,] _Elementos;

        public double Determinate
        {
            get
            {
                return _Determinante(_Elementos);
            }
        }

        private Matriz SubMatriz(int i, int j)
        {
            double[,] Elementos = new double[_Elementos.GetUpperBound(0), _Elementos.GetUpperBound(1)];

            int nextI = 0;
            int nextJ = 0;
            for (int I = _Elementos.GetLowerBound(0); I <= _Elementos.GetUpperBound(0); I++)
            {
                if (I != i)
                {
                    for (int J = _Elementos.GetLowerBound(1); J <= _Elementos.GetUpperBound(1); J++)
                    {
                        if (J != j)
                        {
                            Elementos[nextI, nextJ] = _Elementos[I, J];
                            nextJ++;
                        }
                    }
                    nextI++;
                }
                nextJ = 0;
            }

            return new Matriz(Elementos);
        }

        private double _Determinante(double[,] Mat)
        {
            double Res = 0.0;

            if (_Elementos.Rank == 2)
            {
                if (_Elementos.GetUpperBound(0) == _Elementos.GetUpperBound(1))
                {
                    if(_Elementos.GetUpperBound(0) == 1)
                    {
                        Res = (Mat[0, 0] * Mat[1, 1]) - (Mat[0, 1] * Mat[1, 0]);
                    }
                    else
                    {
                        int Signo = 1;
                        double Det = 0.0;
                        for (int i = _Elementos.GetLowerBound(0); i <= _Elementos.GetUpperBound(0); i++)
                        {
                            Det += (Signo * _Elementos[i, 0] * SubMatriz(i, 0).Determinate);
                            Signo = Signo * -1;
                        }
                    }
                }
                else
                {
                    //Excepción por no ser cuadrada
                    throw new RankException("La matriz no es cuadrada.");
                }
            }
            else
            {
                throw new RankException("La matriz no es de dos dimensiones.");
            }

            return Res;
        }

        public double Elemento(int i, int j)
        {
            return _Elementos[i, j] ;
        }

        public void Elemento(int i, int j, double valor)
        {
            _Elementos[i,j] = valor;
        }

        public Matriz(double[,] Elementos)
        {
            _Elementos = Elementos;
        }
    }
}
