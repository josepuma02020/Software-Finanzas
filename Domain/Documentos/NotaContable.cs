using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasContables;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Base;
using Domain.Documentos.ConfiguracionDocumentos;
using Domain.Entities;
using Domain.Extensions;

namespace Domain.Documentos
{
    public class NotaContable : BaseEntityDocumento
    {
        public bool EditarNota { get; set; }
        public bool EditarRegistros { get; set; }
        public DateTime? Fechabatch { get; set; }
        public DateTime? FechaNota { get; set; }
        public string? Batch { get; set; }
        public string? Comentario { get; set; }
        public long Importe { get; set; }
        public ClasificacionDocumento ClasificacionDocumento { get; set; }
        public Guid ClasificacionDocumentoId => ClasificacionDocumento.Id;
        public string DescripcionClasificacionDocumento => ClasificacionDocumento.Descripcion;
        public List<Registrodenotacontable>? Registrosnota { get; set; }
        public long SalarioMinimoVigente { get; set; }
        public Proceso Proceso { get; set; }
        public Guid? ProcesoId { get; set; }
        public Equipo Equipo { get; set; }
        public Guid EquipoId { get; set; }

        public bool revisable { get; set; }
        public virtual Tiponotacontable Tiponotacontable { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }
        public Guid TipoDocumentoId => TipoDocumento.Id;
        public string TipoEntidadNombre => Tiponotacontable == Tiponotacontable.Soportes ? "Soportes" : "registrosnota";
        #region Metodos
        private NotaContable() : base(null)
        {
            ProcesoDocumento = ProcesosDocumentos.NotasContable;
        }
        public NotaContable(Usuario? usuariocreador):base(usuariocreador)
        {
            ProcesoDocumento = ProcesosDocumentos.NotasContable;
        }
       public NotaContable EditarNotaContable(NotaContable edicionnotacontable)
        {
            this.FechaNota = edicionnotacontable.FechaNota;
            this.Batch = edicionnotacontable.Batch;
            this.ClasificacionDocumento = edicionnotacontable.ClasificacionDocumento;
            this.Comentario = edicionnotacontable.Comentario;
            this.Importe = edicionnotacontable.Importe;
            this.Proceso = edicionnotacontable.Proceso;
            this.TipoDocumento = edicionnotacontable.TipoDocumento;
            this.Tiponotacontable = edicionnotacontable.Tiponotacontable;
            return this;
        }
        public NotaContable SetRegistrosNotaContable(List<Registrodenotacontable> registrosnota)
        {
            this.Registrosnota = registrosnota;
            return this;
        }
        public DomainValidation PuedeEditar(Usuario usuarioeditor)
        {
            var validation = new DomainValidation();
            switch (usuarioeditor.Rol)
            {
                case Rol.Administrador:
                    switch (this.EstadoDocumento)
                    {
                        case EstadoDocumento.Anulado:
                        case EstadoDocumento.RevertidoJD:
                        case EstadoDocumento.Cerrado:validation.AddFallo("No disponible", "La nota contable no esta disponible para ediciones."); break;
                    }
                    break;
                case Rol.AdministradorNotaContable:
                    switch (this.EstadoDocumento)
                    {
                        case EstadoDocumento.Anulado:
                        case EstadoDocumento.RevertidoJD:
                        case EstadoDocumento.Aprobado:
                        case EstadoDocumento.Autorizado:
                        case EstadoDocumento.Verificado:
                        case EstadoDocumento.Cerrado: validation.AddFallo("No disponible", "La nota contable no esta disponible para ediciones."); break;
                        default: 
                            if (usuarioeditor.ProcesoId != this.ProcesoId) validation.AddFallo("No disponible", "El usuario no tiene permiso para editar la nota contable."); break;
                    }
                    break;
                default:
                    switch (this.EstadoDocumento)
                    {
                        case EstadoDocumento.Abierto:if(this.UsuarioCreadorId != usuarioeditor.Id) validation.AddFallo("No disponible", "El usuario no tiene permiso para editar la nota contable.");break;
                        default: validation.AddFallo("No disponible", "La nota contable no esta disponible para ediciones.");break;
                    }break;
            }
            return validation;
        }
        public DomainValidation PuedeAnular(Usuario usuarioqueAnula)
        {
            var validation = new DomainValidation();

            switch (EstadoDocumento)
            {
                case EstadoDocumento.Anulado:
                case EstadoDocumento.Cerrado:
                    validation.AddFallo("No disponible", "La nota contable no esta disponible para anulaciones.");
                    break;
                default:
                    if (usuarioqueAnula.Rol != Rol.AdministradorNotaContable || usuarioqueAnula.Rol != Rol.Verificadordenotascontables || usuarioqueAnula.Rol != Rol.Administrador)
                    { validation.AddFallo("No disponible", "El usuario no tiene permiso para realizar anulaciones."); }
                    break;

            }

            return validation;
        }

