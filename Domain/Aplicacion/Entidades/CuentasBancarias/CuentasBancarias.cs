using Domain.Aplicacion.Entidades.CuentasContables;
using Domain.Base;
using Domain.Entities;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aplicacion.Entidades.CuentasBancarias
{
    public class CuentaBancaria : BaseCuenta
    {

        public virtual TipoCuentaBancaria? TipoCuentaBancaria { get; set; }
        public string? DecTipoCuentaBancaria => TipoCuentaBancaria?.GetDescription();
        public List<CuentaContable> CuentasContable { get; set; }

        private CuentaBancaria() : base(null)
        {

        }
        public CuentaBancaria(Usuario usuariocreador, Entidad entidad) : base(usuariocreador)
        {
            TipoCuenta = TipoCuenta.Bancaria;
            Entidad = entidad;
            EntidadId = entidad.Id;
            
        }
        public CuentaBancaria EditarCuentaBancaria(TipoCuentaBancaria tipoCuentaBancaria,string descripcioncuenta,Usuario editor,Estado estado,Entidad entidad)
        {
            this.TipoCuentaBancaria = tipoCuentaBancaria;
            this.DescripcionCuenta = descripcioncuenta;
            this.UsuarioEditor = editor;
            this.Entidad = entidad;
            this.EntidadId = entidad.Id;
            this.Estado = estado;
            this.UsuarioEditorId = editor.Id;
            this.FechaEdicion = DateTime.Now;
            return this;
        }
    }
    public enum TipoCuentaBancaria
    {
        [Description("Ahorros")] Ahorros,
        [Description("Corriente")] Corriente,
        [Description("Fonde de Inversion")] FondodeInversion,
    }
}
