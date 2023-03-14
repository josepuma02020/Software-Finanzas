﻿using Domain.Aplicacion.EntidadesConfiguracion;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infraestructure.Configuration.ConfigUsuarios
{
    internal class UsuarioEntityConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable(nameof(Usuario), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany().HasForeignKey(t => t.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.UsuarioEditor).WithMany().HasForeignKey(t => t.UsuarioEditorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Proceso).WithMany(t => t.Usuarios).HasForeignKey(t => t.ProcesoId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Area).WithMany(t => t.Usuarios).HasForeignKey(t => t.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.Nombre).HasMaxLength(40);
            builder.Property(t => t.Identificacion).HasMaxLength(30);
            builder.Property(t => t.Email).HasMaxLength(40);

        }
    }
}
