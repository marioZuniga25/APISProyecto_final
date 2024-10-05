using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class octubre2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MateriaPrima_idInventario",
                table: "MateriaPrima",
                column: "idInventario");

            migrationBuilder.AddForeignKey(
                name: "FK_MateriaPrima_Inventario_idInventario",
                table: "MateriaPrima",
                column: "idInventario",
                principalTable: "Inventario",
                principalColumn: "idInventario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MateriaPrima_Inventario_idInventario",
                table: "MateriaPrima");

            migrationBuilder.DropIndex(
                name: "IX_MateriaPrima_idInventario",
                table: "MateriaPrima");
        }
    }
}
