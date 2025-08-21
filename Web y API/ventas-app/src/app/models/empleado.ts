export interface Empleado {
  idEmpleado: number;
  nombre: string;
  apellido: string;
  estado: 'activo' | 'inactivo';
  nombreCompleto: string;
}

export interface CrearEmpleado {
  nombre: string;
  apellido: string;
  estado: 'activo' | 'inactivo';
}