export interface IProveedorResponse {
  idProveedor: number;
  nombreProveedor: string;
  telefono: string;
  correo: string;
  materiaPrima: string;
  materiaPrimaList?: string[];  // Añadir esta propiedad
}

export interface IProveedorRequest {
  idProveedor: number;
  nombreProveedor: string;
  telefono: string;
  correo: string;
  materiaPrimaList: string[]; // Este es un array de strings
}
