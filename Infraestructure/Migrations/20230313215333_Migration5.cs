using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "ConfiguracionFacturaId",
                schema: "Finanzas",
                table: "Cuenta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ConfiguracionFacturaId1",
                schema: "Finanzas",
                table: "Cuenta",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_ConfiguracionFacturaId1",
                schema: "Finanzas",
                table: "Cuenta",
                column: "ConfiguracionFacturaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_CuentasBancariasxFactura_ConfiguracionFacturaId1",
                schema: "Finanzas",
                table: "Cuenta",
                column: "ConfiguracionFacturaId1",
                principalSchema: "Finanzas",
                principalTable: "CuentasBancariasxFactura",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuenta_CuentasBancariasxFactura_ConfiguracionFacturaId1",
                schema: "Finanzas",
                table: "Cuenta");

            migrationBuilder.DropIndex(
                name: "IX_Cuenta_ConfiguracionFacturaId1",
                schema: "Finanzas",
                table: "Cuenta");

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

            migrationBuilder.DropColumn(
                name: "ConfiguracionFacturaId",
                schema: "Finanzas",
                table: "Cuenta");

            migrationBuilder.DropColumn(
                name: "ConfiguracionFacturaId1",
                schema: "Finanzas",
                table: "Cuenta");

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
    }
}
