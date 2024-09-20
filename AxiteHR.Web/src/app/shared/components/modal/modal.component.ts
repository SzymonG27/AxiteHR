import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
	selector: 'app-modal',
	standalone: true,
	imports: [CommonModule],
	templateUrl: './modal.component.html',
	styleUrl: './modal.component.css',
})
export class ModalComponent {
	@Input() isOpen = false;
	@Input() isForm = false;
	@Input() title = 'Modal Title';
	@Input() closeButtonTitle = 'Close';
	@Input() submitButtonTitle = 'Submit';
	@Output() close = new EventEmitter<void>();
	@Output() submit = new EventEmitter<void>();

	closeModal() {
		this.isOpen = false;
		this.close.emit();
	}

	submitForm() {
		this.isOpen = false;
		this.submit.emit();
	}
}
