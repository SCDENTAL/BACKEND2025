using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agenda.Migrations
{
    /// <inheritdoc />
    public partial class calendario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Pacientes_PacienteId",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "FechaHora",
                table: "Turnos");

            migrationBuilder.RenameColumn(
                name: "PacienteId",
                table: "Turnos",
                newName: "IdPaciente");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Turnos",
                newName: "IdCalendario");

            migrationBuilder.RenameColumn(
                name: "Confirmado",
                table: "Turnos",
                newName: "Disponible");

            migrationBuilder.RenameIndex(
                name: "IX_Turnos_PacienteId",
                table: "Turnos",
                newName: "IX_Turnos_IdPaciente");

            migrationBuilder.AlterColumn<int>(
                name: "OdontologoId",
                table: "Turnos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Asistio",
                table: "Turnos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "Turnos",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Horario",
                table: "Turnos",
                type: "TIME(0)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "IdObraSocial",
                table: "Turnos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObraSocialId",
                table: "Turnos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Turnos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Calendarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicioTurnos = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFinTurnos = table.Column<TimeSpan>(type: "time", nullable: false),
                    IntervaloTurnos = table.Column<TimeSpan>(type: "time", nullable: false),
                    CantidadHorarios = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calendarios_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdCalendario",
                table: "Turnos",
                column: "IdCalendario");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_ObraSocialId",
                table: "Turnos",
                column: "ObraSocialId");

            migrationBuilder.CreateIndex(
                name: "IX_Calendarios_IdUsuario",
                table: "Calendarios",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Calendarios_IdCalendario",
                table: "Turnos",
                column: "IdCalendario",
                principalTable: "Calendarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_ObrasSociales_ObraSocialId",
                table: "Turnos",
                column: "ObraSocialId",
                principalTable: "ObrasSociales",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Pacientes_IdPaciente",
                table: "Turnos",
                column: "IdPaciente",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Calendarios_IdCalendario",
                table: "Turnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_ObrasSociales_ObraSocialId",
                table: "Turnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Pacientes_IdPaciente",
                table: "Turnos");

            migrationBuilder.DropTable(
                name: "Calendarios");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_IdCalendario",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_ObraSocialId",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "Asistio",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "Horario",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "IdObraSocial",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "ObraSocialId",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Turnos");

            migrationBuilder.RenameColumn(
                name: "IdPaciente",
                table: "Turnos",
                newName: "PacienteId");

            migrationBuilder.RenameColumn(
                name: "IdCalendario",
                table: "Turnos",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "Disponible",
                table: "Turnos",
                newName: "Confirmado");

            migrationBuilder.RenameIndex(
                name: "IX_Turnos_IdPaciente",
                table: "Turnos",
                newName: "IX_Turnos_PacienteId");

            migrationBuilder.AlterColumn<int>(
                name: "OdontologoId",
                table: "Turnos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHora",
                table: "Turnos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Pacientes_PacienteId",
                table: "Turnos",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
