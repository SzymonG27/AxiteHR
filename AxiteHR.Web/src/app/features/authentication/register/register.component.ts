import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { RegisterRequest } from '../../../core/models/authentication/RegisterRequest';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { mustMatch } from '../../../shared/validators/password-match.validator';
import { HttpErrorResponse, HttpEvent, HttpStatusCode } from '@angular/common/http';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthenticationService } from '../../../core/services/authentication/authentication.service';
import { DataBehaviourService } from '../../../core/services/data/data-behaviour.service';
import { BlockUIService } from '../../../core/services/block-ui.service';

@Component({
	selector: 'app-register',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule,
		FormsModule,
		ReactiveFormsModule,
		TranslateModule
	],
	templateUrl: './register.component.html',
	styleUrl: './register.component.css'
})
export class RegisterComponent {
	focusEmail: boolean = false;
	focusPassword: boolean = false;
	focusUserName: boolean = false;
	focusFirstName: boolean = false;
	focusLastName: boolean = false;
	errorMessage: string | null = null;

	showPassword: boolean = false;
	registerForm: FormGroup;
	registerModel: RegisterRequest = new RegisterRequest();

	private strongPassRegex: RegExp = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*]).{8,}$/;

	constructor(
		private authService: AuthenticationService,
		private router: Router,
		private dataService: DataBehaviourService,
		private blockUI: BlockUIService,
		private translate: TranslateService)
	{
		this.registerForm = new FormGroup({
			Email: new FormControl(this.registerModel.Email, {
				validators: [Validators.required, Validators.email]
			}),
			UserName: new FormControl(this.registerModel.UserName, {
				validators: [Validators.required, Validators.minLength(5)]
			}),
			FirstName: new FormControl(this.registerModel.FirstName, {
				validators: [Validators.required, Validators.minLength(2)]
			}),
			LastName: new FormControl(this.registerModel.LastName, {
				validators: [Validators.required, Validators.minLength(2)]
			}),
			UserPassword: new FormControl(this.registerModel.UserPassword, {
				validators: [Validators.required, Validators.minLength(8), Validators.pattern(this.strongPassRegex)]
			}),
			UserPasswordRepeated: new FormControl(this.registerModel.UserPasswordRepeated, {
				validators: [Validators.required, Validators.minLength(8), Validators.pattern(this.strongPassRegex), mustMatch('UserPassword')]
			})
		});
	}

	register() {
		if (this.registerForm.valid) {
			this.blockUI.start();
			this.registerModel = this.registerForm.value;
			this.authService.Register(this.registerModel).subscribe({
				next: (_response: HttpEvent<any>) => {
					this.dataService.setRegistered(true);
					this.blockUI.stop();
					this.router.navigate(['/Login']);
				},
				error: (error: HttpErrorResponse) => {
					if (error.status === HttpStatusCode.BadRequest && error.error && error.error.value) {
						this.errorMessage = error.error.value.errorMessage;
					} else {
						this.translate.get('Authentication_Login_UnexpectedError').subscribe((translation: string) => {
							this.errorMessage = translation;
						});
					}
					this.blockUI.stop();
				}
			});
		}
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
