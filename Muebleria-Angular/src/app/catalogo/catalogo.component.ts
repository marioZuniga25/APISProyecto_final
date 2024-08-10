import { Component, OnInit } from '@angular/core';
<<<<<<< HEAD
import { ProductosService } from '../services/productos.service'; // Asegúrate de tener la ruta correcta
import { CommonModule } from '@angular/common';
=======
import { ProductosService } from '../services/productos/productos.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
>>>>>>> 4bb3c8c195743339d7882f125118ab377f638f33

@Component({
  selector: 'app-catalogo',
  standalone: true,
<<<<<<< HEAD
  imports: [CommonModule],
=======
  imports: [CommonModule, RouterLink],
>>>>>>> 4bb3c8c195743339d7882f125118ab377f638f33
  templateUrl: './catalogo.component.html',
  styleUrls: ['./catalogo.component.css']
})
<<<<<<< HEAD
export class CatalogoComponent implements OnInit {
  productos: any[] = [];

  constructor(private productosService: ProductosService) {}
=======
export class CatalogoComponent implements OnInit{
  productos: any[] = [];

  constructor(private productosService: ProductosService) { }
>>>>>>> 4bb3c8c195743339d7882f125118ab377f638f33

  ngOnInit(): void {
    this.productosService.getAllProductos().subscribe(
      (data) => {
        this.productos = data;
      },
      (error) => {
<<<<<<< HEAD
        console.error('Error fetching products', error);
=======
        console.error('Error al obtener los productos', error);
>>>>>>> 4bb3c8c195743339d7882f125118ab377f638f33
      }
    );
  }
}
