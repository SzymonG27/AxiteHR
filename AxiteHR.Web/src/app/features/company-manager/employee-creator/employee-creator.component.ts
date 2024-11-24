import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
	FormControl,
	FormGroup,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { EmployeeCreatorRequest } from '../../../core/models/company-manager/employee-creator/EmployeeCreatorRequest';
import { EmployeeService } from '../../../core/services/company-manager/employee.service';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { first, firstValueFrom } from 'rxjs';
import { EmployeeCreatorResponse } from '../../../core/models/company-manager/employee-creator/EmployeeCreatorResponse';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { AlertService } from '../../../core/services/alert/alert.service';

@Component({
	selector: 'app-employee-creator',
	imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, TranslateModule],
	templateUrl: './employee-creator.component.html',
	styleUrl: './employee-creator.component.css',
})
export class EmployeeCreatorComponent {
	focusEmail = false;
	focusPassword = false;
	focusUserName = false;
	focusFirstName = false;
	focusLastName = false;
	errorMessage: string | null = null;

	companyId: number | null = null;
	employeeCreatorForm: FormGroup;
	employeeCreatorModel: EmployeeCreatorRequest = {
		companyId: 0,
		email: '',
		userName: '',
		firstName: '',
		lastName: '',
		insUserId: '',
	};

	constructor(
		private employeeService: EmployeeService,
		private alertService: AlertService,
		private router: Router,
		private blockUI: BlockUIService,
		private translate: TranslateService,
		private route: ActivatedRoute
	) {
		this.companyId = this.route.snapshot.parent?.params['id'];
		if (this.companyId == undefined || this.companyId == null) {
			//ToDo client logger
			this.router.navigate(['Internal-Error']);
			this.blockUI.stop();
		}

		this.employeeCreatorForm = new FormGroup({
			email: new FormControl(this.employeeCreatorModel.email, {
				validators: [Validators.required, Validators.email],
			}),
			userName: new FormControl(this.employeeCreatorModel.userName, {
				validators: [Validators.required, Validators.minLength(5)],
			}),
			firstName: new FormControl(this.employeeCreatorModel.firstName, {
				validators: [Validators.required, Validators.minLength(2)],
			}),
			lastName: new FormControl(this.employeeCreatorModel.lastName, {
				validators: [Validators.required, Validators.minLength(2)],
			}),
		});
	}

	createNewEmployee() {
		if (!this.employeeCreatorForm.valid) {
			return;
		}

		this.blockUI.start();
		this.employeeCreatorModel = this.employeeCreatorForm.value;
		this.employeeCreatorModel.companyId = this.companyId!;
		this.employeeService
			.createNewEmployee(this.employeeCreatorModel)
			.pipe(first())
			.subscribe({
				next: async (response: EmployeeCreatorResponse) => {
					if (response.isSucceeded) {
						const employeeCreatedMessage = await firstValueFrom(
							this.translate.get('Company_EmployeeList_EmployeeCreated')
						);
						this.alertService.showAlert(employeeCreatedMessage);
						this.blockUI.stop();
						this.router.navigate(['/CompanyMenu', this.companyId, 'EmployeeList']);
						return;
					}
					this.errorMessage = response.errorMessage;
					this.blockUI.stop();
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
}
