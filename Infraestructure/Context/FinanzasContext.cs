using Domain.Aplicacion;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Base;
using Domain.Documentos;
using Domain.Documentos.ConfiguracionDocumentos;
using Domain.Entities;
using Infraestructure.Base;
using Infraestructure.Configuration.ConfigDocumentos;
using Infraestructure.Configuration.ConfigAreas;
using Microsoft.EntityFrameworkCore;
using Infraestructure.Configuration.ConfigUsuarios;
using Infraestructure.Configuration;
using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Aplicacion.Entidades.CuentasContables;
using Infraestructure.Configuration.Cuentas;
using Infraestructure.Configuration.Bases;
using Infraestructure.Configuration.Documentos;

namespace Infraestructure.Context
{
    public sealed class FinanzasContext : DbContextBase
    {
        public static string ProviderName { get; private set; }

        public FinanzasContext(DbContextOptions options) : base(options)
        {
            ProviderName = Database.ProviderName;
        }
        public DbSet<Pagos> Pagos { get; set; }
        public DbSet<ConfiguracionServicios> ConfiguracionServicios { get; set; }
        public DbSet<CuentasBancariasxFactura> CuentasxFactura { get; set; }
        public DbSet<CuentaContable> CuentasContables { get; set; }
        public DbSet<CuentaBancaria> CuentasBancarias { get; set; }
        public DbSet<ConfiguracionProcesoNotasContables> ConfiguracionesNotasContables { get; set; }
        public DbSet<BaseEntityDocumento> Documentos { get; set; }
        public DbSet<Usuario> RoleFuncionalidades { get; set; }
        public DbSet<Configuracion> Configuraciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoDocumento> TipoDocumentos { get; set; }
        public DbSet<Tercero> Terceros { get; set; }
        public DbSet<NotaContable> NotasContables { get; set; }
        public DbSet<Registrodenotacontable> Registrsodenotacontable { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Entidad> Entidades { get; set; }
        public DbSet<ClasificacionDocumento> ClasificacionDocumentos { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Proceso> Procesos { get; set; }
        public DbSet<AppFile> AppFiles { get; set; }
        public DbSet<ConceptoFactura> ConceptoFacturas { get; set; }
        public static string DefaultSchema => "Finanzas";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ConfiguracionGeneral
            modelBuilder.ApplyConfiguration(new ConfiguracionGeneralEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BaseEntityConfiguration());

            //Documentos
            modelBuilder.ApplyConfiguration(new DocumentoEntityConfiguration());
            modelBuilder.ApplyConfiguration(new NotaContableEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FacturaEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SaldosEntityConfiguration());

            //Areas
            modelBuilder.ApplyConfiguration(new AreaEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EquipoEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProcesoEntityConfiguration());

            //Clases Primarias
            modelBuilder.ApplyConfiguration(new UsuarioEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CuentasEntityConfiguration());

            //Clases secundarias
            modelBuilder.ApplyConfiguration(new ConceptosCuentasContbalesEntityConfiguration());


            //Inicializaciones
            modelBuilder.Entity<ConfiguracionServicios>().HasData(
                new ConfiguracionServicios { Activo=true,Servicio = NombreServicios.ServicioFactura },
                new ConfiguracionServicios { Activo = true, Servicio = NombreServicios.ServicioNotaContable},
                new ConfiguracionServicios { Activo = true, Servicio = NombreServicios.ServicioFlujodeCaja}
                );

            modelBuilder.Entity<ConceptoFactura>().HasData(
                new ConceptoFactura { Concepto="RI" }
                );


        }
        
    }
}
