using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Tercero:Entity<Guid>
    {
        public string Nombre { get; set; }
        public int codigotercero { get; set; }

    }
}
