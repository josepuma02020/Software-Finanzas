using Domain.Base;
using Domain.Documentos;
using Domain.Repositories;
using Infraestructure.Base;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    public class FacturaRepository : GenericRepository<Factura>,IFacturaRepository
    {
        public FacturaRepository(IDbContext context) : base(context)
        {
        }
        public IEnumerable<ConsultaFacturasDto>  GetFacturasParametrizado(GetFacturasParametrizadaRequest request) {
            var query = Dbset.AsQueryable();

            if (request.FiltroUsuarioFactura == FiltroUsuarioFactura.Creador) query = query.Where(t => t.UsuarioCreador.Nombre.ToLower().Contains(request.NombreUsuario.ToLower()));

            if (request.FiltroUsuarioFactura == FiltroUsuarioFactura.Verficador) query = query.Where(t => t.Verificador.Nombre.ToLower().Contains(request.NombreUsuario.ToLower()));

            if (request.FiltroConcepto) query = query.Where(t => t.ConceptoId.Equals(request.ConceptoId));

            if (request.FiltroTerceroId) query = query.Where(t => t.Tercero.Codigotercero.Contains(request.CodigoTercero));

            if (request.FiltroCuenta) query = query.Where(t => t.CuentaId == request.CuentaId);

            if (request.FiltroValor) query = query.Where(t => t.Valor.Equals(request.Valor));

            if (request.FiltroRI) query = query.Where(t => t.Ri.Contains(request.RI));

            if (request.FiltroObservacion) query = query.Where(t => t.Observaciones.Contains(request.Observaciones));

            if (request.FiltroArea) query = query.Where(t => t.UsuarioCreador.Proceso.AreaId.Equals(request.AreaId));

            switch (request.Estado)
            {
                case EstadoDocumento.Anulado:
                    query = query.Where(t => t.EstadoDocumento.Equals(EstadoDocumento.Anulado));
                    break;
                case EstadoDocumento.Revision:
                    query = query.Where(t => t.EstadoDocumento.Equals(EstadoDocumento.Revision));
                    break;
                case EstadoDocumento.Verificado:
                    query = query.Where(t => t.EstadoDocumento.Equals(EstadoDocumento.Verificado));
                    break;
                case EstadoDocumento.Cerrado:
                    query = query.Where(t => t.EstadoDocumento.Equals(EstadoDocumento.Cerrado));
                    break;
                default:
                    break;
            }

            switch (request.FiltroFechaFactura)
                {
                    case FiltroFechaFactura.Anulacion:
                        query = query.Where(t => t.FechaAnulacion <= request.Hasta && t.FechaAnulacion >= request.Desde);
                        break;
                    case FiltroFechaFactura.Creacion:
                        query = query.Where(t => t.FechaDeCreacion <= request.Hasta && t.FechaDeCreacion >= request.Desde);
                        break;
                    case FiltroFechaFactura.Cierre:
                        query = query.Where(t => t.Fechabot <= request.Hasta && t.Fechabot >= request.Desde);
                        break;
                    case FiltroFechaFactura.Verificacion:
                        query = query.Where(t => t.FechaVerificacion <= request.Hasta && t.FechaVerificacion >= request.Desde);
                        break;
                    case FiltroFechaFactura.Fechapago:
                      query = query.Where(t => t.Fechapago <= request.Hasta && t.Fechapago >= request.Desde);
                        break;
                    default:
                        break;
                  }
            return query.Include(t => t.UsuarioCreador).ThenInclude(t => t.Proceso).Include(t=>t.Tercero)
                .Include(t=>t.Concepto).Select(t=> new ConsultaFacturasDto()
                {
                    CondigoTercero=t.Tercero.Codigotercero,
                    CorreoCreador=t.UsuarioCreador.Email,
                    DescripcionConceptoPago=t.Concepto.Descripcion,
                    DescripcionCuentaBancaria=t.CuentaBancaria.DescripcionCuenta,
                    NombreEntidadCuentaBancaria=t.CuentaBancaria.Entidad.NombreEntidad,
                    FechaCreacion=t.FechaDeCreacion,
                    FechaPago=t.Fechapago,
                    FechaRevision=t.FechaVerificacion,
                    NombreArea=t.UsuarioCreador.Proceso.Area.NombreArea,
                    NombreCreador=t.UsuarioCreador.Nombre,
                    NombreEquipo=t.UsuarioCreador.Proceso.Equipo.NombreEquipo,
                    NombreProceso=t.UsuarioCreador.Proceso.NombreProceso,
                    NombreRevisor=t.Verificador.Nombre,
                    Observaciones=t.Observaciones,
                    RI=t.Ri,
                    Valor=t.Valor,
                });
        }
    }
}
