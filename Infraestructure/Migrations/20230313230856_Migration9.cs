using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                keyColumn: "Id",
                keyValue: new Guid("9c82e138-b525-4863-929a-237dda0d5d14"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("107ebe1c-00cb-4690-9072-e2357cbf802c"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("ae2eed41-6f18-4466-87c6-7d3a3f7a8d1a"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("e9dfd2bb-01fd-489b-b5f6-b296a41f0831"));

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                columns: new[] { "Id", "Concepto", "Descripcion", "FechaDeCreacion", "FechaEdicion", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[] { new Guid("58984691-69b3-407b-8e02-75d9b2e6e972"), "RI", null, new DateTime(2023, 3, 13, 18, 8, 55, 118, DateTimeKind.Local).AddTicks(1197), null, null, null });

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                columns: new[] { "Id", "Activo", "FechaDeCreacion", "FechaEdicion", "Servicio", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[,]
                {
                    { new Guid("1ca5c6df-fe7e-48d0-8d38-546db35cb248"), true, new DateTime(2023, 3, 13, 18, 8, 55, 118, DateTimeKind.Local).AddTicks(791), null, "Facturas", null, null },
                    { new Guid("3a9f297b-8923-4811-9115-42fa12883df3"), true, new DateTime(2023, 3, 13, 18, 8, 55, 118, DateTimeKind.Local).AddTicks(861), null, "Flujo de Caja", null, null },
                    { new Guid("b97da836-ca9e-4e0f-a739-16a47591dc4b"), true, new DateTime(2023, 3, 13, 18, 8, 55, 118, DateTimeKind.Local).AddTicks(858), null, "Notas Contables", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                keyColumn: "Id",
                keyValue: new Guid("58984691-69b3-407b-8e02-75d9b2e6e972"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("1ca5c6df-fe7e-48d0-8d38-546db35cb248"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("3a9f297b-8923-4811-9115-42fa12883df3"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("b97da836-ca9e-4e0f-a739-16a47591dc4b"));

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                columns: new[] { "Id", "Concepto", "Descripcion", "FechaDeCreacion", "FechaEdicion", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[] { new Guid("9c82e138-b525-4863-929a-237dda0d5d14"), "RI", null, new DateTime(2023, 3, 13, 17, 27, 49, 787, DateTimeKind.Local).AddTicks(3267), null, null, null });

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                columns: new[] { "Id", "Activo", "FechaDeCreacion", "FechaEdicion", "Servicio", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[,]
                {
                    { new Guid("107ebe1c-00cb-4690-9072-e2357cbf802c"), true, new DateTime(2023, 3, 13, 17, 27, 49, 787, DateTimeKind.Local).AddTicks(3036), null, "Facturas", null, null },
                    { new Guid("ae2eed41-6f18-4466-87c6-7d3a3f7a8d1a"), true, new DateTime(2023, 3, 13, 17, 27, 49, 787, DateTimeKind.Local).AddTicks(3091), null, "Notas Contables", null, null },
                    { new Guid("e9dfd2bb-01fd-489b-b5f6-b296a41f0831"), true, new DateTime(2023, 3, 13, 17, 27, 49, 787, DateTimeKind.Local).AddTicks(3094), null, "Flujo de Caja", null, null }
                });
        }
    }
}
