import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
import { LoginRequest } from '../../../core/models/authentication/LoginRequest';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { LoginResponse } from '../../../core/models/authentication/LoginResponse';
import { DataBehaviourService } from '../../../services/data/data-behaviour.service';
import { AuthDictionary } from '../../../shared/dictionary/AuthDictionary';
import { AuthStateService } from '../../../services/authentication/auth-state.service';
import { BlockUIService } from '../../../services/block-ui.service';

@Component({
	selector: 'app-login',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule,
		FormsModule
	],
	templateUrl: './login.component.html',
	styleUrl: './login.component.css'
})
export class LoginComponent {
	focusEmail: boolean = false;
	focusPassword: boolean = false;
	showPassword: boolean = false;
	loginMessage: string | null = null;
	errorMessage: string | null = null;
	loginModel: LoginRequest = new LoginRequest();

	constructor(
		private authService: AuthenticationService,
		private dataService: DataBehaviourService,
		private router: Router,
		private authState: AuthStateService,
		private blockUIService: BlockUIService) { }

	ngOnInit(): void {
		this.dataService.currentRegistered.subscribe((value: boolean) => {
			if (value === true) {
				this.loginMessage = 'Registration successful! Please log in.';
			}
		});
		this.dataService.isTokenExpired.subscribe((value: boolean) => {
			if (value === true) {
				this.loginMessage = 'Login session expired. Please log in.';
			}
		});
	}

	login(loginModel: LoginRequest) {
		this.blockUIService.start();
		this.authService.Login(loginModel).subscribe({
			next: (response: LoginResponse) => {
				if (response.isLoggedSuccessful && response.token) {
					localStorage.setItem(AuthDictionary.Token, response.token);
					this.authState.setLoggedIn(true);
					this.blockUIService.stop();
					this.router.navigate(['']);
				} else if (!response.isLoggedSuccessful) {
					this.authState.setLoggedIn(false);
					this.errorMessage = response.errorMessage;
				}
				this.blockUIService.stop();
			},
			error: (error: HttpErrorResponse) => {
				if (error.status === HttpStatusCode.BadRequest && error.error && error.error.value) {
					//Errors from response
					this.errorMessage = error.error.value.errorMessage;
				} else if (error.status == HttpStatusCode.BadRequest && error.error && error.error.errors) {
					let firstError: boolean = true;

					for (let key in error.error.errors) {
						if (error.error.errors.hasOwnProperty(key)) {
							error.error.errors[key].forEach((errText: string) => {
								if (firstError) {
									this.errorMessage = errText;
									firstError = false;
								} else {
									this.errorMessage += `\n*${errText}`;
								}
							});
						}
					}
				} else {
					this.errorMessage = '*An unexpected error occurred. Please try again.';
				}
				this.authState.setLoggedIn(false);
				this.blockUIService.stop();
			}
		});
	}

	onFocusEmail() {
		this.focusEmail = true;
	}
	onBlurEmail() {
		this.focusEmail = false;
	}

	onFocusPassword() {
		this.focusPassword = true;
	}
	onBlurPassword() {
		this.focusPassword = false;
	}

	togglePasswordVisibility() {
		this.showPassword = !this.showPassword;
	}
}
