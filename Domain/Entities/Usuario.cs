﻿using Domain.Aplicacion;
using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasContables;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Base;
using Domain.Documentos.ConfiguracionDocumentos;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Usuario :Entity<Guid>
    {
        public string? Identificacion { get; set; }
        public string Nombre { get; set; }
        public string? Email { get; set; }
        public DateTime? Ultingreso { get; set; }
        public Proceso ? Proceso { get; set; }
        public Equipo Equipo { get; set; }
        public Area Area { get; set; }
        public Guid AreaId { get; set; }
        public Guid EquipoId { get; set; }
        public Guid? ProcesoId { get; set; }
        public Rol  Rol { get; set; }

        //Configuraciones
        #region Configuracion de relaciones
        #region ConfiguracionGeneral
        public IEnumerable<Configuracion> ConfiguracionesCreadas { get; set; }
        public IEnumerable<Cuenta> CuentasEditadas { get; set; }
        #endregion
        #region Areas
        public IEnumerable<Cuenta> CuentasCreadas { get; set; }
        public IEnumerable<Area> AreasCreadas { get; set; }
        public IEnumerable<Area> AreasEditadas { get; set; }
        public IEnumerable<Equipo> EquiposCreados { get; set; }
        public IEnumerable<Equipo> EquipoEditados { get; set; }
        public IEnumerable<Proceso> ProcesosCreados { get; set; }
        public IEnumerable<Proceso> ProcesosEditados { get; set; }
        #endregion
        #region Documentos
        public IEnumerable<TipoDocumento> TiposdeDocumentosCreados { get; set; }
        public IEnumerable<Entidad> EntidadesCreadas { get; set; }
        public IEnumerable<ConfiguracionServicios>? ConfiguracionesServicios { get; set; }
        public IEnumerable<ConfiguracionProcesoNotasContables> ConfiguracionesNotasContables { get; set; }
        public IEnumerable<ClasificacionDocumento> ClasificacionesdeDocumentoCreadas { get; set; }
        public IEnumerable<ClasificacionDocumento> ClasificacionesdeDocumentoEditadas { get; set; }
        public IEnumerable<AppFile> ArchivosCargados { get; set; }
        public IEnumerable<BaseEntityDocumento> DocumentosCreados { get; set; }
        public IEnumerable<BaseEntityDocumento> DocumentosAnulados { get; set; }
        public IEnumerable<BaseEntityDocumento> DocumentosAprobados { get; set; }
        public IEnumerable<BaseEntityDocumento> DocumentosAutorizados { get; set; }
        public IEnumerable<BaseEntityDocumento> DocumentosCerrados { get; set; }
        public IEnumerable<BaseEntityDocumento> DocumentosRechazados { get; set; }
        public IEnumerable<BaseEntityDocumento> DocumentosBot { get; set; }
        public IEnumerable<BaseEntityDocumento> DocumentosRevertidos { get; set; }
        public IEnumerable<BaseEntityDocumento> DocumentosEnviadosaRevision { get; set; }
        public IEnumerable<BaseEntityDocumento> DocumentosEditados { get; set; }
        #endregion
        #region Usuarios
        public IEnumerable<Usuario> UsuariosqueAgregoProceso { get; set; }
        #endregion
        #endregion
        public Usuario() : base(null)
        {
            Rol = Rol.Normal;
        }
        public Usuario(Usuario? usuariocreador):base(usuariocreador)
        {
            Rol = Rol.Normal;
        }
        public Usuario SetRole(Rol role)
        {
            this.Rol = role;
            return this;
        }
        public Usuario SetProceso(Proceso proceso,Usuario usuarioAsignaProceso,Guid UsuarioAsignaProcesoId)
        {
            this.Proceso = proceso;
            this.UsuarioEditor = usuarioAsignaProceso; this.UsuarioEditorId = UsuarioAsignaProcesoId;
            return this;
        }
    }
}
