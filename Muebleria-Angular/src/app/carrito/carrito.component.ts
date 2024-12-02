import { CommonModule, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { CarritoService, ProductoCarrito } from '../services/carrito/carrito.service';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-carrito',
  standalone: true,
  imports: [NgIf, CommonModule],
  templateUrl: './carrito.component.html',
  styleUrl: './carrito.component.css'
})
export class CarritoComponent implements OnInit{
  carrito: ProductoCarrito[] = [];
  mostrarBag$ = this.carritoService.mostrarBag$;
  carritoVacio: boolean = false;
  mensajeCarrito: string = ''; 
  
  constructor(private carritoService: CarritoService) {}
  toggleBag() {
    this.carritoService.toggleBag();
  }
  ngOnInit(): void {
    const userId = localStorage.getItem('userId');
  
    if (userId) {
      // Suscripción al BehaviorSubject para actualizar la vista en tiempo real
      this.carritoService.carrito$.subscribe(carrito => {
        this.carrito = carrito;
        this.carritoVacio = carrito.length === 0;
        this.mensajeCarrito = carrito.length === 0 ? 'Tu carrito está vacío' : '';
      });
  
      // Recuperar el carrito desde la API al recargar
      this.carritoService.obtenerCarrito(Number(userId)).subscribe(
        response => {
          if (response.carrito && response.carrito.detalles) {
            // Mapear y actualizar el BehaviorSubject con los datos de la API
            const carritoActualizado = response.carrito.detalles.map((detalle: any) => ({
              idDetalleCarrito: detalle.idDetalleCarrito,
              id: detalle.idProducto,
              nombre: detalle.nombreProducto,
              precio: detalle.precioUnitario,
              cantidad: detalle.cantidad,
              imagen: detalle.imagen,
              fechaAgregado: new Date(detalle.fechaAgregado)
            }));
  
            this.carritoService.actualizarCarrito(carritoActualizado); // Actualiza el BehaviorSubject
          } else {
            this.carritoVacio = true;
            this.mensajeCarrito = 'Tu carrito está vacío';
          }
        },
        error => {
          this.mensajeCarrito = 'Hubo un error al cargar tu carrito.';
          this.carritoVacio = true;
        }
      );
    }
  }

  
  incrementar(producto: any): void {
    this.carritoService.incrementarCantidad(producto.idDetalleCarrito).subscribe(response => {
      producto.cantidad = response.cantidad;
    });
  }
  
  decrementar(producto: any): void {
    this.carritoService.decrementarCantidad(producto.idDetalleCarrito).subscribe(response => {
      producto.cantidad = response.cantidad;
    });
  }
  eliminarProducto(producto: ProductoCarrito): void {
    console.log(producto);
    this.carritoService.eliminarProducto(producto.idDetalleCarrito).subscribe(response => {
      this.carrito = this.carrito.filter(item => item.idDetalleCarrito !== producto.idDetalleCarrito);
    });
  }
}
