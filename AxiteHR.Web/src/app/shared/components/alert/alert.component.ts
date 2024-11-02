import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Alert } from '../../../core/models/alert/Alert';

@Component({
	selector: 'app-alert',
	standalone: true,
	imports: [CommonModule],
	templateUrl: './alert.component.html',
	styleUrl: './alert.component.css',
})
export class AlertComponent implements OnInit {
	@Input() alert!: Alert;
	fadingOut = false;
	fadingIn = false;

	ngOnInit() {
		setTimeout(() => {
			this.fadingIn = true;
		}, 10);

		setTimeout(() => {
			this.fadingOut = true;
		}, this.alert.duration - 1000);
	}

	get alertClasses() {
		return {
			'bg-red-100 text-red-700 border-red-300': this.alert.type === 'error',
			'bg-yellow-100 text-yellow-700 border-yellow-300': this.alert.type === 'warning',
			'bg-green-100 text-green-700 border-green-300': this.alert.type === 'success',
		};
	}

	get combinedClasses() {
		return {
			...this.alertClasses,
			'opacity-0': this.fadingOut || !this.fadingIn,
			'opacity-100': this.fadingIn && !this.fadingOut,
		};
	}
}
