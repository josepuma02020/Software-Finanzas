using Domain.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Aplicacion.Entidades;

namespace Infraestructure.Configuration.Cuentas
{
    public class EntidadesEntityConfiguration : IEntityTypeConfiguration<Entidad>
    {
        public void Configure(EntityTypeBuilder<Entidad> builder)
        {
            builder.ToTable(nameof(Entidad), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany(t => t.EntidadesCreadas).HasForeignKey(t => t.UsuarioCreadorId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.NombreEntidad).HasMaxLength(40);
            builder.Property(t => t.Observaciones).HasMaxLength(50);
        }
    }
}
