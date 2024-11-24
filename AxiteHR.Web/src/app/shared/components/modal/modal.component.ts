import { trigger, state, style, transition, animate, AnimationEvent } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, OnChanges } from '@angular/core';

@Component({
	selector: 'app-modal',
	imports: [CommonModule],
	templateUrl: './modal.component.html',
	styleUrl: './modal.component.css',
	animations: [
		trigger('fadeInOut', [
			state('open', style({ opacity: 1, transform: 'scale(1)' })),
			state('closed', style({ opacity: 0, transform: 'scale(0.9)' })),
			transition('closed => open', [
				style({ opacity: 0, transform: 'scale(0.9)' }),
				animate('300ms ease-out'),
			]),
			transition('open => closed', [
				animate('300ms ease-in', style({ opacity: 0, transform: 'scale(0.9)' })),
			]),
		]),
	],
})
export class ModalComponent implements OnChanges {
	@Input() isOpen = false;
	@Input() isForm = false;
	@Input() title = 'Modal Title';
	@Input() closeButtonTitle = 'Close';
	@Input() submitButtonTitle = 'Submit';
	@Output() closeModalEmitter = new EventEmitter<void>();
	@Output() submitModalEmitter = new EventEmitter<void>();

	isVisible = false;

	ngOnChanges() {
		if (this.isOpen) {
			this.showModal();
		} else {
			this.hideModal();
		}
	}

	showModal() {
		this.isVisible = true;
		this.isOpen = true;
	}

	hideModal() {
		this.isOpen = false;
	}

	closeModal() {
		this.isOpen = false;
		this.closeModalEmitter.emit();
	}

	submitForm() {
		this.isOpen = false;
		this.submitModalEmitter.emit();
	}

	onAnimationDone(event: AnimationEvent) {
		if (event.toState === 'closed') {
			this.isVisible = false;
		}
	}
}
