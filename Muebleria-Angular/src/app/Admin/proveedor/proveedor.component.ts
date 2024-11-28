import { Component, OnInit } from '@angular/core';
import { IProveedorResponse, IProveedorRequest } from '../../interfaces/IProveedorResponse';
import { ProveedoresService } from '../../services/proveedores.service';
import { FormsModule } from '@angular/forms';
import { NgClass, NgForOf, NgIf } from '@angular/common';
import Swal from 'sweetalert2';
import { IUnidadMedida } from '../../interfaces/IUnidadMedida';
import { BuscadorCompartidoComponent } from '../shared/buscador-compartido/buscador-compartido.component';

@Component({
  selector: 'app-proveedor',
  standalone: true,
  imports: [NgForOf, NgClass, FormsModule, NgIf,BuscadorCompartidoComponent],
  templateUrl: './proveedor.component.html',
  styleUrls: ['./proveedor.component.css']
})
export class ProveedorComponent implements OnInit {

  proveedores: IProveedorResponse[] = [];
  unidadesMedida: IUnidadMedida[] = [];
  resultadosBusqueda: IProveedorResponse[] = [];
  isModalOpen: boolean = false;
  esModificacion: boolean = false;

  // Modelo del proveedor actual con la lista de materias primas
proveedorActual: IProveedorRequest & {
  materiasPrimas: {
    idMateriaPrima: number;
    nombreMateriaPrima: string;
    descripcion: string;
    idProveedor: number;
    precio: number;
    stock: number;
    idUnidad: number;
  }[];
} = {
  idProveedor: 0,
  nombreProveedor: '',
  telefono: '',
  correo: '',
  materiasPrimas: [] // Inicialización correcta del array
};

// Inputs para agregar nueva materia prima
materiaPrimaInput = {
  nombreMateriaPrima: '',
  descripcion: '',
  precio: 0,
  stock: 0,
  idUnidad: 0
};


  constructor(private proveedoresService: ProveedoresService) { }

  ngOnInit(): void {
    this.getProveedores();
    this.getUnidadesMedida();
  }

  // Obtener todos los proveedores de la API
  getProveedores(): void {
    this.proveedoresService.getProveedores().subscribe(
      (data: IProveedorResponse[]) => {
        this.proveedores = data.map((proveedor) => ({
          ...proveedor,
          preciosMateriasPrimas: proveedor.preciosMateriasPrimas || [], // Asegura la existencia de la lista
          unidadesMateriasPrimas: proveedor.unidadesMateriasPrimas || [], // Asegura la existencia de la lista
        }));
        this.resultadosBusqueda = this.proveedores;
        console.log('Proveedores obtenidos:', this.proveedores);
      },
      (error) => {
        Swal.fire('Error', 'Error al obtener los proveedores.', 'error');
      }
    );
  }
  
  

  // Función para manejar resultados de búsqueda
  onSearchResults(resultados: IProveedorResponse[]): void {

    this.resultadosBusqueda = resultados;
    console.log('Resultados de búsqueda:', this.resultadosBusqueda);
  }

