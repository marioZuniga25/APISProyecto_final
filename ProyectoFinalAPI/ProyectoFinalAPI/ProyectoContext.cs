﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProyectoFinalAPI.Models;
using System.Threading;

namespace ProyectoFinalAPI
{
    public class ProyectoContext : DbContext
    {

        public ProyectoContext(DbContextOptions<ProyectoContext> options) : base(options)
        { }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<DetalleVenta> DetalleVenta { get; set; }
        public DbSet<InstructivoProducto> instructivoProductos { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<MateriaPrima> MateriasPrimas { get; set; }
        public DbSet<Produccion> Produccion { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Tarjetas> Tarjetas { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<CarritoItem> CarritoItems { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Contacto> Contactos { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Categoria>(categoria =>
            {
                categoria.ToTable("Categoria");
                categoria.HasKey(c => c.idCategoria);
                categoria.Property(c => c.idCategoria).ValueGeneratedOnAdd().UseIdentityColumn();
                categoria.Property(c => c.nombreCategoria).IsRequired();
                categoria.Property(c => c.descripcion).IsRequired();
                //ciudad.Property(p => p.Nombre).IsRequired().HasMaxLength(150);

            });

            modelBuilder.Entity<DetalleVenta>(dVenta =>
            {
                dVenta.ToTable("DetalleVenta");
                dVenta.HasKey(dv => dv.idDetalleVenta);
                dVenta.Property(dv => dv.idDetalleVenta).ValueGeneratedOnAdd().UseIdentityColumn();
                dVenta.Property(dv => dv.cantidad).IsRequired();
                dVenta.Property(dv => dv.idVenta).IsRequired();
                dVenta.Property(dv => dv.precioUnitario).IsRequired();


                //Agregar datos iniciales
                //empleado.HasData(empleadoInit);
            });

            modelBuilder.Entity<InstructivoProducto>(InstructivoProducto =>
            {
                InstructivoProducto.HasKey(i => i.id);
                InstructivoProducto.Property(i => i.idProducto);
                InstructivoProducto.Property(i => i.idMateriaPrima);
                InstructivoProducto.Property(i => i.cantidad);

            });

            modelBuilder.Entity<Inventario>(inventario =>
            {
                inventario.ToTable("Inventario");
                inventario.HasKey(i => i.idInventario);
                inventario.Property(i => i.idInventario).ValueGeneratedOnAdd().UseIdentityColumn();
                inventario.Property(i => i.nombre).IsRequired();
                inventario.Property(i => i.cantidad);

            });


            modelBuilder.Entity<MateriaPrima>(matPrim =>
            {
                matPrim.ToTable("MateriaPrima");
                matPrim.HasKey(i => i.idMateriaPrima);
                matPrim.Property(i => i.idMateriaPrima).ValueGeneratedOnAdd().UseIdentityColumn();
                matPrim.Property(i => i.nombreMateriaPrima).IsRequired();
                matPrim.Property(i => i.descripcion);
                matPrim.Property(i => i.idInventario).IsRequired();

                // Configurar la relación con Inventario
                matPrim.HasOne(mp => mp.Inventario) // Definir la relación
                    .WithMany() // Asumiendo que Inventario no tiene una colección de MateriaPrimas
                    .HasForeignKey(mp => mp.idInventario); // Definir la clave foránea
            });

            modelBuilder.Entity<Produccion>(produccion =>
            {
                produccion.ToTable("Produccion");
                produccion.HasKey(p => p.idProduccion);
                produccion.Property(p => p.idProduccion).ValueGeneratedOnAdd().UseIdentityColumn();
                produccion.Property(p => p.cantidad).IsRequired();
                produccion.Property(p => p.idProducto).IsRequired();

            });


            modelBuilder.Entity<Producto>(producto =>
            {
                producto.ToTable("Producto");
                producto.HasKey(i => i.idProducto);
                producto.Property(i => i.idProducto).ValueGeneratedOnAdd().UseIdentityColumn();
                producto.Property(i => i.nombreProducto).IsRequired();
                producto.Property(i => i.descripcion);
                producto.Property(i => i.idInventario).IsRequired();
                producto.Property(i => i.precio).IsRequired();
                producto.Property(i => i.stock).IsRequired();
                producto.Property(i => i.idCategoria).IsRequired();
                producto.Property(i => i.idInventario).IsRequired();

            });

            modelBuilder.Entity<Proveedor>(producto =>
            {
                producto.ToTable("Proovedor");
                producto.HasKey(i => i.idProveedor);
                producto.Property(i => i.idProveedor).ValueGeneratedOnAdd().UseIdentityColumn();
                producto.Property(i => i.nombreProveedor).IsRequired();

            });
            modelBuilder.Entity<Tarjetas>(producto =>
            {
                producto.ToTable("Tarjetas");
                producto.HasKey(i => i.idTarjeta);
                producto.Property(i => i.idTarjeta).ValueGeneratedOnAdd().UseIdentityColumn();
                producto.Property(i => i.idUsuario).IsRequired();
                producto.Property(i => i.nombrePropietario).IsRequired();
                producto.Property(i => i.numeroTarjeta).IsRequired();
                producto.Property(i => i.fechaVencimiento).IsRequired();
                producto.Property(i => i.ccv).IsRequired();


            });

            modelBuilder.Entity<Usuario>(usuario =>
            {
                usuario.ToTable("Usuario");
                usuario.HasKey(i => i.idUsuario);
                usuario.Property(i => i.idUsuario).ValueGeneratedOnAdd().UseIdentityColumn();
                usuario.Property(i => i.nombreUsuario).IsRequired();
                usuario.Property(i => i.correo).IsRequired();
                usuario.Property(i => i.contrasenia).IsRequired();
                usuario.Property(i => i.rol).IsRequired();

            });

            modelBuilder.Entity<Venta>(venta =>
            {
                venta.ToTable("Venta");
                venta.HasKey(i => i.idVenta);
                venta.Property(i => i.idVenta).ValueGeneratedOnAdd().UseIdentityColumn();
                venta.Property(i => i.total).IsRequired();
                venta.Property(i => i.fechaVenta).IsRequired();
                venta.Property(i => i.idUsuario).IsRequired();

            });

            modelBuilder.Entity<CarritoItem>(carritoItem =>
            {
                carritoItem.ToTable("CarritoItem");
                carritoItem.HasKey(i => i.id);
                carritoItem.Property(i => i.id).ValueGeneratedOnAdd().UseIdentityColumn();
                carritoItem.Property(i => i.productoId).IsRequired();
                carritoItem.Property(i => i.nombreProducto).IsRequired();
                carritoItem.Property(i => i.cantidad).IsRequired();
                carritoItem.Property(i => i.precio).IsRequired();
                carritoItem.Property(i => i.imagen).IsRequired();
            });

            modelBuilder.Entity<Receta>()
    .HasOne(r => r.Producto)
    .WithMany()
    .HasForeignKey(r => r.idProducto)
    .OnDelete(DeleteBehavior.Restrict); // O DeleteBehavior.Cascade, dependiendo de tus necesidades

            modelBuilder.Entity<RecetaDetalle>()
                .HasOne(rd => rd.Receta)
                .WithMany(r => r.Detalles)
                .HasForeignKey(rd => rd.idReceta)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecetaDetalle>()
                .HasOne(rd => rd.MateriaPrima)
                .WithMany()
                .HasForeignKey(rd => rd.idMateriaPrima)
                .OnDelete(DeleteBehavior.Restrict); // O DeleteBehavior.Cascade, dependiendo de tus necesidades

        }
    }

}

