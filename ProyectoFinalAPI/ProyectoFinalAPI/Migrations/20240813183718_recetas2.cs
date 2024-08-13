using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class recetas2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecetaDetalle_MateriaPrima_MateriaPrimaidMateriaPrima",
                table: "RecetaDetalle");

            migrationBuilder.DropForeignKey(
                name: "FK_RecetaDetalle_Recetas_RecetaidReceta",
                table: "RecetaDetalle");

            migrationBuilder.DropForeignKey(
                name: "FK_Recetas_Producto_ProductoidProducto",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_Recetas_ProductoidProducto",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_RecetaDetalle_MateriaPrimaidMateriaPrima",
                table: "RecetaDetalle");

            migrationBuilder.DropIndex(
                name: "IX_RecetaDetalle_RecetaidReceta",
                table: "RecetaDetalle");

            migrationBuilder.DropColumn(
                name: "ProductoidProducto",
                table: "Recetas");

            migrationBuilder.DropColumn(
                name: "MateriaPrimaidMateriaPrima",
                table: "RecetaDetalle");

            migrationBuilder.DropColumn(
                name: "RecetaidReceta",
                table: "RecetaDetalle");

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_idProducto",
                table: "Recetas",
                column: "idProducto");

            migrationBuilder.CreateIndex(
                name: "IX_RecetaDetalle_idMateriaPrima",
                table: "RecetaDetalle",
                column: "idMateriaPrima");

            migrationBuilder.CreateIndex(
                name: "IX_RecetaDetalle_idReceta",
                table: "RecetaDetalle",
                column: "idReceta");

            migrationBuilder.AddForeignKey(
                name: "FK_RecetaDetalle_MateriaPrima_idMateriaPrima",
                table: "RecetaDetalle",
                column: "idMateriaPrima",
                principalTable: "MateriaPrima",
                principalColumn: "idMateriaPrima",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecetaDetalle_Recetas_idReceta",
                table: "RecetaDetalle",
                column: "idReceta",
                principalTable: "Recetas",
                principalColumn: "idReceta",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recetas_Producto_idProducto",
                table: "Recetas",
                column: "idProducto",
                principalTable: "Producto",
                principalColumn: "idProducto",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecetaDetalle_MateriaPrima_idMateriaPrima",
                table: "RecetaDetalle");

            migrationBuilder.DropForeignKey(
                name: "FK_RecetaDetalle_Recetas_idReceta",
                table: "RecetaDetalle");

            migrationBuilder.DropForeignKey(
                name: "FK_Recetas_Producto_idProducto",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_Recetas_idProducto",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_RecetaDetalle_idMateriaPrima",
                table: "RecetaDetalle");

            migrationBuilder.DropIndex(
                name: "IX_RecetaDetalle_idReceta",
                table: "RecetaDetalle");

            migrationBuilder.AddColumn<int>(
                name: "ProductoidProducto",
                table: "Recetas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MateriaPrimaidMateriaPrima",
                table: "RecetaDetalle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecetaidReceta",
                table: "RecetaDetalle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_ProductoidProducto",
                table: "Recetas",
                column: "ProductoidProducto");

            migrationBuilder.CreateIndex(
                name: "IX_RecetaDetalle_MateriaPrimaidMateriaPrima",
                table: "RecetaDetalle",
                column: "MateriaPrimaidMateriaPrima");

            migrationBuilder.CreateIndex(
                name: "IX_RecetaDetalle_RecetaidReceta",
                table: "RecetaDetalle",
                column: "RecetaidReceta");

            migrationBuilder.AddForeignKey(
                name: "FK_RecetaDetalle_MateriaPrima_MateriaPrimaidMateriaPrima",
                table: "RecetaDetalle",
                column: "MateriaPrimaidMateriaPrima",
                principalTable: "MateriaPrima",
                principalColumn: "idMateriaPrima",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecetaDetalle_Recetas_RecetaidReceta",
                table: "RecetaDetalle",
                column: "RecetaidReceta",
                principalTable: "Recetas",
                principalColumn: "idReceta",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recetas_Producto_ProductoidProducto",
                table: "Recetas",
                column: "ProductoidProducto",
                principalTable: "Producto",
                principalColumn: "idProducto",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
