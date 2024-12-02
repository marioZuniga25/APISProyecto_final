import { Component, OnInit } from '@angular/core';
import { ProductosService } from '../services/productos/productos.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-catalogo',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './catalogo.component.html',
  styleUrls: ['./catalogo.component.css']
})
export class CatalogoComponent implements OnInit {
  productos: any[] = []; // Lista de productos final con o sin descuento

  constructor(private productosService: ProductosService) {}

  ngOnInit(): void {
    // Obtener ambos endpoints y combinar los datos
    this.productosService.getAllProductos().subscribe(
      (productos) => {
        this.productosService.getAllProductosPromociones().subscribe(
          (productosPromociones) => {
            // Mezclar datos: buscar si el producto tiene descuento
            this.productos = productos.map((producto) => {
              const productoPromocion = productosPromociones.find(
                (promo) => promo.idProducto === producto.idProducto
              );

              // Si hay promoción, agregar precioFinal y porcentajeDescuento
              if (productoPromocion) {
                return {
                  ...producto,
                  precioOriginal: producto.precio,
                  precioConDescuento: productoPromocion.precioFinal,
                  porcentajeDescuento: productoPromocion.porcentajeDescuento
                };
              }

              // Si no hay promoción, retornar el producto sin cambios
              return producto;
            });
          },
          (error) => {
            console.error('Error al obtener los productos promociones', error);
          }
        );
      },
      (error) => {
        console.error('Error al obtener los productos', error);
      }
    );
  }
}
