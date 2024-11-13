import { Component,CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { RouterOutlet } from '@angular/router';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { BuscadorComponent } from './buscador/buscador.component';
import { CarritoComponent } from './carrito/carrito.component';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MenuComponent } from './Admin/menu/menu.component';
import { AuthService } from './services/auth.service';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FooterComponent, HeaderComponent, BuscadorComponent, CarritoComponent, NgIf, FormsModule, MenuComponent, RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppComponent {
  title = 'Muebleria-Angular';
  mostrarComponentes: boolean = true;
  mostrarBag : boolean = false;
  isChatOpen: boolean = false;
  mostrarChat: boolean = true;

  constructor(private router: Router, private authService: AuthService) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.mostrarComponentes = !event.url.includes('/admin');
        this.mostrarChat = this.authService.isLoggedIn() && !event.url.includes('/login') && !event.url.includes('/admin');
        
      }
    });
  }
  toggleBag() {
    this.mostrarBag = !this.mostrarBag;
  }
  toggleChat() {
    this.isChatOpen = !this.isChatOpen;
  }
}
