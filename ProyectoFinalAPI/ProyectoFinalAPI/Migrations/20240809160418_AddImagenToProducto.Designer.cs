﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProyectoFinalAPI;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    [DbContext(typeof(ProyectoContext))]
    [Migration("20240809160418_AddImagenToProducto")]
    partial class AddImagenToProducto
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("ProyectoFinalAPI.Models.Inventario", b =>
                {
                    b.Property<int>("idInventario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idInventario"));

                    b.Property<int>("cantidad")
                        .HasColumnType("int");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idInventario");

                    b.ToTable("Inventario", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.MateriaPrima", b =>
                {
                    b.Property<int>("idMateriaPrima")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idMateriaPrima"));

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("idInventario")
                        .HasColumnType("int");

                    b.Property<string>("nombreMateriaPrima")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idMateriaPrima");

                    b.ToTable("MateriaPrima", (string)null);
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

                    b.Property<string>("nombreProveedor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idProveedor");

                    b.ToTable("Proovedor", (string)null);
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
#pragma warning restore 612, 618
        }
    }
}
