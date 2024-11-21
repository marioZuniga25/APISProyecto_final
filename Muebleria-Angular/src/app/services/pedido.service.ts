import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IPedidos, IPedidosResponse } from '../interfaces/IPedidos';

@Injectable({
  providedIn: 'root',
})
export class PedidoService {
  private apiUrl = 'http://localhost:5194/api/Pedido'; // Define tu URL base para las APIs
  constructor(private http: HttpClient) {}

  getPedidos(filtro: string): Observable<IPedidosResponse[]> {
    return this.http.get<IPedidosResponse[]>(`${this.apiUrl}/GetPedidos/${filtro}`);
  }

  // Agregar un nuevo pedido
  guardarPedido(pedido: IPedidos): Observable<IPedidos> {
    return this.http.post<IPedidos>(this.apiUrl, pedido);
  }
  

  // Actualizar el estatus de un pedido
  updatePedido(id: number, pedido: Partial<IPedidos>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, pedido);
  }
  

}
