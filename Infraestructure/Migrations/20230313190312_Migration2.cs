using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Usuario_UsuarioAsignaProcesoId",
                schema: "Finanzas",
                table: "Usuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_UsuarioAsignaProcesoId",
                schema: "Finanzas",
                table: "Usuario");

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConceptoFactura",
                keyColumn: "Id",
                keyValue: new Guid("6089896c-116a-424d-b308-ab4e2522af32"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("48d1127e-2cc7-4098-9eee-9e9a51beb4b6"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("6704bfdb-46a8-4c1e-b3b7-14b89c788f02"));

            migrationBuilder.DeleteData(
                schema: "Finanzas",
                table: "ConfiguracionServicios",
                keyColumn: "Id",
                keyValue: new Guid("78b26555-959a-4eee-94ab-1649666c1650"));

            migrationBuilder.DropColumn(
                name: "UsuarioAsignaProcesoId",
                schema: "Finanzas",
                table: "Usuario");

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                schema: "Finanzas",
                table: "Usuario",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_UsuarioId",
                schema: "Finanzas",
                table: "Usuario",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Usuario",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Usuario_UsuarioId",
                schema: "Finanzas",
                table: "Usuario",
                column: "UsuarioId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Usuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Usuario_UsuarioId",
                schema: "Finanzas",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_UsuarioId",
                schema: "Finanzas",
                table: "Usuario");

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

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                schema: "Finanzas",
                table: "Usuario");

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioAsignaProcesoId",
                schema: "Finanzas",
                table: "Usuario",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                name: "IX_Usuario_UsuarioAsignaProcesoId",
                schema: "Finanzas",
                table: "Usuario",
                column: "UsuarioAsignaProcesoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Usuario_UsuarioAsignaProcesoId",
                schema: "Finanzas",
                table: "Usuario",
                column: "UsuarioAsignaProcesoId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Usuario_UsuarioEditorId",
                schema: "Finanzas",
                table: "Usuario",
                column: "UsuarioEditorId",
                principalSchema: "Finanzas",
                principalTable: "Usuario",
                principalColumn: "Id");
        }
    }
}
