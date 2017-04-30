using System;

namespace Geometry.Geometrias
{
    [Serializable]
    ///<Summary>
    /// Linea definida por punto inicial y final
    ///</Summary>
    public class Linea
    {
        ///<Summary>
        /// Punto de inicio
        ///</Summary>
        public Punto3D Inicio;

        ///<Summary>
        /// Punto de fin
        ///</Summary>
        public Punto3D Fin;

        ///<Summary>
        /// Inicializa una línea de longitud 0.0
        ///</Summary>
        public Linea()
        {
            Inicio = new Punto3D();
            Fin = new Punto3D();
        }

        ///<Summary>
        /// Inicializa una línea con los valores del punto inicial y el final
        ///</Summary>
        public Linea(Punto3D PuntoInicio, Punto3D PuntoFin)
        {
            Inicio = PuntoInicio;
            Fin = PuntoFin;
        }
    }
}
