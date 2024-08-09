import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IProducto } from '../interfaces/IProducto';
import { IVenta } from '../interfaces/IVenta';
import { IDetalleVenta } from '../interfaces/IDetalleVenta';
import { IProductoResponse } from '../interfaces/IProductoResponse';

@Injectable({
  providedIn: 'root'
})
export class VentasService {

  

  private _endpoint: string = environment.endpoint;
  private _apiUrlP: string = this._endpoint + 'Producto/';
  private _apiUrlV: string = this._endpoint + 'Ventas/';


  constructor(private _http: HttpClient) {}

    
    getList(): Observable<IProductoResponse[]>{
      return this._http.get<IProductoResponse[]>( `${this._apiUrlP}ListadoProductos`);
    }

    addVenta(request: IVenta): Observable<number> {
      return this._http.post<number>(`${this._apiUrlV}AgregarVenta`, request);
    }

    addDetalleVenta(request: IDetalleVenta[]): Observable<void> {
      const headers = new HttpHeaders({
        'Content-Type': 'application/json'
      });
  
      return this._http.post<void>(`${this._apiUrlV}AgregarDetalleVenta`, request);
    }

    filtrarProductos(request: string): Observable<IProductoResponse[]> {
      return this._http.get<IProductoResponse[]>(`${this._apiUrlP}FiltrarProductos?term=${request}`);
    }

    /*search(name: string): Observable<IProducto[]>{
      return this._http.get<IProducto[]>( `${this._apiUrl}search?name=${name}`);
    }*/
}
