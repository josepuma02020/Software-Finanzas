
using Domain.Entities;
using Domain.Extensions;
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
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(IDbContext context) : base(context)
        {
        }
        public IEnumerable<ConsultaUsuarioDto> GetUsuarioParametrizados(GetUsuariosParametrizadaRequest request)
        {

            var query = Dbset.AsQueryable();
            switch (request.FiltroRol)
            {
                case FiltroRol.AdministradorFactura:query = query.Where(t => t.Rol == Rol.AdministradorFactura);break;
                case FiltroRol.VerificadorFacturas: query = query.Where(t => t.Rol == Rol.VerificadorFacturas);break;
                case FiltroRol.Administrador: query = query.Where(t => t.Rol == Rol.Administrador); break;
                case FiltroRol.Bot: query = query.Where(t => t.Rol == Rol.Bot); break;
                case FiltroRol.AdministradorNotaContable: query = query.Where(t => t.Rol == Rol.AdministradorNotaContable); break;
                case FiltroRol.Aprobadordenotascontables: query = query.Where(t => t.Rol == Rol.Aprobadordenotascontables); break;
                case FiltroRol.Autorizadordenotascontables: query = query.Where(t => t.Rol == Rol.Autorizadordenotascontables); break;
                case FiltroRol.Normal: query = query.Where(t => t.Rol == Rol.Normal); break;
            }
            if(request.FiltroNombre) query = query.Where(t => t.Nombre.Contains(request.Nombre));
            if (request.FiltroEmail) query = query.Where(t => t.Email.Contains(request.Email));
            if (request.FiltroIdentificacion) query = query.Where(t => t.Identificacion.Contains(request.Identificacion));
            if (request.FiltroProceso) query = query.Where(t => t.ProcesoId == request.ProcesoId);
            if (request.FiltroEquipo) query = query.Where(t => t.EquipoId == request.EquipoId);
            if (request.FiltroArea) query = query.Where(t => t.AreaId == request.AreaId);

            return query.Include(t => t.Proceso).Include(t => t.Area).Include(t => t.Equipo).Include(t => t.Rol).Include(t=>t.UsuarioAsignaProceso)
                .Select(t => new ConsultaUsuarioDto()
                {
                     Email=t.Email,
                     Identificacion=t.Identificacion,
                     NombreArea=t.Area.NombreArea,
                     NombreEquipo=t.Equipo.NombreEquipo,
                     NombreProceso=t.Proceso.NombreProceso,
                     NombreUsuario=t.Nombre,
                     NombreUsuarioAsignoProceso=t.UsuarioAsignaProceso.Nombre,
                     Rol=t.Rol.GetDescription(),
                });
        }


    }

}
