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
        public virtual ConceptoCuenta Concepto { get;  set; }
        public string DescConcepto => Concepto == ConceptoCuenta.Debito ? "Debito" : "Credito";
        public virtual ClasificacionCuenta Clasificacioncuenta { get;  set; }
        public string DescClasificacionCuenta => Clasificacioncuenta ==ClasificacionCuenta.Banco ? "Banco" : "Normal";
        public Cuenta SetConcepto(ConceptoCuenta nuevoconcepto)
        {
            this.Concepto= nuevoconcepto;
            return this;
        }
        public Cuenta SetClasificacionCuenta(ClasificacionCuenta nuevaclasificacion)
        {
            this.Clasificacioncuenta = nuevaclasificacion;
            return this;
        }
    }
    public enum ConceptoCuenta
    {
        Debito,
        Credito,
    }
    public enum ClasificacionCuenta
    {
        Banco,
        Normal,
    }
}
