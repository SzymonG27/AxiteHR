import { Component } from '@angular/core';
import { NavManagerComponent } from '../../../core/components/nav-manager/nav-manager.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-company-manager',
  standalone: true,
  imports: [
    NavManagerComponent,
    RouterOutlet
  ],
  templateUrl: './company-manager.component.html',
  styleUrl: './company-manager.component.css'
})
export class CompanyManagerComponent {

}
