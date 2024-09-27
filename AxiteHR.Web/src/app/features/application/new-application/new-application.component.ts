import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
	selector: 'app-new-application',
	standalone: true,
	imports: [],
	templateUrl: './new-application.component.html',
	styleUrl: './new-application.component.css',
})
export class NewApplicationComponent {
	constructor(private router: Router) {}

	goBack(): void {
		if (window.history.length > 1) {
			window.history.back();
		} else {
			this.router.navigateByUrl('/Dashboard');
		}
	}
}
