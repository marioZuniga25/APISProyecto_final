import { Component } from '@angular/core';
import { IMateriaPrima } from '../../interfaces/IMateriaPrima';
import { MateriaprimaService } from '../../services/materiaprima.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-materia-prima',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './materia-prima.component.html',
  styleUrl: './materia-prima.component.css'
})
export class MateriaPrimaComponent {
  materiasPrimas: IMateriaPrima[] = [];
  selectedMateriaPrima: IMateriaPrima = {
    idMateriaPrima: 0,
    nombreMateriaPrima: '',
    descripcion: '',
    idInventario: 0
  };
  nuevaMateriaPrima: IMateriaPrima = {
    idMateriaPrima: 0,
    nombreMateriaPrima: '',
    descripcion: '',
    idInventario: 0
  };
  isResultLoaded = false;
  isUpdateFormActive = false;
  isModalOpen = false;
  isEditModalOpen = false;
  

  constructor(private _materiaPrimaService: MateriaprimaService) {
    this.getMateriasPrimas();
  }

  
  getMateriasPrimas(){
    this._materiaPrimaService.getList().subscribe({
    
      next:(data) => {
        this.materiasPrimas = data;
        console.log(data);
        this.isResultLoaded = true;
        
      }, 
      error:(e) =>{console.log(e)}
    
    });
  }

  openModal() {
    this.isModalOpen = true;
  }

  
  closeModal() {
    this.isModalOpen = false;
    this.isEditModalOpen = false;
    this.selectedMateriaPrima = {
      idMateriaPrima: 0,
      nombreMateriaPrima: '',
      descripcion: '',
      idInventario: 0
    };
  }

  openEditModal(materiaPrima: IMateriaPrima): void {
    this.selectedMateriaPrima = { ...materiaPrima }; 
    this.isEditModalOpen = true;
    console.log(this.selectedMateriaPrima);
  }

  addMateriaPrima() {
    console.log(this.nuevaMateriaPrima);
    this._materiaPrimaService.addMateriaPrima(this.nuevaMateriaPrima).subscribe({

      next:(data) => {
        
        

        this.nuevaMateriaPrima = {
          idMateriaPrima: 0,
          nombreMateriaPrima: '',
          descripcion: '',
          idInventario: 0
        };
        this.closeModal();
        this.getMateriasPrimas();
        
      }, 
      error:(e) =>{console.log(e)}
    
    });
  }

  saveChanges(): void {
    if (this.selectedMateriaPrima) {
      console.log(this.selectedMateriaPrima);
        this._materiaPrimaService.updateMateriaPrima(this.selectedMateriaPrima).subscribe({
          next:(data) => {

            this.closeModal();
            this.getMateriasPrimas();

          }, error:(e) => {console.log(e);}
        });
      } 
  }

    deleteMateria(){
      this._materiaPrimaService.deleteMateriaPrima(this.selectedMateriaPrima).subscribe({
        next:(data) => {
          this.closeModal();
          this.getMateriasPrimas();
        }, error:(e) => {console.log(e);}
      });
    }
  

}
