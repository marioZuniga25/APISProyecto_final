import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { IUsuarioDetalle } from '../../interfaces/IUsuarioDetalle';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { AuthResponse } from '../../interfaces/AuthResponse';

@Component({
  selector: 'app-login-usuario',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login-usuario.component.html',
  styleUrl: './login-usuario.component.css'
})
export class LoginUsuarioComponent {
  loginData: IUsuarioDetalle = {
    idUsuario: 0,
    nombreUsuario: '',
    correo: '',
    contrasenia: '',
    rol: 0,
  };
  errorMessage: string = '';
  idUsuarioLocal: string = '';
  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(form: NgForm): void {
    if (form.invalid) {
      return;
    }

    this.authService.login(this.loginData).subscribe(
      (response: AuthResponse) => {
        if (response && response.user) {
          this.idUsuarioLocal = response.user.idUsuario.toString();
          console.log(this.idUsuarioLocal);
          localStorage.setItem('userId', this.idUsuarioLocal);
          // Redirigir a la página principal después de iniciar sesión
          this.router.navigate(['/']);
        }
      },
      (error) => {
        console.error('Error en el inicio de sesión', error);
        this.errorMessage = 'Correo o contraseña incorrectos. Inténtalo de nuevo.';
      }
    );
  }
}

