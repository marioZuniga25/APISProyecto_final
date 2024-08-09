import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductosService } from '../services/productos/productos.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-detalle',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './detalle.component.html',
  styleUrl: './detalle.component.css'
})
export class DetalleComponent implements OnInit {
  producto: any;

  constructor(
    private route: ActivatedRoute,
    private productosService: ProductosService
  ) {}

  ngOnInit(): void {
    // Get the product ID from the route
    this.route.paramMap.subscribe(params => {
      const idStr = params.get('id');
      const id = idStr ? +idStr : null;
      if (id !== null) {
        this.productosService.getProductoById(id).subscribe(producto => {
          this.producto = producto;
        });
      } else {
        console.error('Product ID is null or invalid');
      }
    });
  }
}