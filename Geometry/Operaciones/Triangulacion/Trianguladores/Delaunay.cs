using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    class Delaunay : ITriangulador
    {
        public IList<Triangulo> Triangular(Poligono Perimetro, IList<Linea> LineasRuptura, IList<Punto3D> Puntos)
        {
            IList<Triangulo> ResTriang = new List<Triangulo>();
            //TODO: https://es.wikipedia.org/wiki/Triangulaci%C3%B3n_de_Delaunay

            //Condición de Delaunay
            //Analisis.AnalisisGeometrico.PuntoCircunscrito(Triangulo,Ptest) 

            //el espacio se divide en el número de procesadores de la máquina y se lanzan sucesivos thread para resolver cada una de las zonas
            //Según terminan se van uniendo y resolviendo cada una de las divisiones en función de cuando terminen

            int currentManagedThread = Environment.CurrentManagedThreadId;
            int processorCount = Environment.ProcessorCount;



            return ResTriang;
        }
    }
}
