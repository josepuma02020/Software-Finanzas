using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Documentos.ConfiguracionDocumentos;
using Domain.Entities;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base
{
    public abstract class BaseEntityDocumento : Entity<Guid>
    {
        public bool EditarSoportes { get; set; }
        public Usuario? Rechazador { get; set; }
        public Usuario? Aprobador { get; set; }
        public Usuario? Autorizador { get; set; }
        public Usuario? Verificador { get; set; }
        public Usuario? Anulador { get; set; }
        public Usuario? ReversorJD { get; set; }
        public Usuario? UsuarioBot { get; set; }
        public Usuario? UsuarioEnviaRevision { get; set; }
        public List<AppFile> Soportes { get; set; }
        public Guid? UsuarioRevisionId { get; set; }
        public Guid? UsuarioBotId { get; set; }
        public Guid? ReversorJDId { get; set; }
        public Guid? RechazadorId { get; set; }
        public Guid? AnuladorId { get; set; }
        public Guid? VerificadorId { get; set; }
        public Guid? AutorizadorId { get; set; }
        public Guid? AprobadorId { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public DateTime? FechaAnulacion { get; set; }
        public DateTime? Fechabot { get; set; }
        public Equipo EquipoCreador { get; set; }
        public Guid EquipoCreadorId { get; set; }
        public ProcesosDocumentos ProcesoDocumento { get; set; }
        public virtual EstadoDocumento EstadoDocumento { get; set; }
        public string DescripcionEstadoDocumento => EstadoDocumento.GetDescription();
        private BaseEntityDocumento() : base(null)
        {
            EstadoDocumento = EstadoDocumento.Abierto;
        }
        public BaseEntityDocumento (Usuario usuariocreador) : base(usuariocreador)
        {
            EstadoDocumento = EstadoDocumento.Abierto;
        }
        public BaseEntityDocumento SetSoportes(List<AppFile> soportes,Usuario usuarioeditor)
        {
            this.UsuarioEditor = usuarioeditor;
            this.FechaEdicion = DateTime.Now;
            this.Soportes = soportes;
            return this;
        }
        public DomainValidation PuedeAgregarSoporte(Usuario usuarioAgregaSoporte)
        {
            var validation = new DomainValidation();

            switch (this.EstadoDocumento)
            {
                case EstadoDocumento.Abierto:
                    if(this.UsuarioCreadorId != usuarioAgregaSoporte.Id) 
                    { 
                        switch (usuarioAgregaSoporte.Rol)
                        {
                            case Rol.Administrador:break;
                            case Rol.AdministradorNotaContable:
                                if (this.ProcesoDocumento != ProcesosDocumentos.NotasContable)
                                    validation.AddFallo("No disponible", "El usuario no tiene permiso para anexar soportes al documento.");break;
                            case Rol.AdministradorFactura:
                                if (this.ProcesoDocumento != ProcesosDocumentos.Facturas)
                                    validation.AddFallo("No disponible", "El usuario no tiene permiso para anexar soportes al documento.");break;
                            default:validation.AddFallo("No disponible", "El usuario no tiene permiso para anexar soportes al documento.");break;
                        }
                    } break;
                case EstadoDocumento.Revision:
                    switch (usuarioAgregaSoporte.Rol)
                    {
                        case Rol.Administrador:break;
                        case Rol.AdministradorNotaContable:
                            if (this.ProcesoDocumento != ProcesosDocumentos.NotasContable)
                                validation.AddFallo("No disponible", "El usuario no tiene permiso para anexar soportes al documento."); break;
                        case Rol.AdministradorFactura:
                            if (this.ProcesoDocumento != ProcesosDocumentos.Facturas)
                                validation.AddFallo("No disponible", "El usuario no tiene permiso para anexar soportes al documento."); break;
                        default:
                            validation.AddFallo("No disponible", "El documento no esta disponible.");break;
                    }
                    break;
                default: validation.AddFallo("No disponible", "El documento no esta disponible."); break;


            }
            return validation;
        }
        public BaseEntityDocumento EnviaraRevision(Usuario usuario)
        {
            this.EstadoDocumento = EstadoDocumento.Revision;
            this.UsuarioEnviaRevision = usuario;
            this.UsuarioRevisionId = usuario.Id;
            return this;
        }
        public BaseEntityDocumento SetAprobador(Usuario aprobador)
        {
            this.EstadoDocumento = EstadoDocumento.Aprobado;
            this.Aprobador = aprobador;
            this.AprobadorId = aprobador.Id;
            this.FechaAprobacion = DateTime.Now;
            return this;
        }
        public BaseEntityDocumento SetAutorizador(Usuario autorizador)
        {
            this.EstadoDocumento = EstadoDocumento.Autorizado;
            this.Autorizador = autorizador;
            this.AutorizadorId=autorizador.Id;
            this.FechaAutorizacion = DateTime.Now;
            return this;
        }
        public BaseEntityDocumento SetVerificador(Usuario verificador)
        {
            this.EstadoDocumento = EstadoDocumento.Verificado;
            this.Verificador = verificador;
            this.VerificadorId = verificador.Id;
            this.FechaVerificacion = DateTime.Now;
            return this;
        }
        public BaseEntityDocumento AnularDocumento(Usuario anulador)
        {
            this.EstadoDocumento = EstadoDocumento.Anulado;
            this.Anulador = anulador;
            this.AnuladorId = anulador.Id;
            this.FechaAnulacion = DateTime.Now;
            return this;
        }
        public BaseEntityDocumento RechazarDocumento(Usuario rechazador)
        {
            this.EstadoDocumento = EstadoDocumento.Abierto;
            this.Autorizador = null;
            this.AutorizadorId = null;
            this.AprobadorId = null;
            this.Aprobador = null;
            this.Rechazador = rechazador;
            this.RechazadorId = rechazador.Id;
            return this;
        }
       
    }
    public enum EstadoDocumento
    {
        [Description("Abierto")] Abierto,
        [Description("Revision")] Revision,
        [Description("Aprobado")] Aprobado,
        [Description("Autorizado")] Autorizado,
        [Description("Verificado")] Verificado,
        [Description("Anulado")] Anulado,
        [Description("Anulado")] RevertidoJD,
        [Description("Cerrado")] Cerrado,

    }

}
