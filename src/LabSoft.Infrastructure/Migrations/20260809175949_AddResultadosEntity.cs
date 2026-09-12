using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddResultadosEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Resultados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrdenDetalleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValorResultado = table.Column<string>(type: "text", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    FechaProcesamiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resultados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resultados_OrdenDetalles_OrdenDetalleId",
                        column: x => x.OrdenDetalleId,
                        principalTable: "OrdenDetalles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resultados_OrdenDetalleId",
                table: "Resultados",
                column: "OrdenDetalleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resultados");
        }
    }
}
