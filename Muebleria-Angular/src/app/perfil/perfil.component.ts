import { Component, OnInit } from '@angular/core';
import { PerfilService } from '../services/perfil.service';
import { IUsuarioDetalle } from '../interfaces/IUsuarioDetalle';
import { ActivatedRoute } from '@angular/router';
import { IUtarjetas } from '../interfaces/ITarjetas';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { FormsModule, NgModel } from '@angular/forms';

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css'
})
export class PerfilComponent implements OnInit {
  user: IUsuarioDetalle | null = null;
  tarjetas: IUtarjetas[] = [];
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
}