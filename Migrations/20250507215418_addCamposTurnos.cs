using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agenda.Migrations
{
    /// <inheritdoc />
    public partial class addCamposTurnos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Consultorio",
                table: "Turnos");

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "Turnos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Confirmado",
                table: "Turnos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmado",
                table: "Turnos");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Turnos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Consultorio",
                table: "Turnos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
