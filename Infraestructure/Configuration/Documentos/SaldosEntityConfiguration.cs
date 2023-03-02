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

namespace Infraestructure.Configuration.Documentos
{
    internal class SaldosEntityConfiguration : IEntityTypeConfiguration<Saldos>
    {
        public void Configure(EntityTypeBuilder<Saldos> builder)
        {
            builder.ToTable(nameof(BaseEntityDocumento), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.CuentaBancaria).WithMany(t => t.Saldos).HasForeignKey(t => t.CuentaBancariaId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
