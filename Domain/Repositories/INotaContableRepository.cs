using Domain.Aplicacion;
using Domain.Base;
using Domain.Contracts;
using Domain.Documentos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface INotaContableRepository : IGenericRepository<NotaContable>
    {
        IEnumerable<ConsultaNotasContablesDTO> GetNotasContablesParametrizadas(GetNotasContablesParametrizadaRequest request);
    }
    public class GetNotasContablesParametrizadaRequest
    {
        public TipoFiltroFecha? TipoFiltroFecha { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechasHasta { get; set; }
        public TipoFiltroUsuario? TipoFiltroUsuario { get; set; }
        public Guid? IdUsuarioFiltro { get; set; }
        public bool FiltroArea { get; set; }
        public Guid? AreaId { get; set; }
        public bool FiltroProcesoReceptor { get; set; }
        public Guid? ProcesoReceptorId { get; set; }
        public bool FiltroTipoDocumento { get; set; }
        public Guid? TipoDocumentoId { get; set; }
        public bool FiltroClasificacionDocumento { get; set; }
        public Guid? ClasificacionDocumentoId { get; set; }
        public bool FiltroBatch { get; set; }
        public string? Batch { get; set; }
        public EstadoDocumento? EstadoDocumento { get; set; }
        public bool FiltroComentario { get; set; }
        public string Comentario { get; set; }

    }
    public enum TipoFiltroFecha
    {
        Bot, Creacion, Aprobacion, Autorizacion, Verificacion, Anulacion, Batch
    }
    public enum TipoFiltroUsuario
    {
        Anulador,Verificador, Aprobador, Autorizador, Creador, All
    }
    public class ConsultaNotasContablesDTO
    {
        public string? NombreUsuarioCreador { get; set; }
        public string? NombreUsuarioVerificador { get; set; }
        public string? NombreUsuarioAprobador { get; set; }
        public string? NombreUsuarioAutorizador { get; set; }
        public string? NombreUsuarioAnulador { get; set; }
        public string? NombreProcesoUsuarioCreador { get; set; }
        public string? NombreEquipoUsuarioCreador { get; set; }
        public string? NombreAreaUsuarioCreador { get; set; }
        public string? NombreProcesoNota { get; set; }
        public EstadoDocumento EstadoDocumento { get; set; }
        public string? Comentario { get; set; }
        public string? Batch { get; set; }
        public string? DescripcionClasificacion {get;set;}
        public string? CodigoTipoDocumento { get; set; }
        public string? DescripcionTipoDocumento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaBot { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public DateTime? FechaAnulacion { get; set; }
        public DateTime? FechaBatch { get; set; }

    }
}
