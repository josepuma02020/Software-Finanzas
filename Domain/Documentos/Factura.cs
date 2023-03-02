using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Base;
using Domain.Documentos.ConfiguracionDocumentos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Documentos
{
    public class Factura : BaseEntityDocumento
    {
        public Tercero Tercero { get; set; }
        public long Valor { get; set; }
        public string? Observaciones { get; set; }
        public DateTime Fechapago { get; set; }
        public string? Ri { get; set; }
        public  CuentaBancaria CuentaBancaria { get; set; }
        public  Guid CuentaBancariaId { get; set; }
        public ConceptoFactura Concepto { get; set; }
        public Guid CuentaId { get; set; }
        public Guid ConceptoId { get; set; }
        public Guid TerceroId { get; set; }
        #region Constructores
        private Factura() : base(null)
        {
            ProcesoDocumento = ProcesosDocumentos.Facturas;
        }
        public Factura(Usuario? usuariocreador) : base(usuariocreador)
        {
            ProcesoDocumento = ProcesosDocumentos.Facturas;
        }
        public Factura(Usuario? usuariocreador, CuentaBancaria cuentabancaria) : base(usuariocreador)
        {
            ProcesoDocumento = ProcesosDocumentos.Facturas; CuentaBancaria = cuentabancaria; CuentaBancariaId = cuentabancaria.Id;
        }
        #endregion
        #region Metodos
        public Factura EditarFactura (Usuario editor,Tercero tercero,string? observaciones,DateTime fechapago,string? ri,CuentaBancaria cuentabancaria,ConceptoFactura conceptofactura)
        {
            this.UsuarioEditor = editor;this.UsuarioEditorId = editor.Id;this.FechaEdicion = DateTime.Now;
            this.Tercero = tercero;this.TerceroId = tercero.Id;
            this.Observaciones = observaciones;
            this.Fechapago = fechapago;
            this.Ri = ri;
            this.CuentaBancaria = cuentabancaria;this.CuentaBancariaId = CuentaBancariaId;
            this.Concepto = conceptofactura;this.ConceptoId = conceptofactura.Id;
            return this;
        }
        public DomainValidation PuedeEditar(Usuario usuarioeditor)
        {

            var validation = new DomainValidation();

            switch (EstadoDocumento)
            {
                case EstadoDocumento.Aprobado:
                case EstadoDocumento.Autorizado:
                case EstadoDocumento.Anulado:
                case EstadoDocumento.Cerrado:
                case EstadoDocumento.RevertidoJD:
                    validation.AddFallo("No disponible", "La factura no esta disponible para ediciones."); break;

                case EstadoDocumento.Abierto:
                    if (usuarioeditor.Rol != Rol.AdministradorFactura || usuarioeditor.Rol != Rol.Administrador)
                    {
                        if (usuarioeditor.Id != this.UsuarioCreadorId) { validation.AddFallo("No disponible", "EL usuario editor no es el creador de la factura."); }
                    }
                    break;
                case EstadoDocumento.Revision:
                case EstadoDocumento.Verificado:
                    if (usuarioeditor.Rol != Rol.AdministradorFactura || usuarioeditor.Rol != Rol.Administrador)
                    {
                        validation.AddFallo("No disponible", "El usuario no tiene permiso para editar facturas.");
                    }
                    break;
            }

            return validation;
        }
        public Factura CerrarFactura(Usuario Usuario)
        {
            Fechabot = DateTime.Now;
            EstadoDocumento = EstadoDocumento.Cerrado;
            UsuarioBot = Usuario;
            return this;
        }
        public DomainValidation PuedeAnular(Usuario usuarioqueAnula)
        {

            var validation = new DomainValidation();

            switch (EstadoDocumento)
            {
                case EstadoDocumento.Revision:
                case EstadoDocumento.Verificado:
                case EstadoDocumento.Abierto:
                    switch (usuarioqueAnula.Rol)
                    {
                        case Rol.Administrador:
                        case Rol.AdministradorFactura:
                            break;
                        default:
                            validation.AddFallo("No disponible", "El usuario no tiene permiso para anular facturas.");
                            break;
                    }
                    break;
                default:
                    validation.AddFallo("No disponible", "El documento no esta disponible para anulaciones.");
                    break;
            }

            return validation;
        }
        public DomainValidation PuedeRechazar(Usuario usuarioqueRechaza)
        {

            var validation = new DomainValidation();
            if (EstadoDocumento != EstadoDocumento.Revision)
            { validation.AddFallo("No disponible", "El documento no esta disponible para revisión."); }
            else
            {
                if (usuarioqueRechaza.Rol != Rol.VerificadorFacturas || usuarioqueRechaza.Rol != Rol.AdministradorFactura || usuarioqueRechaza.Rol != Rol.Administrador)
                { validation.AddFallo("No disponible", "El usuario no tiene permisos de revisar facturas."); }
            }
            return validation;
        }
        public DomainValidation PuedeVerificar(Usuario usuarioqueVerifica)
        {
            var validation = new DomainValidation();
            if (EstadoDocumento != EstadoDocumento.Revision)
            { validation.AddFallo("No disponible", "El documento no esta disponible para revisión."); }
            else
            {
                if (usuarioqueVerifica.Rol != Rol.VerificadorFacturas || usuarioqueVerifica.Rol != Rol.AdministradorFactura || usuarioqueVerifica.Rol != Rol.Administrador)
                { validation.AddFallo("No disponible", "El usuario no tiene permisos de revisar facturas."); }
            }
            return validation;
        }
        #endregion

    }
    public class ConceptoFactura : Entity<Guid>
    {
        public string Concepto { get; set; }
        public string? Descripcion { get; set; }
        public ConceptoFactura() : base(null)
        {

        }
        public ConceptoFactura(Usuario? usuariocreador):base(usuariocreador)
        {
             
        }
        public IEnumerable<Factura> Facturas { get; set; }
    }
    public class CuentasBancariasxFactura : Entity<Guid>
    {
        public CuentaBancaria CuentaBancaria { get; set; }
        public Guid CuentaBancariaId { get; set; }
        private CuentasBancariasxFactura() : base(null)
        {

        }
        public CuentasBancariasxFactura(Usuario usuariocreador, CuentaBancaria cuenta) : base(usuariocreador)
        {
            CuentaBancaria = cuenta;
            CuentaBancariaId = cuenta.Id;
        }
    }
}
