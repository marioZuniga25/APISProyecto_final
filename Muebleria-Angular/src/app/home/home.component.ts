import { Component, AfterViewInit, OnInit } from '@angular/core';
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

  constructor(private productosService: ProductosService) {}
  
  ngOnInit(): void {
    this.productosService.getAllProductos().subscribe(
      (data) => {
        this.productos = data;
      },
      (error) => {
        console.error('Error al obtener los productos', error);
      }
    );
  }

  ngAfterViewInit(): void {
    // Initialize the carousel if needed
    $('.carrousel-colecciones').slick({
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
  }
}
