using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agenda.Migrations
{
    /// <inheritdoc />
    public partial class AgregarIdAdministradorOdontologo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdministradorId",
                table: "Odontologos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Odontologos_AdministradorId",
                table: "Odontologos",
                column: "AdministradorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Odontologos_Usuarios_AdministradorId",
                table: "Odontologos",
                column: "AdministradorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Odontologos_Usuarios_AdministradorId",
                table: "Odontologos");

            migrationBuilder.DropIndex(
                name: "IX_Odontologos_AdministradorId",
                table: "Odontologos");

            migrationBuilder.DropColumn(
                name: "AdministradorId",
                table: "Odontologos");
        }
    }
}
