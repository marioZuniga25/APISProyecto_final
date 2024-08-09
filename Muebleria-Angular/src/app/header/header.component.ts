import { Component } from '@angular/core';
import { CarritoService } from '../services/carrito/carrito.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  constructor(private carritoService: CarritoService) {}

  toggleBag() {
    this.carritoService.toggleBag();
  }
}
