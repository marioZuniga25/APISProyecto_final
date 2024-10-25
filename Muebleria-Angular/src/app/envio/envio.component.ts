import { Component, OnInit } from '@angular/core';
import { CarritoService, ProductoCarrito } from '../services/carrito/carrito.service';
import { CommonModule } from '@angular/common';
import { PerfilService } from '../services/perfil.service';
import { IUtarjetas } from '../interfaces/ITarjetas';
import { PedidoService } from '../services/pedido.service';
import { ProductosService } from '../services/productos/productos.service';
import { IPedidos } from '../interfaces/IPedidos';
import { IDetalleVenta } from '../interfaces/IDetalleVenta';
import { IVenta } from '../interfaces/IVenta';
import { VentasService } from '../services/ventas.service';
import Swal from 'sweetalert2';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-envio',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './envio.component.html',
  styleUrl: './envio.component.css'
})
export class EnvioComponent implements OnInit{
  carrito: ProductoCarrito[] = [];
  tarjetas: IUtarjetas[] = [];
  subtotal: number = 0;
  total: number = 0;
  idUsuario: string = '';
  nombre: string = '';
  apellidos: string = '';
  telefono: string = '';
  correo: string = '';
  calle: string = '';
  numero: string = '';
  colonia: string = '';
  ciudad: string = '';
  estado: string = '';
  codigoPostal: string = '';
  idTarjeta: number = 0;
  constructor(private carritoService: CarritoService, private perfilService: PerfilService, private pedidoService: PedidoService,private ventasService: VentasService, private productosService: ProductosService) {}

  ngOnInit(): void {
    this.cargarCarrito();
    this.calcularTotales();
    this.idUsuario = localStorage.getItem('userId')!;
    this.perfilService.getUserCards(+this.idUsuario).subscribe(
      (data: IUtarjetas[]) => {
        this.tarjetas = data;
      },
      (error) => {
        console.error('Error fetching user cards', error);
      }
    );
  }
  formatCardNumber(cardNumber: string): string {
    return cardNumber.replace(/(\d{4})(?=\d)/g, '$1-');
}

  cargarCarrito() {
    this.carrito = this.carritoService.obtenerCarrito();
  }

