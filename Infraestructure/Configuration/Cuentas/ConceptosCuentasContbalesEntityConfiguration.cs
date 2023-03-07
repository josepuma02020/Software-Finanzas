using Domain.Base;
using Domain.Entities;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Aplicacion.Entidades.CuentasContables;

namespace Infraestructure.Configuration.Cuentas
{
    public class ConceptosCuentasContbalesEntityConfiguration : IEntityTypeConfiguration<ConceptoxCuentaContable>
    {
        public void Configure(EntityTypeBuilder<ConceptoxCuentaContable> builder)
        {
            builder.ToTable(nameof(ConceptoxCuentaContable), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany().HasForeignKey(t => t.UsuarioCreadorId)
              .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.UsuarioEditor).WithMany().HasForeignKey(t => t.UsuarioEditorId)
             .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.CuentaContableCredito).WithMany(t => t.ConceptosCuentasContableCredito).HasForeignKey(t => t.CuentaContableCreditoId)
              .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.CuentaContableDebito).WithMany(t => t.ConceptosCuentasContableDebito).HasForeignKey(t => t.CuentaContableDebitoId)
              .OnDelete(DeleteBehavior.Restrict);

        }
    }
}