import { CommonModule, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { CarritoService } from '../services/carrito/carrito.service';

@Component({
  selector: 'app-carrito',
  standalone: true,
  imports: [NgIf, CommonModule],
  templateUrl: './carrito.component.html',
  styleUrl: './carrito.component.css'
})
export class CarritoComponent implements OnInit{
  mostrarBag$ = this.carritoService.mostrarBag$;
  itemsCarrito$ = this.carritoService.itemsCarrito$;

  constructor(private carritoService: CarritoService) {}

  ngOnInit(): void {
    this.carritoService.obtenerCarrito();
  }

  toggleBag() {
    this.carritoService.toggleBag();
  }

  eliminarDelCarrito(id: number) {
    this.carritoService.eliminarProductoDelCarrito(id).subscribe(
      () => {
        console.log('Producto eliminado del carrito');
        this.carritoService.obtenerCarrito(); // Actualiza la lista después de eliminar un producto
      },
      (error) => console.error('Error al eliminar producto del carrito', error)
    );
  }
}
