using System;
using System.Collections.Generic;
using System.Text;
using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    class ProcesoMerge : IProceso
    {
        private TriangulacionMultiProceso.Estado _Estado = TriangulacionMultiProceso.Estado.Vacio;
        private System.Exception _Error = new Exception("NoError");

        private IList<Triangulo> _Triangulacion1 = new List<Triangulo>();
        private IList<Triangulo> _Triangulacion2 = new List<Triangulo>();

        private Triangulaciones.Delaunay.ResultadoDelaunay _ResTriangulacion = new Triangulaciones.Delaunay.ResultadoDelaunay();

        public TriangulacionMultiProceso.Estado Estado
        {
            get
            {
                return _Estado;
            }
        }
        public System.Exception Error
        {
            get
            {
                return _Error;
            }
        }

        public ProcesoMerge (Triangulaciones.Delaunay.ResultadoDelaunay Triang1, Triangulaciones.Delaunay.ResultadoDelaunay Triang2)
        { 
            _Triangulacion1 = Triang1.Resultado;
            _Triangulacion2 = Triang2.Resultado;

            if (Triang1.Seccion.MallaAnteriorSiguiente.MallaAnterior == Triang2.Seccion.MallaAnteriorSiguiente.MallaSiguiente)
            {
                // 1,2 - 0,1
                _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaAnterior = 
                    Triang1.Seccion.MallaAnteriorSiguiente.MallaSiguiente;
                _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaSiguiente =
                    Triang2.Seccion.MallaAnteriorSiguiente.MallaAnterior;
            }
            else
            {
                if (Triang1.Seccion.MallaAnteriorSiguiente.MallaSiguiente == Triang2.Seccion.MallaAnteriorSiguiente.MallaAnterior)
                {
                    // 0,1 - 1,2
                    _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaAnterior =
                        Triang1.Seccion.MallaAnteriorSiguiente.MallaAnterior;
                    _ResTriangulacion.Seccion.MallaAnteriorSiguiente.MallaSiguiente =
                        Triang2.Seccion.MallaAnteriorSiguiente.MallaSiguiente;
                }
                else
                {
                    //No son consecutivas
                }
            }

            _Estado = TriangulacionMultiProceso.Estado.EnEspera;
        }

        public void Ejecutar()
        {
            //Ejecuta el merge de las dos triangulaciones
            try
            {
                //TODO: Ejecutar Merge

                

                _Estado = TriangulacionMultiProceso.Estado.Terminado;

            }
            catch (Exception sysEx)
            {
                _Error = sysEx;
                _Estado = TriangulacionMultiProceso.Estado.ConErrores;
            }
            finally
            {
                //Lanza evento
                FinProcesoEvent?.Invoke(this, new EventArgs());
            }
        }

        public Triangulaciones.Delaunay.ResultadoDelaunay Resultado()
        {
            return _ResTriangulacion;
        }

        public EventHandler FinProcesoEvent;
        public event EventHandler OnFinProceso
        {
            add
            {
                lock (this)
                {
                    FinProcesoEvent += value;
                }
            }
            remove
            {
                lock (this)
                {
                    FinProcesoEvent -= value;
                }
            }
        }
    }
}
