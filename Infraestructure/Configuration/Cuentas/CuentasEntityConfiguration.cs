using Domain.Entities;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Base;

namespace Infraestructure.Configuration.Cuentas
{
    internal class CuentasEntityConfiguration : IEntityTypeConfiguration<BaseCuenta>
    {
        public void Configure(EntityTypeBuilder<BaseCuenta> builder)
        {
            builder.ToTable(nameof(Usuario), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany(t => t.CuentasCreadas).HasForeignKey(t => t.UsuarioCreadorId)
              .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Entidad).WithMany(t => t.CuentasConfiguracion).HasForeignKey(t => t.EntidadId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
