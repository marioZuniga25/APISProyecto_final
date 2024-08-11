import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { IProveedorResponse } from '../interfaces/IProveedorResponse';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ProveedoresService {

  apiUrl: string = environment.endpoint;

  constructor(private http: HttpClient) { }

  // Obtener todos los proveedores
  getProveedores(): Observable<IProveedorResponse[]> {
    return this.http.get<IProveedorResponse[]>(`${this.apiUrl}proveedores`)
      .pipe(
        retry(1), // Reintenta la solicitud una vez en caso de error
        catchError(this.handleError)
      );
  }

  // Obtener un proveedor por ID
  getProveedor(id: number): Observable<IProveedorResponse> {
    const url = `${this.apiUrl}proveedores/${id}`;
    return this.http.get<IProveedorResponse>(url)
      .pipe(
        retry(1),
        catchError(this.handleError)
      );
  }

  // Crear un nuevo proveedor
  addProveedor(proveedor: IProveedorResponse): Observable<IProveedorResponse> {
    return this.http.post<IProveedorResponse>(`${this.apiUrl}proveedores`, proveedor, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  // Actualizar un proveedor existente
  updateProveedor(id: number, proveedor: IProveedorResponse): Observable<any> {
    const url = `${this.apiUrl}proveedores/${id}`;
    return this.http.put(url, proveedor, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  // Eliminar un proveedor
  deleteProveedor(id: number): Observable<any> {
    const url = `${this.apiUrl}proveedores/${id}`;
    return this.http.delete(url, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  // Manejo de errores
  private handleError(error: any) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Error del lado del cliente
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Error del lado del servidor
      errorMessage = `Código de error: ${error.status}\nMensaje: ${error.message}`;
    }
    window.alert(errorMessage);
    return throwError(errorMessage);
  }

  // Opciones de cabecera para solicitudes HTTP
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
}
