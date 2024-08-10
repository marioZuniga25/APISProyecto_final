export interface User {
    idUsuario: number;
    nombreUsuario: string;
    correo: string;
    contrasenia: string;
    rol: number;
  }
  
  export interface AuthResponse {
    message: string;
    user: User;
  }
  