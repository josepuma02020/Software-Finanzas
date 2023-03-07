using Domain.Aplicacion.EntidadesConfiguracion;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Aplicacion;
using Domain.Base;

namespace Infraestructure.Configuration
{
    public class ConfiguracionGeneralEntityConfiguration : IEntityTypeConfiguration<Configuracion>
    {
        public void Configure(EntityTypeBuilder<Configuracion> builder)
        {
            builder.ToTable(nameof(Configuracion), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany().HasForeignKey(t => t.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
    public class ConfiguracionNotasContablesEntityConfiguration : IEntityTypeConfiguration<ConfiguracionProcesoNotasContables>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionProcesoNotasContables> builder)
        {
            builder.ToTable(nameof(ConfiguracionProcesoNotasContables), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany().HasForeignKey(t => t.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.ProcesoNotaContable).WithMany(t => t.ConfiguracionesNotasContables).HasForeignKey(t => t.ProcesoId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
    public class ConfiguracionServiciosEntityConfiguration : IEntityTypeConfiguration<ConfiguracionServicios>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionServicios> builder)
        {
            builder.ToTable(nameof(ConfiguracionServicios), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany().HasForeignKey(t => t.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.Servicio).HasMaxLength(40);

        }
    }

}
