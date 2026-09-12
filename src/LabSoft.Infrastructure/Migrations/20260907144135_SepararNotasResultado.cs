using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SepararNotasResultado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Observaciones",
                table: "Resultados",
                newName: "NotaInterna");

            migrationBuilder.AddColumn<bool>(
                name: "ImprimirObservacion",
                table: "Resultados",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NotaClinica",
                table: "Resultados",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImprimirObservacion",
                table: "Resultados");

            migrationBuilder.DropColumn(
                name: "NotaClinica",
                table: "Resultados");

            migrationBuilder.RenameColumn(
                name: "NotaInterna",
                table: "Resultados",
                newName: "Observaciones");
        }
    }
}
