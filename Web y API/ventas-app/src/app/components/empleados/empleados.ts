import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Empleado, CrearEmpleado } from '../../models/empleado';
import { EmpleadoService } from '../../services/empleado';
import { VentaService } from '../../services/venta';
import { DummyApiService } from '../../services/dummy-api';
import { FechaConMasVentas } from '../../models/venta';
import { DummyApiResponse } from '../../models/dummy-api';
import { LoadingState } from '../../models/loading-state';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-empleados',
  templateUrl: './empleados.html',
  styleUrls: ['./empleados.css'],
  imports: [CommonModule, FormsModule]
})
export class EmpleadosComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  // Estado de los empleados
  empleados: Empleado[] = [];
  empleadosActivos: Empleado[] = [];
  empleadosInactivos: Empleado[] = [];

  // Estado del formulario
  nuevoEmpleado: CrearEmpleado = {
    nombre: '',
    apellido: '',
    estado: 'activo'
  };
  
  editandoEmpleado: Empleado | null = null;
  empleadoEditado: CrearEmpleado = {
    nombre: '',
    apellido: '',
    estado: 'activo'
  };

  // Estados de loading
  loadingState: LoadingState = {
    loading: false,
    error: null
  };

  // Datos del punto 4 (fecha con más ventas)
  fechaConMasVentas: FechaConMasVentas | null = null;

  // Datos del punto 6 (API externa)
  datosDummy: DummyApiResponse | null = null;
  loadingDummy = false;
  errorDummy: string | null = null;

  // Filtros y vista
  filtroEstado: 'todos' | 'activo' | 'inactivo' = 'todos';
  mostrarFormulario = false;

  constructor(
    private empleadoService: EmpleadoService,
    private ventaService: VentaService,
    private dummyApiService: DummyApiService
  ) {}

  ngOnInit(): void {
    this.cargarDatos();
    this.suscribirAEmpleados();
    //debugger;
    this.obtenerFechaConMasVentas();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // Suscribirse a los cambios de empleados
  private suscribirAEmpleados(): void {
    this.empleadoService.empleados$
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (empleados) => {
          this.empleados = empleados;
          this.filtrarEmpleados();
        },
        error: (error) => {
          this.loadingState.error = 'Error al cargar empleados';
          console.error('Error:', error);
        }
      });
  }

  // Cargar datos iniciales
  private cargarDatos(): void {
    this.loadingState.loading = true;
    this.loadingState.error = null;

    this.empleadoService.cargarEmpleados();
    this.loadingState.loading = false;
  }

  // Filtrar empleados por estado
  private filtrarEmpleados(): void {
    this.empleadosActivos = this.empleados.filter(e => e.estado === 'activo');
    this.empleadosInactivos = this.empleados.filter(e => e.estado === 'inactivo');
  }

  // Obtener empleados filtrados según la selección actual
  get empleadosFiltrados(): Empleado[] {
    switch (this.filtroEstado) {
      case 'activo':
        return this.empleadosActivos;
      case 'inactivo':
        return this.empleadosInactivos;
      default:
        return this.empleados;
    }
  }

  // Cambiar filtro de estado
  cambiarFiltro(estado: 'todos' | 'activo' | 'inactivo'): void {
    this.filtroEstado = estado;
  }

  // Mostrar/ocultar formulario
  toggleFormulario(): void {
    this.mostrarFormulario = !this.mostrarFormulario;
    if (!this.mostrarFormulario) {
      this.resetFormulario();
    }
  }

  // Reset del formulario
  resetFormulario(): void {
    this.nuevoEmpleado = {
      nombre: '',
      apellido: '',
      estado: 'activo'
    };
    this.editandoEmpleado = null;
    this.empleadoEditado = {
      nombre: '',
      apellido: '',
      estado: 'activo'
    };
  }

  // Validar formulario
  private validarFormulario(empleado: CrearEmpleado): boolean {
    if (!empleado.nombre.trim()) {
      alert('El nombre es requerido');
      return false;
    }
    if (!empleado.apellido.trim()) {
      alert('El apellido es requerido');
      return false;
    }
    return true;
  }

  // Crear empleado
  crearEmpleado(): void {
    if (!this.validarFormulario(this.nuevoEmpleado)) {
      return;
    }

    this.loadingState.loading = true;
    
    this.empleadoService.crear(this.nuevoEmpleado)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (empleado) => {
          if (empleado) {
            alert('Empleado creado exitosamente');
            this.resetFormulario();
            this.mostrarFormulario = false;
          }
          this.loadingState.loading = false;
        },
        error: (error) => {
          this.loadingState.error = 'Error al crear empleado';
          this.loadingState.loading = false;
        }
      });
  }

  // Editar empleado
  editarEmpleado(empleado: Empleado): void {
    this.editandoEmpleado = empleado;
    this.empleadoEditado = {
      nombre: empleado.nombre,
      apellido: empleado.apellido,
      estado: empleado.estado
    };
  }

  // Actualizar empleado
  actualizarEmpleado(): void {
    if (!this.editandoEmpleado || !this.validarFormulario(this.empleadoEditado)) {
      return;
    }

    this.loadingState.loading = true;
    
    this.empleadoService.actualizar(this.editandoEmpleado.idEmpleado, this.empleadoEditado)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (empleado) => {
          if (empleado) {
            alert('Empleado actualizado exitosamente');
            this.resetFormulario();
          }
          this.loadingState.loading = false;
        },
        error: (error) => {
          this.loadingState.error = 'Error al actualizar empleado';
          this.loadingState.loading = false;
        }
      });
  }

  // Cancelar edición
  cancelarEdicion(): void {
    this.resetFormulario();
  }

  // Eliminar empleado
  eliminarEmpleado(empleado: Empleado): void {
    if (confirm(`¿Está seguro de eliminar al empleado ${empleado.nombreCompleto}?`)) {
      this.loadingState.loading = true;
      
      this.empleadoService.eliminar(empleado.idEmpleado)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (success) => {
            if (success) {
              alert('Empleado eliminado exitosamente');
            }
            this.loadingState.loading = false;
          },
          error: (error) => {
            this.loadingState.error = 'Error al eliminar empleado';
            this.loadingState.loading = false;
          }
        });
    }
  }

  // Punto 4: Obtener fecha con más ventas
  private obtenerFechaConMasVentas(): void {
    this.ventaService.obtenerFechaConMasVentas()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (fecha) => {
          this.fechaConMasVentas = fecha;
        },
        error: (error) => {
          console.error('Error al obtener fecha con más ventas:', error);
        }
      });
  }

  // Punto 6: Consumir API externa
  consultarApiExterna(): void {
    this.loadingDummy = true;
    this.datosDummy = null;
    this.errorDummy = null;
    
    this.dummyApiService.obtenerDatosDummy()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (datos) => {
          if (datos) {
            this.datosDummy = datos;
            console.log('Datos obtenidos de la API:', datos);
          } else {
            console.warn('La API no devolvió datos');
            this.errorDummy = 'La API no devolvió datos válidos';
          }
          this.loadingDummy = false;
        },
        error: (error) => {
          console.error('Error al consultar API externa:', error);
          this.loadingDummy = false;
          this.errorDummy = `Error al consultar la API externa: ${error.message || 'Error desconocido'}`;
        }
      });
  }

  // Formatear fecha para mostrar
  formatearFecha(fechaString: string): string {
    try {
      const fecha = new Date(fechaString);
      return fecha.toLocaleDateString('es-AR');
    } catch {
      return fechaString;
    }
  }

  // TrackBy function para optimizar la lista de empleados
  trackByEmpleado(index: number, empleado: Empleado): number {
    return empleado.idEmpleado;
  }
}