using Domain.Base;
using Domain.Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        IEnumerable<ConsultaUsuarioDto> GetNotasContablesParametrizadas(GetUsuariosParametrizadaRequest request);
    }
    public class GetUsuariosParametrizadaRequest
    {
        public FiltroRol FiltroRol { get; set; } = FiltroRol.All;
        public bool FiltroNombre { get; set; }
        public bool FiltroIdentificacion { get; set; }

        public string? Identificacion { get; set; }
        public string? Nombre { get; set; }
        public bool FiltroEmail { get; set; }
        public string? Email { get; set; }
        public bool FiltroProceso {get;set;}
        public Guid ProcesoId { get; set; }
        public bool FiltroEquipo { get; set; }
        public Guid EquipoId { get; set; }
        public bool FiltroArea { get; set; }
        public Guid AreaId { get; set; }
            

    }
    public enum FiltroRol
    {
        [Description("Normal")] Normal,
        [Description("Verificador de notascontables")] Verificadordenotascontables,
        [Description("Aprobador de notascontables")] Aprobadordenotascontables,
        [Description("Autorizador de notascontables")] Autorizadordenotascontables,
        [Description("Verificador de facturas")] VerificadorFacturas,
        [Description("Administrador")] Administrador,
        [Description("Administrador de notas contables")] AdministradorNotaContable,
        [Description("Administrador de facturas")] AdministradorFactura,
        [Description("Robot Uipath")] Bot,
        [Description("Robot Uipath")] All,
    }
    public class ConsultaUsuarioDto
    {
        public string NombreUsuario { get; set; }
        public string NombreProceso { get; set; }
        public string NombreEquipo { get; set; }
        public string NombreArea { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public string Identificacion { get; set; }
        public string NombreUsuarioAsignoProceso{get;set;}

    }

}