        public DomainValidation PuedeRechazar(Usuario usuarioqueRechaza)
        {

            var validation = new DomainValidation();
            if (revisable == true)
            {
                switch (usuarioqueRechaza.Rol)
                {
                    case Rol.Aprobadordenotascontables:
                        if (EstadoDocumento != EstadoDocumento.Revision)
                        { validation.AddFallo("No disponible", "La nota contable no esta disponible para aprobaciones."); }
                        else
                        {
                            if (EquipoCreadorId != usuarioqueRechaza.EquipoId) { validation.AddFallo("No disponible", "El usuario no esta vinculado al equipo creador de la nota contable."); }
                        }
                        break;
                    case Rol.Autorizadordenotascontables:
                        if (EstadoDocumento != EstadoDocumento.Aprobado)
                        {
                            validation.AddFallo("No disponible", "La nota contable no esta disponible para autorizaciones.");
                        }
                        else
                        {
                            if (EquipoId != usuarioqueRechaza.EquipoId) { validation.AddFallo("No disponible", "El usuario no esta vinculado al equipo receptor de la nota contable."); }
                        }
                        break;
                    case Rol.Verificadordenotascontables:
                        if (EstadoDocumento != EstadoDocumento.Autorizado)
                        { validation.AddFallo("No disponible", "La nota contable no esta disponible para revisiones."); }
                        else
                        {
                            if (ProcesoId != usuarioqueRechaza.ProcesoId)
                            { validation.AddFallo("No disponible", "El usuario no esta vinculado al proceso receptor de la nota contable."); }
                        }
                        break;
                    default:
                        validation.AddFallo("No disponible", "El usuario no tiene permisos para revisar notas contables.");
                        break;
                }
            }
            else
            {
                if (usuarioqueRechaza.Rol != Rol.Verificadordenotascontables) { validation.AddFallo("No disponible", "El usuario no tiene permisos."); }
                if (EstadoDocumento != EstadoDocumento.Revision) { validation.AddFallo("No disponible", "La nota contable esta disponible para revisiones."); }
            }
            return validation;
        }
        public DomainValidation PuedeVerificar(Usuario usuarioqueVerifica)
        {
            var validation = new DomainValidation();

            if (revisable)
            {
                switch (EstadoDocumento)
                {
                    case EstadoDocumento.Revision:
                    case EstadoDocumento.Abierto:
                    case EstadoDocumento.Aprobado:
                    case EstadoDocumento.Verificado:
                    case EstadoDocumento.Anulado:
                    case EstadoDocumento.RevertidoJD:
                    case EstadoDocumento.Cerrado:
                        validation.AddFallo("No disponible", "El documento no esta disponible para verificaciones.");
                        break;
                    case EstadoDocumento.Autorizado:
                        switch (usuarioqueVerifica.Rol)
                        {
                            case Rol.Normal:
                            case Rol.AdministradorFactura:
                            case Rol.Aprobadordenotascontables:
                            case Rol.Autorizadordenotascontables:
                            case Rol.VerificadorFacturas:
                                validation.AddFallo("No disponible", "El usuario no puede verificar notas contables.");
                                break;
                            case Rol.Verificadordenotascontables:
                                if (ProcesoId != usuarioqueVerifica.EquipoId) { validation.AddFallo("No disponible", "El usuario no esta vinculado al proceso de la nota contable."); }
                                break;
                        }
                        break;
                }
            }
            return validation;
        }
        #endregion
    }
    public enum Tiponotacontable
    {
        Soportes,
        registrosnota,
    }
    public class Registrodenotacontable : Entity<Guid>
    {
       
        public DateTime? Fecha { get; set; }
        public long Importe { get; set; }
        public string? Tipolm => LM != null ? "A":null;
        public string? LM { get; set; }
        public Tercero Tercero { get; set; }
        public Guid TerceroId { get; set; }
        public Guid CuentaContableId { get; set; }
        public CuentaContable CuentaContable { get; set; }
        public Guid NotaContableId { get; set; }
        public NotaContable NotaContable { get; set; }
        private Registrodenotacontable() : base(null)
        {
        }
        public Registrodenotacontable(Usuario? usuariocreador) : base(usuariocreador)
        {
        }
        public Registrodenotacontable(Usuario usuariocreador,Tercero tercero,CuentaContable cuentacontable,NotaContable notacontable) : base(usuariocreador)
        {
            Tercero = tercero;TerceroId = tercero.Id;
            CuentaContable = cuentacontable;CuentaContableId = cuentacontable.Id;
            NotaContable = notacontable;NotaContableId = notacontable.Id;
        }
    }

    //Configuracion de Procesos para recibir notas contables
    public class NotaContablexProceso : Entity<Guid>
    {
        public Guid NotaContableId { get; set; }
        public NotaContable NotaContable { get; set; }
        public Guid ProcesoId { get; set; }
        public Proceso Proceso { get; set; }
        public NotaContablexProceso(Usuario? usuariocreador) : base(usuariocreador)
        {
        }

    }
}
