import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { DummyApiResponse } from '../models/dummy-api';

@Injectable({
  providedIn: 'root'
})
export class DummyApiService {
  private readonly dummyApiUrl = 'https://svct.cartasur.com.ar/api/dummy';

  constructor(private http: HttpClient) { }

  // Consumir la API externa (Punto 6 del examen)
  obtenerDatosDummy(): Observable<DummyApiResponse | null> {
    return this.http.get<DummyApiResponse>(this.dummyApiUrl)
      .pipe(
        catchError(this.handleError<DummyApiResponse>('obtenerDatosDummy'))
      );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      
      // En caso de error devolver null
      return of(null as T);
    };
  }
}