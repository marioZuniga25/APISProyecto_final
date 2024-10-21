﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProyectoFinalAPI;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    [DbContext(typeof(ProyectoContext))]
    partial class ProyectoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProyectoFinalAPI.Models.Categoria", b =>
                {
                    b.Property<int>("idCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idCategoria"));

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombreCategoria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idCategoria");

                    b.ToTable("Categoria", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Contacto", b =>
                {
                    b.Property<int>("IdContacto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdContacto"));

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mensaje")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdContacto");

                    b.ToTable("Contactos");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.DetalleOrdenCompra", b =>
                {
                    b.Property<int>("idDetalleOrdenCompra")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idDetalleOrdenCompra"));

                    b.Property<double>("cantidad")
                        .HasColumnType("float");

                    b.Property<int>("idMateriaPrima")
                        .HasColumnType("int");

                    b.Property<int>("idOrdenCompra")
                        .HasColumnType("int");

                    b.Property<decimal>("precioUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("idDetalleOrdenCompra");

                    b.HasIndex("idMateriaPrima");

                    b.HasIndex("idOrdenCompra");

                    b.ToTable("DetalleOrdenCompra", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.DetalleVenta", b =>
                {
                    b.Property<int>("idDetalleVenta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idDetalleVenta"));

                    b.Property<int>("cantidad")
                        .HasColumnType("int");

                    b.Property<int>("idProducto")
                        .HasColumnType("int");

                    b.Property<int>("idVenta")
                        .HasColumnType("int");

                    b.Property<double>("precioUnitario")
                        .HasColumnType("float");

                    b.HasKey("idDetalleVenta");

                    b.ToTable("DetalleVenta", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.InstructivoProducto", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<double>("cantidad")
                        .HasColumnType("float");

                    b.Property<int>("idMateriaPrima")
                        .HasColumnType("int");

                    b.Property<int>("idProducto")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("instructivoProductos");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.MateriaPrima", b =>
                {
                    b.Property<int>("idMateriaPrima")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idMateriaPrima"));

                    b.Property<int?>("ProveedoridProveedor")
                        .HasColumnType("int");

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("idUnidad")
                        .HasColumnType("int");

                    b.Property<string>("nombreMateriaPrima")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("precio")
                        .HasColumnType("decimal(18,2)");

                    b.Property<double>("stock")
                        .HasColumnType("float");

                    b.HasKey("idMateriaPrima");

                    b.HasIndex("ProveedoridProveedor");

                    b.ToTable("MateriaPrima", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.OrdenCompra", b =>
                {
                    b.Property<int>("idOrdenCompra")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idOrdenCompra"));

                    b.Property<DateTime>("fechaCompra")
                        .HasColumnType("datetime2");

                    b.Property<int>("idProveedor")
                        .HasColumnType("int");

                    b.HasKey("idOrdenCompra");

                    b.HasIndex("idProveedor");

                    b.ToTable("OrdenCompra", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Pedidos", b =>
                {
                    b.Property<int>("idPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idPedido"));

                    b.Property<string>("apellidos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("calle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ciudad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("codigoPostal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("colonia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("estatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("idTarjeta")
                        .HasColumnType("int");

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.Property<int>("idVenta")
                        .HasColumnType("int");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("numero")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idPedido");

                    b.ToTable("Pedidos", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Produccion", b =>
                {
                    b.Property<int>("idProduccion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idProduccion"));

                    b.Property<int>("cantidad")
                        .HasColumnType("int");

                    b.Property<int>("idProducto")
                        .HasColumnType("int");

                    b.HasKey("idProduccion");

                    b.ToTable("Produccion", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Producto", b =>
                {
                    b.Property<int>("idProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idProducto"));

                    b.Property<string>("NombreCategoria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("idCategoria")
                        .HasColumnType("int");

                    b.Property<int>("idInventario")
                        .HasColumnType("int");

                    b.Property<string>("imagen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombreProducto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("precio")
                        .HasColumnType("float");

                    b.Property<int>("stock")
                        .HasColumnType("int");

                    b.HasKey("idProducto");

                    b.ToTable("Producto", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Proveedor", b =>
                {
                    b.Property<int>("idProveedor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idProveedor"));

                    b.Property<string>("correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombreProveedor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idProveedor");

                    b.ToTable("Proovedor", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Receta", b =>
                {
                    b.Property<int>("idReceta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idReceta"));

                    b.Property<int>("idProducto")
                        .HasColumnType("int");

                    b.HasKey("idReceta");

                    b.HasIndex("idProducto");

                    b.ToTable("Recetas");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.RecetaDetalle", b =>
                {
                    b.Property<int>("idRecetaDetalle")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idRecetaDetalle"));

                    b.Property<double>("cantidad")
                        .HasColumnType("float");

                    b.Property<int>("idMateriaPrima")
                        .HasColumnType("int");

                    b.Property<int>("idReceta")
                        .HasColumnType("int");

                    b.HasKey("idRecetaDetalle");

                    b.HasIndex("idMateriaPrima");

                    b.HasIndex("idReceta");

                    b.ToTable("RecetaDetalles");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Tarjetas", b =>
                {
                    b.Property<int>("idTarjeta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idTarjeta"));

                    b.Property<string>("ccv")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fechaVencimiento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.Property<string>("nombrePropietario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("numeroTarjeta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idTarjeta");

                    b.ToTable("Tarjetas", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.UnidadMedida", b =>
                {
                    b.Property<int>("idUnidad")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idUnidad"));

                    b.Property<string>("nombreUnidad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idUnidad");

                    b.ToTable("UnidadMedida", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Usuario", b =>
                {
                    b.Property<int>("idUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idUsuario"));

                    b.Property<string>("contrasenia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombreUsuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("rol")
                        .HasColumnType("int");

                    b.HasKey("idUsuario");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Venta", b =>
                {
                    b.Property<int>("idVenta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idVenta"));

                    b.Property<DateTime>("fechaVenta")
                        .HasColumnType("datetime2");

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.Property<double>("total")
                        .HasColumnType("float");

                    b.HasKey("idVenta");

                    b.ToTable("Venta", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.DetalleOrdenCompra", b =>
                {
                    b.HasOne("ProyectoFinalAPI.Models.MateriaPrima", "MateriaPrima")
                        .WithMany()
                        .HasForeignKey("idMateriaPrima")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProyectoFinalAPI.Models.OrdenCompra", "OrdenCompra")
                        .WithMany("Detalles")
                        .HasForeignKey("idOrdenCompra")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MateriaPrima");

                    b.Navigation("OrdenCompra");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.MateriaPrima", b =>
                {
                    b.HasOne("ProyectoFinalAPI.Models.Proveedor", null)
                        .WithMany("MateriasPrimas")
                        .HasForeignKey("ProveedoridProveedor");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.OrdenCompra", b =>
                {
                    b.HasOne("ProyectoFinalAPI.Models.Proveedor", "Proveedor")
                        .WithMany()
                        .HasForeignKey("idProveedor")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Proveedor");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Receta", b =>
                {
                    b.HasOne("ProyectoFinalAPI.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("idProducto")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.RecetaDetalle", b =>
                {
                    b.HasOne("ProyectoFinalAPI.Models.MateriaPrima", "MateriaPrima")
                        .WithMany()
                        .HasForeignKey("idMateriaPrima")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProyectoFinalAPI.Models.Receta", "Receta")
                        .WithMany("Detalles")
                        .HasForeignKey("idReceta")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MateriaPrima");

                    b.Navigation("Receta");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.OrdenCompra", b =>
                {
                    b.Navigation("Detalles");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Proveedor", b =>
                {
                    b.Navigation("MateriasPrimas");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Receta", b =>
                {
                    b.Navigation("Detalles");
                });
#pragma warning restore 612, 618
        }
    }
}
