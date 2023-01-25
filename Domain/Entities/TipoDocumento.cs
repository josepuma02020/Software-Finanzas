using Domain.Base;
using Domain.Clases;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TipoDocumento : Entity<Guid>
    {
        public string CodigoTipoDocumento { get; set; }
        public string DescripcionTipoDocumento { get; set; }
        public List<ClasificacionTipoDocumento> Clasificaciones { get; set; }
    }
    public enum ClasificacionTipoDocumento
    {
        [Description("Facturas")] Facturas,
        [Description("NotasContableFinanciacion")] NotasContableFinanciacion,
        [Description("NotasContableContabilidad")] NotasContableContabilidad,
    }
}
