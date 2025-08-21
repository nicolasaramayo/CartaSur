import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { Empleado, CrearEmpleado } from '../models/empleado';
import { ApiResponse } from '../models/api-response';

@Injectable({
  providedIn: 'root'
})
export class EmpleadoService {
  private readonly apiUrl = 'https://localhost:7299/api/empleados'; // to fix
  
  // Estado reactivo para la lista de empleados
  private empleadosSubject = new BehaviorSubject<Empleado[]>([]);
  public empleados$ = this.empleadosSubject.asObservable();

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) {
    this.cargarEmpleados();
  }

  // Obtener todos los empleados
  obtenerTodos(): Observable<Empleado[]> {
    return this.http.get<Empleado[]>(this.apiUrl, this.httpOptions)
      .pipe(
        map(response => response || []),
        catchError(this.handleError<Empleado[]>('obtenerTodos', []))
      );
  }

  // Cargar empleados y actualizar el estado
  cargarEmpleados(): void {
    this.obtenerTodos().subscribe({
      next: (empleados) => {
        this.empleadosSubject.next(empleados);
      },
      error: (error) => {
        console.error('Error al cargar empleados:', error);
        this.empleadosSubject.next([]);
      }
    });
  }

  // Obtener empleados por estado
  obtenerPorEstado(estado: 'activo' | 'inactivo'): Observable<Empleado[]> {
    return this.http.get<Empleado[]>(`${this.apiUrl}/estado/${estado}`, this.httpOptions)
      .pipe(
        map(response => response || []),
        catchError(this.handleError<Empleado[]>('obtenerPorEstado', []))
      );
  }

  // Obtener empleado por ID
  obtenerPorId(id: number): Observable<Empleado | null> {
    return this.http.get<Empleado>(`${this.apiUrl}/${id}`, this.httpOptions)
      .pipe(
        catchError(this.handleError<Empleado>('obtenerPorId'))
      );
  }

  // Crear nuevo empleado
  crear(empleado: CrearEmpleado): Observable<Empleado | null> {
    return this.http.post<Empleado>(this.apiUrl, empleado, this.httpOptions)
      .pipe(
        map(response => {
          if (response) {
            // Actualizar la lista local
            this.cargarEmpleados();
            return response;
          }
          return null;
        }),
        catchError(this.handleError<Empleado>('crear'))
      );
  }

  // Actualizar empleado
  actualizar(id: number, empleado: CrearEmpleado): Observable<Empleado | null> {
    return this.http.put<Empleado>(`${this.apiUrl}/${id}`, empleado, this.httpOptions)
      .pipe(
        map(response => {
          if (response) {
            // Actualizar la lista local
            this.cargarEmpleados();
            return response;
          }
          return null;
        }),
        catchError(this.handleError<Empleado>('actualizar'))
      );
  }

  // Eliminar empleado
  eliminar(id: number): Observable<boolean> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`, this.httpOptions)
      .pipe(
        map(response => {
          // Actualizar la lista local
          this.cargarEmpleados();
          return true;
        }),
        catchError(this.handleError<boolean>('eliminar', false))
      );
  }

  // Manejo de errores genérico
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      
      
      let errorMessage = 'Ha ocurrido un error inesperado';
      
      if (error.error?.mensaje) {
        errorMessage = error.error.mensaje;
      } else if (error.status === 0) {
        errorMessage = 'No se puede conectar con el servidor';
      } else if (error.status === 404) {
        errorMessage = 'Recurso no encontrado';
      } else if (error.status >= 500) {
        errorMessage = 'Error interno del servidor';
      }
      
      
      alert(errorMessage);
      
      return new Observable<T>(observer => {
        observer.next(result as T);
        observer.complete();
      });
    };
  }
}