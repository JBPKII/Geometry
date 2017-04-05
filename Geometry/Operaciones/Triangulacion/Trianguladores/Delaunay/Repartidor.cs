using System;
using System.Collections.Generic;
using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Delaunay
{
    /// <summary>
    /// Se encarga de dividir el espacio y los datos para cada uno de los threads
    /// </summary>
    public class Repartidor
    {
        /*public Repartidor()
        {
        }*/

        internal static void ConfigurarThreads(ref ThreadDelaunay[] threadCollection, Poligono perimetro, IList<Linea> lineasRuptura, IList<Punto3D> puntos)
        {

            threadCollection = new ThreadDelaunay[Environment.ProcessorCount];

            for (int i = threadCollection.GetLowerBound(0); i <= threadCollection.GetUpperBound(0); i++)
            {
                threadCollection[i] = new ThreadDelaunay();

            }
        }

        private IList<>
    }
}
