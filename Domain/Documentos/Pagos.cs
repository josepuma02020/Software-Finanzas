using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Documentos
{
    public class Pagos : Entity<Guid> 
    { 
        public long Valor { get; set; }
        public string Concepto { get; set; }
        public bool Estimado { get; set; }
        public string? Observaciones { get; set; }
        public DateTime Fecha { get; set; }
        public CuentaBancaria CuentaBancaria { get; set; }
        public Guid CuentaBancariaId { get; set; }
        private Pagos():base(null) { }
        public Pagos(Usuario creador) : base(creador) { }
    }
    public class ConceptosPagos : Entity<Guid>
    {
        public string Concepto { get; set; }
        public CuentaBancaria CuentaBancaria { get; set; }
        public TipoPago TipoPago { get; set; }
        private ConceptosPagos() : base(null)
        {

        }
        public ConceptosPagos(Usuario creador) : base(creador)
        {

        }
    }
    public class TipoPago : Entity<Guid>
    {
        public string IdTipoPago { get; set; }
        public string Descripcion { get; set; }
        public Estado Estado { get; set; }
        private TipoPago():base(null)
        {

        }
        public TipoPago(Usuario creador) : base(creador) { Estado = Estado.Activo; }
    }
}
