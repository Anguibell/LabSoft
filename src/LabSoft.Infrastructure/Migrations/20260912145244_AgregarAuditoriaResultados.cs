using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAuditoriaResultados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsModificado",
                table: "Resultados",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ProcesadoPorId",
                table: "Resultados",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ValidadoPorId",
                table: "Resultados",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsModificado",
                table: "Resultados");

            migrationBuilder.DropColumn(
                name: "ProcesadoPorId",
                table: "Resultados");

            migrationBuilder.DropColumn(
                name: "ValidadoPorId",
                table: "Resultados");
        }
    }
}
