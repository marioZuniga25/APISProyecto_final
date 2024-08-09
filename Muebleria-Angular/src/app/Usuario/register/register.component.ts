import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { IUsuarioDetalle } from '../../interfaces/IUsuarioDetalle';
import { Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  usuario: IUsuarioDetalle = {
    idUsuario: 0,
    nombreUsuario: '',
    correo: '',
    contrasenia: '',
    rol: 0,
    confirmPassword: '',
  };

  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  registrarUsuario(form: NgForm): void {
    if (form.invalid) {
      return;
    }

    if (this.usuario.contrasenia !== this.usuario.confirmPassword) {
      this.errorMessage = 'Las contraseñas no coinciden.';
      return;
    }

    this.authService.registerUsuario(this.usuario).subscribe(
      response => {
        console.log('Usuario registrado con éxito', response);
        this.router.navigate(['/login']); // Redirige al login después del registro
      },
      error => {
        console.error('Error al registrar el usuario', error);
        this.errorMessage = 'Hubo un problema al registrar el usuario. Inténtalo de nuevo.';
      }
    );
  }
}

