using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class addImgenCarritoItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductoId",
                table: "CarritoItem",
                newName: "productoId");

            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "CarritoItem",
                newName: "precio");

            migrationBuilder.RenameColumn(
                name: "NombreProducto",
                table: "CarritoItem",
                newName: "nombreProducto");

            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "CarritoItem",
                newName: "cantidad");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CarritoItem",
                newName: "id");

            migrationBuilder.AddColumn<string>(
                name: "imagen",
                table: "CarritoItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imagen",
                table: "CarritoItem");

            migrationBuilder.RenameColumn(
                name: "productoId",
                table: "CarritoItem",
                newName: "ProductoId");

            migrationBuilder.RenameColumn(
                name: "precio",
                table: "CarritoItem",
                newName: "Precio");

            migrationBuilder.RenameColumn(
                name: "nombreProducto",
                table: "CarritoItem",
                newName: "NombreProducto");

            migrationBuilder.RenameColumn(
                name: "cantidad",
                table: "CarritoItem",
                newName: "Cantidad");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CarritoItem",
                newName: "Id");
        }
    }
}
