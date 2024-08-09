import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { RouterOutlet } from '@angular/router';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { BuscadorComponent } from './buscador/buscador.component';
import { CarritoComponent } from './carrito/carrito.component';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MenuComponent } from './Admin/menu/menu.component';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FooterComponent, HeaderComponent, BuscadorComponent, CarritoComponent, NgIf, FormsModule, MenuComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Muebleria-Angular';
  mostrarComponentes: boolean = true;
  mostrarBag : boolean = false;

  constructor(private router: Router) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.mostrarComponentes = !event.url.includes('/admin');
      }
    });
  }
  toggleBag() {
    this.mostrarBag = !this.mostrarBag;
  }
}
