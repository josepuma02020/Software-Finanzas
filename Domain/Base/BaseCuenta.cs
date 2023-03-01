using Domain.Aplicacion.Entidades;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base
{
    public class BaseCuenta : Entity<Guid>
    {
        public Estado Estado { get; set; }
        public Entidad Entidad { get; set; }
        public Guid EntidadId { get; set; }
        public string NumeroCuenta { get; set; }
        public string DescripcionCuenta { get; set; }
        public TipoCuenta TipoCuenta { get; set; }
        private BaseCuenta():base(null) { }
        public BaseCuenta(Usuario usuariocreador) : base(usuariocreador) { Estado = Estado.Activo; }
    }
    public enum TipoCuenta
    {
        Contable, Bancaria
    }
}
