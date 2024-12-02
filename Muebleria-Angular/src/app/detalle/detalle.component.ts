import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductosService } from '../services/productos/productos.service';
import { CommonModule } from '@angular/common';
import { BuscadorService } from '../services/buscador.service';
import { CarritoService, ProductoCarrito } from '../services/carrito/carrito.service';
import { IProductoResponse } from '../interfaces/IProductoResponse';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-detalle',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './detalle.component.html',
  styleUrls: ['./detalle.component.css'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA] 
})
export class DetalleComponent implements OnInit {
  producto!: IProductoResponse;
  cantidad: number = 1;
  precioConDescuento!: number;
  descuento!: number;
  tienePromocion: boolean = false;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productosService: ProductosService,
    private buscadorService: BuscadorService,
    private carritoService: CarritoService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const idStr = params.get('id');
      const id = idStr ? +idStr : null;
      const descuentoStr = this.route.snapshot.queryParamMap.get('descuento');
      const precioConDescuentoStr = this.route.snapshot.queryParamMap.get('precioConDescuento');
      const idPromocionRandom = this.route.snapshot.queryParamMap.get('idPromocionRandom'); // Captura el ID de promoción
      // Verifica si hay promoción
      this.descuento = descuentoStr ? +descuentoStr : 0;
      this.precioConDescuento = precioConDescuentoStr ? +precioConDescuentoStr : 0;
      this.tienePromocion = this.descuento > 0 && this.precioConDescuento > 0;

      if (id !== null) {
        this.productosService.getProductoById(id).subscribe(producto => {
          if (producto) {
            this.producto = producto;
            // Si no hay promoción, asigna el precio normal al precioConDescuento
            if (!this.tienePromocion) {
              this.precioConDescuento = this.producto.precio;
            }
            this.buscadorService.closeBuscador();
          } else {
            console.error('Producto no encontrado');
          }
        });
      } else {
        console.error('El ID del producto es nulo o inválido');
      }
    });
  }

  agregarAlCarrito() {
    const userId = localStorage.getItem('userId');
    if (!userId) {
      Swal.fire({
        icon: 'warning',
        title: 'Inicia sesión',
        text: 'Debes iniciar sesión para agregar productos al carrito.',
        confirmButtonText: 'Ir al Login'
      }).then((result) => {
        if (result.isConfirmed) {
          this.router.navigate(['/login']);
        }
      });
    } else {
      if (this.producto.stock < this.cantidad) {
        Swal.fire('Error', 'No hay suficiente stock disponible.', 'error');
        return;
      }
  
      const productoCarrito: ProductoCarrito = {
        idDetalleCarrito: 0, 
        id: this.producto.idProducto,
        nombre: this.producto.nombreProducto,
        precio: this.precioConDescuento,
        cantidad: this.cantidad,
        imagen: this.producto.imagen,
        stock: this.producto.stock,
        fechaAgregado: new Date() // Agrega la fecha de forma explícita
      };
  
      this.carritoService.agregarAlCarrito(Number(userId), productoCarrito).subscribe(
        () => {
          Swal.fire('Éxito', 'Producto agregado al carrito.', 'success');
        },
        (error) => {
          Swal.fire('Error', 'No se pudo agregar el producto al carrito.', 'error');
        }
      );
    }
  }  
}
