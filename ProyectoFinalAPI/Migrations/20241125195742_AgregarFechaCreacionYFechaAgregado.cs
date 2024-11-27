using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFechaCreacionYFechaAgregado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleCarrito_Carrito_IdCarrito",
                table: "DetalleCarrito");

            migrationBuilder.DropIndex(
                name: "IX_DetalleCarrito_IdCarrito",
                table: "DetalleCarrito");

            migrationBuilder.RenameColumn(
                name: "FechaAgregado",
                table: "Carrito",
                newName: "FechaCreacion");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAgregado",
                table: "DetalleCarrito",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaAgregado",
                table: "DetalleCarrito");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Carrito",
                newName: "FechaAgregado");

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
    }
}
