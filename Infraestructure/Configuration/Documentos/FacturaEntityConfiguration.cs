using Domain.Base;
using Domain.Documentos;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Configuration.ConfigDocumentos
{
    internal class FacturaEntityConfiguration : IEntityTypeConfiguration<Factura>
    {
        public void Configure(EntityTypeBuilder<Factura> builder)
        {
            builder.ToTable(nameof(BaseEntityDocumento), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.Tercero).WithMany(t => t.Facturas).HasForeignKey(t => t.TerceroId)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.CuentaBancaria).WithMany(t => t.Facturas).HasForeignKey(t => t.CuentaBancariaId)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Concepto).WithMany(t => t.Facturas).HasForeignKey(t => t.ConceptoId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
