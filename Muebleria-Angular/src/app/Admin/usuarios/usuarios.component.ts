import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AsyncPipe, NgForOf, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DetalleUsuarioComponent } from "../detalle-usuario/detalle-usuario.component";
import { AuthService } from '../../services/auth.service';
import { IUsuarioDetalle } from '../../interfaces/IUsuarioDetalle';
import { BuscadorCompartidoComponent } from '../shared/buscador-compartido/buscador-compartido.component';

@Component({
  selector: 'app-usuarios',
  standalone: true,
  imports: [AsyncPipe, NgForOf, NgIf, FormsModule, DetalleUsuarioComponent, BuscadorCompartidoComponent],
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.css']
})
export class UsuariosComponent implements OnInit {
  usuarios: IUsuarioDetalle[] = []; // Mantiene la lista completa de usuarios
  resultadosBusqueda: IUsuarioDetalle[] = []; // Propiedad para almacenar los resultados de la búsqueda
  usuarioSeleccionado$: Observable<IUsuarioDetalle> | null = null;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.getUsuarios();
  }

  getUsuarios(): void {
    this.authService.getAllUsuarios().subscribe(
      (data: IUsuarioDetalle[]) => {
        this.usuarios = data;  // Asigna todos los usuarios a la lista completa
        this.resultadosBusqueda = data; // Inicializa con todos los usuarios
      },
      (error) => {
        console.error('Error al obtener los usuarios', error);
      }
    );
  }

  onSearchResults(resultados: IUsuarioDetalle[]): void {
    this.resultadosBusqueda = resultados;
  }

  actualizarUsuario(): void {
    this.authService.getAllUsuarios().subscribe(users => this.resultadosBusqueda = users);
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
