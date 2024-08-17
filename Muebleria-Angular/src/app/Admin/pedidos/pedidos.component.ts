import { Component, OnInit } from '@angular/core';
import { IPedidos } from '../../interfaces/IPedidos';
import { PedidoService } from '../../services/pedido.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pedidos',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pedidos.component.html',
  styleUrl: './pedidos.component.css'
})
export class PedidosComponent implements OnInit {
  pedidos: IPedidos[] = [];

  constructor(private pedidoService: PedidoService) {}

  ngOnInit(): void {
    this.cargarPedidos();
  }

  cargarPedidos(): void {
    this.pedidoService.getPedidos().subscribe(
      (data: IPedidos[]) => {
        this.pedidos = data;
      },
      (error) => {
        console.error('Error al obtener los pedidos', error);
      }
    );
  }

  enviarPedido(id: number): void {
    const pedidoActualizado: IPedidos = this.pedidos.find(p => p.idPedido === id)!;
    pedidoActualizado.estatus = 'enviado';

    this.pedidoService.updatePedido(id, pedidoActualizado).subscribe(
      () => {
        console.log('Pedido actualizado con éxito');
        this.cargarPedidos(); // Volver a cargar la lista de pedidos para reflejar los cambios
      },
      (error) => {
        console.error('Error al actualizar el pedido', error);
      }
    );
  }
}