using Domain.Base;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ClasificacionDocumento : Entity<Guid>
    {
        public string Descripcion {get;set;}
        public ClasificacionTipoDocumento ClasificacionProceso { get; set; }
        public string NombreClasificacion => ClasificacionProceso.GetDescription();
    }
}
