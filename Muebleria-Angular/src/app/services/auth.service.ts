import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable, switchMap } from 'rxjs';
import { IUsuarioDetalle } from '../interfaces/IUsuarioDetalle';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl : string = environment.endpoint;

  constructor(private http: HttpClient) { }

  // Obtener detalles de un usuario por ID
  getUsuarioById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}usuario/DetalleUsuario/${id}`);
  }

  // Obtener todos los usuarios
  getAllUsuarios = (): Observable<IUsuarioDetalle[]> =>
    this.http.get<any[]>(`${this.apiUrl}usuario/listado`);
  

  // Actualizar el rol de un usuario (0 = usuario, 1 = administrador)
  updateUsuario(idUsuario: number, nuevoRol: number): Observable<any> {
    // Primero, obtenemos el usuario por su ID
    return this.getUsuarioById(idUsuario).pipe(
      // Luego, usamos el operador 'switchMap' para actualizar el rol del usuario y enviarlo de vuelta al servidor
      switchMap((usuario: any) => {
        // Modificamos el rol del usuario
        usuario.rol = nuevoRol;
  
        // Ahora, enviamos el usuario completo con el rol actualizado
        return this.http.put(`${this.apiUrl}usuario/ModificarUsuario/${idUsuario}`, usuario);
      })
    );
  }
  
}
