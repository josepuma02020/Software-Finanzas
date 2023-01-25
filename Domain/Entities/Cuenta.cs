using Domain.Base;
using Domain.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cuenta : Entity<Guid>
    {
        public string ? Descripcion { get; set; }
        public string ? CodigoCuenta { get; set; }
        public virtual Concepto Concepto { get;  set; }
        public string DescConcepto => Concepto == Concepto.Debito ? "Debito" : "Credito";
        public virtual ClasificacionCuenta Clasificacion { get;  set; }
        public string DescClasificacion => Clasificacion.DescripcionClasificacionCuenta;
    }
    public enum Concepto
    {
        Debito,
        Credito,
    }
    public class ClasificacionCuenta : Entity<Guid>
    {
        public string DescripcionClasificacionCuenta { get; set; }
    }
}
