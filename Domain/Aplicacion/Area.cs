using Domain.Base;
using Domain.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aplicacion
{
    public class Area : Entity<Guid>
    {
        public string? NombreArea { get; set; }
        public List<Equipo> ? Equipos { get; set; }
        public Area(string nombreArea)
        {
            NombreArea = nombreArea;
        }

    }
    public class Equipo : Entity<Guid>
    {
        public string NombreEquipo { get; set; }
        public Guid AreaId { get; set; }
        public List<Proceso>? Procesos { get; set; }
        public Equipo(string nombreEquipo, Guid areaId)
        {
            NombreEquipo = nombreEquipo;
            AreaId = areaId;
        }
    }
    public class Proceso : Entity<Guid>
    {
        public string NombreProceso { get; set; }
        public Guid EquipoId { get; set; }

        public Proceso(string nombreProceso, Guid equipoId)
        {
            NombreProceso = nombreProceso;
            EquipoId = equipoId;
        }
    }
}
