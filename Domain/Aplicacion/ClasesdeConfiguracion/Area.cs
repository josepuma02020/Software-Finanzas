using Domain.Base;
using Domain.Documentos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aplicacion.EntidadesConfiguracion
{
    public class Area : Entity<Guid>
    {
        public string? NombreArea { get; set; }
        public string CodigoDependencia { get; set; }
        #region Configuracion
        public IEnumerable<Proceso> Procesos { get; set; }
        public IEnumerable<Usuario> Usuarios { get; set; }
        #endregion
        public List<Equipo>? Equipos { get; set; }
        public Area() : base(null)
        {
            Id = Guid.NewGuid();
        }
        public Area(Usuario? usuariocreador) : base(usuariocreador)
        {
           
        }
        public Area(string nombreArea,Usuario? usuariocreador):base(usuariocreador)
        {
            NombreArea = nombreArea;
        }
        public Area(string nombreArea) : base(null)
        {
            NombreArea = nombreArea;
        }

    }
    public class Equipo : Entity<Guid>
    {
        public string NombreEquipo { get; set; }
        public string? CodigoEquipo { get; set; }
        public Guid AreaId { get; set; }
        public Area Area { get; set; }
        public List<Proceso>? Procesos { get; set; }
        public List<BaseEntityDocumento>? DocumentosCreados { get; set; }

        #region Configuracion
        public IEnumerable<NotaContable> NotasContables { get; set; }
        #endregion
        public Equipo() : base(null)
        {

        }
        public Equipo( Usuario? usuariocreador) : base(usuariocreador)
        {

        }
        public Equipo(string nombreEquipo,Usuario? usuariocreador):base(usuariocreador)
        {
            NombreEquipo = nombreEquipo;
        }
    }
    public class Proceso : Entity<Guid>
    {
        public string NombreProceso { get; set; }
        public Guid EquipoId { get; set; }
        public Guid AreaId { get; set; }
        public Equipo Equipo { get; set; }
        public Area Area { get; set; }

        #region Configuracion
        public IEnumerable<Usuario> Usuarios { get; set; }
        public IEnumerable<NotaContable> NotasContables { get; set; }
        public List<ConfiguracionProcesoNotasContables>? ConfiguracionesNotasContables { get; set; }
        #endregion
        public Proceso() : base(null)
        {

        }
        public Proceso(string nombreProceso, Usuario? usuariocreador) : base(usuariocreador)
        {
            NombreProceso = nombreProceso;

        }
        public Proceso( Usuario? usuariocreador) : base(usuariocreador)
        {

        }
    }
}
