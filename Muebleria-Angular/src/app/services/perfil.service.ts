import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUsuarioDetalle } from '../interfaces/IUsuarioDetalle';
import { IUtarjetas } from '../interfaces/ITarjetas';

@Injectable({
  providedIn: 'root'
})
export class PerfilService {

  private apiUrl = 'http://localhost:5194/api/Usuario';
  private tarjetas = 'http://localhost:5194/api/Tarjetas';

  constructor(private http: HttpClient) { }

  getUserDetails(userId: number): Observable<IUsuarioDetalle> {
    return this.http.get<IUsuarioDetalle>(`${this.apiUrl}/DetalleUsuario/${userId}`);
  }
  getUserCards(userId: number): Observable<IUtarjetas[]> {
    return this.http.get<IUtarjetas[]>(`${this.tarjetas}/usuario/${userId}`);
  }
  addCard(card: IUtarjetas): Observable<IUtarjetas> {
    return this.http.post<IUtarjetas>(this.tarjetas, card);
  }
  deleteCard(cardId: number): Observable<void> {
    return this.http.delete<void>(`${this.tarjetas}/${cardId}`);
  }
  
}
