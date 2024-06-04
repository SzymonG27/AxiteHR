import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { AuthStateService } from '../../services/authentication/auth-state.service';

@Component({
	selector: 'app-home',
	standalone: true,
	imports: [],
	templateUrl: './home.component.html',
	styleUrl: './home.component.css'
})
export class HomeComponent {
	isLoggedIn: boolean = false;

	constructor(private authState: AuthStateService) { }

	ngOnInit() {
		this.authState.isLoggedIn.subscribe((status: boolean) => {
			this.isLoggedIn = status;
		});
	}
}
