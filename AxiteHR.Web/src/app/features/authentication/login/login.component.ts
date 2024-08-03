import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LoginRequest } from '../../../core/models/authentication/LoginRequest';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { LoginResponse } from '../../../core/models/authentication/LoginResponse';
import { AuthDictionary } from '../../../shared/dictionary/AuthDictionary';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthenticationService } from '../../../core/services/authentication/authentication.service';
import { DataBehaviourService } from '../../../core/services/data/data-behaviour.service';
import { AuthStateService } from '../../../core/services/authentication/auth-state.service';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { first, firstValueFrom } from 'rxjs';

@Component({
	selector: 'app-login',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule,
		FormsModule,
		TranslateModule
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
	returnUrl: string;

	constructor(
		private authService: AuthenticationService,
		private dataService: DataBehaviourService,
		private router: Router,
		private authState: AuthStateService,
		private blockUIService: BlockUIService,
		private translate: TranslateService,
		private route: ActivatedRoute) {
			this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '';
		}

	ngOnInit(): void {
		this.dataService.currentRegistered.pipe(first()).subscribe(async (value: boolean) => {
			if (value === true) {
				this.loginMessage = await firstValueFrom(this.translate.get('Authentication_Login_RegistrationSuccessful'));
				this.dataService.setRegistered(false);
			}
		});
		this.dataService.isTokenExpired.pipe(first()).subscribe(async (value: boolean) => {
			if (value === true) {
				this.loginMessage = await firstValueFrom(this.translate.get('Authentication_Login_SessionExpired'))
				this.dataService.setIsTokenExpired(false);
			}
		});
	}

	login(loginModel: LoginRequest) {
		this.blockUIService.start();
		this.authService.Login(loginModel).pipe(first()).subscribe({
			next: (response: LoginResponse) => {
				if (response.isLoggedSuccessful && response.token) {
					localStorage.setItem(AuthDictionary.Token, response.token); //ToDo HttpOnly cookie
					this.authState.setLoggedIn(true);
					this.blockUIService.stop();
					this.router.navigateByUrl(this.returnUrl);
				} else if (!response.isLoggedSuccessful) {
					this.authState.setLoggedIn(false);
					this.errorMessage = response.errorMessage;
				}
				this.blockUIService.stop();
			},
			error: async (error: HttpErrorResponse) => {
				if (error.status === HttpStatusCode.BadRequest && error.error && error.error.errorMessage) {
					//Errors from response
					this.errorMessage = error.error.errorMessage;
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
					let unexpectedErrorTranslation: string = await firstValueFrom(this.translate.get('Authentication_Login_UnexpectedError'));
					this.errorMessage = '*' + unexpectedErrorTranslation;
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
