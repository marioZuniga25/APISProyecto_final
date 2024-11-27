import { Component, NgModule, OnInit } from '@angular/core';
import { CarritoService, ProductoCarrito } from '../services/carrito/carrito.service';
import { CommonModule } from '@angular/common';
import { PerfilService } from '../services/perfil.service';
import { IUtarjetas } from '../interfaces/ITarjetas';
import { PedidoService } from '../services/pedido.service';
import { IVenta } from '../interfaces/IVenta';
import { IPedidos } from '../interfaces/IPedidos';
import { VentasService } from '../services/ventas.service';
import Swal from 'sweetalert2';
import { RouterLink } from '@angular/router';
import { User } from '../interfaces/AuthResponse';
import { IDetalleVenta } from '../interfaces/IDetalleVenta';
import { FormsModule, NgModel } from '@angular/forms';
import * as CryptoJS from 'crypto-js';
@Component({
  selector: 'app-envio',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './envio.component.html',
  styleUrls: ['./envio.component.css']
})
export class EnvioComponent implements OnInit {
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
  estados: string[] = [
    'Aguascalientes', 'Baja California', 'Baja California Sur', 'Campeche',
    'Chiapas', 'Chihuahua', 'Coahuila', 'Colima', 'Durango', 'Guanajuato',
    'Guerrero', 'Hidalgo', 'Jalisco', 'Mexico', 'Michoacán', 'Morelos',
    'Nayarit', 'Nuevo León', 'Oaxaca', 'Puebla', 'Querétaro',
    'Quintana Roo', 'San Luis Potosí', 'Sinaloa', 'Sonora',
    'Tabasco', 'Tamaulipas', 'Tlaxcala', 'Veracruz',
    'Yucatán', 'Zacatecas'
  ];
  constructor(
    private carritoService: CarritoService,
    private perfilService: PerfilService,
    private pedidoService: PedidoService,
    private ventasService: VentasService
  ) { }
  ngOnInit(): void {
    this.idUsuario = localStorage.getItem('userId')!;
    this.cargarDatosUsuario(); 
    this.carritoService.obtenerCarrito(+this.idUsuario).subscribe(
      response => {
        console.log('Respuesta del carrito:', response); 
        if (response && response.carrito && Array.isArray(response.carrito.detalles)) {
          this.carrito = response.carrito.detalles.map((detalle: any) => ({
            idDetalleCarrito: detalle.idDetalleCarrito,
            id: detalle.idProducto,
            nombre: detalle.nombreProducto,
            precio: detalle.precioUnitario,  
            cantidad: detalle.cantidad,
            imagen: detalle.imagen,
            fechaAgregado: new Date(detalle.fechaAgregado)
          }));
          this.calcularTotales();
        } else {
          console.error('La estructura de la respuesta no es válida:', response);
          this.carrito = [];
        }
      },
      error => {
        console.error('Error al obtener el carrito:', error);
      }
    );
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
  cargarDatosUsuario(): void {
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      const user: User = JSON.parse(storedUser);
      if (user.persona) {
        this.nombre = user.persona.nombre;
        this.apellidos = user.persona.apellidos;
        this.telefono = user.persona.telefono;
        this.correo = user.correo;
        const direccionPredeterminada = user.persona.direccionesEnvio?.find(d => d.esPredeterminada);
        if (direccionPredeterminada) {
          this.calle = direccionPredeterminada.calle;
          this.numero = direccionPredeterminada.numero;
          this.colonia = direccionPredeterminada.colonia;
          this.ciudad = direccionPredeterminada.ciudad;
          this.estado = direccionPredeterminada.estado;
          this.codigoPostal = direccionPredeterminada.codigoPostal;
        }
      }
    }
  }
  calcularTotales(): void {
    if (Array.isArray(this.carrito)) {
      this.subtotal = this.carrito.reduce((acc, producto) => acc + (producto.precio * producto.cantidad), 0);
      this.total = this.subtotal;
      console.log('Subtotal:', this.subtotal); 
    } else {
      console.error('Carrito no es un array:', this.carrito);
    }
  }
  confirmarPedido(): void {
    const idUsuario = localStorage.getItem('userId')!;
    const idTarjeta = (document.querySelector('input[name="tarjeta"]:checked') as HTMLInputElement)?.value;
  
    if (!this.nombre || !this.apellidos || !this.telefono || !this.correo ||
      !this.calle || !this.numero || !this.colonia || !this.ciudad ||
      !this.estado || !this.codigoPostal || !idTarjeta) {
      Swal.fire('Campos incompletos', 'Por favor, llena todos los campos y selecciona una tarjeta.', 'warning');
      return;
    }
  
    // Crear el objeto de venta
    const nuevaVenta: IVenta = {
      idUsuario: parseInt(idUsuario),
      fechaVenta: new Date(),
      total: this.total,
      tipoVenta: ''
    };
  
    // Llamada a la API para registrar la venta
    this.ventasService.addVentaOnline(nuevaVenta).subscribe(
      idVentaGenerado => {
        const detallesVenta: IDetalleVenta[] = this.carrito.map(producto => ({
          idDetalleVenta: 0,
          idVenta: idVentaGenerado,
          idProducto: producto.id,
          cantidad: producto.cantidad,
          precioUnitario: producto.precio
        }));
  
        this.ventasService.addDetalleVenta(detallesVenta).subscribe(
          () => {
            // Crear el pedido
            const nuevoPedido: IPedidos = {
              idPedido: 0,
              idVenta: idVentaGenerado,
              idUsuario: parseInt(idUsuario),
              idTarjeta: parseInt(idTarjeta),
              nombre: this.nombre,
              apellidos: this.apellidos,
              telefono: this.telefono,
              correo: this.correo,
              calle: this.calle,
              numero: this.numero,
              colonia: this.colonia,
              ciudad: this.ciudad,
              estado: this.estado,
              codigoPostal: this.codigoPostal,
              estatus: 'Pedido'
            };
  
            this.pedidoService.guardarPedido(nuevoPedido).subscribe(
              () => {
                // Limpiar el carrito en la base de datos
                this.carritoService.limpiarCarrito(parseInt(idUsuario)).subscribe(
                  () => {
                    Swal.fire({
                      title: 'Procesando la transacción...',
                      allowOutsideClick: false,
                      didOpen: () => {
                        Swal.showLoading();
                      }
                    });
                    setTimeout(() => {
                      Swal.close();
                      Swal.fire({
                        title: '¡Venta exitosa!',
                        icon: 'success',
                        showConfirmButton: true
                      }).then(() => {
                        const secretKey = 'tu_clave_secreta';
                        const encryptedId = CryptoJS.AES.encrypt(idVentaGenerado.toString(), secretKey).toString();
                        window.location.href = `/gracias/${encodeURIComponent(encryptedId)}`;
                      });
                    }, 5000);
                  },
                  (error) => {
                    Swal.fire('Error', 'No se pudo limpiar el carrito.', 'error');
                    console.error('Error al limpiar el carrito', error);
                  }
                );
              },
              (error) => {
                Swal.fire('Error', 'Error al guardar el pedido', 'error');
                console.error('Error al guardar el pedido', error);
              }
            );
          },
          error => {
            Swal.fire('Error', 'No se pudieron agregar los detalles de la venta.', 'error');
            console.error('Error al agregar los detalles de la venta', error);
          }
        );
      },
      error => {
        Swal.fire('Error', 'No se pudo procesar la venta.', 'error');
        console.error('Error al procesar la venta', error);
      }
    );
  }
  
}