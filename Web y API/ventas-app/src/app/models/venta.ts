export interface Venta {
  idVenta: number;
  fechaVenta: string;
  importeTotal: number;
  nombreEmpleado: string;
  nombreCliente: string;
  nombreSucursal: string;
}

export interface FechaConMasVentas {
  fecha: string;
  cantidadVentas: number;
  mensaje: string;
}