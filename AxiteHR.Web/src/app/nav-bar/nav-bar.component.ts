import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.css',
  animations: [
    trigger('menuAnimation', [
      state('closed', style({
        height: '0px'
      })),
      state('open', style({
        height: '*'
      })),
      transition('closed => open', [
        animate('0.5s ease-in-out')
      ]),
      transition('open => closed', [
        animate('0.5s ease-in-out')
      ])
    ])
  ]
})
export class NavBarComponent {

  isMenuOpen = false;

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

}