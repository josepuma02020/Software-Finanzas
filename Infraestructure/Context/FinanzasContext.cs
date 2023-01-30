using Domain.Aplicacion;
using Domain.Clases;
using Domain.Entities;
using Infraestructure.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Context
{
    public sealed class FinanzasContext : DbContextBase
    {
        public static string ProviderName { get; private set; }

        public FinanzasContext(DbContextOptions options) : base(options)
        {
            ProviderName = Database.ProviderName;
        }
        public DbSet<Usuario> RoleFuncionalidades { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoDocumento> TipoDocumentos { get; set; }
        public DbSet<Tercero> Terceros { get; set; }
        public DbSet<NotaContable> NotasContables { get; set; }
        public DbSet<Registrodenotacontable> Registrsodenotacontable { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<ClasificacionDocumento> ClasificacionDocumentos { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Proceso> Procesos { get; set; }
        public DbSet<AppFile> AppFiles { get; set; }
        public static string DefaultSchema => "Finanzas";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EntityTypeConfiguration<NotaContable>());
        }
    }
}
