import { Component, OnInit } from '@angular/core';
import { PerfilService } from '../services/perfil.service';
import { IUsuarioDetalle } from '../interfaces/IUsuarioDetalle';
import { ActivatedRoute } from '@angular/router';
import { IUtarjetas } from '../interfaces/ITarjetas';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { FormsModule } from '@angular/forms';
import { IDireccionEnvio } from '../interfaces/IDireccionEnvio';

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css']
})
export class PerfilComponent implements OnInit {
  user: IUsuarioDetalle | null = null;
  personaId: number = parseInt(localStorage.getItem('userId') || '0') || 0;
  tarjetas: IUtarjetas[] = [];
  isModalOpen = false;
  userEdit: IUsuarioDetalle | null = null;
  contraseniaActual: string = '';
  nuevaContrasenia: string = '';
  isEditing: boolean = false; 
  mostrarErrorContrasenia: boolean = false;
  nuevaTarjeta: IUtarjetas = {
    idTarjeta: 0,
    idUsuario: 0,
    nombrePropietario: '',
    numeroTarjeta: '',
    fechaVencimiento: '',
    ccv: ''
  };

  estados: string[] = [
    'Aguascalientes', 'Baja California', 'Baja California Sur', 'Campeche', 
    'Chiapas', 'Chihuahua', 'Coahuila', 'Colima', 'Durango', 'Guanajuato', 
    'Guerrero', 'Hidalgo', 'Jalisco', 'Mexico', 'Michoacán', 'Morelos', 
    'Nayarit', 'Nuevo León', 'Oaxaca', 'Puebla', 'Querétaro', 
    'Quintana Roo', 'San Luis Potosí', 'Sinaloa', 'Sonora', 
    'Tabasco', 'Tamaulipas', 'Tlaxcala', 'Veracruz', 
    'Yucatán', 'Zacatecas'
  ];


  errorMessages = { // Cambiar a un objeto para mensajes específicos
    nombreDireccion: '',
    calle: '',
    numero: '',
    colonia: '',
    ciudad: '',
    estado: '',
    codigoPostal: '',
  };

  isFormValid = true;

  direcciones: IDireccionEnvio[] = []; // Añadir variable para las direcciones
  nuevaDireccion: IDireccionEnvio = {
    id: 0,
    nombreDireccion: '',
    esPredeterminada: false,
    calle: '',
    numero: '',
    colonia: '',
    ciudad: '',
    estado: '',
    codigoPostal: '',
    personaId: 0,
  };

  constructor(private route: ActivatedRoute, private perfilService: PerfilService) { }

  ngOnInit(): void {
    const userId = this.route.snapshot.params['id'];
    this.cargarDatosUsuario(userId);
    this.cargarTarjetasUsuario(userId);
    this.getDireccionesPorPersona(userId); // Cargar direcciones
  }

  cargarDatosUsuario(userId: number): void {
    this.perfilService.getUserDetails(userId).subscribe(
      (data: IUsuarioDetalle) => {
        this.user = data;
        // Si tienes más direcciones y deseas gestionar alguna, puedes agregar lógica aquí
      },
      (error) => {
        console.error('Error fetching user details', error);
      }
    );
  }

  cargarTarjetasUsuario(userId: number): void {
    this.perfilService.getUserCards(userId).subscribe(
      (data: IUtarjetas[]) => {
        this.tarjetas = data;
        console.log(data);
      },
      (error) => {
        console.error('Error fetching user cards', error);
      }
    );
  }

  // Método para obtener las direcciones de una persona por ID
  getDireccionesPorPersona(userId: number): void {
    this.perfilService.getDireccionesPorPersona(userId).subscribe(
      (data: IDireccionEnvio[]) => {
        this.direcciones = data;
      },
      (error) => {
        console.error('Error fetching user addresses', error);
      }
    );
  }

