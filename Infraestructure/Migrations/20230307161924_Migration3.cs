using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Finanzas");

            migrationBuilder.CreateTable(
                name: "AppFile",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    BaseEntityDocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Area",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreArea = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CodigoDependencia = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseEntityDocumento",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EditarSoportes = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioRevisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioBotId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReversorJDId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RechazadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnuladorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VerificadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AutorizadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AprobadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FechaAprobacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaAutorizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaVerificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaAnulacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fechabot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EquipoCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcesoDocumento = table.Column<int>(type: "int", nullable: false),
                    EstadoDocumento = table.Column<int>(type: "int", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseEntityDocumento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClasificacionDocumento",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ClasificacionProceso = table.Column<int>(type: "int", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClasificacionDocumento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConceptoFactura",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptoFactura", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConceptoxCuentaContable",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CuentaContableDebitoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CuentaContableCreditoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptoxCuentaContable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuracion",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Salariominimo = table.Column<long>(type: "bigint", nullable: false),
                    MultiploRevisarNotaContable = table.Column<int>(type: "int", nullable: false),
                    Año = table.Column<int>(type: "int", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionProcesoNotasContables",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    Año = table.Column<int>(type: "int", nullable: false),
                    ProcesoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionProcesoNotasContables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionServicios",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Servicio = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionServicios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cuenta",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    EntidadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroCuenta = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DescripcionCuenta = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    TipoCuenta = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoCuentaBancaria = table.Column<int>(type: "int", nullable: true),
                    CuentaBancariaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cuenta_Cuenta_CuentaBancariaId",
                        column: x => x.CuentaBancariaId,
                        principalSchema: "Finanzas",
                        principalTable: "Cuenta",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Saldos",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CuentaBancariaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntidadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SaldoTotal = table.Column<long>(type: "bigint", nullable: false),
                    SaldoDisponible = table.Column<long>(type: "bigint", nullable: true),
                    TieneDisponible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saldos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Saldos_BaseEntityDocumento_Id",
                        column: x => x.Id,
                        principalSchema: "Finanzas",
                        principalTable: "BaseEntityDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Saldos_Cuenta_CuentaBancariaId",
                        column: x => x.CuentaBancariaId,
                        principalSchema: "Finanzas",
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CuentasBancariasxFactura",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CuentaBancariaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentasBancariasxFactura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuentasBancariasxFactura_Cuenta_CuentaBancariaId",
                        column: x => x.CuentaBancariaId,
                        principalSchema: "Finanzas",
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Entidad",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreEntidad = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entidad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipo",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreEquipo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CodigoEquipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipo_Area_AreaId",
                        column: x => x.AreaId,
                        principalSchema: "Finanzas",
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Factura",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Valor = table.Column<long>(type: "bigint", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fechapago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ri = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CuentaBancariaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CuentaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConceptoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TerceroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factura_BaseEntityDocumento_Id",
                        column: x => x.Id,
                        principalSchema: "Finanzas",
                        principalTable: "BaseEntityDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factura_ConceptoFactura_ConceptoId",
                        column: x => x.ConceptoId,
                        principalSchema: "Finanzas",
                        principalTable: "ConceptoFactura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factura_Cuenta_CuentaBancariaId",
                        column: x => x.CuentaBancariaId,
                        principalSchema: "Finanzas",
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotaContable",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RevisionesFinanciacion = table.Column<bool>(type: "bit", nullable: false),
                    RevisionesGestionContable = table.Column<bool>(type: "bit", nullable: false),
                    Fechabatch = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaNota = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Comentario = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Importe = table.Column<long>(type: "bigint", nullable: false),
                    ClasificacionDocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalarioMinimoVigente = table.Column<long>(type: "bigint", nullable: false),
                    ProcesoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    revisable = table.Column<bool>(type: "bit", nullable: false),
                    Tiponotacontable = table.Column<int>(type: "int", nullable: false),
                    TipoDocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotaContable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotaContable_BaseEntityDocumento_Id",
                        column: x => x.Id,
                        principalSchema: "Finanzas",
                        principalTable: "BaseEntityDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotaContable_ClasificacionDocumento_ClasificacionDocumentoId",
                        column: x => x.ClasificacionDocumentoId,
                        principalSchema: "Finanzas",
                        principalTable: "ClasificacionDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotaContable_Equipo_EquipoId",
                        column: x => x.EquipoId,
                        principalSchema: "Finanzas",
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Valor = table.Column<long>(type: "bigint", nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Estimado = table.Column<bool>(type: "bit", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CuentaBancariaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Cuenta_CuentaBancariaId",
                        column: x => x.CuentaBancariaId,
                        principalSchema: "Finanzas",
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Proceso",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreProceso = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    EquipoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proceso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proceso_Area_AreaId",
                        column: x => x.AreaId,
                        principalSchema: "Finanzas",
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Proceso_Equipo_EquipoId",
                        column: x => x.EquipoId,
                        principalSchema: "Finanzas",
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Identificacion = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Ultingreso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcesoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rol = table.Column<int>(type: "int", nullable: false),
                    UsuarioAsignaProcesoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Area_AreaId",
                        column: x => x.AreaId,
                        principalSchema: "Finanzas",
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuario_Equipo_EquipoId",
                        column: x => x.EquipoId,
                        principalSchema: "Finanzas",
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuario_Proceso_ProcesoId",
                        column: x => x.ProcesoId,
                        principalSchema: "Finanzas",
                        principalTable: "Proceso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuario_Usuario_UsuarioAsignaProcesoId",
                        column: x => x.UsuarioAsignaProcesoId,
                        principalSchema: "Finanzas",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuario_Usuario_UsuarioCreadorId",
                        column: x => x.UsuarioCreadorId,
                        principalSchema: "Finanzas",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuario_Usuario_UsuarioEditorId",
                        column: x => x.UsuarioEditorId,
                        principalSchema: "Finanzas",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tercero",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ObservacionAdicional = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Codigotercero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tercero", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tercero_Usuario_UsuarioCreadorId",
                        column: x => x.UsuarioCreadorId,
                        principalSchema: "Finanzas",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tercero_Usuario_UsuarioEditorId",
                        column: x => x.UsuarioEditorId,
                        principalSchema: "Finanzas",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumento",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoTipoDocumento = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DescripcionTipoDocumento = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoDocumento_Usuario_UsuarioCreadorId",
                        column: x => x.UsuarioCreadorId,
                        principalSchema: "Finanzas",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TipoDocumento_Usuario_UsuarioEditorId",
                        column: x => x.UsuarioEditorId,
                        principalSchema: "Finanzas",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TipoPago",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdTipoPago = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoPago", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoPago_Usuario_UsuarioCreadorId",
                        column: x => x.UsuarioCreadorId,
                        principalSchema: "Finanzas",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TipoPago_Usuario_UsuarioEditorId",
                        column: x => x.UsuarioEditorId,
                        principalSchema: "Finanzas",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Registrodenotacontable",
                schema: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Importe = table.Column<long>(type: "bigint", nullable: false),
                    TerceroLMId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TerceroAN8Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CuentaContableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotaContableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioEditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrodenotacontable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registrodenotacontable_Cuenta_CuentaContableId",
                        column: x => x.CuentaContableId,
                        principalSchema: "Finanzas",
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrodenotacontable_NotaContable_NotaContableId",
                        column: x => x.NotaContableId,
                        principalSchema: "Finanzas",
                        principalTable: "NotaContable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrodenotacontable_Tercero_TerceroAN8Id",
                        column: x => x.TerceroAN8Id,
                        principalSchema: "Finanzas",
                        principalTable: "Tercero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrodenotacontable_Tercero_TerceroLMId",
                        column: x => x.TerceroLMId,
                        principalSchema: "Finanzas",
                        principalTable: "Tercero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrodenotacontable_Usuario_UsuarioCreadorId",
                        column: x => x.UsuarioCreadorId,
                        principalSchema: "Finanzas",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Registrodenotacontable_Usuario_UsuarioEditorId",
                        column: x => x.UsuarioEditorId,
                        principalSchema: "Finanzas",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                columns: new[] { "Id", "Concepto", "Descripcion", "FechaDeCreacion", "FechaEdicion", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[] { new Guid("6089896c-116a-424d-b308-ab4e2522af32"), "RI", null, new DateTime(2023, 3, 7, 11, 19, 24, 116, DateTimeKind.Local).AddTicks(4333), null, null, null });

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                columns: new[] { "Id", "Activo", "FechaDeCreacion", "FechaEdicion", "Servicio", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[,]
                {
                    { new Guid("48d1127e-2cc7-4098-9eee-9e9a51beb4b6"), true, new DateTime(2023, 3, 7, 11, 19, 24, 116, DateTimeKind.Local).AddTicks(3968), null, "Flujo de Caja", null, null },
                    { new Guid("6704bfdb-46a8-4c1e-b3b7-14b89c788f02"), true, new DateTime(2023, 3, 7, 11, 19, 24, 116, DateTimeKind.Local).AddTicks(3898), null, "Facturas", null, null },
                    { new Guid("78b26555-959a-4eee-94ab-1649666c1650"), true, new DateTime(2023, 3, 7, 11, 19, 24, 116, DateTimeKind.Local).AddTicks(3964), null, "Notas Contables", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppFile_BaseEntityDocumentoId",
                schema: "Finanzas",
                table: "AppFile",
                column: "BaseEntityDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppFile_UsuarioCreadorId",
                schema: "Finanzas",
                table: "AppFile",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppFile_UsuarioEditorId",
                schema: "Finanzas",
                table: "AppFile",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Area_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Area",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Area_UsuarioEditorId",
                schema: "Finanzas",
                table: "Area",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntityDocumento_AnuladorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "AnuladorId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntityDocumento_AprobadorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "AprobadorId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntityDocumento_AutorizadorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "AutorizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntityDocumento_EquipoCreadorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "EquipoCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntityDocumento_RechazadorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "RechazadorId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntityDocumento_ReversorJDId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "ReversorJDId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntityDocumento_UsuarioBotId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "UsuarioBotId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntityDocumento_UsuarioCreadorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntityDocumento_UsuarioEditorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntityDocumento_UsuarioRevisionId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "UsuarioRevisionId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntityDocumento_VerificadorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "VerificadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClasificacionDocumento_UsuarioCreadorId",
                schema: "Finanzas",
                table: "ClasificacionDocumento",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClasificacionDocumento_UsuarioEditorId",
                schema: "Finanzas",
                table: "ClasificacionDocumento",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptoFactura_UsuarioCreadorId",
                schema: "Finanzas",
                table: "ConceptoFactura",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptoFactura_UsuarioEditorId",
                schema: "Finanzas",
                table: "ConceptoFactura",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptoxCuentaContable_CuentaContableCreditoId",
                schema: "Finanzas",
                table: "ConceptoxCuentaContable",
                column: "CuentaContableCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptoxCuentaContable_CuentaContableDebitoId",
                schema: "Finanzas",
                table: "ConceptoxCuentaContable",
                column: "CuentaContableDebitoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptoxCuentaContable_UsuarioCreadorId",
                schema: "Finanzas",
                table: "ConceptoxCuentaContable",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptoxCuentaContable_UsuarioEditorId",
                schema: "Finanzas",
                table: "ConceptoxCuentaContable",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Configuracion",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_UsuarioEditorId",
                schema: "Finanzas",
                table: "Configuracion",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionProcesoNotasContables_ProcesoId",
                schema: "Finanzas",
                table: "ConfiguracionProcesoNotasContables",
                column: "ProcesoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionProcesoNotasContables_UsuarioCreadorId",
                schema: "Finanzas",
                table: "ConfiguracionProcesoNotasContables",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionProcesoNotasContables_UsuarioEditorId",
                schema: "Finanzas",
                table: "ConfiguracionProcesoNotasContables",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionServicios_UsuarioCreadorId",
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionServicios_UsuarioEditorId",
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_CuentaBancariaId",
                schema: "Finanzas",
                table: "Cuenta",
                column: "CuentaBancariaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_EntidadId",
                schema: "Finanzas",
                table: "Cuenta",
                column: "EntidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Cuenta",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_UsuarioEditorId",
                schema: "Finanzas",
                table: "Cuenta",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentasBancariasxFactura_CuentaBancariaId",
                schema: "Finanzas",
                table: "CuentasBancariasxFactura",
                column: "CuentaBancariaId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentasBancariasxFactura_UsuarioCreadorId",
                schema: "Finanzas",
                table: "CuentasBancariasxFactura",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentasBancariasxFactura_UsuarioEditorId",
                schema: "Finanzas",
                table: "CuentasBancariasxFactura",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Entidad_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Entidad",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Entidad_UsuarioEditorId",
                schema: "Finanzas",
                table: "Entidad",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_AreaId",
                schema: "Finanzas",
                table: "Equipo",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Equipo",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_UsuarioEditorId",
                schema: "Finanzas",
                table: "Equipo",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Factura_ConceptoId",
                schema: "Finanzas",
                table: "Factura",
                column: "ConceptoId");

            migrationBuilder.CreateIndex(
                name: "IX_Factura_CuentaBancariaId",
                schema: "Finanzas",
                table: "Factura",
                column: "CuentaBancariaId");

            migrationBuilder.CreateIndex(
                name: "IX_Factura_TerceroId",
                schema: "Finanzas",
                table: "Factura",
                column: "TerceroId");

            migrationBuilder.CreateIndex(
                name: "IX_NotaContable_ClasificacionDocumentoId",
                schema: "Finanzas",
                table: "NotaContable",
                column: "ClasificacionDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_NotaContable_EquipoId",
                schema: "Finanzas",
                table: "NotaContable",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_NotaContable_ProcesoId",
                schema: "Finanzas",
                table: "NotaContable",
                column: "ProcesoId");

            migrationBuilder.CreateIndex(
                name: "IX_NotaContable_TipoDocumentoId",
                schema: "Finanzas",
                table: "NotaContable",
                column: "TipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CuentaBancariaId",
                schema: "Finanzas",
                table: "Pagos",
                column: "CuentaBancariaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Pagos",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_UsuarioEditorId",
                schema: "Finanzas",
                table: "Pagos",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Proceso_AreaId",
                schema: "Finanzas",
                table: "Proceso",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Proceso_EquipoId",
                schema: "Finanzas",
                table: "Proceso",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Proceso_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Proceso",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Proceso_UsuarioEditorId",
                schema: "Finanzas",
                table: "Proceso",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrodenotacontable_CuentaContableId",
                schema: "Finanzas",
                table: "Registrodenotacontable",
                column: "CuentaContableId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrodenotacontable_NotaContableId",
                schema: "Finanzas",
                table: "Registrodenotacontable",
                column: "NotaContableId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrodenotacontable_TerceroAN8Id",
                schema: "Finanzas",
                table: "Registrodenotacontable",
                column: "TerceroAN8Id");

            migrationBuilder.CreateIndex(
                name: "IX_Registrodenotacontable_TerceroLMId",
                schema: "Finanzas",
                table: "Registrodenotacontable",
                column: "TerceroLMId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrodenotacontable_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Registrodenotacontable",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrodenotacontable_UsuarioEditorId",
                schema: "Finanzas",
                table: "Registrodenotacontable",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Saldos_CuentaBancariaId",
                schema: "Finanzas",
                table: "Saldos",
                column: "CuentaBancariaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tercero_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Tercero",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tercero_UsuarioEditorId",
                schema: "Finanzas",
                table: "Tercero",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumento_UsuarioCreadorId",
                schema: "Finanzas",
                table: "TipoDocumento",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumento_UsuarioEditorId",
                schema: "Finanzas",
                table: "TipoDocumento",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoPago_UsuarioCreadorId",
                schema: "Finanzas",
                table: "TipoPago",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoPago_UsuarioEditorId",
                schema: "Finanzas",
                table: "TipoPago",
                column: "UsuarioEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_AreaId",
                schema: "Finanzas",
                table: "Usuario",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_EquipoId",
                schema: "Finanzas",
                table: "Usuario",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_ProcesoId",
                schema: "Finanzas",
                table: "Usuario",
                column: "ProcesoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_UsuarioAsignaProcesoId",
                schema: "Finanzas",
                table: "Usuario",
                column: "UsuarioAsignaProcesoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Usuario",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Usuario",
                column: "UsuarioEditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppFile_BaseEntityDocumento_BaseEntityDocumentoId",
                schema: "Finanzas",
                table: "AppFile",
                column: "BaseEntityDocumentoId",
                principalSchema: "Finanzas",
                principalTable: "BaseEntityDocumento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppFile_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "AppFile",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppFile_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "AppFile",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Area_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Area",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Area_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Area",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntityDocumento_Equipo_EquipoCreadorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "EquipoCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Equipo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntityDocumento_Usuario_AnuladorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "AnuladorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntityDocumento_Usuario_AprobadorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "AprobadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntityDocumento_Usuario_AutorizadorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "AutorizadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntityDocumento_Usuario_RechazadorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "RechazadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntityDocumento_Usuario_ReversorJDId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "ReversorJDId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntityDocumento_Usuario_UsuarioBotId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "UsuarioBotId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntityDocumento_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntityDocumento_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntityDocumento_Usuario_UsuarioRevisionId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "UsuarioRevisionId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntityDocumento_Usuario_VerificadorId",
                schema: "Finanzas",
                table: "BaseEntityDocumento",
                column: "VerificadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClasificacionDocumento_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "ClasificacionDocumento",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClasificacionDocumento_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "ClasificacionDocumento",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConceptoFactura_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "ConceptoFactura",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConceptoFactura_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "ConceptoFactura",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConceptoxCuentaContable_Cuenta_CuentaContableCreditoId",
                schema: "Finanzas",
                table: "ConceptoxCuentaContable",
                column: "CuentaContableCreditoId",
                principalSchema: "Finanzas",
                principalTable: "Cuenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConceptoxCuentaContable_Cuenta_CuentaContableDebitoId",
                schema: "Finanzas",
                table: "ConceptoxCuentaContable",
                column: "CuentaContableDebitoId",
                principalSchema: "Finanzas",
                principalTable: "Cuenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConceptoxCuentaContable_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "ConceptoxCuentaContable",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConceptoxCuentaContable_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "ConceptoxCuentaContable",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Configuracion_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Configuracion",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Configuracion_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Configuracion",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfiguracionProcesoNotasContables_Proceso_ProcesoId",
                schema: "Finanzas",
                table: "ConfiguracionProcesoNotasContables",
                column: "ProcesoId",
                principalSchema: "Finanzas",
                principalTable: "Proceso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfiguracionProcesoNotasContables_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "ConfiguracionProcesoNotasContables",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfiguracionProcesoNotasContables_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "ConfiguracionProcesoNotasContables",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfiguracionServicios_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfiguracionServicios_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_Entidad_EntidadId",
                schema: "Finanzas",
                table: "Cuenta",
                column: "EntidadId",
                principalSchema: "Finanzas",
                principalTable: "Entidad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Cuenta",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Cuenta",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CuentasBancariasxFactura_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "CuentasBancariasxFactura",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CuentasBancariasxFactura_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "CuentasBancariasxFactura",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entidad_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Entidad",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entidad_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Entidad",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Equipo",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Equipo",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Factura_Tercero_TerceroId",
                schema: "Finanzas",
                table: "Factura",
                column: "TerceroId",
                principalSchema: "Finanzas",
                principalTable: "Tercero",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotaContable_Proceso_ProcesoId",
                schema: "Finanzas",
                table: "NotaContable",
                column: "ProcesoId",
                principalSchema: "Finanzas",
                principalTable: "Proceso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotaContable_TipoDocumento_TipoDocumentoId",
                schema: "Finanzas",
                table: "NotaContable",
                column: "TipoDocumentoId",
                principalSchema: "Finanzas",
                principalTable: "TipoDocumento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Pagos",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Pagos",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Proceso_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Proceso",
                column: "UsuarioCreadorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proceso_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Proceso",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Area_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_Area_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Equipo");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Equipo");

            migrationBuilder.DropForeignKey(
                name: "FK_Proceso_Usuario_UsuarioCreadorId",
                schema: "Finanzas",
                table: "Proceso");

            migrationBuilder.DropForeignKey(
                name: "FK_Proceso_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Proceso");

            migrationBuilder.DropTable(
                name: "AppFile",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "ConceptoxCuentaContable",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "Configuracion",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "ConfiguracionProcesoNotasContables",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "ConfiguracionServicios",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "CuentasBancariasxFactura",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "Factura",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "Pagos",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "Registrodenotacontable",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "Saldos",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "TipoPago",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "ConceptoFactura",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "NotaContable",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "Tercero",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "Cuenta",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "BaseEntityDocumento",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "ClasificacionDocumento",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "TipoDocumento",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "Entidad",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "Proceso",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "Equipo",
                schema: "Finanzas");

            migrationBuilder.DropTable(
                name: "Area",
                schema: "Finanzas");
        }
    }
}
