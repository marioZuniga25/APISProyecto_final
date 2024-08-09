import { Component } from '@angular/core';
import { User } from '../interfaces/AuthResponse';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BuscadorComponent } from '../buscador/buscador.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, BuscadorComponent],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  user: User | null = null;
  isDropdownOpen = false;
  isBuscadorVisible: boolean = false;
  
  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.authService.currentUser.subscribe(user => {
      this.user = user;
    });
  }

  toggleDropdown(event: Event): void {
    event.preventDefault();
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  toggleBuscador(): void {
    this.isBuscadorVisible = !this.isBuscadorVisible;
  }

  logout(): void {
    this.authService.removeUser();
    this.router.navigate(['/login']);
  }
}
