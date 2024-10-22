using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class newMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Contactos",
                columns: table => new
                {
                    IdContacto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contactos", x => x.IdContacto);
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
                name: "OrdenCompra",
                columns: table => new
                {
                    idOrdenCompra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idProveedor = table.Column<int>(type: "int", nullable: false),
                    fechaCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    usuario = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenCompra", x => x.idOrdenCompra);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    idPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idVenta = table.Column<int>(type: "int", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    idTarjeta = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    calle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    colonia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ciudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    codigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.idPedido);
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
                    NombreCategoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    correo = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "UnidadMedida",
                columns: table => new
                {
                    idUnidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreUnidad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadMedida", x => x.idUnidad);
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
                    rol = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "DetalleOrdenCompra",
                columns: table => new
                {
                    idDetalleOrdenCompra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idMateriaPrima = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrdenCompraidOrdenCompra = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleOrdenCompra", x => x.idDetalleOrdenCompra);
                    table.ForeignKey(
                        name: "FK_DetalleOrdenCompra_OrdenCompra_OrdenCompraidOrdenCompra",
                        column: x => x.OrdenCompraidOrdenCompra,
                        principalTable: "OrdenCompra",
                        principalColumn: "idOrdenCompra");
                });

            migrationBuilder.CreateTable(
                name: "Recetas",
                columns: table => new
                {
                    idReceta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recetas", x => x.idReceta);
                    table.ForeignKey(
                        name: "FK_Recetas_Producto_idProducto",
                        column: x => x.idProducto,
                        principalTable: "Producto",
                        principalColumn: "idProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MateriaPrima",
                columns: table => new
                {
                    idMateriaPrima = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreMateriaPrima = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idUnidad = table.Column<int>(type: "int", nullable: false),
                    precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    stock = table.Column<double>(type: "float", nullable: false),
                    ProveedoridProveedor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriaPrima", x => x.idMateriaPrima);
                    table.ForeignKey(
                        name: "FK_MateriaPrima_Proovedor_ProveedoridProveedor",
                        column: x => x.ProveedoridProveedor,
                        principalTable: "Proovedor",
                        principalColumn: "idProveedor");
                });

            migrationBuilder.CreateTable(
                name: "RecetaDetalles",
                columns: table => new
                {
                    idRecetaDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idReceta = table.Column<int>(type: "int", nullable: false),
                    idMateriaPrima = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecetaDetalles", x => x.idRecetaDetalle);
                    table.ForeignKey(
                        name: "FK_RecetaDetalles_MateriaPrima_idMateriaPrima",
                        column: x => x.idMateriaPrima,
                        principalTable: "MateriaPrima",
                        principalColumn: "idMateriaPrima",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecetaDetalles_Recetas_idReceta",
                        column: x => x.idReceta,
                        principalTable: "Recetas",
                        principalColumn: "idReceta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleOrdenCompra_OrdenCompraidOrdenCompra",
                table: "DetalleOrdenCompra",
                column: "OrdenCompraidOrdenCompra");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaPrima_ProveedoridProveedor",
                table: "MateriaPrima",
                column: "ProveedoridProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_RecetaDetalles_idMateriaPrima",
                table: "RecetaDetalles",
                column: "idMateriaPrima");

            migrationBuilder.CreateIndex(
                name: "IX_RecetaDetalles_idReceta",
                table: "RecetaDetalles",
                column: "idReceta");

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_idProducto",
                table: "Recetas",
                column: "idProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Contactos");

            migrationBuilder.DropTable(
                name: "DetalleOrdenCompra");

            migrationBuilder.DropTable(
                name: "DetalleVenta");

            migrationBuilder.DropTable(
                name: "instructivoProductos");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Produccion");

            migrationBuilder.DropTable(
                name: "RecetaDetalles");

            migrationBuilder.DropTable(
                name: "Tarjetas");

            migrationBuilder.DropTable(
                name: "UnidadMedida");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Venta");

            migrationBuilder.DropTable(
                name: "OrdenCompra");

            migrationBuilder.DropTable(
                name: "MateriaPrima");

            migrationBuilder.DropTable(
                name: "Recetas");

            migrationBuilder.DropTable(
                name: "Proovedor");

            migrationBuilder.DropTable(
                name: "Producto");
        }
    }
}
