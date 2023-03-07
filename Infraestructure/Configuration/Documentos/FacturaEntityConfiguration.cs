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
using Domain.Aplicacion.EntidadesConfiguracion;

namespace Infraestructure.Configuration.ConfigDocumentos
{
    public class FacturaEntityConfiguration : IEntityTypeConfiguration<Factura>
    {
        public void Configure(EntityTypeBuilder<Factura> builder)
        {
            builder.ToTable(nameof(Factura), FinanzasContext.DefaultSchema);
            builder.HasOne(t => t.Tercero).WithMany(t => t.Facturas).HasForeignKey(t => t.TerceroId)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.CuentaBancaria).WithMany(t => t.Facturas).HasForeignKey(t => t.CuentaBancariaId)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Concepto).WithMany(t => t.Facturas).HasForeignKey(t => t.ConceptoId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.Observaciones).HasMaxLength(50);
            builder.Property(t => t.Ri).HasMaxLength(15);
        }
    }
    public class ConceptoFacturaEntityConfiguration : IEntityTypeConfiguration<ConceptoFactura>
    {
        public void Configure(EntityTypeBuilder<ConceptoFactura> builder)
        {
            builder.ToTable(nameof(ConceptoFactura), FinanzasContext.DefaultSchema);

            builder.Property(t => t.Concepto).HasMaxLength(25);
            builder.Property(t => t.Descripcion).HasMaxLength(30);
        }
    }
    public class CuentasBancariasxFacturaEntityConfiguration : IEntityTypeConfiguration<CuentasBancariasxFactura>
    {
        public void Configure(EntityTypeBuilder<CuentasBancariasxFactura> builder)
        {
            builder.ToTable(nameof(CuentasBancariasxFactura), FinanzasContext.DefaultSchema);

            builder.HasOne(t => t.CuentaBancaria).WithMany(t => t.CuentasxFacturas).HasForeignKey(t => t.CuentaBancariaId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
