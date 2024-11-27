import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import Swal from 'sweetalert2';

export interface ProductoCarrito {
  id: number;
  nombre: string;
  precio: number;
  cantidad: number;
  imagen:string;
  stock: number;
  descuento?: number;
}

@Injectable({
  providedIn: 'root'
})
export class CarritoService {
  private carrito: ProductoCarrito[] = [];
  private mostrarBagSubject = new BehaviorSubject<boolean>(false);
  mostrarBag$ = this.mostrarBagSubject.asObservable();

  private carritoSubject = new BehaviorSubject<ProductoCarrito[]>(this.carrito);
  carrito$ = this.carritoSubject.asObservable(); // Observable para que el carrito se actualice en tiempo real


  constructor() {
    this.cargarCarrito();
  }

  agregarAlCarrito(producto: ProductoCarrito) {
    const index = this.carrito.findIndex(item => item.id === producto.id);
    if (index !== -1) {
      this.carrito[index].cantidad += producto.cantidad;
    } else {
      this.carrito.push(producto);
    }
    this.guardarCarrito();
    this.carritoSubject.next([...this.carrito]);
  }

  incrementarCantidad(productoId: number, maxStock: number) {
    const index = this.carrito.findIndex(item => item.id === productoId);
    if (index !== -1) {
      if (this.carrito[index].cantidad < maxStock) {
        this.carrito[index].cantidad++;
        this.guardarCarrito();
      } else {
        Swal.fire('Error', 'No hay suficiente stock disponible', 'error');
      }
    }
  }

  decrementarCantidad(productoId: number) {
    const index = this.carrito.findIndex(item => item.id === productoId);
    if (index !== -1 && this.carrito[index].cantidad > 1) {
      this.carrito[index].cantidad--;
      this.guardarCarrito();
      this.carritoSubject.next([...this.carrito]);
    }
  }

  eliminarDelCarrito(productoId: number) {
    this.carrito = this.carrito.filter(item => item.id !== productoId);
    this.guardarCarrito();
    this.carritoSubject.next([...this.carrito]);
  }

  obtenerCarrito() {
    return this.carrito;
  }

  toggleBag() {
    this.mostrarBagSubject.next(!this.mostrarBagSubject.value);
  }

  private guardarCarrito() {
    const userId = localStorage.getItem('userId');
    console.log('user id' +  userId);

    if (userId) {
      localStorage.setItem(`carrito_${userId}`, JSON.stringify(this.carrito));
    }
  }

  cargarCarrito() {
    const userId = localStorage.getItem('userId');
    console.log('user id' +  userId);
    if (userId) {
      const carritoGuardado = localStorage.getItem(`carrito_${userId}`);
      if (carritoGuardado) {
        this.carrito = JSON.parse(carritoGuardado);
        this.carritoSubject.next([...this.carrito]);
      }
    }
  }
  limpiarCarrito() {
    const userId = localStorage.getItem('userId');
    this.carrito = [];
    localStorage.removeItem(`carrito_${userId}`);
    this.carritoSubject.next([...this.carrito]); // Limpia el carrito del local storage si se usa
  }
}