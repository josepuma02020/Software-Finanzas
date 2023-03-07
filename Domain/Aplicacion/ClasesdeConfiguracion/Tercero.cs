using Domain.Aplicacion.Entidades;
using Domain.Base;
using Domain.Documentos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aplicacion.EntidadesConfiguracion
{
    public class Tercero : Entity<Guid>
    {
        public string Nombre { get; set; }
        public string? ObservacionAdicional { get; set; }
        public string Codigotercero { get; set; }
        public Estado Estado { get; set; }

        #region Configuracion
        public IEnumerable<Factura> Facturas { get; set; }
        public IEnumerable<Registrodenotacontable> TercerosLM { get; set; }
        public IEnumerable<Registrodenotacontable> TercerosAN8 { get; set; }
        #endregion
        private Tercero() : base(null)
        {
            Estado = Estado.Activo;
        }
        public Tercero(Usuario? usuariocreador):base(usuariocreador)
        {
            Estado = Estado.Activo;
        }
        public Tercero EditarTercero(string observacion,Estado estado,Usuario Usuarioeditor)
        {
            this.UsuarioEditor = UsuarioEditor;
            this.ObservacionAdicional = observacion;
            this.UsuarioEditorId = Usuarioeditor.Id;
            return this;
        }

    }
}
