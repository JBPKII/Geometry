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

        private IList<Triangulo> _Triangulacion1;
        private IList<Triangulo> _Triangulacion2;

        private IList<Triangulo> _ResTriangulacion;

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

        public ProcesoMerge (IList<Triangulo> Triang1, IList<Triangulo> Triang2)
        {
            _Triangulacion1 = Triang1;
            _Triangulacion2 = Triang2;

            _Estado = TriangulacionMultiProceso.Estado.EnEspera;
        }

        public void Ejecutar()
        {
            //Ejecuta el merge de las dos triangulaciones
            try
            {
                //TODO: Ejecutar Merge

                //TODO: Lanzar evento

                _Estado = TriangulacionMultiProceso.Estado.Terminado;

            }
            catch (Exception sysEx)
            {
                _Error = sysEx;
                _Estado = TriangulacionMultiProceso.Estado.ConErrores;
            }
            finally
            {
                FinProcesoEvent?.Invoke(this, new EventArgs());
            }
        }

        public IList<Triangulo> Resultado()
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
