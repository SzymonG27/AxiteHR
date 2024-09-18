import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AuthStateService } from '../../core/services/authentication/auth-state.service';

@Component({
	selector: 'app-home',
	standalone: true,
	imports: [TranslateModule],
	templateUrl: './home.component.html',
	styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
	isLoggedIn = false;

	constructor(private authState: AuthStateService) {}

	ngOnInit() {
		this.authState.isLoggedIn.subscribe((status: boolean) => {
			this.isLoggedIn = status;
		});
	}
}
