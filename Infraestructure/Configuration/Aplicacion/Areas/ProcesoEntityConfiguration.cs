using Domain.Aplicacion.EntidadesConfiguracion;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Configuration.Aplicacion.Areas
{
    public class ProcesoEntityConfiguration : IEntityTypeConfiguration<Proceso>
    {
        public void Configure(EntityTypeBuilder<Proceso> builder)
        {
            builder.ToTable(nameof(Proceso), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany(t => t.ProcesosCreados).HasForeignKey(t => t.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Equipo).WithMany(t => t.Procesos).HasForeignKey(t => t.EquipoId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(t => t.NombreProceso).HasMaxLength(40);
        }
    }
}