  // Agregar un proveedor a la base de datos
  agregarProveedor(): void {
    const proveedorToSave: IProveedorRequest = {
      idProveedor: this.proveedorActual.idProveedor,
      nombreProveedor: this.proveedorActual.nombreProveedor,
      telefono: this.proveedorActual.telefono,
      correo: this.proveedorActual.correo,
      materiasPrimas: this.proveedorActual.materiasPrimas.map(mp => ({
        nombreMateriaPrima: mp.nombreMateriaPrima,
        descripcion: mp.descripcion,
        idProveedor: this.proveedorActual.idProveedor,
        idUnidad: mp.idUnidad,
        precio: mp.precio,
        stock: mp.stock
      }))
    };
  
    this.proveedoresService.addProveedor(proveedorToSave).subscribe(
      (data) => {
        // Asegurarse de inicializar preciosMateriasPrimas y unidadesMateriasPrimas
        const nuevoProveedor: IProveedorResponse = {
          ...data,
          preciosMateriasPrimas: data.preciosMateriasPrimas || [], // Inicializa si no está definido
          unidadesMateriasPrimas: data.unidadesMateriasPrimas || [] // Inicializa si no está definido
        };
  
        this.proveedores = [...this.proveedores, nuevoProveedor];
        this.resultadosBusqueda = this.proveedores;
        this.cerrarModal();
        this.resetFormulario();
        Swal.fire('Éxito', 'Proveedor agregado exitosamente.', 'success');
      },
      (error) => {
        Swal.fire('Error', 'Error al agregar proveedor.', 'error');
      }
    );
  }
  
  
  obtenerNombreUnidad(idUnidad: string): string {
    console.log('ID Unidad recibida:', idUnidad);
    console.log('Unidades de Medida disponibles:', this.unidadesMedida);

    // Convertir idUnidad a número
    const idUnidadNum = parseInt(idUnidad, 10);
    console.log('ID Unidad convertido a número:', idUnidadNum);

    const unidad = this.unidadesMedida.find(u => {
        console.log(`Comparando con idUnidad: ${u.idUnidad} (tipo: ${typeof u.idUnidad})`);
        console.log(`Contra idUnidad recibido: ${idUnidadNum} (tipo: ${typeof idUnidadNum})`);
        return u.idUnidad === idUnidadNum; // Comparar ambos como números
    });

    console.log('Resultado de la búsqueda:', unidad);

    return unidad ? unidad.nombreUnidad : 'Sin Unidad';
}


// Editar proveedor: asignar los datos del proveedor seleccionado
editarProveedor(proveedor: IProveedorResponse): void {
  this.proveedorActual = {
    idProveedor: proveedor.idProveedor,
    nombreProveedor: proveedor.nombreProveedor,
    telefono: proveedor.telefono,
    correo: proveedor.correo,
    materiasPrimas: proveedor.nombresMateriasPrimas.map((nombre, index) => ({
      idMateriaPrima: 0, // Asigna un ID válido si está disponible
      nombreMateriaPrima: nombre,
      descripcion: '', // Ajusta si la descripción está disponible
      idProveedor: proveedor.idProveedor,
      precio: parseFloat(proveedor.preciosMateriasPrimas?.[index] || '0'), // Usa un valor predeterminado si está indefinido
      stock: 0, // Ajustar si tienes stock disponible
      idUnidad: this.obtenerIdUnidad(proveedor.unidadesMateriasPrimas?.[index] || '') // Usa un valor predeterminado si está indefinido
    }))
  };
  this.esModificacion = true;
  this.isModalOpen = true;
}

obtenerIdUnidad(nombreUnidad: string): number {
  const unidad = this.unidadesMedida.find((u) => u.nombreUnidad === nombreUnidad);
  return unidad ? unidad.idUnidad : 0; // Retorna 0 si no encuentra la unidad
}



  // Modificar un proveedor
  modificarProveedor(): void {
    const proveedorToUpdate: IProveedorRequest = {
      idProveedor: this.proveedorActual.idProveedor,
      nombreProveedor: this.proveedorActual.nombreProveedor,
      telefono: this.proveedorActual.telefono,
      correo: this.proveedorActual.correo,
      materiasPrimas: this.proveedorActual.materiasPrimas.map(mp => ({
        nombreMateriaPrima: mp.nombreMateriaPrima,
        descripcion: mp.descripcion,
        idProveedor: this.proveedorActual.idProveedor,
        idUnidad: mp.idUnidad,
        precio: mp.precio,
        stock: mp.stock
      }))
    };
  
    this.proveedoresService.updateProveedor(proveedorToUpdate.idProveedor, proveedorToUpdate).subscribe(
      () => {
        this.getProveedores(); // Actualiza la lista de proveedores
        this.cerrarModal(); // Cierra el modal después de la modificación
        this.resetFormulario(); // Resetea el formulario para el siguiente uso
        Swal.fire('Éxito', 'Proveedor modificado exitosamente.', 'success');
      },
      error => {
        Swal.fire('Error', 'Error al modificar proveedor.', 'error');
      }
    );
  }
  

