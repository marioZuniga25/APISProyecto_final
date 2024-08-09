import { Component } from '@angular/core';
import { IUsuarioDetalle } from '../../interfaces/IUsuarioDetalle';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { AuthResponse } from '../../interfaces/AuthResponse';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginData: IUsuarioDetalle = {
    idUsuario: 0,
    nombreUsuario: '',
    correo: '',
    contrasenia: '',
    rol: 0,
  };
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(form: NgForm): void {
    if (form.invalid) {
      return;
    }

    this.authService.login(this.loginData).subscribe(
      (response: AuthResponse) => {
        if (response && response.user) {
          // Actualizar el estado del usuario en el componente (por si se necesita en otros lugares)
          this.user = response.user; 

          // Redirigir a la página principal después de iniciar sesión
            this.router.navigate(['/admin/inicio']).then(() => {
              window.location.reload();
            });
          
        }
      },
      (error) => {
        console.error('Error en el inicio de sesión', error);
        this.errorMessage = 'Correo o contraseña incorrectos. Inténtalo de nuevo.';
      }
    );
  }

  // Declaración de la variable user en el componente, como en MenuComponent
  user: IUsuarioDetalle | null = null;
}
