export interface ApiResponse<T> {
  data?: T;
  mensaje?: string;
  detalle?: string;
}