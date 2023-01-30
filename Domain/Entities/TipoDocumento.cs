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
        public List<ProcesosDocumentos> ProcesosDocumento { get; set; }
    }
    public enum ProcesosDocumentos
    {
        [Description("Facturas")] Facturas,
        [Description("Notas Contable Financiacion")] NotasContable,
    }
}
