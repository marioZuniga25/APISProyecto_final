import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductosService {

  private apiUrl = 'http://localhost:5194/api/Producto/ListadoProductos';

  constructor(private http: HttpClient) { }

  getAllProductos(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
  getProductoById(id: number): Observable<any> {
    const url = `http://localhost:5194/api/Producto/${id}`;
    return this.http.get<any>(url);
  }
}
