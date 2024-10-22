using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarModelosNew2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MateriaPrima_UnidadMedida_idUnidad",
                table: "MateriaPrima");

            migrationBuilder.DropIndex(
                name: "IX_MateriaPrima_idUnidad",
                table: "MateriaPrima");

            migrationBuilder.DropColumn(
                name: "idProveedor",
                table: "MateriaPrima");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "idProveedor",
                table: "MateriaPrima",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MateriaPrima_idUnidad",
                table: "MateriaPrima",
                column: "idUnidad");

            migrationBuilder.AddForeignKey(
                name: "FK_MateriaPrima_UnidadMedida_idUnidad",
                table: "MateriaPrima",
                column: "idUnidad",
                principalTable: "UnidadMedida",
                principalColumn: "idUnidad",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
