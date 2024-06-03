import { Component } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
import { LoginRequest } from '../../../models/authentication/LoginRequest';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { LoginResponse } from '../../../models/authentication/LoginResponse';
import { DataBehaviourService } from '../../../services/data/data-behaviour.service';

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

	constructor(private authService: AuthenticationService, private route: ActivatedRoute, private dataService: DataBehaviourService) { }

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
		this.authService.Login(loginModel).subscribe({
			next: (response: LoginResponse) => {
				if (response.isLoggedSuccessful && response.token) {
					localStorage.setItem('authToken', response.token);
				} else if (!response.isLoggedSuccessful) {
					this.errorMessage = response.errorMessage;
				}
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
