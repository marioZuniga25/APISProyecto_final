import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IProductoRequest, IProductoResponse } from '../../interfaces/IProductoResponse';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ProductosService {

  _apiUrl: string = environment.endpoint;

  constructor(private http: HttpClient) { }

  getAllProductos(): Observable<any[]> {
    return this.http.get<any[]>(`${this._apiUrl}producto/ListadoProductos`);
  }

  getProductoById(id: number): Observable<any> {
    const url = `${this._apiUrl}producto/${id}`;
    return this.http.get<any>(url);
  }

  getProductos(request: string): Observable<IProductoResponse[]> {
    return this.http.get<IProductoResponse[]>(`${this._apiUrl}producto/FiltrarProductos?term=${request}`);
  }

  addProducto(request: IProductoRequest): Observable<IProductoRequest> {
    return this.http.post<IProductoRequest>(`${this._apiUrl}Producto/Agregar`, request);
  }

  updateProducto(id: number, request: IProductoRequest): Observable<IProductoRequest> {
    return this.http.post<any>(`${this._apiUrl}Producto/Modificar/${id}`, request);
  }

  deleteProducto(id: number): Observable<any> {
    return this.http.delete<any>(`${this._apiUrl}Producto/Eliminar/${id}`);
  }
}
