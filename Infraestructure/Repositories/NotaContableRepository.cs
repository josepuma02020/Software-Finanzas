using Domain.Base;
using Domain.Documentos;
using Domain.Entities;
using Domain.Repositories;
using Infraestructure.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class NotaContableRepository : GenericRepository<NotaContable>, INotaContableRepository
    {
        public NotaContableRepository(IDbContext context) : base(context)
        {
        }
        public IEnumerable<ConsultaNotasContablesDTO> GetNotasContablesParametrizadas(GetNotasContablesParametrizadaRequest request)
        {
            var query = Dbset.AsQueryable();

            if (request.FiltroArea) query = query.Where(t => t.UsuarioCreador.Proceso.AreaId == request.AreaId);

            if (request.FiltroProcesoReceptor) query = query.Where(t => t.ProcesoId == request.ProcesoReceptorId);

            if (request.FiltroTipoDocumento) query = query.Where(t => t.TipoDocumentoId== request.TipoDocumentoId);

            if (request.FiltroClasificacionDocumento) query = query.Where(t => t.ClasificacionDocumentoId == request.ClasificacionDocumentoId);

            if (request.FiltroBatch) query = query.Where(t => t.Batch.Contains(request.Batch));

            if (request.FiltroComentario) query = query.Where(t => t.Comentario.Contains(request.Comentario));

            switch (request.EstadoDocumento)
            {
                case EstadoDocumento.Revision:
                    query = query.Where(t => t.EstadoDocumento == EstadoDocumento.Revision);
                    break;
                case EstadoDocumento.Anulado:
                    query = query.Where(t => t.EstadoDocumento == EstadoDocumento.Anulado);
                    break;
                case EstadoDocumento.Cerrado:
                    query = query.Where(t => t.EstadoDocumento == EstadoDocumento.Cerrado);
                    break;
                case EstadoDocumento.Aprobado:
                    query = query.Where(t => t.EstadoDocumento == EstadoDocumento.Aprobado);
                    break;
                case EstadoDocumento.Autorizado:
                    query = query.Where(t => t.EstadoDocumento == EstadoDocumento.Autorizado);
                    break;
                case EstadoDocumento.Abierto:
                    query = query.Where(t => t.EstadoDocumento == EstadoDocumento.Abierto);
                    break;
                case EstadoDocumento.Verificado:
                    query = query.Where(t => t.EstadoDocumento == EstadoDocumento.Verificado);
                    break;
            }

            switch (request.TipoFiltroUsuario)
            {
                case TipoFiltroUsuario.All:
                    query = query.Where(t => t.AnuladorId == request.IdUsuarioFiltro || t.AprobadorId == request.IdUsuarioFiltro ||
                    t.VerificadorId == request.IdUsuarioFiltro || t.AutorizadorId == request.IdUsuarioFiltro || t.UsuarioCreadorId == request.IdUsuarioFiltro);
                    break;
                case TipoFiltroUsuario.Creador:
                    query = query.Where(t => t.UsuarioCreadorId == request.IdUsuarioFiltro);
                    break;
                case TipoFiltroUsuario.Aprobador:
                    query = query.Where(t => t.AprobadorId == request.IdUsuarioFiltro);
                    break;
                case TipoFiltroUsuario.Autorizador:
                    query = query.Where(t => t.AutorizadorId == request.IdUsuarioFiltro);
                    break;
                case TipoFiltroUsuario.Verificador:
                    query = query.Where(t => t.VerificadorId == request.IdUsuarioFiltro);
                    break;
                default:
                    break;
            }

            switch (request.TipoFiltroFecha)
            {
                case TipoFiltroFecha.Bot:
                    query = query.Where(t => t.Fechabot >= request.FechaDesde && t.Fechabot <= request.FechasHasta);
                    break;
                case TipoFiltroFecha.Aprobacion:
                    query = query.Where(t => t.FechaAprobacion >= request.FechaDesde && t.FechaAprobacion <= request.FechasHasta);
                    break;
                case TipoFiltroFecha.Creacion:
                    query = query.Where(t => t.FechaDeCreacion >= request.FechaDesde && t.FechaDeCreacion <= request.FechasHasta);
                    break;
                case TipoFiltroFecha.Anulacion:
                    query = query.Where(t => t.FechaAnulacion >= request.FechaDesde && t.FechaAnulacion <= request.FechasHasta);
                    break;
                case TipoFiltroFecha.Autorizacion:
                    query = query.Where(t => t.FechaAutorizacion >= request.FechaDesde && t.FechaAutorizacion <= request.FechasHasta);
                    break;
                case TipoFiltroFecha.Batch:
                    query = query.Where(t => t.Fechabatch >= request.FechaDesde && t.Fechabatch <= request.FechasHasta);
                    break;
                case TipoFiltroFecha.Verificacion:
                    query = query.Where(t => t.FechaVerificacion >= request.FechaDesde && t.FechaVerificacion <= request.FechasHasta);
                    break;
                default:
                    break;
            }

            return query.Include(t => t.Proceso).Include(t => t.UsuarioCreador).ThenInclude(t => t.Proceso).Include(t => t.ClasificacionDocumento)
                .Include(t => t.TipoDocumento).Select(t => new ConsultaNotasContablesDTO()
                {
                    EstadoDocumento = t.EstadoDocumento,
                    NombreAreaUsuarioCreador = t.UsuarioCreador.Proceso.Area.NombreArea,
                    Batch = t.Batch,
                    NombreProcesoNota = t.Proceso.NombreProceso,
                    CodigoTipoDocumento = t.TipoDocumento.CodigoTipoDocumento,
                    DescripcionTipoDocumento = t.TipoDocumento.DescripcionTipoDocumento,
                    Comentario = t.Comentario,
                    DescripcionClasificacion = t.ClasificacionDocumento.NombreClasificacion,
                    FechaAnulacion = t.FechaAnulacion,
                    FechaAprobacion = t.FechaAprobacion,
                    FechaAutorizacion = t.FechaAutorizacion,
                    FechaBatch = t.Fechabatch,
                    FechaBot = t.Fechabot,
                    FechaCreacion = t.FechaDeCreacion,
                    FechaVerificacion = t.FechaVerificacion,
                    NombreEquipoUsuarioCreador = t.UsuarioCreador.Proceso.Equipo.NombreEquipo,
                    NombreProcesoUsuarioCreador = t.UsuarioCreador.Proceso.NombreProceso,
                    NombreUsuarioAnulador = t.Anulador.Nombre,
                    NombreUsuarioAprobador = t.Aprobador.Nombre,
                    NombreUsuarioAutorizador = t.Autorizador.Nombre,
                    NombreUsuarioCreador = t.UsuarioCreador.Nombre,
                    NombreUsuarioVerificador = t.Verificador.Nombre??String.Empty,               
                });
        }
            
    }
}
