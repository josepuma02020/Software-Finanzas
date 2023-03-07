using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Aplicacion.EntidadesConfiguracion;
using Microsoft.VisualBasic;
using Infraestructure.Context;

namespace Infraestructure.Configuration.Aplicacion.Areas

{
    internal class AreaEntityConfiguration : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.ToTable(nameof(Area), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany(t => t.AreasCreadas).HasForeignKey(t => t.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.UsuarioEditor).WithMany(t => t.AreasEditadas).HasForeignKey(t => t.UsuarioEditorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.NombreArea).HasMaxLength(40);
            builder.Property(t => t.CodigoDependencia).HasMaxLength(20);
        }
    }
}
