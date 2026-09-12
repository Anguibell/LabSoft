using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarModuloPacientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Pacientes");

            migrationBuilder.RenameColumn(
                name: "Observaciones",
                table: "Pacientes",
                newName: "TelefonoAlternativo");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Pacientes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Cedula",
                table: "Pacientes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Pacientes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorreoAlternativo",
                table: "Pacientes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Domicilio",
                table: "Pacientes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsRecienNacido",
                table: "Pacientes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NotaMedica",
                table: "Pacientes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroAfiliado",
                table: "Pacientes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Parentesco",
                table: "Pacientes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TitularId",
                table: "Pacientes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_TitularId",
                table: "Pacientes",
                column: "TitularId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Pacientes_TitularId",
                table: "Pacientes",
                column: "TitularId",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Pacientes_TitularId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_TitularId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "CorreoAlternativo",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Domicilio",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "EsRecienNacido",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "NotaMedica",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "NumeroAfiliado",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Parentesco",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "TitularId",
                table: "Pacientes");

            migrationBuilder.RenameColumn(
                name: "TelefonoAlternativo",
                table: "Pacientes",
                newName: "Observaciones");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Pacientes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cedula",
                table: "Pacientes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Pacientes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
