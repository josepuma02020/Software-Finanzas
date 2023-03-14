using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
