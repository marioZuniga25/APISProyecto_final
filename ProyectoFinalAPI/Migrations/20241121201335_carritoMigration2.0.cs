using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class carritoMigration20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "precioUnitario",
                table: "DetalleCarrito",
                newName: "PrecioUnitario");

            migrationBuilder.RenameColumn(
                name: "idProducto",
                table: "DetalleCarrito",
                newName: "IdProducto");

            migrationBuilder.RenameColumn(
                name: "idCarrito",
                table: "DetalleCarrito",
                newName: "IdCarrito");

            migrationBuilder.RenameColumn(
                name: "cantidad",
                table: "DetalleCarrito",
                newName: "Cantidad");

            migrationBuilder.RenameColumn(
                name: "idDetalleCarrito",
                table: "DetalleCarrito",
                newName: "IdDetalleCarrito");

            migrationBuilder.RenameColumn(
                name: "total",
                table: "Carrito",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "idUsuario",
                table: "Carrito",
                newName: "IdUsuario");

            migrationBuilder.RenameColumn(
                name: "fechaAgregado",
                table: "Carrito",
                newName: "FechaAgregado");

            migrationBuilder.RenameColumn(
                name: "idCarrito",
                table: "Carrito",
                newName: "IdCarrito");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCarrito_IdCarrito",
                table: "DetalleCarrito",
                column: "IdCarrito");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleCarrito_Carrito_IdCarrito",
                table: "DetalleCarrito",
                column: "IdCarrito",
                principalTable: "Carrito",
                principalColumn: "IdCarrito",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleCarrito_Carrito_IdCarrito",
                table: "DetalleCarrito");

            migrationBuilder.DropIndex(
                name: "IX_DetalleCarrito_IdCarrito",
                table: "DetalleCarrito");

            migrationBuilder.RenameColumn(
                name: "PrecioUnitario",
                table: "DetalleCarrito",
                newName: "precioUnitario");

            migrationBuilder.RenameColumn(
                name: "IdProducto",
                table: "DetalleCarrito",
                newName: "idProducto");

            migrationBuilder.RenameColumn(
                name: "IdCarrito",
                table: "DetalleCarrito",
                newName: "idCarrito");

            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "DetalleCarrito",
                newName: "cantidad");

            migrationBuilder.RenameColumn(
                name: "IdDetalleCarrito",
                table: "DetalleCarrito",
                newName: "idDetalleCarrito");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Carrito",
                newName: "total");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "Carrito",
                newName: "idUsuario");

            migrationBuilder.RenameColumn(
                name: "FechaAgregado",
                table: "Carrito",
                newName: "fechaAgregado");

            migrationBuilder.RenameColumn(
                name: "IdCarrito",
                table: "Carrito",
                newName: "idCarrito");
        }
    }
}
