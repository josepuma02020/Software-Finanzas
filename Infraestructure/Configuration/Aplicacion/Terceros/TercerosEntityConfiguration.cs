using Domain.Aplicacion.EntidadesConfiguracion;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Configuration.Aplicacion.Terceros
{
    public class TercerosEntityConfiguration : IEntityTypeConfiguration<Tercero>
    {
        public void Configure(EntityTypeBuilder<Tercero> builder)
        {
            builder.ToTable(nameof(Tercero), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Nombre).HasMaxLength(40);
            builder.Property(t => t.Codigotercero).HasMaxLength(20);
            builder.Property(t => t.ObservacionAdicional).HasMaxLength(20);
        }
    }
}
