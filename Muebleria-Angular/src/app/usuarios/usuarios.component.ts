import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AsyncPipe, NgForOf, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EnvioComponent } from "../envio/envio.component";
import { DetalleComponent } from "../detalle/detalle.component";
import { DetalleUsuarioComponent } from "../detalle-usuario/detalle-usuario.component";

@Component({
  selector: 'app-usuarios',
  standalone: true,
  imports: [AsyncPipe, NgForOf, NgIf, FormsModule, EnvioComponent, DetalleComponent, DetalleUsuarioComponent],
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.css']
})
export class UsuariosComponent {
  user$ = this.authService.getAllUsuarios();

  constructor(private authService: AuthService) { }

  verDetalle(idUsuario: number): void {
    this.authService.getUsuarioById(idUsuario).subscribe(
      (data) => {
        console.log('Detalle del usuario:', data);
      },
      (error) => {
        console.error('Error al obtener detalles del usuario', error);
      }
    );
  }

  actualizarRol(idUsuario: number, nuevoRol: number): void {
    this.authService.updateUsuario(idUsuario, nuevoRol).subscribe(
      (response) => {
        console.log('Rol actualizado con éxito', response);
      },
      (error) => {
        console.error('Error al actualizar el rol', error);
      }
    );
  }

  trackById(index: number, usuario: any): number {
    return usuario.idUsuario;
  }
}
