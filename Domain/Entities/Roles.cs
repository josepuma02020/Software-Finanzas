using Domain.Aplicacion;
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
    internal class Role
    {
        public int Id { get; set; }
        public Rol Rol { get; set; }
        public string NombreRol => Rol.GetDescription();
        public IEnumerable<Usuario> Usuarios { get; set; }

       
    }
    public enum Rol
    {
        [Description("Normal")] Normal,
        [Description("Verificador de notascontables")] Verificadordenotascontables,
        [Description("Aprobador de notascontables")] Aprobadordenotascontables,
        [Description("Autorizador de notascontables")] Autorizadordenotascontables,
        [Description("Aprobador de facturas")] Aprobadordefacturas,
        [Description("Administrador")] Administrador,
    }

    public class Funcionalidades
    {
        public int Id { get; set; }
        public string DescripcionFuncionalidad { get; set; }
    }

    public class RolFuncionalidades
    {
        public Funcionalidades Funcionalidad { get; set; }
        public Rol Rol { get; set; }
    }


}
