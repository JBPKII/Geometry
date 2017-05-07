using System;
using System.Collections.Generic;

using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulacion.Trianguladores.Delaunay
{
    public class MergeDelaunay
    {
        private IList<Triangulo> _triangulacion1;
        private IList<Triangulo> _triangulacion2;

        public MergeDelaunay(IList<Triangulo> triangulacion1, IList<Triangulo> triangulacion2)
        {
            _triangulacion1 = triangulacion1;
            _triangulacion2 = triangulacion2;
        }

        public IList<Triangulo> DoMerge()
        {
            //TODO: Desarrollar Merge Delaunay
            IList<Triangulo> ResTriang = new List<Triangulo>();


            return ResTriang;
        }
    }
}
