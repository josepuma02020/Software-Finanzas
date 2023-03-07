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
    public class NotaContableEntityConfiguration : IEntityTypeConfiguration<NotaContable>
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
            builder.Property(t => t.Batch).HasMaxLength(20);
            builder.Property(t => t.Comentario).HasMaxLength(30);
        }
    }
    public class RegistrosNotaContableEntityConfiguration : IEntityTypeConfiguration<Registrodenotacontable>
    {
        public void Configure(EntityTypeBuilder<Registrodenotacontable> builder)
        {
            builder.ToTable(nameof(Registrodenotacontable), FinanzasContext.DefaultSchema);
            builder.HasOne(t => t.TerceroLM).WithMany(t => t.TercerosLM).HasForeignKey(t => t.TerceroLMId)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.TerceroAN8).WithMany(t => t.TercerosAN8).HasForeignKey(t => t.TerceroAN8Id)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.TerceroAN8).WithMany(t => t.TercerosAN8).HasForeignKey(t => t.TerceroAN8Id)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.CuentaContable).WithMany(t => t.RegistrosNotaContable).HasForeignKey(t => t.CuentaContableId)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.NotaContable).WithMany(t => t.Registrosnota).HasForeignKey(t => t.NotaContableId)
               .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
