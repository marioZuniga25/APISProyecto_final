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
      this.carritoService.obtenerCarrito(Number(userId)).subscribe(
        response => {
          console.log(response);
          if (response.Message) {
            this.mensajeCarrito = response.Message;
            this.carritoVacio = true;
          } else {
            this.carrito = response.carrito.detalles.map((detalle: any) => ({
              idDetalleCarrito: detalle.idDetalleCarrito,
              id: detalle.idProducto,
              nombre: detalle.nombreProducto,
              precio: detalle.precioUnitario,
              cantidad: detalle.cantidad,
              imagen: detalle.imagen,
              fechaAgregado: new Date(detalle.fechaAgregado) 
            }));
            this.carritoVacio = this.carrito.length === 0; 
            this.mensajeCarrito = ''; 
          }
        },
        error => {
          this.mensajeCarrito = 'Hubo un error al cargar tu carrito.';
          this.carritoVacio = true;
        }
      );
    } else {
     
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
