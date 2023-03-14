using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                keyColumn: "Id",
                keyValue: new Guid("6495ac34-c2e1-42cb-b342-e3645b77ffc4"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("3079367e-ca28-4cde-af2e-5be882d1eed4"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("e2c3c32d-faaa-417d-91cc-beea24193838"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("f1771e65-5a1f-4f2a-ba3e-6b36ae11cbe6"));

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                columns: new[] { "Id", "Concepto", "Descripcion", "FechaDeCreacion", "FechaEdicion", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[] { new Guid("f190b3f5-dfad-4b63-8856-13996939437e"), "RI", null, new DateTime(2023, 3, 13, 16, 30, 13, 821, DateTimeKind.Local).AddTicks(9650), null, null, null });

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                columns: new[] { "Id", "Activo", "FechaDeCreacion", "FechaEdicion", "Servicio", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[,]
                {
                    { new Guid("1b70d3d4-e56e-409b-b4b1-bc65db57dca7"), true, new DateTime(2023, 3, 13, 16, 30, 13, 821, DateTimeKind.Local).AddTicks(9388), null, "Notas Contables", null, null },
                    { new Guid("7c18fa5e-0eb1-4920-9b32-73fa27dd7915"), true, new DateTime(2023, 3, 13, 16, 30, 13, 821, DateTimeKind.Local).AddTicks(9391), null, "Flujo de Caja", null, null },
                    { new Guid("bb4f80cd-8497-4b57-96e5-5732330a61b4"), true, new DateTime(2023, 3, 13, 16, 30, 13, 821, DateTimeKind.Local).AddTicks(9325), null, "Facturas", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                keyColumn: "Id",
                keyValue: new Guid("f190b3f5-dfad-4b63-8856-13996939437e"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("1b70d3d4-e56e-409b-b4b1-bc65db57dca7"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("7c18fa5e-0eb1-4920-9b32-73fa27dd7915"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("bb4f80cd-8497-4b57-96e5-5732330a61b4"));

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                columns: new[] { "Id", "Concepto", "Descripcion", "FechaDeCreacion", "FechaEdicion", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[] { new Guid("6495ac34-c2e1-42cb-b342-e3645b77ffc4"), "RI", null, new DateTime(2023, 3, 13, 14, 3, 11, 663, DateTimeKind.Local).AddTicks(1286), null, null, null });

            migrationBuilder.InsertData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                columns: new[] { "Id", "Activo", "FechaDeCreacion", "FechaEdicion", "Servicio", "UsuarioCreadorId", "UsuarioEditorId" },
                values: new object[,]
                {
                    { new Guid("3079367e-ca28-4cde-af2e-5be882d1eed4"), true, new DateTime(2023, 3, 13, 14, 3, 11, 663, DateTimeKind.Local).AddTicks(1085), null, "Flujo de Caja", null, null },
                    { new Guid("e2c3c32d-faaa-417d-91cc-beea24193838"), true, new DateTime(2023, 3, 13, 14, 3, 11, 663, DateTimeKind.Local).AddTicks(1083), null, "Notas Contables", null, null },
                    { new Guid("f1771e65-5a1f-4f2a-ba3e-6b36ae11cbe6"), true, new DateTime(2023, 3, 13, 14, 3, 11, 663, DateTimeKind.Local).AddTicks(1035), null, "Facturas", null, null }
                });
        }
    }
}