  // Método para validar la dirección
  validarDireccion(direccion: IDireccionEnvio): boolean {
    this.errorMessages = { // Cambiar a un objeto para mensajes específicos
      nombreDireccion: '',
      calle: '',
      numero: '',
      colonia: '',
      ciudad: '',
      estado: '',
      codigoPostal: '',
    };

    let isValid = true;

    if (!direccion.nombreDireccion) {
      this.errorMessages.nombreDireccion = 'El nombre de la dirección es requerido.';
      isValid = false;
    }
    if (!direccion.calle) {
      this.errorMessages.calle = 'La calle es requerida.';
      isValid = false;
    }
    if (!direccion.numero) {
      this.errorMessages.numero = 'El número es requerido.';
      isValid = false;
    }
    if (!direccion.colonia) {
      this.errorMessages.colonia = 'La colonia es requerida.';
      isValid = false;
    }
    if (!direccion.ciudad) {
      this.errorMessages.ciudad = 'La ciudad es requerida.';
      isValid = false;
    }
    if (!direccion.estado) {
      this.errorMessages.estado = 'El estado es requerido.';
      isValid = false;
    }
    if (!direccion.codigoPostal) {
      this.errorMessages.codigoPostal = 'El código postal es requerido.';
      isValid = false;
    } else if (!/^\d{5}$/.test(direccion.codigoPostal)) {
      this.errorMessages.codigoPostal = 'El código postal debe tener 5 dígitos.';
      isValid = false;
    }

    return isValid;
  }

  // Guardar o modificar la dirección según el estado
  guardarDireccion(): void {
    if (this.isEditing) {
      this.modificarDireccion();
    } else {
      this.agregarDireccion();
    }
  }

  agregarDireccion(): void {
    this.nuevaDireccion.personaId = this.personaId ?? 0; // Asigna el ID del usuario
  
    if (this.direcciones.length === 0) {
      // Si es la primera dirección, la establecemos como predeterminada
      this.nuevaDireccion.esPredeterminada = true;
      this.enviarDireccion();
    } else {
      // Preguntar al usuario si desea agregarla como predeterminada
      Swal.fire({
        title: '¿Deseas establecer esta dirección como predeterminada?',
        text: `${this.nuevaDireccion.nombreDireccion}`,
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Sí',
        cancelButtonText: 'No'
      }).then((result) => {
        if (result.isConfirmed) {
          this.setAllDirectionsToFalse(() => {
            this.nuevaDireccion.esPredeterminada = true;
            this.enviarDireccion();
          });
        } else {
          this.nuevaDireccion.esPredeterminada = false;
          this.enviarDireccion();
        }
      });
    }
  }

