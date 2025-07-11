import { Component, HostBinding } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { RegisterRequest } from '../../../core/models/authentication/RegisterRequest';
import {
	FormControl,
	FormGroup,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { mustMatch } from '../../../shared/validators/password-match.validator';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthenticationService } from '../../../core/services/authentication/authentication.service';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { first, firstValueFrom } from 'rxjs';
import { routeAnimationState } from '../../../shared/animations/routeAnimationState';
import { Environment } from '../../../environment/Environment';
import { AlertService } from '../../../core/services/alert/alert.service';
import { AuthStateService } from '../../../core/services/authentication/auth-state.service';

@Component({
	selector: 'app-register',
	imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, TranslateModule],
	templateUrl: './register.component.html',
	styleUrl: './register.component.css',
	animations: [routeAnimationState],
})
export class RegisterComponent {
	@HostBinding('@routeAnimationTrigger') routeAnimation = true;

	focusEmail = false;
	focusPassword = false;
	focusUserName = false;
	focusFirstName = false;
	focusLastName = false;
	errorMessage: string | null = null;

	showPassword = false;
	registerForm: FormGroup;
	registerModel: RegisterRequest = {
		email: '',
		userName: '',
		firstName: '',
		lastName: '',
		userPassword: '',
		userPasswordRepeated: '',
		phoneNumber: '',
	};

	constructor(
		private authService: AuthenticationService,
		private authStateService: AuthStateService,
		private router: Router,
		private blockUI: BlockUIService,
		private translate: TranslateService,
		private alertService: AlertService
	) {
		this.registerForm = new FormGroup({
			Email: new FormControl(this.registerModel.email, {
				validators: [Validators.required, Validators.email],
			}),
			UserName: new FormControl(this.registerModel.userName, {
				validators: [Validators.required, Validators.minLength(5)],
			}),
			FirstName: new FormControl(this.registerModel.firstName, {
				validators: [Validators.required, Validators.minLength(2)],
			}),
			LastName: new FormControl(this.registerModel.lastName, {
				validators: [Validators.required, Validators.minLength(2)],
			}),
			UserPassword: new FormControl(this.registerModel.userPassword, {
				validators: [
					Validators.required,
					Validators.minLength(8),
					Validators.pattern(Environment.strongPasswordRegex),
				],
			}),
			UserPasswordRepeated: new FormControl(this.registerModel.userPasswordRepeated, {
				validators: [
					Validators.required,
					Validators.minLength(8),
					Validators.pattern(Environment.strongPasswordRegex),
					mustMatch('UserPassword'),
				],
			}),
		});
	}

	register() {
		if (!this.registerForm.valid) {
			return;
		}
		this.blockUI.start();
		this.registerModel = this.registerForm.value;
		this.authService
			.Register(this.registerModel)
			.pipe(first())
			.subscribe({
				next: () => {
					this.authStateService.setRegistered(true);
					this.blockUI.stop();
					this.router.navigate(['/Login']);
				},
				error: async (error: HttpErrorResponse) => {
					if (
						error.status === HttpStatusCode.BadRequest &&
						error.error &&
						error.error.errorMessage
					) {
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
						const unexpectedErrorMessage = await firstValueFrom(
							this.translate.get('Authentication_Login_UnexpectedError')
						);
						this.errorMessage = null;
						this.alertService.showAlert(unexpectedErrorMessage, 'error');
					}
					this.blockUI.stop();
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

	onFocusUserName() {
		this.focusUserName = true;
	}
	onBlurUserName() {
		this.focusUserName = false;
	}

	onFocusFirstName() {
		this.focusFirstName = true;
	}
	onBlurFirstName() {
		this.focusFirstName = false;
	}

	onFocusLastName() {
		this.focusLastName = true;
	}
	onBlurLastName() {
		this.focusLastName = false;
	}

	togglePasswordVisibility() {
		this.showPassword = !this.showPassword;
	}
}
