using Domain.Base;
using Domain.Contracts;
using Domain.Documentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IFacturaRepository : IGenericRepository<Factura>
    {
        IEnumerable<ConsultaFacturasDto> GetFacturasParametrizado(GetFacturasParametrizadaRequest request);
    }
    public class GetFacturasParametrizadaRequest
    {
        public bool FiltroConcepto { get; set; }
        public Guid? ConceptoId { get; set; }
        public bool FiltroTerceroId { get; set; }
        public string? CodigoTercero { get; set; }
        public bool FiltroTerceroNombre { get; set; }
        public string? TerceroNombre { get; set; }
        public FiltroUsuarioFactura FiltroUsuarioFactura { get; set; }
        public string? NombreUsuario { get; set; }
        public bool FiltroCuenta { get; set; }
        public Guid? CuentaId { get; set; }
        public bool FiltroValor { get; set; }
        public int Valor { get; set; }
        public FiltroFechaFactura FiltroFechaFactura { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public bool FiltroRI { get; set; }
        public string? RI { get; set; }
        public bool FiltroObservacion { get; set; }
        public string? Observaciones { get; set; }
        public bool FiltroArea { get; set; }
        public string? AreaId { get; set; }
        public EstadoDocumento Estado {get;set;}
    }
    public enum FiltroUsuarioFactura
    {
        Creador,Verficador
    }
    public enum FiltroFechaFactura
    {
        Creacion,Verificacion,Cierre,Anulacion, Fechapago
    }
    public class ConsultaFacturasDto
    {
        public string NombreCreador { get;set; }
        public string? NombreRevisor{ get; set; }
        public string NombreArea { get; set; }
        public string NombreEquipo { get; set; }
        public string NombreProceso { get; set; }
        public string CorreoCreador { get; set; }
        public string DescripcionConceptoPago { get; set; }
        public string CondigoTercero { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaRevision { get; set; }
        public long Valor { get; set; }
        public string DescripcionCuentaBancaria { get; set; }
        public string NombreEntidadCuentaBancaria { get; set; }
        public string? RI { get; set; }
        public string Observaciones { get; set; }


    }
}
