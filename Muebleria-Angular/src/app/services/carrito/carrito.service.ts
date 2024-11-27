import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Observable, throwError } from 'rxjs';
import Swal from 'sweetalert2';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
export interface ProductoCarrito {
  idDetalleCarrito: number;
  id: number;
  nombre: string;
  precio: number;
  cantidad: number;
  imagen: string;
  stock: number;
  fechaAgregado?: Date; 
  descuento?: number;
}
@Injectable({
  providedIn: 'root'
})
export class CarritoService {
  private apiUrl = 'http://localhost:5194/api/carrito'; 
  private carrito: ProductoCarrito[] = [];
  private mostrarBagSubject = new BehaviorSubject<boolean>(false);
  mostrarBag$ = this.mostrarBagSubject.asObservable();
  constructor(private http: HttpClient) {
  }
  agregarAlCarrito(idUsuario: number, producto: ProductoCarrito): Observable<any> {
    const detalle = {
      IdProducto: producto.id,
      Cantidad: producto.cantidad,
      PrecioUnitario: producto.precio,
      FechaAgregado: producto.fechaAgregado || new Date() 
    };
    return this.http.post(`${this.apiUrl}/${idUsuario}/agregar`, detalle);
  }
  obtenerCarrito(idUsuario: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${idUsuario}/carrito`);
  }
  incrementarCantidad(idDetalleCarrito: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/incrementar/${idDetalleCarrito}`, {});
  }
  
  decrementarCantidad(idDetalleCarrito: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/decrementar/${idDetalleCarrito}`, {});
  }
  eliminarProducto(idDetalleCarrito: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/eliminar/${idDetalleCarrito}`);
  }  
  limpiarCarrito(idUsuario: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${idUsuario}/limpiar`);
  }  
  toggleBag() {
    this.mostrarBagSubject.next(!this.mostrarBagSubject.value);
  }
}