using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionResultadosColeccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Resultados_OrdenDetalleId",
                table: "Resultados");

            migrationBuilder.AddColumn<Guid>(
                name: "ExamenId",
                table: "Resultados",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Resultados_ExamenId",
                table: "Resultados",
                column: "ExamenId");

            migrationBuilder.CreateIndex(
                name: "IX_Resultados_OrdenDetalleId",
                table: "Resultados",
                column: "OrdenDetalleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resultados_Examenes_ExamenId",
                table: "Resultados",
                column: "ExamenId",
                principalTable: "Examenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resultados_Examenes_ExamenId",
                table: "Resultados");

            migrationBuilder.DropIndex(
                name: "IX_Resultados_ExamenId",
                table: "Resultados");

            migrationBuilder.DropIndex(
                name: "IX_Resultados_OrdenDetalleId",
                table: "Resultados");

            migrationBuilder.DropColumn(
                name: "ExamenId",
                table: "Resultados");

            migrationBuilder.CreateIndex(
                name: "IX_Resultados_OrdenDetalleId",
                table: "Resultados",
                column: "OrdenDetalleId",
                unique: true);
        }
    }
}
