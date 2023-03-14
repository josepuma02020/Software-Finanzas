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
using Domain.Aplicacion.Entidades.CuentasContables;
using Domain.Documentos;

namespace Infraestructure.Configuration.Cuentas
{
    public class CuentasEntityConfiguration : IEntityTypeConfiguration<Cuenta>
    {
        public void Configure(EntityTypeBuilder<Cuenta> builder)
        {
            builder.ToTable(nameof(Cuenta), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany(t => t.CuentasCreadas).HasForeignKey(t => t.UsuarioCreadorId)
              .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.UsuarioEditor).WithMany(t => t.CuentasEditadas).HasForeignKey(t => t.UsuarioEditorId)
              .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Entidad).WithMany(t => t.Cuentas).HasForeignKey(t => t.EntidadId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.NumeroCuenta).HasMaxLength(40);
            builder.Property(t => t.DescripcionCuenta).HasMaxLength(60);
        }
    }


    public class CuentaBancariaEntityConfiguration : IEntityTypeConfiguration<CuentaBancaria>
    {
        public void Configure(EntityTypeBuilder<CuentaBancaria> builder)
        {

        }
    }
    public class CuentaContableEntityConfiguration : IEntityTypeConfiguration<CuentaContable>
    {
        public void Configure(EntityTypeBuilder<CuentaContable> builder)
        {
            builder.ToTable(nameof(CuentaContable), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.CuentaBancaria).WithMany(t => t.CuentasContables).HasForeignKey(t => t.CuentaBancariaId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
