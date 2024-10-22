import { Component, OnInit } from '@angular/core';
import { PerfilService } from '../services/perfil.service';
import { IUsuarioDetalle } from '../interfaces/IUsuarioDetalle';
import { ActivatedRoute } from '@angular/router';
import { IUtarjetas } from '../interfaces/ITarjetas';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css']
})
export class PerfilComponent implements OnInit {
  user: IUsuarioDetalle | null = null;
  tarjetas: IUtarjetas[] = [];
  isModalOpen = false;
  userEdit: IUsuarioDetalle | null = null;
  contraseniaActual: string = ''; // Variable para la contraseña actual
  nuevaContrasenia: string = ''; // Variable para la nueva contraseña
  mostrarErrorContrasenia: boolean = false; // Nueva variable para mostrar el mensaje de error
  nuevaTarjeta: IUtarjetas = {
    idTarjeta: 0,
    idUsuario: 0,
    nombrePropietario: '',
    numeroTarjeta: '',
    fechaVencimiento: '',
    ccv: ''
  };

  constructor(private route: ActivatedRoute, private perfilService: PerfilService) {}

  ngOnInit(): void {
    const userId = this.route.snapshot.params['id'];
    this.perfilService.getUserDetails(+userId).subscribe(
      (data: IUsuarioDetalle) => {
        this.user = data;
        this.nuevaTarjeta.idUsuario = this.user?.idUsuario || 0;
      },
      (error) => {
        console.error('Error fetching user details', error);
      }
    );
    this.perfilService.getUserCards(+userId).subscribe(
      (data: IUtarjetas[]) => {
        this.tarjetas = data;
      },
      (error) => {
        console.error('Error fetching user cards', error);
      }
    );
  }

  agregarTarjeta(): void {
    console.log('Datos de la nueva tarjeta:', this.nuevaTarjeta);
    this.perfilService.addCard(this.nuevaTarjeta).subscribe(
      (tarjeta: IUtarjetas) => {
        this.tarjetas.push(tarjeta);
        Swal.fire('Éxito', 'Tarjeta agregada exitosamente.', 'success');
        this.nuevaTarjeta = {
          idTarjeta: 0,
          idUsuario: this.user?.idUsuario || 0,
          nombrePropietario: '',
          numeroTarjeta: '',
          fechaVencimiento: '',
          ccv: ''
        };
      },
      (error) => {
        Swal.fire('Error', 'Hubo un problema al agregar la tarjeta.', 'error');
        console.error('Error adding card', error);
      }
    );
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
    console.log('Abrir modal llamado con idUsuario:', idUsuario);
    if (idUsuario) {
      this.perfilService.getUserDetails(idUsuario).subscribe(
        (data: IUsuarioDetalle) => {
          this.userEdit = data;
          this.isModalOpen = true;
          console.log('Datos del usuario cargados para editar:', this.userEdit);
        },
        (error) => {
          console.error('Error fetching user details for edit', error);
        }
      );
    }
  }

  cerrarModal(): void {
    this.isModalOpen = false;
    this.userEdit = null; // Limpiar userEdit al cerrar el modal
  }

  onSubmit(): void {
    // Validación de la contraseña actual
    if (!this.contraseniaActual) {
      this.mostrarErrorContrasenia = true; // Muestra el error si la contraseña está vacía
      return;
    }

    this.mostrarErrorContrasenia = false; // Oculta el error si la contraseña no está vacía

    // Lógica para enviar los datos
    if (this.userEdit?.idUsuario == null) {
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

    // Llama al servicio para actualizar el usuario
    this.perfilService.updateUser(usuarioDetalle.idUsuario || 0, { ...usuarioDetalle, contrasenia: this.contraseniaActual })
      .subscribe(
        respuesta => {
          console.log("Usuario actualizado exitosamente:", respuesta);
          Swal.fire('Éxito', 'Usuario actualizado exitosamente.', 'success');
          this.cerrarModal();
          this.ngOnInit();
        },
        error => {
          console.error("Error al actualizar el usuario:", error);
          Swal.fire('Error', 'Hubo un problema al actualizar el usuario.', 'error');
        }
      );
  }
}