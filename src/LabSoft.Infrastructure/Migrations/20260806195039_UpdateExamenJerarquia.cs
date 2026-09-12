using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExamenJerarquia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsOpcionalDinamico",
                table: "Examenes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ExamenPadreId",
                table: "Examenes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Examenes_ExamenPadreId",
                table: "Examenes",
                column: "ExamenPadreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Examenes_Examenes_ExamenPadreId",
                table: "Examenes",
                column: "ExamenPadreId",
                principalTable: "Examenes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examenes_Examenes_ExamenPadreId",
                table: "Examenes");

            migrationBuilder.DropIndex(
                name: "IX_Examenes_ExamenPadreId",
                table: "Examenes");

            migrationBuilder.DropColumn(
                name: "EsOpcionalDinamico",
                table: "Examenes");

            migrationBuilder.DropColumn(
                name: "ExamenPadreId",
                table: "Examenes");
        }
    }
}
