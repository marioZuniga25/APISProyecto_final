// pedidos.component.ts

import { Component, OnInit } from '@angular/core';
import { PedidoService } from '../../services/pedido.service';
import { CommonModule } from '@angular/common';
import { IPedidos, IPedidosResponse } from '../../interfaces/IPedidos';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-pedidos',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pedidos.component.html',
  styleUrl: './pedidos.component.css'
})
export class PedidosComponent implements OnInit {
  pedidos: IPedidosResponse[] = [];
  pedidoActual: IPedidosResponse | null = null;
  isModalOpen: boolean = false;
  filtroActual: string = 'Pedido';

  constructor(private pedidoService: PedidoService) {}

  ngOnInit(): void {
    this.cargarPedidos();
  }

  cargarPedidos(): void {
    this.pedidoService.getPedidos(this.filtroActual).subscribe(
      (data: IPedidosResponse[]) => {
        this.pedidos = data;
      },
      (error) => {
        console.error('Error al obtener los pedidos', error);
      }
    );
  }

  abrirModal(pedido: IPedidosResponse): void {
    this.pedidoActual = pedido;
    this.isModalOpen = true;
  }

  cerrarModal(): void {
    this.isModalOpen = false;
  }
  actualizarEstatus(idPedido: number, nuevoEstatus: string): void {
    const pedidoActualizadoResponse = this.pedidos.find(p => p.idPedido === idPedido)!;
    let mensaje = '';
    let confirmacionBoton = '';
    let accion = '';
  
    if (nuevoEstatus === 'Enviado') {
      mensaje = `¿Estás seguro de enviar el pedido #${idPedido} al cliente ${pedidoActualizadoResponse.nombre} ${pedidoActualizadoResponse.apellidos}, a la dirección ${pedidoActualizadoResponse.calle} ${pedidoActualizadoResponse.numero}, ${pedidoActualizadoResponse.colonia}, ${pedidoActualizadoResponse.ciudad}, ${pedidoActualizadoResponse.estado}?`;
      confirmacionBoton = 'Enviar';
      accion = 'Enviado';
    } else if (nuevoEstatus === 'Entregado') {
      mensaje = `¿Estás seguro de entregar el pedido #${idPedido} al cliente ${pedidoActualizadoResponse.nombre} ${pedidoActualizadoResponse.apellidos}?`;
      confirmacionBoton = 'Entregar';
      accion = 'Entregado';
    }
    Swal.fire({
      title: 'Confirmar',
      text: mensaje,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: confirmacionBoton,
      cancelButtonText: 'Cancelar',
      reverseButtons: true
    }).then((result) => {
      if (result.isConfirmed) {
        this.procesarActualizacion(idPedido, accion);
      }
    });
  }
  
  procesarActualizacion(idPedido: number, nuevoEstatus: string): void {
    const pedidoActualizadoResponse = this.pedidos.find(p => p.idPedido === idPedido)!;
    const pedidoActualizado: IPedidos = {
      idPedido: pedidoActualizadoResponse.idPedido,
      idVenta: pedidoActualizadoResponse.idVenta,
      nombre: pedidoActualizadoResponse.nombre,
      apellidos: pedidoActualizadoResponse.apellidos,
      telefono: pedidoActualizadoResponse.telefono,
      correo: pedidoActualizadoResponse.correo,
      calle: pedidoActualizadoResponse.calle,
      numero: pedidoActualizadoResponse.numero,
      colonia: pedidoActualizadoResponse.colonia,
      ciudad: pedidoActualizadoResponse.ciudad,
      estado: pedidoActualizadoResponse.estado,
      codigoPostal: pedidoActualizadoResponse.codigoPostal,
      estatus: nuevoEstatus,
      idUsuario: pedidoActualizadoResponse.idUsuario,
      idTarjeta: pedidoActualizadoResponse.idTarjeta
    };
  
    this.pedidoService.updatePedido(idPedido, pedidoActualizado).subscribe(
      () => {
        console.log(`Pedido #${idPedido} actualizado a estatus: ${nuevoEstatus}`);
        this.cargarPedidos();
      },
      (error) => {
        console.error('Error al actualizar el pedido', error);
      }
    );
  }
  

cambiarFiltro(filtro: string): void {
  this.filtroActual = filtro;
  this.cargarPedidos();
}

}
