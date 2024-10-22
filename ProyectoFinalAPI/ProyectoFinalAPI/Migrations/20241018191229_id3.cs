using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class id3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MateriaPrima_Proovedor_ProveedoridProveedor",
                table: "MateriaPrima");

            migrationBuilder.AlterColumn<int>(
                name: "ProveedoridProveedor",
                table: "MateriaPrima",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "idProveedor",
                table: "MateriaPrima",
                type: "int",
                nullable: true);

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

            migrationBuilder.DropColumn(
                name: "idProveedor",
                table: "MateriaPrima");

            migrationBuilder.AlterColumn<int>(
                name: "ProveedoridProveedor",
                table: "MateriaPrima",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MateriaPrima_Proovedor_ProveedoridProveedor",
                table: "MateriaPrima",
                column: "ProveedoridProveedor",
                principalTable: "Proovedor",
                principalColumn: "idProveedor",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
