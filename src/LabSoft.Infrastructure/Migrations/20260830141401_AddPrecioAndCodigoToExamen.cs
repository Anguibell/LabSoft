using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPrecioAndCodigoToExamen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Codigo",
                table: "Examenes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Examenes",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Examenes_Codigo",
                table: "Examenes",
                column: "Codigo",
                unique: true,
                filter: "\"Codigo\" > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Examenes_Codigo",
                table: "Examenes");

            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Examenes");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Examenes");
        }
    }
}
