using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry.Log
{
    public class Log
    {
        public IList<ProcessLog> LogProcesos = new List<ProcessLog>();
    }

    public enum TypeProceso
    {
        Multiproceso,
        Triangulacion,
        Merge
    }
    public class ProcessLog
    {
        private string _idLog = "";
        public string IDLog
        {
            get
            {
                return _idLog;
            }
        }
        private TypeProceso _tipoProceso = TypeProceso.Triangulacion;
        public TypeProceso TipoProceso
        {
            get
            {
                return _tipoProceso;
            }
        }
        private IList<EventoLog> _eventos = new List<EventoLog>();
        public IList<EventoLog> Eventos
        {
            get
            {
                return _eventos;
            }
        }

        public ProcessLog(string nombreProceso, TypeProceso tipoProceso)
        {
            lock (this)
            {
                _idLog = nombreProceso + " ID: {0}";
                _tipoProceso = tipoProceso;
                _eventos = new List<EventoLog>();
            }
        }

        public void ActualizaProcessLog(System.Threading.Thread proceso)
        {
            lock (this)
            {
                try
                {
                    _idLog = string.Format(_idLog, proceso.ManagedThreadId.ToString());
                }
                catch (Exception) { }
            }
        }

        public void Add(EventoLog eventoLog)
        {
            lock (_eventos)
            {
                _eventos.Add(eventoLog);
            }
        }
    }

    public enum TypeEvento
    {
        Inicio,
        Fin,
        Informacion,
        Advertencia,
        Error
    }
    public class EventoLog
    {
        private TypeEvento _tipoEvento = TypeEvento.Inicio;
        public TypeEvento TipoEvento
        {
            get
            {
                return _tipoEvento;
            }
            private set
            {
                _tipoEvento = value;
            }
        }
        DateTime _timeStamp = DateTime.Now;
        public DateTime TimeStamp
        {
            get
            {
                return _timeStamp;
            }
            private set
            {
                _timeStamp = value;
            }
        }
        string _descripcion = "";
        public string Descripcion
        {
            get
            {
                return _descripcion;
            }
            set
            {
                _descripcion = value;
            }
        }

        public EventoLog(TypeEvento tipoEvento, string descripcion)
        {
            _tipoEvento = tipoEvento;
            _timeStamp = DateTime.Now;
            _descripcion = descripcion;
        }
    }
}
