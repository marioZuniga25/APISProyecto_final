import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
  styleUrl: './detalle.component.css'
})
export class DetalleComponent implements OnInit {
  producto!: IProductoResponse;
    cantidad: number = 1;

  constructor(
    private route: ActivatedRoute,
    private productosService: ProductosService,
    private buscadorService: BuscadorService,
    private carritoService: CarritoService
  ) {}

  ngOnInit(): void {
    // Obtén el ID del producto de la ruta
    this.route.paramMap.subscribe(params => {
      const idStr = params.get('id');
      const id = idStr ? +idStr : null;
      if (id !== null) {
        this.productosService.getProductoById(id).subscribe(producto => {
          if (producto) {
            this.producto = producto;
            // Cierra el buscador después de establecer el producto
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
    const productoCarrito: ProductoCarrito = {
      id: this.producto.idProducto,
      nombre: this.producto.nombreProducto,
      precio: this.producto.precio,
      cantidad: this.cantidad,
      imagen: this.producto.imagen,
      stock: this.producto.stock
    };
    console.log('carrito productos' + productoCarrito);
    this.carritoService.agregarAlCarrito(productoCarrito);
    Swal.fire('Exito', 'Se agrego al carrito el articulo', 'success');
  }
}
