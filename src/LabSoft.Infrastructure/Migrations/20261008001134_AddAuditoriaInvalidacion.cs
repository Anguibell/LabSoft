using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditoriaInvalidacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInvalidacion",
                table: "Resultados",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InvalidadoPorId",
                table: "Resultados",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoInvalidacion",
                table: "Resultados",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaInvalidacion",
                table: "Resultados");

            migrationBuilder.DropColumn(
                name: "InvalidadoPorId",
                table: "Resultados");

            migrationBuilder.DropColumn(
                name: "MotivoInvalidacion",
                table: "Resultados");
        }
    }
}
