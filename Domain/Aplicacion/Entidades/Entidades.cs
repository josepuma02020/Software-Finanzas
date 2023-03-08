using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Base;
using Domain.Entities;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aplicacion.Entidades
{
    public class Entidad : Entity<Guid>
    {
        public string NombreEntidad { get; set; }
        public string? Observaciones { get; set; }
        public Estado Estado { get; set; }
        public List<BaseCuenta> Cuentas { get; set; }
        #region Metodos
        public Entidad() : base(null)
        {
            Estado = Estado.Activo;
        }
        public Entidad(Usuario? usuariocreador) : base(usuariocreador)
        {
            Estado = Estado.Activo;
        }
        public Entidad EditarEntidad(Usuario Usuarioeditor, Estado estado,string observacion,string nombreentidad)
        {
            this.NombreEntidad = nombreentidad;
            this.Observaciones = observacion;
            this.UsuarioEditor = Usuarioeditor;
            this.UsuarioEditorId = Usuarioeditor.Id;
            this.Estado = estado;
            return this;
        }
        #endregion
        #region Configuracion
        public IEnumerable<BaseCuenta> CuentasConfiguracion { get; set; }
        #endregion
    }
    public enum Estado
    {
        Activo,
        Inactivo,
    }

}
