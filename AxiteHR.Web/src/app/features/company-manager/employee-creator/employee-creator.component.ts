import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { EmployeeCreatorRequest } from '../../../core/models/company-manager/employee-creator/EmployeeCreatorRequest';
import { EmployeeService } from '../../../core/services/company-manager/employee.service';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { first, firstValueFrom } from 'rxjs';
import { EmployeeCreatorResponse } from '../../../core/models/company-manager/employee-creator/EmployeeCreatorResponse';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { DataBehaviourService } from '../../../core/services/data/data-behaviour.service';

@Component({
	selector: 'app-employee-creator',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule,
		FormsModule,
		ReactiveFormsModule,
		TranslateModule
	],
	templateUrl: './employee-creator.component.html',
	styleUrl: './employee-creator.component.css'
})
export class EmployeeCreatorComponent {
	focusEmail: boolean = false;
	focusPassword: boolean = false;
	focusUserName: boolean = false;
	focusFirstName: boolean = false;
	focusLastName: boolean = false;
	errorMessage: string | null = null;

	companyId: number | null = null;
	employeeCreatorForm: FormGroup;
	employeeCreatorModel: EmployeeCreatorRequest = new EmployeeCreatorRequest();

	constructor(
		private employeeService: EmployeeService,
		private dataBehaviourService: DataBehaviourService,
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
				validators: [Validators.required, Validators.email]
			}),
			userName: new FormControl(this.employeeCreatorModel.userName, {
				validators: [Validators.required, Validators.minLength(5)]
			}),
			firstName: new FormControl(this.employeeCreatorModel.firstName, {
				validators: [Validators.required, Validators.minLength(2)]
			}),
			lastName: new FormControl(this.employeeCreatorModel.lastName, {
				validators: [Validators.required, Validators.minLength(2)]
			})
		});
	}

	createNewEmployee() {
		if (!this.employeeCreatorForm.valid) {
			return;
		}
		
		this.blockUI.start();
		this.employeeCreatorModel = this.employeeCreatorForm.value;
		this.employeeCreatorModel.companyId = this.companyId!;
		this.employeeService.createNewEmployee(this.employeeCreatorModel).pipe(first()).subscribe({
			next: (response: EmployeeCreatorResponse) => {
				if (response.isSucceeded) {
					this.dataBehaviourService.setNewEmployeeCreated(true);
					this.blockUI.stop();
					this.router.navigate(['/Manager', this.companyId, 'EmployeeList']);
					return;
				}
				this.errorMessage = response.errorMessage;
				this.blockUI.stop();
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
				this.blockUI.stop();
			}
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
