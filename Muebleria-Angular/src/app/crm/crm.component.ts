import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule, NgFor } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Router } from '@angular/router';
declare var $: any;

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, NgFor, RouterLink],
  templateUrl: './crm.component.html',
  styleUrls: ['./crm.component.css'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA] 
})
export class CrmComponent {

}
