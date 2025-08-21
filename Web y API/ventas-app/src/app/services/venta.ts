import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { Venta, FechaConMasVentas } from '../models/venta';

@Injectable({
  providedIn: 'root'
})
export class VentaService {
  private readonly apiUrl = 'https://localhost:7299/api/ventas';
  
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  // Obtener fecha con más ventas (Punto 2 del examen)
  obtenerFechaConMasVentas(): Observable<FechaConMasVentas | null> {
    return this.http.get<FechaConMasVentas>(`${this.apiUrl}/fecha-mas-ventas`, this.httpOptions)
      .pipe(
        catchError(this.handleError<FechaConMasVentas>('obtenerFechaConMasVentas'))
      );
  }

  // Obtener todas las ventas
  obtenerTodas(): Observable<Venta[]> {
    return this.http.get<Venta[]>(this.apiUrl, this.httpOptions)
      .pipe(
        map(response => response || []),
        catchError(this.handleError<Venta[]>('obtenerTodas', []))
      );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return new Observable<T>(observer => {
        observer.next(result as T);
        observer.complete();
      });
    };
  }
}
