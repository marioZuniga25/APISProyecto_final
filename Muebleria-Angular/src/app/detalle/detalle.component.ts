import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductosService } from '../services/productos/productos.service';
import { CommonModule } from '@angular/common';
import { CarritoService } from '../services/carrito/carrito.service';
import { BuscadorService } from '../services/buscador.service';

@Component({
  selector: 'app-detalle',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './detalle.component.html',
  styleUrl: './detalle.component.css'
})
export class DetalleComponent implements OnInit {
  producto: any;
  cantidad: number = 1;

  constructor(
    private route: ActivatedRoute,
    private carritoService: CarritoService,
    private productosService: ProductosService,
    private buscadorService: BuscadorService,
  ) {}

  ngOnInit(): void {
    // Get the product ID from the route
    this.route.paramMap.subscribe(params => {
      const idStr = params.get('id');
      const id = idStr ? +idStr : null;
      if (id !== null) {
        this.productosService.getProductoById(id).subscribe(producto => {
          this.producto = producto;
          
          // Close the buscador after setting the product
          this.buscadorService.closeBuscador();
        });
      } else {
        console.error('Product ID is null or invalid');
      }
    });
  }

  agregarAlCarrito(): void {
    if (this.cantidad <= this.producto.stock) {
      this.carritoService.agregarProductoAlCarrito(this.producto, this.cantidad).subscribe(
        () => {
          console.log('Producto agregado al carrito');
          this.carritoService.obtenerCarrito(); // Actualiza el carrito después de agregar un producto
        },
        (error) => console.error('Error al agregar producto al carrito', error)
      );
    } else {
      alert('No hay suficientes productos en stock');
    }
  }
}
