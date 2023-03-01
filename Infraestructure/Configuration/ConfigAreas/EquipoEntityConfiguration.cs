using Domain.Aplicacion.EntidadesConfiguracion;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Configuration.ConfigAreas
{
    internal class EquipoEntityConfiguration : IEntityTypeConfiguration<Equipo>
    {
        public void Configure(EntityTypeBuilder<Equipo> builder)
        {
            builder.ToTable(nameof(Equipo), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany(t => t.EquiposCreados).HasForeignKey(t => t.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.UsuarioEditor).WithMany(t => t.EquipoEditados).HasForeignKey(t => t.UsuarioEditorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Area).WithMany(t => t.EquiposdeArea).HasForeignKey(t => t.AreaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
