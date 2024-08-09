import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CarritoService {
  private mostrarBagSubject = new BehaviorSubject<boolean>(false);
  mostrarBag$ = this.mostrarBagSubject.asObservable();
  private apiUrl = 'http://localhost:5194/api/Carrito';
  private itemsCarritoSubject = new BehaviorSubject<any[]>([]);
  itemsCarrito$ = this.itemsCarritoSubject.asObservable();

  constructor(private http: HttpClient) {}

  toggleBag() {
    this.mostrarBagSubject.next(!this.mostrarBagSubject.value);
  }

  setBagState(state: boolean) {
    this.mostrarBagSubject.next(state);
  }

  getCarrito(): Observable<any[]> {
    return this.itemsCarrito$;
  }

  agregarProductoAlCarrito(producto: any, cantidad: number): Observable<any> {
    const carritoItem = {
      productoId: producto.idProducto,
      nombreProducto: producto.nombreProducto,
      cantidad: cantidad,
      precio: producto.precio,
      imagen: producto.imagen 
    };
  
    return this.http.post(`${this.apiUrl}/Agregar`, carritoItem);
  }
  

  obtenerCarrito(): void {
    this.http.get<any[]>(`${this.apiUrl}/Obtener`).subscribe(
      (items) => {
        this.itemsCarritoSubject.next(items || []);
      },
      (error) => console.error('Error al obtener el carrito', error)
    );
  }
  
  actualizarCantidadProducto(productId: number, cantidad: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/Actualizar`, { idProducto: productId, cantidad });
  }

  eliminarProductoDelCarrito(productId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Eliminar/${productId}`);
  }
}
