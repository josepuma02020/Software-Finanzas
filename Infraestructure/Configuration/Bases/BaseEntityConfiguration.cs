using Domain.Aplicacion.Entidades.CuentasContables;
using Domain.Entities;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Infraestructure.Configuration.Bases
{
    internal class BaseEntityConfiguration : IEntityTypeConfiguration<BaseEntity>
    {
        public void Configure(EntityTypeBuilder<BaseEntity> builder)
        {
            builder.ToTable(nameof(Usuario), FinanzasContext.DefaultSchema);
            builder.HasOne(t => t.UsuarioCreador).WithMany(t => t.EntityCreadas).HasForeignKey(t => t.UsuarioCreadorId)
              .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.UsuarioEditor).WithMany(t => t.EntityEditadas).HasForeignKey(t => t.UsuarioEditorId)
              .OnDelete(DeleteBehavior.Restrict);

        }
    }
}