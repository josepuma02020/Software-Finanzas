using Domain.Aplicacion.EntidadesConfiguracion;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Documentos.ConfiguracionDocumentos;

namespace Infraestructure.Configuration.ConfigDocumentos
{
    public class DocumentoEntityConfiguration : IEntityTypeConfiguration<BaseEntityDocumento>
    {
        public void Configure(EntityTypeBuilder<BaseEntityDocumento> builder)
        {
            builder.ToTable(nameof(BaseEntityDocumento), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany(t => t.DocumentosCreados).HasForeignKey(t => t.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Anulador).WithMany(t => t.DocumentosAnulados).HasForeignKey(t => t.AnuladorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Aprobador).WithMany(t => t.DocumentosAprobados).HasForeignKey(t => t.AprobadorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Autorizador).WithMany(t => t.DocumentosAutorizados).HasForeignKey(t => t.AutorizadorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Rechazador).WithMany(t => t.DocumentosRechazados).HasForeignKey(t => t.RechazadorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.ReversorJD).WithMany(t => t.DocumentosRevertidos).HasForeignKey(t => t.ReversorJDId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.UsuarioEnviaRevision).WithMany(t => t.DocumentosEnviadosaRevision).HasForeignKey(t => t.UsuarioRevisionId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.UsuarioBot).WithMany(t => t.DocumentosBot).HasForeignKey(t => t.UsuarioBotId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.UsuarioEditor).WithMany(t => t.DocumentosEditados).HasForeignKey(t => t.UsuarioEditorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.EquipoCreador).WithMany(t=>t.DocumentosCreados).HasForeignKey(t => t.EquipoCreadorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    public class ClasificacionDocumentosEntityConfiguration : IEntityTypeConfiguration<ClasificacionDocumento>
    {
        public void Configure(EntityTypeBuilder<ClasificacionDocumento> builder)
        {
            builder.ToTable(nameof(ClasificacionDocumento), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany(t => t.ClasificacionesdeDocumentoCreadas).HasForeignKey(t => t.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.UsuarioEditor).WithMany(t => t.ClasificacionesdeDocumentoEditadas).HasForeignKey(t => t.UsuarioEditorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.Descripcion).HasMaxLength(40);


        }
    }
    public class TiposdeDocumentosEntityConfiguration : IEntityTypeConfiguration<TipoDocumento>
    {
        public void Configure(EntityTypeBuilder<TipoDocumento> builder)
        {
            builder.ToTable(nameof(TipoDocumento), FinanzasContext.DefaultSchema);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.UsuarioCreador).WithMany(t => t.TiposdeDocumentosCreados).HasForeignKey(t => t.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Property(t => t.CodigoTipoDocumento).HasMaxLength(10);
            builder.Property(t => t.DescripcionTipoDocumento).HasMaxLength(30);


        }
    }
}
