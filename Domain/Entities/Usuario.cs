using Domain.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Usuario :Entity<Guid>
    {
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Ultingreso { get; set; }
        public new Estado Estado  { get; set; }
        public Rol  Rol { get; set; }
        public Usuario()
        {
            Estado = Estado.Activo;
        }
        public Usuario SetRole(Rol role)
        {
            this.Rol = role;
            return this;
        }
        public Usuario SetEstado(Estado estado)
        {
            this.Estado = estado;
            return this;
        }
    }
    
    public enum Estado
    {
        Inactivo,
        Activo
    }
}
