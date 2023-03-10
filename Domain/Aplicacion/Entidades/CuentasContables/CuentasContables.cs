using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Base;
using Domain.Documentos;
using Domain.Entities;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aplicacion.Entidades.CuentasContables
{
    public class CuentaContable : Cuenta
    {
        public CuentaBancaria? CuentaBancaria { get; set; }
        public Guid? CuentaBancariaId { get; set; }
        public List<Registrodenotacontable> RegistrosNotaContable { get; set; }
        private CuentaContable() : base(null)
        {
        }
        public CuentaContable(Usuario usuariocreador, Entidad entidad) : base(usuariocreador)
        {
            TipoCuenta = TipoCuenta.Contable;
            Entidad = entidad;
            EntidadId = entidad.Id;
        }
        public CuentaContable EditarCuentaContable(Usuario editor,CuentaBancaria? cuentabancaria,string descripcion,Estado estado,Entidad entidad)
        {
            this.UsuarioEditor = editor;
            this.UsuarioEditorId = editor.Id;
            this.DescripcionCuenta = descripcion;
            this.Estado = estado;
            this.Entidad = entidad;this.EntidadId = entidad.Id;
            this.FechaEdicion = DateTime.Now;
            return this;
        }
        #region Configuracion
        public IEnumerable<ConceptoxCuentaContable> ConceptosCuentasContableCredito { get; set; }
        public IEnumerable<ConceptoxCuentaContable>? ConceptosCuentasContableDebito { get; set; }
        #endregion
    }
    public class ConceptoxCuentaContable : Entity<Guid>
    {
        public CuentaContable CuentaContableDebito { get; set; }
        public CuentaContable CuentaContableCredito { get; set; }
        public Guid CuentaContableDebitoId { get; set; }
        public Guid CuentaContableCreditoId { get; set; }
        private ConceptoxCuentaContable() : base(null) { }

        public ConceptoxCuentaContable(Usuario creador) : base(creador) { }
    }
}
