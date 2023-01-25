using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppFile
    {
        public AppFile(AppFileBuilder appFileBuilder)
        {
            Path = appFileBuilder.Path;
            Nombre = appFileBuilder.Nombre;
            UsuarioQueCargoElArchivo = appFileBuilder.UsuarioQueCargoElArchivo;
        }
        public string Path { get; set; }
        public string Nombre { get; set; }
        public Usuario UsuarioQueCargoElArchivo { get; set; }
        public int UsuarioQueCargoElArchivoId { get; set; }
        public class AppFileBuilder
        {
            public string Path { get; set; }
            public string Nombre { get; set; }
            public Usuario UsuarioQueCargoElArchivo { get; set; }
            public AppFileBuilder SetPath(string path)
            {
                Path = path;
                return this;
            }
            public AppFileBuilder SetNombre(string nombre)
            {
                Nombre = nombre;
                return this;
            }
            public AppFileBuilder SetUsuarioQueCargoElArchivo(Usuario usuarioQueCargoElArchivo)
            {
                UsuarioQueCargoElArchivo = usuarioQueCargoElArchivo;
                return this;
            }
            public AppFile Build()
            {
                AppFile appFile = new AppFile(this);
                return appFile;
            }
        }

    }
}
