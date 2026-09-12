using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ValoresReferenciaPorSexo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ValoresReferencia",
                table: "Examenes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ValoresReferenciaF",
                table: "Examenes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValoresReferenciaM",
                table: "Examenes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValoresReferenciaF",
                table: "Examenes");

            migrationBuilder.DropColumn(
                name: "ValoresReferenciaM",
                table: "Examenes");

            migrationBuilder.AlterColumn<string>(
                name: "ValoresReferencia",
                table: "Examenes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
