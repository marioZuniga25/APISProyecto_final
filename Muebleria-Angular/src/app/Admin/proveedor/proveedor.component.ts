import { Component, OnInit } from '@angular/core';
import { IProveedorResponse, IProveedorRequest } from '../../interfaces/IProveedorResponse';
import { ProveedoresService } from '../../services/proveedores.service';
import { FormsModule } from '@angular/forms';
import { NgClass, NgForOf, NgIf } from '@angular/common';
import { BuscadorCompartidoComponent } from '../shared/buscador-compartido/buscador-compartido.component';
// Importa Swal de SweetAlert2
import Swal from 'sweetalert2';

@Component({
  selector: 'app-proveedor',
  standalone: true,
  imports: [NgForOf, NgClass, FormsModule, NgIf, BuscadorCompartidoComponent],
  templateUrl: './proveedor.component.html',
  styleUrls: ['./proveedor.component.css']
})
export class ProveedorComponent implements OnInit {

  proveedores: IProveedorResponse[] = [];
  resultadosBusqueda: IProveedorResponse[] = []; // Propiedad para almacenar los resultados de la búsqueda
  isModalOpen: boolean = false;
  esModificacion: boolean = false;
  materiaPrimaInput: string = ''; 
  proveedorActual: IProveedorResponse & { materiaPrimaList: string[] } = {
    idProveedor: 0,
    nombreProveedor: '',
    telefono: '',
    correo: '',
    materiaPrima: '',
    materiaPrimaList: []  // Nueva propiedad que gestionará la lista de materias primas
  };  

  constructor(private proveedoresService: ProveedoresService) { }

  ngOnInit(): void {
    this.getProveedores();
  }

  getProveedores(): void {
    this.proveedoresService.getProveedores().subscribe(
      (data: IProveedorResponse[]) => {
        this.proveedores = data.map(proveedor => ({
          ...proveedor,
          materiaPrimaList: proveedor.materiaPrima.split(',').map(mp => mp.trim())
        }));
        this.resultadosBusqueda = this.proveedores;
      },
      (error) => {
        Swal.fire('Error', 'Error al obtener los proveedores.', 'error');
      }
    );
  }

  onSearchResults(resultados: IProveedorResponse[]): void {
    this.resultadosBusqueda = resultados;
  }

  agregarProveedor(): void {
    const proveedorToSave: IProveedorResponse = {
      ...this.proveedorActual,
      materiaPrima: this.proveedorActual.materiaPrimaList.join(', ')
    };

    this.proveedoresService.addProveedor(proveedorToSave).subscribe(
      data => {
        this.proveedores.push(data);
        this.resultadosBusqueda = this.proveedores;
        this.cerrarModal();
        this.resetFormulario();
        Swal.fire('Éxito', 'Proveedor agregado exitosamente.', 'success');
      },
      error => {
        Swal.fire('Error', 'Error al agregar proveedor.', 'error');
      }
    );
  }

  modificarProveedor(): void {
    if (this.proveedorActual.idProveedor !== undefined) {
      const proveedorToUpdate: IProveedorResponse = {
        ...this.proveedorActual,
        materiaPrima: this.proveedorActual.materiaPrimaList.join(', ')
      };

      this.proveedoresService.updateProveedor(proveedorToUpdate.idProveedor, proveedorToUpdate).subscribe(
        () => {
          this.getProveedores();
          this.cerrarModal();
          this.resetFormulario();
          Swal.fire('Éxito', 'Proveedor modificado exitosamente.', 'success');
        },
        error => {
          Swal.fire('Error', 'Error al modificar proveedor.', 'error');
        }
      );
    } else {
      Swal.fire('Error', 'No se puede modificar el proveedor: idProveedor es undefined.', 'error');
    }
  }

  eliminarProveedor(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "No podrás revertir esto",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.proveedoresService.deleteProveedor(id).subscribe(
          () => {
            this.proveedores = this.proveedores.filter(p => p.idProveedor !== id);
            this.resultadosBusqueda = this.proveedores;
            Swal.fire('Eliminado', 'El proveedor ha sido eliminado.', 'success');
          },
          error => {
            Swal.fire('Error', 'Error al eliminar proveedor.', 'error');
          }
        );
      }
    });
  }

  eliminarProveedorDesdeModal(): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "No podrás revertir esto",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        if (this.proveedorActual.idProveedor !== undefined) {
          this.proveedoresService.deleteProveedor(this.proveedorActual.idProveedor).subscribe(
            () => {
              this.proveedores = this.proveedores.filter(p => p.idProveedor !== this.proveedorActual.idProveedor);
              this.resultadosBusqueda = this.proveedores;
              this.cerrarModal();
              this.resetFormulario();
              Swal.fire('Eliminado', 'El proveedor ha sido eliminado.', 'success');
            },
            error => {
              Swal.fire('Error', 'Error al eliminar proveedor.', 'error');
            }
          );
        }
      }
    });
  }

  abrirModalAgregar(): void {
    this.resetFormulario();
    this.esModificacion = false;
    this.isModalOpen = true;
  }

  abrirModalModificar(proveedor: IProveedorResponse): void {
    this.proveedorActual = {
      idProveedor: proveedor.idProveedor,
      nombreProveedor: proveedor.nombreProveedor,
      telefono: proveedor.telefono,
      correo: proveedor.correo,
      materiaPrima: proveedor.materiaPrima,
      materiaPrimaList: proveedor.materiaPrima.split(',').map(mp => mp.trim())
    };
    this.esModificacion = true;
    this.isModalOpen = true;
  }

  cerrarModal(): void {
    this.isModalOpen = false;
  }

  resetFormulario(): void {
    this.proveedorActual = {
      idProveedor: 0,
      nombreProveedor: '',
      telefono: '',
      correo: '',
      materiaPrima: '',
      materiaPrimaList: [],
    };
    this.materiaPrimaInput = '';
  }

  agregarMateriaPrima(): void {
    if (this.materiaPrimaInput.trim() !== '') {
      this.proveedorActual.materiaPrimaList.push(this.materiaPrimaInput.trim());
      this.materiaPrimaInput = '';
    }
  }

  eliminarMateriaPrima(index: number): void {
    this.proveedorActual.materiaPrimaList.splice(index, 1);
  }

  guardarProveedor(): void {
    if (this.esModificacion) {
      this.modificarProveedor();
    } else {
      this.agregarProveedor();
    }
  }
}
