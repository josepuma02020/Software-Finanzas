using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                keyColumn: "Id",
                keyValue: new Guid("847bf73a-56d4-4c67-a5cb-209a54c90a9d"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("11a97d55-e1e1-4dc8-a931-9d31c77b98d1"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("38dba2d9-223b-4e0a-b61b-1b8d2c2b80f4"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("cd8a0338-3c52-4910-b1ab-ecafd781d717"));

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                columns: new[] { "Id", "Concepto", "Descripcion", "FechaDeCreacion", "FechaEdicion", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[] { new Guid("f98c296a-a3b3-4a97-abca-1f649f0dd705"), "RI", null, new DateTime(2023, 3, 13, 17, 27, 23, 140, DateTimeKind.Local).AddTicks(1687), null, null, null });

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                columns: new[] { "Id", "Activo", "FechaDeCreacion", "FechaEdicion", "Servicio", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[,]
                {
                    { new Guid("0f79c215-5dfd-423f-9d67-1814183bef7c"), true, new DateTime(2023, 3, 13, 17, 27, 23, 140, DateTimeKind.Local).AddTicks(1436), null, "Notas Contables", null, null },
                    { new Guid("6d69a0e9-ef70-4634-bec0-1e9f7a405630"), true, new DateTime(2023, 3, 13, 17, 27, 23, 140, DateTimeKind.Local).AddTicks(1387), null, "Facturas", null, null },
                    { new Guid("d21230b4-a402-47e2-aabb-0369f5719baa"), true, new DateTime(2023, 3, 13, 17, 27, 23, 140, DateTimeKind.Local).AddTicks(1438), null, "Flujo de Caja", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                keyColumn: "Id",
                keyValue: new Guid("f98c296a-a3b3-4a97-abca-1f649f0dd705"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("0f79c215-5dfd-423f-9d67-1814183bef7c"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("6d69a0e9-ef70-4634-bec0-1e9f7a405630"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("d21230b4-a402-47e2-aabb-0369f5719baa"));

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                columns: new[] { "Id", "Concepto", "Descripcion", "FechaDeCreacion", "FechaEdicion", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[] { new Guid("847bf73a-56d4-4c67-a5cb-209a54c90a9d"), "RI", null, new DateTime(2023, 3, 13, 16, 53, 31, 517, DateTimeKind.Local).AddTicks(7831), null, null, null });

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                columns: new[] { "Id", "Activo", "FechaDeCreacion", "FechaEdicion", "Servicio", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[,]
                {
                    { new Guid("11a97d55-e1e1-4dc8-a931-9d31c77b98d1"), true, new DateTime(2023, 3, 13, 16, 53, 31, 517, DateTimeKind.Local).AddTicks(7463), null, "Flujo de Caja", null, null },
                    { new Guid("38dba2d9-223b-4e0a-b61b-1b8d2c2b80f4"), true, new DateTime(2023, 3, 13, 16, 53, 31, 517, DateTimeKind.Local).AddTicks(7458), null, "Notas Contables", null, null },
                    { new Guid("cd8a0338-3c52-4910-b1ab-ecafd781d717"), true, new DateTime(2023, 3, 13, 16, 53, 31, 517, DateTimeKind.Local).AddTicks(7400), null, "Facturas", null, null }
                });
        }
    }
}
