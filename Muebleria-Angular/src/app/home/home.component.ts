import { Component, AfterViewInit, OnInit, ChangeDetectorRef } from '@angular/core';
import * as jQuery from 'jquery';
import 'slick-carousel';
import { ProductosService } from '../services/productos/productos.service';
import { CommonModule, NgFor } from '@angular/common';
import { RouterLink } from '@angular/router';

declare var $: any;

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, NgFor, RouterLink],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements AfterViewInit, OnInit {
  productos: any[] = [];

  constructor(
    private productosService: ProductosService,
    private cdr: ChangeDetectorRef
  ) {}
  
  ngOnInit(): void {
    this.productosService.getAllProductos().subscribe(
      (data) => {
        this.productos = data;
        this.cdr.detectChanges(); // Asegurarse de que la vista se ha actualizado
        this.initializeSlickSlider(); // Inicializa después de detectar los cambios
      },
      (error) => {
        console.error('Error al obtener los productos', error);
      }
    );
  }
  

  ngAfterViewInit(): void {
    // Ensure Slick Slider is initialized after view is completely initialized
    this.initializeSlickSlider();
  }

  private initializeSlickSlider(): void {
    // Delay initialization to ensure DOM updates
    setTimeout(() => {
      const $carousel = $('.carrousel-colecciones');
      if ($carousel.length && $carousel.find('.item').length > 0) {
        $carousel.slick({
          slidesToShow: 4,
          slidesToScroll: 1,
          autoplay: true,
          autoplaySpeed: 2000,
          pauseOnHover: false,
          swipeToSlide: true,
          responsive: [
            {
              breakpoint: 840,
              settings: {
                slidesToShow: 3
              }
            },
            {
              breakpoint: 520,
              settings: {
                slidesToShow: 2
              }
            }
          ]
        });
      } else {
        console.error('El elemento .carrousel-colecciones no se encontró o no tiene elementos.');
      }
    }, 300); // Ajusta el tiempo si es necesario
  }
  
}
