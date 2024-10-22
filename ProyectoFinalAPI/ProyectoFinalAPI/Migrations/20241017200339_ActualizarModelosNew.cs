using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarModelosNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MateriaPrima_Inventario_InventarioidInventario",
                table: "MateriaPrima");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropIndex(
                name: "IX_MateriaPrima_InventarioidInventario",
                table: "MateriaPrima");

            migrationBuilder.DropColumn(
                name: "InventarioidInventario",
                table: "MateriaPrima");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventarioidInventario",
                table: "MateriaPrima",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    idInventario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidad = table.Column<double>(type: "float", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => x.idInventario);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MateriaPrima_InventarioidInventario",
                table: "MateriaPrima",
                column: "InventarioidInventario");

            migrationBuilder.AddForeignKey(
                name: "FK_MateriaPrima_Inventario_InventarioidInventario",
                table: "MateriaPrima",
                column: "InventarioidInventario",
                principalTable: "Inventario",
                principalColumn: "idInventario");
        }
    }
}