  // Lógica para modificar dirección
  modificarDireccion(): void {
    Swal.fire({
      title: '¿Deseas establecer esta dirección como predeterminada?',
      text: `${this.nuevaDireccion.nombreDireccion}`,
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Sí',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.isConfirmed) {
        this.setAllDirectionsToFalse(() => {
          this.nuevaDireccion.esPredeterminada = true;
          this.enviarModificacion();
        });
      } else {
        this.nuevaDireccion.esPredeterminada = false;
        this.enviarModificacion();
      }
    });
  }
  
  enviarModificacion(): void {
    this.perfilService.updateAddress(this.nuevaDireccion.id, this.nuevaDireccion).subscribe(
      () => {
        const index = this.direcciones.findIndex(d => d.id === this.nuevaDireccion.id);
        if (index !== -1) {
          this.direcciones[index] = { ...this.nuevaDireccion };
        }
        Swal.fire('Éxito', 'Dirección modificada exitosamente.', 'success');
        this.resetDireccion();  // Limpiar formulario después de modificar
        this.isEditing = false;
      },
      (error) => {
        Swal.fire('Error', 'Hubo un problema al modificar la dirección.', 'error');
        console.error('Error al modificar dirección:', error);
      }
    );
  }

  
  enviarDireccion(): void {
    this.perfilService.addAddress(this.nuevaDireccion.personaId || 0, this.nuevaDireccion).subscribe(
      (direccion: IDireccionEnvio) => {
        this.direcciones.push(direccion);
        this.resetDireccion();
      },
      (error) => {
        console.error('Error adding address', error);
      }
    );
  }
  
  // Método para establecer todas las direcciones como no predeterminadas
  setAllDirectionsToFalse(callback: () => void): void {
    const updates = this.direcciones.map(direccion => {
      direccion.esPredeterminada = false;
      return this.perfilService.updateAddress(direccion.id, direccion).toPromise();
    });
  
    Promise.all(updates)
      .then(() => callback())
      .catch(error => console.error('Error updating addresses', error));
  }
  

  resetDireccion(): void {
    this.nuevaDireccion = {
      id: 0,
      nombreDireccion: '',
      esPredeterminada: true,
      calle: '',
      numero: '',
      colonia: '',
      ciudad: '',
      estado: '',
      codigoPostal: '',
      personaId: 0,
    };
  }

  abrirModalEditarDireccion(direccion: IDireccionEnvio): void {
    this.nuevaDireccion = { ...direccion }; // Populate the form with current address data
    this.isEditing = true;
  }

  

  agregarTarjeta(): void {
    this.perfilService.addCard(this.nuevaTarjeta).subscribe(
      (tarjeta: IUtarjetas) => {
        this.tarjetas.push(tarjeta);
        Swal.fire('Éxito', 'Tarjeta agregada exitosamente.', 'success');
        this.resetTarjeta();
      },
      (error) => {
        Swal.fire('Error', 'Hubo un problema al agregar la tarjeta.', 'error');
        console.error('Error adding card', error);
      }
    );
  }

  resetTarjeta(): void {
    this.nuevaTarjeta = {
      idTarjeta: 0,
      idUsuario: this.user?.idUsuario || 0,
      nombrePropietario: '',
      numeroTarjeta: '',
      fechaVencimiento: '',
      ccv: ''
    };
  }

  eliminarTarjeta(cardId: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: 'Esta acción no se puede deshacer.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.perfilService.deleteCard(cardId).subscribe(
          () => {
            this.tarjetas = this.tarjetas.filter(t => t.idTarjeta !== cardId);
            Swal.fire('Eliminada', 'La tarjeta ha sido eliminada.', 'success');
          },
          (error) => {
            Swal.fire('Error', 'Hubo un problema al eliminar la tarjeta.', 'error');
            console.error('Error deleting card', error);
          }
        );
      }
    });
  }

  abrirModalModificar(idUsuario: number | undefined): void {
    if (idUsuario) {
      this.perfilService.getUserDetails(idUsuario).subscribe(
        (data: IUsuarioDetalle) => {
          this.userEdit = data;
          this.isModalOpen = true;
        },
        (error) => {
          console.error('Error fetching user details for edit', error);
        }
      );
    }
  }

  cerrarModal(): void {
    this.isModalOpen = false;
    this.userEdit = null;
  }

  // Método para eliminar una dirección
 // Método para eliminar una dirección
eliminarDireccion(id: number) {
  Swal.fire({
    title: '¿Estás seguro?',
    text: 'Esta acción no se puede deshacer.',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Eliminar',
    cancelButtonText: 'Cancelar'
  }).then((result) => {
    if (result.isConfirmed) {
      this.perfilService.deleteAddress(id).subscribe(() => {
        // Filtra el array para eliminar la dirección que coincide con el ID
        this.direcciones = this.direcciones.filter(d => d.id !== id);
        Swal.fire('Eliminada', 'La dirección ha sido eliminada.', 'success');
      }, (error) => {
        Swal.fire('Error', 'Hubo un problema al eliminar la dirección.', 'error');
        console.error('Error deleting address', error);
      });
    }
  });
}




  onSubmit(): void {
    if (!this.contraseniaActual) {
      this.mostrarErrorContrasenia = true;
      return;
    }

    this.mostrarErrorContrasenia = false;

    if (!this.userEdit?.idUsuario) {
      console.error("El ID del usuario no puede estar vacío.");
      return;
    }

    const usuarioDetalle: IUsuarioDetalle = {
      idUsuario: this.userEdit.idUsuario,
      nombreUsuario: this.userEdit.nombreUsuario,
      correo: this.userEdit.correo,
      contrasenia: this.nuevaContrasenia,
      rol: this.userEdit.rol,
      confirmPassword: this.nuevaContrasenia,
      type: this.userEdit.type
    };

    this.perfilService.updateUser(usuarioDetalle.idUsuario ?? 0, { ...usuarioDetalle, contrasenia: this.contraseniaActual })
      .subscribe(
        respuesta => {
          Swal.fire('Éxito', 'Usuario actualizado exitosamente.', 'success');
          this.cerrarModal();
          this.ngOnInit();
        },
        error => {
          Swal.fire('Error', 'Hubo un problema al actualizar el usuario.', 'error');
          console.error("Error al actualizar el usuario:", error);
        }
      );


  }

}
