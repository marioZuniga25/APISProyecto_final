import { NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { CarritoService } from '../services/carrito/carrito.service';

@Component({
  selector: 'app-bag',
  standalone: true,
  imports: [NgIf],
  templateUrl: './bag.component.html',
  styleUrl: './bag.component.css'
})
export class BagComponent implements OnInit {
  mostrarBag: boolean = false;

  constructor(private carritoService: CarritoService) {}

  ngOnInit() {
    this.carritoService.mostrarBag$.subscribe(state => {
      this.mostrarBag = state;
    });
  }
}
