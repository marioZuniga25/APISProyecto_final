import { Injectable } from '@angular/core';
import { environment,python } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, switchMap, forkJoin  } from 'rxjs';
import { IUsuarioDetalle } from '../interfaces/IUsuarioDetalle';
import { AuthResponse, User } from '../interfaces/AuthResponse';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl: string = environment.endpoint;
  apiPython: string = python.endpoint;
  private userKey = "currentUser"; // Asegúrate de tener una clave única
  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser: Observable<User | null>;


  constructor(private http: HttpClient) { 
    const storedUser = JSON.parse(localStorage.getItem(this.userKey) || 'null');
    this.currentUserSubject = new BehaviorSubject<User | null>(storedUser);
    this.currentUser = this.currentUserSubject.asObservable();
  }
  // Obtener detalles de un usuario por ID
  getUsuarioById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}usuario/DetalleUsuario/${id}`);
  }

  setUser(user: User): void {
    try {
      const userString = JSON.stringify(user);
      localStorage.setItem(this.userKey, userString);
      this.currentUserSubject.next(user); // Notifica a los suscriptores del cambio
    } catch (error) {
      console.error('Error al guardar en LocalStorage:', error);
    }
  }
  

  createUsuario(usuario: IUsuarioDetalle): Observable<any> {
    return this.http.post(`${this.apiUrl}usuario/registrarInterno`, usuario);
  }
  
  resetPassword(token: string, nuevaContrasenia: string): Observable<any> {
    const body = { token, nuevaContrasenia };
    return this.http.post(`${this.apiUrl}usuario/reset-password`, body);
  }
  
  validateToken(token: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}usuario/validate-token?token=${token}`);
  }

  //Saber si es admin
  isAdmin(): boolean {
    const user = this.getUser();
    return user?.rol === 1; 
  }

  //Saber si está logueado
  isLoggedIn(): boolean {
    return this.getUser() !== null;
  }

  // Obtener todos los usuarios
  getAllUsuarios = (): Observable<{ externos: IUsuarioDetalle[], internos: IUsuarioDetalle[] }> =>
    this.http.get<{ externos: IUsuarioDetalle[], internos: IUsuarioDetalle[] }>(`${this.apiUrl}usuario/listado`);

  registerUsuario (data: IUsuarioDetalle): Observable<any>{
    return this.http.post<any>(`${this.apiUrl}usuario/registrar`, data);
  }

  forgotPassword(correo: string): Observable<any> {
    return this.http.post(`${this.apiUrl}usuario/forgot-password`, { correo });
  }

  getUser(): User | null {
    try {
      const storedUser = localStorage.getItem(this.userKey);
      if (storedUser) {
        return JSON.parse(storedUser);
      }
      return null;
    } catch (error) {
      console.error('Error al leer desde LocalStorage:', error);
      return null;
    }
  }
  

  removeUser(): void {
    localStorage.removeItem(this.userKey);
    this.currentUserSubject.next(null); // Notifica a los suscriptores del cambio
  }
  
 // auth.service.ts en Angular
login(data: IUsuarioDetalle): Observable<AuthResponse> {
  // Llamadas a ambas APIs
  const dotnetLogin = this.http.post<AuthResponse>(`${this.apiUrl}usuario/login`, data);

  // Solo enviar los datos necesarios para Python, sin la contraseña
  const pythonLogin = dotnetLogin.pipe(
    switchMap(dotnetResponse => {
      if (dotnetResponse && dotnetResponse.user) {
        return this.http.post<any>(`${this.apiPython}login`, {
          user_id: dotnetResponse.user.idUsuario,
          username: dotnetResponse.user.nombreUsuario,
          rol: dotnetResponse.user.rol
        }, { withCredentials: true });
      } else {
        throw new Error('Usuario no autenticado en .NET');
      }
    })
  );

  // Ejecutar ambas llamadas en paralelo
  return forkJoin([dotnetLogin, pythonLogin]).pipe(
    map(([dotnetResponse, pythonResponse]) => {
      // Manejar la respuesta de .NET (dotnetResponse)
      if (dotnetResponse && dotnetResponse.user) {
        console.log('Respuesta de .NET:', dotnetResponse.user);
        this.setUser(dotnetResponse.user); // Guardar el usuario en LocalStorage
      }

      // Manejar la respuesta de Python (pythonResponse)
      if (pythonResponse && pythonResponse.status === 'success') {
        console.log('Sala creada exitosamente en la API de Python');
      } else {
        console.error('Error al crear la sala en la API de Python');
      }

      return dotnetResponse; // Retorna solo la respuesta de .NET
    })
  );
}

  

  searchUsuariosPorNombre(nombre: string): Observable<IUsuarioDetalle[]> {
    return this.http.get<IUsuarioDetalle[]>(`${this.apiUrl}usuario/BuscarPorNombre?nombre=${nombre}`);
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

  // Método para obtener el último inicio de sesión de un usuario
  getUltimoInicioSesion(userId: number): Observable<{ fechaInicioSesion: Date }> {
    return this.http.get<{ fechaInicioSesion: Date }>(`${this.apiUrl}usuario/UltimoInicioSesion/${userId}`);
  }
  
}
