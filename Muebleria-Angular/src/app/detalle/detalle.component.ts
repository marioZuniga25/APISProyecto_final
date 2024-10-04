import { Component, OnInit } from '@angular/core';
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
  styleUrl: './detalle.component.css'
})
export class DetalleComponent implements OnInit {
  producto!: IProductoResponse;
    cantidad: number = 1;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
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
      const productoCarrito: ProductoCarrito = {
        id: this.producto.idProducto,
        nombre: this.producto.nombreProducto,
        precio: this.producto.precio,
        cantidad: this.cantidad,
        imagen: this.producto.imagen,
        stock: this.producto.stock
      };
      this.carritoService.agregarAlCarrito(productoCarrito);
      Swal.fire('Éxito', 'Se agregó el producto al carrito.', 'success');
    }
  }
}
