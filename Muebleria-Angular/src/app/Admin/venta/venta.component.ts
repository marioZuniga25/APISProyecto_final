import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule, RouterOutlet } from '@angular/router';
import { VentasService } from '../../services/ventas.service';
import { IProducto } from '../../interfaces/IProducto';
import { IVenta } from '../../interfaces/IVenta';
import { IDetalleVenta } from '../../interfaces/IDetalleVenta';
import { IProductoSeleccionado } from '../../interfaces/IProductoAux';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-venta',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, RouterOutlet],
  templateUrl: './venta.component.html',
  styleUrl: './venta.component.css'
})
export class VentaComponent {

  products: IProducto[] = [];
  selectedProducts: IProductoSeleccionado[] = [];
  
  total: number = 0;

  isResultLoaded = false;
  isUpdateFormActive = false;

  nuevaVenta: IVenta = {
    idUsuario: 1,
    fechaVenta: new Date(),
    total: 0
  };
  

  detalleVenta: IDetalleVenta[] = [];
  terminoBusqueda: string = '';

  



  constructor( private route: ActivatedRoute, private _ventaService: VentasService){

    //this.obtenerProductos();
    this.buscarProductos();
    console.log(this.products, this.total);
  }

  obtenerProductos(){
    this._ventaService.getList().subscribe({
    
      next:(data) => {
        this.products = data;
        this.isResultLoaded = true;
        
      }, 
      error:(e) =>{console.log(e)}
    
    });
  }


  agregarProducto(producto: IProducto) {
    const productoExistente = this.selectedProducts.find(item => item.producto.idProducto === producto.idProducto);
    if (productoExistente) {
      alert('El producto ya está en la lista.');
      return;
    }
    const nuevoProducto: IProductoSeleccionado = {
      producto: producto,
      cantidad: 1,
      subtotal: producto.precio
    };

    this.selectedProducts.push(nuevoProducto);
  }


  eliminarProducto(id: number) {
    this.selectedProducts = this.selectedProducts.filter(item => item.producto.idProducto !== id);
  }

  actualizarSubtotal(index: number) {
    const item = this.selectedProducts[index];
    item.subtotal = item.producto.precio * item.cantidad;
  }

  calcularTotal(): number {
    return this.selectedProducts.reduce((total, item) => total + item.subtotal, 0);
  }


  guardarVenta() {  
    this.nuevaVenta.total = this.calcularTotal();

    this._ventaService.addVenta(this.nuevaVenta).subscribe({
      next: (idVentaGenerada) => {
        console.log('ID de la venta generada:', idVentaGenerada);
        alert(`Venta creada exitosamente con ID: ${idVentaGenerada}`);

        const detalleVenta = this.generarDetalleVenta(idVentaGenerada);
        console.log('Detalle de Venta:', detalleVenta);

        this._ventaService.addDetalleVenta(detalleVenta).subscribe({
          next: () => console.log('Detalles de venta guardados exitosamente.'),
          error: (error) => console.error('Error al guardar los detalles de venta:', error)
        });

        this.nuevaVenta = { fechaVenta: new Date(), total: 0 , idUsuario: 1};
        this.selectedProducts = [];
      },
      error: (error) => {
        console.error('Error al crear la venta:', error);
        alert('Hubo un error al crear la venta. Inténtalo nuevamente.');
      }
    });
  }


  generarDetalleVenta(idVenta: number): IDetalleVenta[] {
    return this.selectedProducts.map((item) => ({
      idDetalleVenta: 0,
      idProducto: item.producto.idProducto,
      idVenta: idVenta,
      cantidad: item.cantidad,
      precioUnitario: item.producto.precio
    }));
  }



  buscarProductos() {
    this._ventaService.filtrarProductos(this.terminoBusqueda).subscribe({
      next: (data) => {
        this.products = data;
      },
      error: (error) => {
        console.error('Error al obtener los productos:', error);
      },
      complete: () => {
        console.log('Consulta de productos completada');
      }
    });
  }

  

}
