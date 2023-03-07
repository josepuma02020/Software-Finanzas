using Domain.Aplicacion;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infraestructure.Configuration.Entidades
{
    public class AppFilesEntityConfiguration : IEntityTypeConfiguration<AppFile>
    {
        public void Configure(EntityTypeBuilder<AppFile> builder)
        {
            builder.ToTable(nameof(AppFile), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany(t => t.ArchivosCargados).HasForeignKey(t => t.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.Path).HasMaxLength(40);
            builder.Property(t => t.Nombre).HasMaxLength(40);
        }
    }
}
