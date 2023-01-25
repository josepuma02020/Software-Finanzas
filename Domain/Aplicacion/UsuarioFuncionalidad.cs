
using Domain.Base;
using Domain.Entities;

namespace Domain.Aplicacion
{
    public sealed class UsuarioFuncionalidad : Entity<int>
    {
        public int UsuarioId { get; set; }
        public int FuncionalidadId { get; set; }
        public Usuario Usuario { get; set; }
        public Funcionalidad Funcionalidad { get; set; }
        public UsuarioFuncionalidad()
        {

        }
        public UsuarioFuncionalidad(Usuario usuario, Funcionalidad funcionalidad)
        {
            Usuario = usuario;
            Funcionalidad = funcionalidad;
            UsuarioId = usuario.Id;
            FuncionalidadId = funcionalidad.Id;
        }
    }
}
