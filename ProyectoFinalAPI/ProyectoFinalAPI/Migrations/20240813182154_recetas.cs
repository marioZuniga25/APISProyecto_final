﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class recetas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarritoItem",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productoId = table.Column<int>(type: "int", nullable: false),
                    nombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    imagen = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarritoItem", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    idCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreCategoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.idCategoria);
                });

            migrationBuilder.CreateTable(
                name: "DetalleVenta",
                columns: table => new
                {
                    idDetalleVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idVenta = table.Column<int>(type: "int", nullable: false),
                    idProducto = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precioUnitario = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleVenta", x => x.idDetalleVenta);
                });

            migrationBuilder.CreateTable(
                name: "instructivoProductos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idProducto = table.Column<int>(type: "int", nullable: false),
                    idMateriaPrima = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instructivoProductos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    idInventario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => x.idInventario);
                });

            migrationBuilder.CreateTable(
                name: "MateriaPrima",
                columns: table => new
                {
                    idMateriaPrima = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreMateriaPrima = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idInventario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriaPrima", x => x.idMateriaPrima);
                });

            migrationBuilder.CreateTable(
                name: "Produccion",
                columns: table => new
                {
                    idProduccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    idProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produccion", x => x.idProduccion);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    idProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    precio = table.Column<double>(type: "float", nullable: false),
                    stock = table.Column<int>(type: "int", nullable: false),
                    idCategoria = table.Column<int>(type: "int", nullable: false),
                    idInventario = table.Column<int>(type: "int", nullable: false),
                    imagen = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.idProducto);
                });

            migrationBuilder.CreateTable(
                name: "Proovedor",
                columns: table => new
                {
                    idProveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreProveedor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    materiaPrima = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proovedor", x => x.idProveedor);
                });

            migrationBuilder.CreateTable(
                name: "Tarjetas",
                columns: table => new
                {
                    idTarjeta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    nombrePropietario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numeroTarjeta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fechaVencimiento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ccv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarjetas", x => x.idTarjeta);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contrasenia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Venta",
                columns: table => new
                {
                    idVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    fechaVenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venta", x => x.idVenta);
                });

            migrationBuilder.CreateTable(
                name: "Recetas",
                columns: table => new
                {
                    idReceta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idProducto = table.Column<int>(type: "int", nullable: false),
                    ProductoidProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recetas", x => x.idReceta);
                    table.ForeignKey(
                        name: "FK_Recetas_Producto_ProductoidProducto",
                        column: x => x.ProductoidProducto,
                        principalTable: "Producto",
                        principalColumn: "idProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecetaDetalle",
                columns: table => new
                {
                    idRecetaDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idReceta = table.Column<int>(type: "int", nullable: false),
                    RecetaidReceta = table.Column<int>(type: "int", nullable: false),
                    idMateriaPrima = table.Column<int>(type: "int", nullable: false),
                    MateriaPrimaidMateriaPrima = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecetaDetalle", x => x.idRecetaDetalle);
                    table.ForeignKey(
                        name: "FK_RecetaDetalle_MateriaPrima_MateriaPrimaidMateriaPrima",
                        column: x => x.MateriaPrimaidMateriaPrima,
                        principalTable: "MateriaPrima",
                        principalColumn: "idMateriaPrima",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecetaDetalle_Recetas_RecetaidReceta",
                        column: x => x.RecetaidReceta,
                        principalTable: "Recetas",
                        principalColumn: "idReceta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecetaDetalle_MateriaPrimaidMateriaPrima",
                table: "RecetaDetalle",
                column: "MateriaPrimaidMateriaPrima");

            migrationBuilder.CreateIndex(
                name: "IX_RecetaDetalle_RecetaidReceta",
                table: "RecetaDetalle",
                column: "RecetaidReceta");

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_ProductoidProducto",
                table: "Recetas",
                column: "ProductoidProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarritoItem");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "DetalleVenta");

            migrationBuilder.DropTable(
                name: "instructivoProductos");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropTable(
                name: "Produccion");

            migrationBuilder.DropTable(
                name: "Proovedor");

            migrationBuilder.DropTable(
                name: "RecetaDetalle");

            migrationBuilder.DropTable(
                name: "Tarjetas");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Venta");

            migrationBuilder.DropTable(
                name: "MateriaPrima");

            migrationBuilder.DropTable(
                name: "Recetas");

            migrationBuilder.DropTable(
                name: "Producto");
        }
    }
}