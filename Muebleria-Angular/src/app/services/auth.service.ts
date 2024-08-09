import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable, switchMap } from 'rxjs';
import { IUsuarioDetalle } from '../interfaces/IUsuarioDetalle';
import { AuthResponse, User } from '../interfaces/AuthResponse';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl : string = environment.endpoint;
  private userKey = "";

  constructor(private http: HttpClient) { }

  // Obtener detalles de un usuario por ID
  getUsuarioById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}usuario/DetalleUsuario/${id}`);
  }

  setUser(user: User): void {
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }

  // Obtener todos los usuarios
  getAllUsuarios = (): Observable<IUsuarioDetalle[]> =>
    this.http.get<any[]>(`${this.apiUrl}usuario/listado`);

  registerUsuario (data: IUsuarioDetalle): Observable<any>{
    return this.http.post<any>(`${this.apiUrl}usuario/registrar`, data);
  }

  getUser(): User | null {
    const userJson = localStorage.getItem(this.userKey);
    return userJson ? JSON.parse(userJson) : null;
  }

  removeUser(){
    localStorage.removeItem(this.userKey);
  }
  
  
  login(data: IUsuarioDetalle): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}usuario/login`, data)
      .pipe(
        map((response: AuthResponse) => {  // Especifica el tipo de response aquí
          if (response && response.user) {
            localStorage.setItem(this.userKey, JSON.stringify(response.user));
          }
          return response;
        })
      );
  }

  // Actualizar el rol de un usuario (0 = usuario, 1 = administrador)
  updateUsuarioRol(idUsuario: number, nuevoRol: number): Observable<any> {
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

  updateUsuario(idUsuario: number, usuario: any): Observable<any> {
    return this.http.put(`${this.apiUrl}usuario/ModificarUsuario/${idUsuario}`, usuario);
  }
  
}
