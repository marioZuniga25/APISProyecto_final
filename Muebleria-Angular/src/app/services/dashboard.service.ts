import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IContacto } from '../interfaces/IContactoResponse';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  private apiUrl = `${environment.endpoint}Dashboard`;

  constructor(private http: HttpClient) {}

  // Obtener total de ventas
    getTotalVentas(): Observable<{ totalVentas: number }> {
    return this.http.get<{ totalVentas: number }>(`${this.apiUrl}/ventas-totales`);
  }

  // Obtener productos más vendidos
    getProductosMasVendidos(): Observable<{ nombreProducto: string, totalVendidos: number }[]> {
    return this.http.get<{ nombreProducto: string, totalVendidos: number }[]>(`${this.apiUrl}/productos-mas-vendidos`);
  }

  // Obtener usuarios activos
    getUsuariosActivos(): Observable<{ usuariosActivos: number }> {
    return this.http.get<{ usuariosActivos: number }>(`${this.apiUrl}/usuarios-activos`);
  }

  // Obtener productos por categoríaget
    ProductosPorCategoria(): Observable<{ nombreCategoria: string, totalProductos: number }[]> {
    return this.http.get<{  nombreCategoria: string, totalProductos: number }[]>(`${this.apiUrl}/productos-por-categoria`);
  }
}