using Domain.Base;
using Domain.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities 
{
    public class Area : Entity<Guid>
    {
        public string NombreArea { get; set; }
        public List<Equipo>  Equipos { get; set; }

    }
    public class Equipo : Entity<Guid>
    {
        public string NombreEquipo { get; set; }
        public int AreaId { get; set; }
        public List<Proceso> Procesos  { get; set; }
    }
    public class Proceso : Entity<Guid>
    { 
        public string NombreProceso { get; set; }
        public int EquipoId { get; set; }

    }
}
