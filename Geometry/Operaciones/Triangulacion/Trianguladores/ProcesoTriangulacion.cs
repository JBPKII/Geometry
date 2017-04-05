using System;
using System.Collections.Generic;
using System.Text;
using Geometry.Geometrias;

namespace Geometry.Operaciones.Triangulaciones.Trianguladores
{
    class ProcesoTriangulacion : IProceso
    {
        private TriangulacionMultiProceso.Estado _Estado = TriangulacionMultiProceso.Estado.Vacio;
        private System.Exception _Error = new Exception("NoError");

        private IList<Punto3D> _Vertices;
        private IList<Linea> _Rupturas;
        private Triangulo _Envolvente;

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

        public ProcesoTriangulacion(IList<Punto3D> Vertices, IList<Linea> Rupturas, Triangulo Envolvente)
        {
            _Vertices = Vertices;
            _Rupturas = Rupturas;
            _Envolvente = Envolvente;

            _Estado = TriangulacionMultiProceso.Estado.EnEspera;
        }

        public void Ejecutar()
        {
            //Ejecuta procesa _Vértices y _Rupturas para obtener _ResTriangulación
            try
            {
                //TODO: Ejecutar Triangulación

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
