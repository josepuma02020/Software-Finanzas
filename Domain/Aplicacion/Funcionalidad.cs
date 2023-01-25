
using Domain.Base;
using System.Collections.Generic;

namespace Domain.Aplicacion
{
    public class Funcionalidad : Entity<int>
    {
        public string Titulo { get; set; }
        public string Icono { get; set; }
        public string Url { get; set; }
        public int ModuloId { get; set; }
        public List<RoleFuncionalidad> RoleFuncionalidades { get; set; }
        public Modulo Modulo { get; set; }
    }
}
