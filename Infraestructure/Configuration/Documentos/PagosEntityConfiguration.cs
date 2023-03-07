using Domain.Documentos;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Configuration.Documentos
{
    public class PagosEntityConfiguration : IEntityTypeConfiguration<Pagos>
    {
        public void Configure(EntityTypeBuilder<Pagos> builder)
        {
            builder.ToTable(nameof(Pagos), FinanzasContext.DefaultSchema);
            builder.HasOne(t => t.CuentaBancaria).WithMany(t => t.Pagos).HasForeignKey(t => t.CuentaBancariaId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.Concepto).HasMaxLength(30);
            builder.Property(t => t.Observaciones).HasMaxLength(40);

        }
    }
    public class TipoPagoEntityConfiguration : IEntityTypeConfiguration<TipoPago>
    {
        public void Configure(EntityTypeBuilder<TipoPago> builder)
        {
            builder.ToTable(nameof(TipoPago), FinanzasContext.DefaultSchema);

            builder.Property(t => t.IdTipoPago).HasMaxLength(10);
            builder.Property(t => t.Descripcion).HasMaxLength(40);

        }
    }
}
