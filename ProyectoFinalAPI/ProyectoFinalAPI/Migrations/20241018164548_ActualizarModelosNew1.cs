using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarModelosNew1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MateriaPrima_Proovedor_idProveedor",
                table: "MateriaPrima");

            migrationBuilder.DropIndex(
                name: "IX_MateriaPrima_idProveedor",
                table: "MateriaPrima");

            migrationBuilder.AddColumn<int>(
                name: "ProveedoridProveedor",
                table: "MateriaPrima",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MateriaPrima_ProveedoridProveedor",
                table: "MateriaPrima",
                column: "ProveedoridProveedor");

            migrationBuilder.AddForeignKey(
                name: "FK_MateriaPrima_Proovedor_ProveedoridProveedor",
                table: "MateriaPrima",
                column: "ProveedoridProveedor",
                principalTable: "Proovedor",
                principalColumn: "idProveedor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MateriaPrima_Proovedor_ProveedoridProveedor",
                table: "MateriaPrima");

            migrationBuilder.DropIndex(
                name: "IX_MateriaPrima_ProveedoridProveedor",
                table: "MateriaPrima");

            migrationBuilder.DropColumn(
                name: "ProveedoridProveedor",
                table: "MateriaPrima");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaPrima_idProveedor",
                table: "MateriaPrima",
                column: "idProveedor");

            migrationBuilder.AddForeignKey(
                name: "FK_MateriaPrima_Proovedor_idProveedor",
                table: "MateriaPrima",
                column: "idProveedor",
                principalTable: "Proovedor",
                principalColumn: "idProveedor",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
