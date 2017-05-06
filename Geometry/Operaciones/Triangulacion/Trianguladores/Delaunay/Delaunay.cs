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
    }
}
