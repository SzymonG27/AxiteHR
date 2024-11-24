import { Component, HostBinding } from '@angular/core';
import { routeAnimationState } from '../../../shared/animations/routeAnimationState';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import {
	FormControl,
	FormGroup,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { TempPasswordChangeRequest } from '../../../core/models/authentication/TempPasswordChangeRequest';
import { Environment } from '../../../environment/Environment';
import { mustMatch } from '../../../shared/validators/password-match.validator';
import { AuthenticationService } from '../../../core/services/authentication/authentication.service';
import { AuthStateService } from '../../../core/services/authentication/auth-state.service';
import { DataBehaviourService } from '../../../core/services/data/data-behaviour.service';
import { firstValueFrom, take } from 'rxjs';
import { TempPasswordChangeResponse } from '../../../core/models/authentication/TempPasswordChangeResponse';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';

@Component({
	selector: 'app-temp-password-change',
	imports: [CommonModule, TranslateModule, ReactiveFormsModule, RouterModule, FormsModule],
	templateUrl: './temp-password-change.component.html',
	styleUrl: './temp-password-change.component.css',
	animations: [routeAnimationState],
})
export class TempPasswordChangeComponent {
	@HostBinding('@routeAnimationTrigger') routeAnimation = true;

	passwordState = false;
	confirmPasswordState = false;
	showPassword = false;

	tempPasswordChangeForm: FormGroup;
	errorMessage: string | null = null;
	tempPasswordChangeModel: TempPasswordChangeRequest = {
		userId: '',
		password: '',
		confirmPassword: '',
	};

	constructor(
		private blockUI: BlockUIService,
		private authService: AuthenticationService,
		private authStateService: AuthStateService,
		private dataService: DataBehaviourService,
		private translate: TranslateService,
		private router: Router
	) {
		this.tempPasswordChangeForm = new FormGroup({
			password: new FormControl(this.tempPasswordChangeModel.password, {
				validators: [
					Validators.required,
					Validators.minLength(8),
					Validators.pattern(Environment.strongPasswordRegex),
				],
			}),
			confirmPassword: new FormControl(this.tempPasswordChangeModel.confirmPassword, {
				validators: [
					Validators.required,
					Validators.minLength(8),
					Validators.pattern(Environment.strongPasswordRegex),
					mustMatch('password'),
				],
			}),
		});
	}

	async changeTempPassword() {
		if (!this.tempPasswordChangeForm.valid) {
			return;
		}

		this.blockUI.start();

		const tempPasswordUserId = this.authStateService.getTempPasswordUserId();
		if (!tempPasswordUserId) {
			this.dataService.setTempPasswordError(
				await firstValueFrom(
					this.translate.get('Authentication_TempPasswordChange_InternalError')
				)
			);
			this.blockUI.stop();
			this.router.navigate(['Login']);
			return;
		}

		this.tempPasswordChangeModel = this.tempPasswordChangeForm.value;
		this.tempPasswordChangeModel.userId = tempPasswordUserId;

		this.authService
			.TempPasswordChange(this.tempPasswordChangeModel)
			.pipe(take(1))
			.subscribe({
				next: async (response: TempPasswordChangeResponse) => {
					if (response.isSucceeded) {
						this.dataService.setTempPasswordSuccess(
							await firstValueFrom(
								this.translate.get('Authentication_TempPasswordChange_Success')
							)
						);
						this.blockUI.stop();
						this.router.navigate(['Login']);
					} else {
						this.errorMessage = response.errorMessage;
						this.blockUI.stop();
					}
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
						this.errorMessage = '*' + unexpectedErrorTranslation;
					}
					this.blockUI.stop();
				},
			});
	}

	togglePasswordState(state: boolean) {
		if (state) {
			this.passwordState = true;
			return;
		}
		this.passwordState = false;
	}

	toggleConfirmPasswordState(state: boolean) {
		if (state) {
			this.passwordState = true;
			return;
		}
		this.passwordState = false;
	}

	togglePasswordVisibility() {
		this.showPassword = !this.showPassword;
	}
}
