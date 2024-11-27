import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { CarritoService, ProductoCarrito } from '../services/carrito/carrito.service';

@Component({
  selector: 'app-bag',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bag.component.html',
  styleUrls: ['./bag.component.css']
})
export class BagComponent implements OnInit {
  carrito: ProductoCarrito[] = [];
  subtotal: number = 0;
  total: number = 0;

  constructor(private carritoService: CarritoService) {}

  ngOnInit(): void {
    this.cargarCarrito();
  }

  cargarCarrito(): void {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.carritoService.obtenerCarrito(Number(userId)).subscribe(
        response => {
          console.log('Respuesta del carrito:', response); 
          if (response && response.carrito && Array.isArray(response.carrito.detalles)) {
            this.carrito = response.carrito.detalles.map((detalle: any) => ({
              idDetalleCarrito: detalle.idDetalleCarrito,
              id: detalle.idProducto,
              nombre: detalle.nombreProducto,
              precio: detalle.precioUnitario,  
              cantidad: detalle.cantidad,
              imagen: detalle.imagen,
              fechaAgregado: new Date(detalle.fechaAgregado)
            }));
            this.calcularTotales();
          } else {
            console.error('La estructura de la respuesta no es válida:', response);
            this.carrito = [];
          }
        },
        error => {
          console.error('Error al obtener el carrito:', error);
        }
      );
    }
  }

  calcularTotales(): void {
    this.subtotal = this.carrito.reduce((acc, producto) => acc + producto.precio * producto.cantidad, 0);
    this.total = this.subtotal;
  }
}
