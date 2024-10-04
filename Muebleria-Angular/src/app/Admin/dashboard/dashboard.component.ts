import { Component, OnInit } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { Chart, ChartData, ChartOptions, ChartType, registerables } from 'chart.js';  // Importar los controladores
import { DashboardService } from '../../services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [BaseChartDirective],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  ventas: number = 0;
  totalVentas: number = 0;
  productosMasVendidos: { nombreProducto: string, totalVendidos: number }[] = [];
  usuariosActivos: number = 0;
  productosPorCategoria: { nombreCategoria: string, totalProductos: number }[] = [];

  // Datos para los gráficos
  totalVentasData: ChartData<'pie'> = { labels: [], datasets: [] };
  productosMasVendidosChart: ChartData<'bar'> = { labels: [], datasets: [] };
  usuariosActivosChart: ChartData<'pie'> = { labels: [], datasets: [] };
  productosPorCategoriaChart: ChartData<'doughnut'> = { labels: [], datasets: [] };

  // Tipos de gráficos
  pieChartType: ChartType = 'pie';
  barChartType: ChartType = 'bar';
  doughnutChartType: ChartType = 'doughnut';

  chartOptions: ChartOptions<'pie' | 'bar' | 'doughnut'> = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
        labels: {
          font: {
            size: 14,   // Tamaño de la fuente para la leyenda
            family: 'Roboto, sans-serif',  // Fuente elegante
          },
          color: '#333'  // Color de texto de la leyenda
        }
      },
      tooltip: {
        callbacks: {
          label: (context) => {
            return `${context.label}: ${context.raw}`;
          }
        },
        backgroundColor: '#36454F',  // Color del tooltip
        titleColor: '#FFF',  // Color del título del tooltip
        bodyColor: '#FFF',   // Color del cuerpo del tooltip
        cornerRadius: 4      // Bordes redondeados del tooltip
      }
    }
  };

  chartLegend = true;

  constructor(private dashboardService: DashboardService) {
    Chart.register(...registerables);  // Registrar todos los controladores necesarios
  }

  ngOnInit(): void {
    this.getTotalVentas();
    this.getProductosMasVendidos();
    this.getUsuariosActivos();
    this.getProductosPorCategoria();
  }

  // Métodos para obtener datos
  getTotalVentas(): void {
    this.dashboardService.getTotalVentas().subscribe(data => {
      console.log('TotalVentas API Response:', data);
      this.ventas = data.totalVentas;
      this.totalVentasData = {
        labels: ['Total de Ventas'],
        datasets: [{ data: [this.ventas], backgroundColor: ['#002D62'] }]  // Azul Marino
      };
    });
  }

  getProductosMasVendidos(): void {
    this.dashboardService.getProductosMasVendidos().subscribe(data => {
      console.log('ProductosMasVendidos API Response:', data);
      this.productosMasVendidos = data;
      this.productosMasVendidosChart = {
        labels: this.productosMasVendidos.map(p => p.nombreProducto),
        datasets: [
          {
            data: this.productosMasVendidos.map(p => p.totalVendidos),
            label: 'Productos Más Vendidos',
            backgroundColor: '#046307'  // Verde Esmeralda
          }
        ]
      };
    });
  }

  getUsuariosActivos(): void {
    this.dashboardService.getUsuariosActivos().subscribe(data => {
      console.log('UsuariosActivos API Response:', data);
      this.usuariosActivos = data.usuariosActivos;
      this.usuariosActivosChart = {
        labels: ['Usuarios Activos'],
        datasets: [{ data: [this.usuariosActivos], backgroundColor: ['#800020'] }]  // Borgoña
      };
    });
  }

  getProductosPorCategoria(): void {
    this.dashboardService.ProductosPorCategoria().subscribe(data => {
      console.log('ProductosPorCategoria API Response:', data);
      this.productosPorCategoria = data;
      this.productosPorCategoriaChart = {
        labels: this.productosPorCategoria.map(p => p.nombreCategoria),
        datasets: [
          {
            data: this.productosPorCategoria.map(p => p.totalProductos),
            label: 'Productos por Categoría',
            backgroundColor: ['#CC9900', '#002D62', '#046307', '#800020']  // Ocre y otros
          }
        ]
      };
    });
  }
}
