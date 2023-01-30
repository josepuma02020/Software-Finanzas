using Domain.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aplicacion
{
    public class Configuracion : Entity<Guid>
    {
        public int Salariominimo { get; set; }
        public DateTime? Fechafinal { get; set; }
        public Usuario UsuarioqueConfiguro { get; set; }
        public Configuracion(int nuevosalariominimo, Usuario usuarioqueConfiguro)
        {
            Salariominimo = nuevosalariominimo;
            UsuarioqueConfiguro = usuarioqueConfiguro;
        }
        public Configuracion SetFechaCierre(DateTime fechacierre)
        {
            Fechafinal = fechacierre;
            return this;
        }
    }
    public class ConfiguracionProceso : Entity<Guid>
    {
        public DateTime? Fechacierre { get; set; }
        public Configuracion Configuraciongeneral { get; set; }
        public ProcesosDocumentos Procesoconfiguracion { get; set; }
        public Usuario UsuarioConfiguro { get; set; }
        public ConfiguracionProceso(Configuracion configuraciongeneral, ProcesosDocumentos procesoconfiguracion,Usuario usuarioconfiguro)
        {
            Configuraciongeneral = configuraciongeneral;
            Procesoconfiguracion = procesoconfiguracion;
            UsuarioConfiguro = usuarioconfiguro;
        }
        public ConfiguracionProceso SetFechaCierre(DateTime fechacierre)
        {
            Fechacierre = fechacierre;
            return this;
        }
    }

}
