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
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DireccionEnvio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Calle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ciudad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodigoPostal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Colonia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EsPredeterminada")
                        .HasColumnType("bit");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreDireccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonaId");

                    b.ToTable("DireccionesEnvio");
                });

            modelBuilder.Entity("LogInicioSesion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaInicioSesion")
                        .HasColumnType("datetime2");

                    b.Property<string>("IpDireccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LogInicioSesion");
                });

            modelBuilder.Entity("Persona", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId")
                        .IsUnique();

                    b.ToTable("Personas");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Carrito", b =>
                {
                    b.Property<int>("IdCarrito")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCarrito"));

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("IdCarrito");

                    b.ToTable("Carrito", (string)null);
                });

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

            modelBuilder.Entity("ProyectoFinalAPI.Models.ContraseniaInsegura", b =>
                {
                    b.Property<int>("IdContraseniaInsegura")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdContraseniaInsegura"));

                    b.Property<string>("Contrasenia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdContraseniaInsegura");

                    b.ToTable("ContraseniaInsegura", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.DetalleCarrito", b =>
                {
                    b.Property<int>("IdDetalleCarrito")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDetalleCarrito"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaAgregado")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdCarrito")
                        .HasColumnType("int");

                    b.Property<int>("IdProducto")
                        .HasColumnType("int");

                    b.Property<double>("PrecioUnitario")
                        .HasColumnType("float");

                    b.HasKey("IdDetalleCarrito");

                    b.ToTable("DetalleCarrito", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.DetalleOrdenCompra", b =>
                {
                    b.Property<int>("idDetalleOrdenCompra")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idDetalleOrdenCompra"));

                    b.Property<int?>("OrdenCompraidOrdenCompra")
                        .HasColumnType("int");

                    b.Property<int>("cantidad")
                        .HasColumnType("int");

                    b.Property<int>("idMateriaPrima")
                        .HasColumnType("int");

                    b.Property<decimal>("precioUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("idDetalleOrdenCompra");

                    b.HasIndex("OrdenCompraidOrdenCompra");

                    b.ToTable("DetalleOrdenCompra", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.DetallePromocion", b =>
                {
                    b.Property<int>("IdDetallePromocion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDetallePromocion"));

                    b.Property<int>("IdProducto")
                        .HasColumnType("int");

                    b.Property<int>("IdPromocion")
                        .HasColumnType("int");

                    b.Property<double>("PorcentajeDescuento")
                        .HasColumnType("float");

                    b.Property<double>("PrecioFinal")
                        .HasColumnType("float");

                    b.HasKey("IdDetallePromocion");

                    b.HasIndex("IdProducto");

                    b.HasIndex("IdPromocion");

                    b.ToTable("DetallePromocion", (string)null);
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

            modelBuilder.Entity("ProyectoFinalAPI.Models.Merma", b =>
                {
                    b.Property<int>("IdMerma")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdMerma"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("cantidad")
                        .HasColumnType("int");

                    b.Property<string>("causa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("comentarios")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("fechaMerma")
                        .HasColumnType("datetime2");

                    b.Property<int>("idMateria")
                        .HasColumnType("int");

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.Property<string>("unidadMedida")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdMerma");

                    b.ToTable("Merma", (string)null);
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

                    b.Property<string>("usuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idOrdenCompra");

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

                    b.Property<int>("EnPromocion")
                        .HasColumnType("int");

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

            modelBuilder.Entity("ProyectoFinalAPI.Models.Promocion", b =>
                {
                    b.Property<int>("IdPromocion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPromocion"));

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdPromocion");

                    b.ToTable("Promociones", (string)null);
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.PromocionesRandom", b =>
                {
                    b.Property<int>("IdPromocionRandom")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPromocionRandom"));

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime2");

                    b.PrimitiveCollection<string>("Productos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdPromocionRandom");

                    b.ToTable("PromocionesRandom", (string)null);
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

                    b.Property<bool>("EstaBloqueado")
                        .HasColumnType("bit");

                    b.Property<int>("IntentosFallidos")
                        .HasColumnType("int");

                    b.Property<string>("ResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("contrasenia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("loginCount")
                        .HasColumnType("int");

                    b.Property<string>("nombreUsuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("rol")
                        .HasColumnType("int");

                    b.Property<int>("type")
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

                    b.Property<string>("tipoVenta")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<double>("total")
                        .HasColumnType("float");

                    b.HasKey("idVenta");

                    b.ToTable("Venta", (string)null);
                });

            modelBuilder.Entity("DireccionEnvio", b =>
                {
                    b.HasOne("Persona", "Persona")
                        .WithMany("DireccionesEnvio")
                        .HasForeignKey("PersonaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Persona");
                });

            modelBuilder.Entity("Persona", b =>
                {
                    b.HasOne("ProyectoFinalAPI.Models.Usuario", "Usuario")
                        .WithOne("Persona")
                        .HasForeignKey("Persona", "UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.DetalleOrdenCompra", b =>
                {
                    b.HasOne("ProyectoFinalAPI.Models.OrdenCompra", null)
                        .WithMany("Detalles")
                        .HasForeignKey("OrdenCompraidOrdenCompra");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.DetallePromocion", b =>
                {
                    b.HasOne("ProyectoFinalAPI.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("IdProducto")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProyectoFinalAPI.Models.Promocion", "Promocion")
                        .WithMany("Detalles")
                        .HasForeignKey("IdPromocion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");

                    b.Navigation("Promocion");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.MateriaPrima", b =>
                {
                    b.HasOne("ProyectoFinalAPI.Models.Proveedor", null)
                        .WithMany("MateriasPrimas")
                        .HasForeignKey("ProveedoridProveedor");
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

            modelBuilder.Entity("Persona", b =>
                {
                    b.Navigation("DireccionesEnvio");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.OrdenCompra", b =>
                {
                    b.Navigation("Detalles");
                });

            modelBuilder.Entity("ProyectoFinalAPI.Models.Promocion", b =>
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

            modelBuilder.Entity("ProyectoFinalAPI.Models.Usuario", b =>
                {
                    b.Navigation("Persona");
                });
#pragma warning restore 612, 618
        }
    }
}
