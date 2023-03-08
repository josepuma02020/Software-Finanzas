using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aplicacion
{
    public class Configuracion : Entity<Guid>
    {
        public long Salariominimo { get; set; }
        public int MultiploRevisarNotaContable { get; set; }
        public int Año {get;set;}
        public Configuracion() : base(null)
        {

        }
        public Configuracion(Usuario? usuariocreador) : base(usuariocreador) { }
        public Configuracion(long nuevosalariominimo, Usuario? usuarioqueConfiguro):base(usuarioqueConfiguro)
        {
            Salariominimo = nuevosalariominimo;
        }
    }
    public class ConfiguracionProcesoNotasContables : Entity<Guid>
    {
        public DateTime? FechaCierre { get; set; }
        public int Mes { get; set; }
        public int Año { get; set; }
        public Proceso ProcesoNotaContable { get; set; }
        public Guid ProcesoId { get; set; }
        public Guid? IdUsuarioConfiguro { get; set; }
        public Usuario? UsuarioConfiguro { get; set; }
        private ConfiguracionProcesoNotasContables() : base(null) { }
        public ConfiguracionProcesoNotasContables(int mes,int año,Usuario usuariocreador):base(usuariocreador)
        {
            Mes = mes;Año =año;
        }
        public ConfiguracionProcesoNotasContables(int mes, int año, Usuario usuariocreador,Proceso proceso) : base(usuariocreador)
        {
            ProcesoNotaContable = proceso;ProcesoId = proceso.Id;
            Mes = mes; Año = año;
        }

    }
    public class ConfiguracionServicios : Entity<Guid>
    {
        public string Servicio { get; set; }
        public bool Activo { get; set; }
        public ConfiguracionServicios() : base(null)
        {
            Id = Guid.NewGuid();
        }
        public ConfiguracionServicios SetDisponibilidad(bool estado,Usuario editor)
        {
            this.UsuarioEditor = editor;
            this.FechaEdicion = DateTime.Now;
            this.Activo = estado;
            return this;
        }

    }
    public static class NombreServicios 
    {
        public const string ServicioFactura = "Facturas";
        public const string ServicioNotaContable = "Notas Contables";
        public const string ServicioFlujodeCaja = "Flujo de Caja";

    }

}