  // Modal para agregar un proveedor
  abrirModalAgregar(): void {
    this.resetFormulario();
    this.esModificacion = false;
    this.isModalOpen = true;
  }

  cerrarModal(): void {
    this.isModalOpen = false;
  }

  // Resetear el formulario
  resetFormulario(): void {
    this.proveedorActual = {
      idProveedor: 0,
      nombreProveedor: '',
      telefono: '',
      correo: '',
      materiasPrimas: [],
    };
    this.materiaPrimaInput = {
      nombreMateriaPrima: '',
      descripcion: '',
      precio: 0,
      stock: 0,
      idUnidad: this.unidadesMedida.length > 0 ? this.unidadesMedida[0].idUnidad : 0
    };
  }

  // Agregar materia prima al proveedor actual
  agregarMateriaPrima(): void {
    if (this.materiaPrimaInput.nombreMateriaPrima.trim() !== '' && this.materiaPrimaInput.precio > 0) {
      const unidadSeleccionada = this.unidadesMedida.find(u => u.idUnidad === this.materiaPrimaInput.idUnidad);
      console.log('Materia Prima Input:', this.materiaPrimaInput);
      console.log('Unidades de Medida:', this.unidadesMedida);
      
      this.proveedorActual.materiasPrimas.push({
        nombreMateriaPrima: this.materiaPrimaInput.nombreMateriaPrima,
        descripcion: this.materiaPrimaInput.descripcion,
        precio: this.materiaPrimaInput.precio,
        stock: this.materiaPrimaInput.stock,
        idProveedor: this.proveedorActual.idProveedor,        
        idUnidad: this.materiaPrimaInput.idUnidad
       
      });

      this.resetMateriaPrimaInput();
    } else {
      Swal.fire('Error', 'El nombre de la materia prima y el precio son obligatorios.', 'error');
    }
  }

  // Resetear los campos de materia prima
  resetMateriaPrimaInput(): void {
    this.materiaPrimaInput = {
      nombreMateriaPrima: '',
      descripcion: '',
      precio: 0,
      stock: 0,
      idUnidad: 0
    };
  }

  // Eliminar materia prima
  eliminarMateriaPrima(index: number): void {
    this.proveedorActual.materiasPrimas.splice(index, 1);
  }

  // Guardar el proveedor
  guardarProveedor(): void {
    if (this.proveedorActual.materiasPrimas.length === 0) {
      Swal.fire('Error', 'Debes agregar al menos una materia prima.', 'error');
      return;
    }

    if (this.esModificacion) {
      this.modificarProveedor();
    } else {
      this.agregarProveedor();
    }
  }

  // Obtener unidades de medida
  getUnidadesMedida(): void {
    this.proveedoresService.getUnidadesMedida().subscribe(
      (data: IUnidadMedida[]) => {
        this.unidadesMedida = data;
      },
      (error) => {
        Swal.fire('Error', 'Error al obtener las unidades de medida.', 'error');
      }
    );
  }

  confirmarEliminacion() {
    Swal.fire({
      title: '¿Estás seguro?',
      text: 'No podrás revertir esta acción',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed && this.proveedorActual.idProveedor) {
        this.eliminarProveedor(this.proveedorActual.idProveedor);
      }
    });
  }

  eliminarProveedor(id: number): void {
    this.proveedoresService.deleteProveedor(id).subscribe({
      next: () => {
        // Filtrar el proveedor eliminado de la lista local
        this.proveedores = this.proveedores.filter(p => p.idProveedor !== id);
        this.resultadosBusqueda = this.proveedores; // Asegura que los resultados de búsqueda se actualicen
        this.cerrarModal();
        Swal.fire('Éxito', 'El proveedor ha sido eliminado con éxito', 'success');
      },
      error: (err) => {
        Swal.fire('Error', 'No se pudo eliminar el proveedor. Intenta de nuevo.', 'error');
      }
    });
  }

  actualizarLista() {
    // Lógica para refrescar la lista de proveedores
    this.proveedoresService.getProveedores().subscribe({
      next: (data) => {
        this.proveedores = data;
      },
      error: (err) => {
        console.error('Error al actualizar la lista de proveedores:', err);
      }
    });
  }

}
