import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { IProductoResponse } from '../interfaces/IProductoResponse';

@Injectable({
  providedIn: 'root'
})
export class ProductosService {

  _apiUrl: string = environment.endpoint;

  constructor(private _http: HttpClient) { }

  getProductos(request: string): Observable<IProductoResponse[]> {
    return this._http.get<IProductoResponse[]>(`${this._apiUrl}producto/FiltrarProductos?term=${request}`);
  }

  getAllProductos(): Observable<IProductoResponse[]> {
    return this._http.get<IProductoResponse[]>(`${this._apiUrl}producto/ListadoProductos`);
  }


}
