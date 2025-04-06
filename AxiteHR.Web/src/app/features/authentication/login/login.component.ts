import { Component, HostBinding, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LoginRequest } from '../../../core/models/authentication/LoginRequest';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { LoginResponse } from '../../../core/models/authentication/LoginResponse';
import { AuthDictionary } from '../../../shared/dictionary/AuthDictionary';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthenticationService } from '../../../core/services/authentication/authentication.service';
import { AuthStateService } from '../../../core/services/authentication/auth-state.service';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { first, firstValueFrom } from 'rxjs';
import { routeAnimationState } from '../../../shared/animations/routeAnimationState';
import { AlertService } from '../../../core/services/alert/alert.service';

@Component({
	selector: 'app-login',
	imports: [CommonModule, RouterModule, FormsModule, TranslateModule],
	templateUrl: './login.component.html',
	styleUrl: './login.component.css',
	animations: [routeAnimationState],
})
export class LoginComponent implements OnInit {
	@HostBinding('@routeAnimationTrigger') routeAnimation = true;

	focusEmail = false;
	focusPassword = false;
	showPassword = false;
	loginMessage: string | null = null;
	errorMessage: string | null = null;
	loginModel: LoginRequest = {
		email: '',
		password: '',
	};
	returnUrl: string;

	constructor(
		private authService: AuthenticationService,
		private router: Router,
		private authStateService: AuthStateService,
		private blockUIService: BlockUIService,
		private translate: TranslateService,
		private route: ActivatedRoute,
		private alertService: AlertService
	) {
		this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '';
	}

	ngOnInit(): void {
		this.authStateService.currentRegistered.pipe(first()).subscribe(async (value: boolean) => {
			if (value === true) {
				this.loginMessage = await firstValueFrom(
					this.translate.get('Authentication_Login_RegistrationSuccessful')
				);
				this.authStateService.setRegistered(false);
			}
		});
		this.authStateService.isTokenExpired.pipe(first()).subscribe(async (value: boolean) => {
			if (value === true) {
				this.errorMessage = await firstValueFrom(
					this.translate.get('Authentication_Login_SessionExpired')
				);
				this.authStateService.setIsTokenExpired(false);
			}
		});
		this.authStateService.tempPasswordError.pipe(first()).subscribe(async (value: string) => {
			if (value.length > 0) {
				this.errorMessage = value;
				this.authStateService.setTempPasswordError('');
			}
		});
		this.authStateService.tempPasswordSuccess.pipe(first()).subscribe(async (value: string) => {
			if (value.length > 0) {
				this.loginMessage = value;
				this.authStateService.setTempPasswordSuccess('');
			}
		});
	}

	login(loginModel: LoginRequest) {
		this.blockUIService.start();
		this.authService
			.Login(loginModel)
			.pipe(first())
			.subscribe({
				next: async (response: LoginResponse) => {
					if (response.isLoggedSuccessful && response.token) {
						localStorage.setItem(AuthDictionary.Token, response.token); //ToDo HttpOnly cookie
						this.authStateService.setLoggedIn(true);
						const successLoginAlert = await firstValueFrom(
							this.translate.get('Authentication_Login_Success')
						);
						this.alertService.showAlert(successLoginAlert);
						this.blockUIService.stop();
						this.router.navigateByUrl(this.returnUrl);
					} else if (response.isTempPasswordToChange && response.userId) {
						this.authStateService.setLoggedIn(false);
						this.authStateService.setTempPasswordUserId(response.userId);
						this.blockUIService.stop();
						this.router.navigate(['/ChangeTempPassword']);
					} else if (!response.isLoggedSuccessful) {
						this.authStateService.setLoggedIn(false);
						this.errorMessage = response.errorMessage;
					}
					this.blockUIService.stop();
				},
				error: async (error: HttpErrorResponse) => {
					if (
						error.status === HttpStatusCode.BadRequest &&
						error.error &&
						error.error.errorMessage
					) {
						//Errors from response
						this.errorMessage = error.error.errorMessage;
					} else if (
						error.status == HttpStatusCode.BadRequest &&
						error.error &&
						error.error.errors
					) {
						let firstError = true;

						for (const key in error.error.errors) {
							if (Object.prototype.hasOwnProperty.call(error.error.errors, key)) {
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
						const unexpectedErrorTranslation: string = await firstValueFrom(
							this.translate.get('Authentication_Login_UnexpectedError')
						);
						this.errorMessage = null;
						this.alertService.showAlert(unexpectedErrorTranslation, 'error');
					}
					this.authStateService.setLoggedIn(false);
					this.blockUIService.stop();
				},
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
