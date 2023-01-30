using Domain.Base;
using Domain.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Factura : BaseEntityDocumento
    {

        public Tercero ? Tercero { get; set; }
        public int Valor { get; set; }
        public string  Observaciones { get; set; }
        public DateTime fechapago { get; set; }
        public string  ri { get; set; }
        public AppFile soporte { get; set; }
        public string rutasoporte => soporte.Path;

    }
}
