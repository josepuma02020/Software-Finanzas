using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppFile : Entity<Guid>
    {
        public AppFile(AppFileBuilder appFileBuilder,Usuario usuariocreador):base(usuariocreador)
        {
            Path = appFileBuilder.Path;
            Nombre = appFileBuilder.Nombre;
        }
        public string Path { get; set; }
        public string Nombre { get; set; }

        private AppFile() : base( null)
        {

        }
        public AppFile(Usuario usuariocreador) : base(usuariocreador)
        {

        }
        public class AppFileBuilder
        {
            public string Path { get; set; }
            public string Nombre { get; set; }
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
            public AppFile Build()
            {
                AppFile appFile = new AppFile(this,null);
                return appFile;
            }
        }

    }
}
