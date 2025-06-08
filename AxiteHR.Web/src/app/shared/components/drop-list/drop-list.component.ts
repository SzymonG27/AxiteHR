import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
	selector: 'app-drop-list',
	imports: [CommonModule],
	templateUrl: './drop-list.component.html',
	styleUrl: './drop-list.component.css',
})
export class DropListComponent {
	isDropListVisible = false;

	@Input() title = 'Drop List Title';

	toggleDropList() {
		this.isDropListVisible = !this.isDropListVisible;
	}
}