  calcularTotales() {
    this.subtotal = this.carrito.reduce((acc, producto) => acc + producto.precio * producto.cantidad, 0);
    this.total = this.subtotal; // Aquí puedes sumar impuestos o descuentos si es necesario
  }

confirmarPedido() {
    // Obtén el idTarjeta seleccionado
    const idTarjeta = (document.querySelector('input[name="tarjeta"]:checked') as HTMLInputElement)?.value;
    const tipoVenta = '';
  
    // Obtén los datos del formulario de envío
    const nombre = (document.querySelector('input[name="nombre"]') as HTMLInputElement).value;
    const apellidos = (document.querySelector('input[name="apellidos"]') as HTMLInputElement).value;
    const telefono = (document.querySelector('input[name="telefono"]') as HTMLInputElement).value;
    const correo = (document.querySelector('input[name="correo"]') as HTMLInputElement).value;
    const calle = (document.querySelector('input[name="calle"]') as HTMLInputElement).value;
    const numero = (document.querySelector('input[name="numero"]') as HTMLInputElement).value;
    const colonia = (document.querySelector('input[name="colonia"]') as HTMLInputElement).value;
    const ciudad = (document.querySelector('input[name="cuidad"]') as HTMLInputElement).value;
    const estado = (document.querySelector('select[name="estado"]') as HTMLSelectElement).value;
    const codigoPostal = (document.querySelector('input[name="codigo"]') as HTMLInputElement).value;
  
    // Validar que todos los campos estén llenos
    if (!nombre || !apellidos || !telefono || !correo || !calle || !numero || !colonia || !ciudad || !estado || !codigoPostal || !idTarjeta) {
        Swal.fire({
            title: 'Campos incompletos',
            text: 'Por favor, llena todos los campos requeridos y selecciona una tarjeta.',
            icon: 'warning',
            confirmButtonText: 'Aceptar'
        });
        return; // Si algún campo está vacío, detenemos la ejecución
    }
  
    // Validar stock de todos los productos en el carrito antes de confirmar el pedido
    const productosSinStock: any[] = [];
    let validacionCompletada = 0;
  
    this.carrito.forEach((producto, index) => {
      this.productosService.validarStock(producto.id).subscribe(stockDisponible => {
        if (stockDisponible === 0) {
          productosSinStock.push(producto);
        }
        validacionCompletada++;
  
        // Cuando todas las validaciones hayan terminado
        if (validacionCompletada === this.carrito.length) {
          if (productosSinStock.length > 0) {
            Swal.fire({
              title: 'Stock insuficiente',
              text: `Algunos productos en tu carrito están agotados: ${productosSinStock.map(p => p.nombre).join(', ')}`,
              icon: 'error',
              confirmButtonText: 'Aceptar'
            });
          } else {
            // Si todos los productos tienen stock, proceder con la confirmación del pedido
            Swal.fire({
                title: '¿Estás seguro?',
                text: "¿Deseas confirmar tu compra?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Aceptar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Si el usuario confirma, procesamos la venta
                    const idUsuario = localStorage.getItem('userId')!;
                    const total = this.total;
  
                    const nuevaVenta: IVenta = {
                        idUsuario: parseInt(idUsuario),
                        fechaVenta: new Date(),
                        total: total,
                        tipoVenta: tipoVenta
                    };
  
                    this.ventasService.addVentaOnline(nuevaVenta).subscribe(
                        (idVentaGenerado) => {
                            // Inserción de los detalles de la venta
                            const detallesVenta: IDetalleVenta[] = this.carrito.map(producto => ({
                                idDetalleVenta: 0,
                                idVenta: idVentaGenerado,
                                idProducto: producto.id,
                                cantidad: producto.cantidad,
                                precioUnitario: producto.precio
                            }));
  
                            this.ventasService.addDetalleVenta(detallesVenta).subscribe(
                                () => {
                                    // Crear el objeto del pedido
                                    const nuevoPedido: IPedidos = {
                                        idPedido: 0,
                                        idVenta: idVentaGenerado,
                                        idUsuario: parseInt(idUsuario),
                                        idTarjeta: parseInt(idTarjeta),
                                        nombre: nombre,
                                        apellidos: apellidos,
                                        telefono: telefono,
                                        correo: correo,
                                        calle: calle,
                                        numero: numero,
                                        colonia: colonia,
                                        ciudad: ciudad,
                                        estado: estado,
                                        codigoPostal: codigoPostal,
                                        estatus: 'Pedido'
                                    };
  
                                    // Insertar el nuevo pedido
                                    this.pedidoService.guardarPedido(nuevoPedido).subscribe(
                                        () => {
                                            Swal.fire({
                                                title: 'Procesando la transacción...',
                                                allowOutsideClick: false,
                                                didOpen: () => {
                                                    Swal.showLoading();
                                                }
                                            });
                                            this.carritoService.limpiarCarrito();
                                            setTimeout(() => {
                                                Swal.close();
                                                Swal.fire({
                                                    title: '¡Venta exitosa!',
                                                    icon: 'success',
                                                    showConfirmButton: true
                                                }).then(() => {
                                                    window.location.href = `/gracias/${idVentaGenerado}`;
                                                });
                                            }, 5000);
                                        },
                                        (error) => {
                                            Swal.fire('Error', 'Error al guardar el pedido', 'error');
                                            console.error('Error al guardar el pedido', error);
                                        }
                                    );
                                },
                                (error) => {
                                    Swal.fire('Error', 'Error al agregar los detalles de la venta', 'error');
                                    console.error('Error al agregar los detalles de la venta', error);
                                }
                            );
                        },
                        (error) => {
                            Swal.fire('Error', 'Error al procesar la venta', 'error');
                            console.error('Error al procesar la venta', error);
                        }
                    );
                }
            });
          }
        }
      });
    });
  }
  

}


