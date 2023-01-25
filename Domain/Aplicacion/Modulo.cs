using System.Collections.Generic;
using Domain.Base;

namespace Domain.Aplicacion
{
    public class Modulo : Entity<int>
    {
        public string Titulo { get; set; }
        public string Icono { get; set; }
        public List<Funcionalidad> Funcionalidades { get; set; }
    }
}
