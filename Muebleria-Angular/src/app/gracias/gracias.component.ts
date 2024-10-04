import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VentasService } from '../services/ventas.service';
import { IDetalleVenta } from '../interfaces/IDetalleVenta';
import { IVenta } from '../interfaces/IVenta';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-gracias',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './gracias.component.html',
  styleUrl: './gracias.component.css'
})
export class GraciasComponent implements OnInit {
  venta: IVenta | undefined;
  detallesVenta: IDetalleVenta[] = [];
  total: number = 0;

  constructor(
    private route: ActivatedRoute,
    private ventasService: VentasService
  ) {}

  ngOnInit(): void {
    // Obtén el ID de la venta desde la URL
    const idVenta = +this.route.snapshot.paramMap.get('id')!;
    
    // Carga los datos de la venta
    this.ventasService.getVentaById(idVenta).subscribe(
      (venta: IVenta) => {
        this.venta = venta;
        this.total = venta.total;

        // Carga los detalles de la venta
        this.ventasService.getDetalleVentaByVentaId(idVenta).subscribe(
          (detalles: IDetalleVenta[]) => {
            this.detallesVenta = detalles;
            console.log(this.detallesVenta);
          },
          (error) => {
            console.error('Error al cargar los detalles de la venta', error);
          }
        );
      },
      (error) => {
        console.error('Error al cargar la venta', error);
      }
    );
  }
}