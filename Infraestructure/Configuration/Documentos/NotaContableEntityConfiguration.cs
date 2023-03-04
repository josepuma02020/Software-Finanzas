using Domain.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Documentos;

namespace Infraestructure.Configuration.ConfigDocumentos
{
    internal class NotaContableEntityConfiguration : IEntityTypeConfiguration<NotaContable>
    {
        public void Configure(EntityTypeBuilder<NotaContable> builder)
        {
            builder.ToTable(nameof(NotaContable), FinanzasContext.DefaultSchema);
            builder.HasOne(t => t.ClasificacionDocumento).WithMany(t => t.NotasContables).HasForeignKey(t => t.ClasificacionDocumentoId)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Proceso).WithMany(t => t.NotasContables).HasForeignKey(t => t.ProcesoId)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Equipo).WithMany(t => t.NotasContables).HasForeignKey(t => t.EquipoId)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.TipoDocumento).WithMany(t => t.NotasContables).HasForeignKey(t => t.TipoDocumentoId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
