import { Injectable } from '@angular/core';
import { IMateriaPrima } from '../interfaces/IMateriaPrima';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IProveedorResponse } from '../interfaces/IProveedorResponse';

@Injectable({
  providedIn: 'root'
})
export class MateriaprimaService {

  private _endpoint: string = environment.endpoint;
  private _apiUrlMP: string = this._endpoint + 'MateriaPrima/';


  constructor(private _http: HttpClient) {}

    
    getList(): Observable<IMateriaPrima[]>{
      return this._http.get<IMateriaPrima[]>( `${this._apiUrlMP}ListadoMateriasP`);
    }

    addMateriaPrima(request: IMateriaPrima): Observable<number> {
      return this._http.post<number>(`${this._apiUrlMP}AgregarMateriaP`, request);
    }

    updateMateriaPrima(request: IMateriaPrima): Observable<void>{
      return this._http.put<void>( `${this._apiUrlMP}ModificarMateriaP?id=${request.idMateriaPrima}`, request);
    }

    deleteMateriaPrima(request: IMateriaPrima): Observable<void>{
      return this._http.delete<void>( `${this._apiUrlMP}EliminarMateriaP/${request.idMateriaPrima}`);
    }

    getProveedores(): Observable<IProveedorResponse[]>{
      return this._http.get<IProveedorResponse[]>(`${this._apiUrlMP}ListadoProveedores`);
    }

}
