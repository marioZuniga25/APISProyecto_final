import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AsyncPipe, NgForOf, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DetalleUsuarioComponent } from "../detalle-usuario/detalle-usuario.component";
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-usuarios',
  standalone: true,
  imports: [AsyncPipe, NgForOf, NgIf, FormsModule, DetalleUsuarioComponent],
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.css']
})
export class UsuariosComponent {
  user$ = this.authService.getAllUsuarios();
  usuarioSeleccionado$: Observable<any> | null = null; // Observable para el usuario seleccionado

  constructor(private authService: AuthService) { }

  verDetalle(idUsuario: number): void {
    this.usuarioSeleccionado$ = this.authService.getUsuarioById(idUsuario);
  }

  actualizarUsuario(): void {
    this.user$ = this.authService.getAllUsuarios(); // Refrescar la lista de usuarios después de la actualización
    this.usuarioSeleccionado$ = null; // Limpiar la selección para ocultar el formulario
  }

  cancelar(): void {
    this.usuarioSeleccionado$ = null; // Limpiar la selección para ocultar el formulario
  }

  trackById(index: number, usuario: any): number {
    return usuario.idUsuario;
  }

    actualizarRol(idUsuario: number, nuevoRol: number): void {
    this.authService.updateUsuarioRol(idUsuario, nuevoRol).subscribe(
      (response) => {
        console.log('Rol actualizado con éxito', response);
      },
      (error) => {
        console.error('Error al actualizar el rol', error);
      }
    );
  }
}



