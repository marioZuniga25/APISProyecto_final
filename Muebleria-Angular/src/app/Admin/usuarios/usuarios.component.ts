import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AsyncPipe, NgForOf, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DetalleUsuarioComponent } from "../detalle-usuario/detalle-usuario.component";
import { AuthService } from '../../services/auth.service';
import { BuscadorUsuarioComponent } from "../buscador-usuario/buscador-usuario.component";
import { IUsuarioDetalle } from '../../interfaces/IUsuarioDetalle';

@Component({
  selector: 'app-usuarios',
  standalone: true,
  imports: [AsyncPipe, NgForOf, NgIf, FormsModule, DetalleUsuarioComponent, BuscadorUsuarioComponent],
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.css']
})
export class UsuariosComponent implements OnInit {
  user$: Observable<IUsuarioDetalle[]> = of([]);
  filteredUsers: IUsuarioDetalle[] = [];
  usuarioSeleccionado$: Observable<IUsuarioDetalle> | null = null;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.user$ = this.authService.getAllUsuarios();
    this.user$.subscribe(users => this.filteredUsers = users); // Inicializar con todos los usuarios
  }

  onSearch(users: IUsuarioDetalle[]): void {
    // Actualizar directamente con los resultados
    this.filteredUsers = users.length > 0 ? users : [];
  }

  actualizarUsuario(): void {
    this.authService.getAllUsuarios().subscribe(users => this.filteredUsers = users);
    this.usuarioSeleccionado$ = null;
  }

  cancelar(): void {
    this.usuarioSeleccionado$ = null;
  }

  trackById(index: number, usuario: IUsuarioDetalle): number {
    return usuario.idUsuario;
  }

  verDetalle(idUsuario: number): void {
    this.usuarioSeleccionado$ = this.authService.getUsuarioById(idUsuario);
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
