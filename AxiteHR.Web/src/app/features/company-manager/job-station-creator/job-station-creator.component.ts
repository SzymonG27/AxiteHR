import { Component } from '@angular/core';
import {
	FormControl,
	FormGroup,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { JobStationCreatorRequest } from '../../../core/models/company-manager/job-station/JobStationCreatorRequest';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { JobStationService } from '../../../core/services/company-manager/job-station.service';
import { first, firstValueFrom } from 'rxjs';
import { JobStationCreatorResponse } from '../../../core/models/company-manager/job-station/JobStationCreatorResponse';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { AlertService } from '../../../core/services/alert/alert.service';

@Component({
	selector: 'app-job-station-creator',
	imports: [CommonModule, FormsModule, ReactiveFormsModule, TranslateModule],
	templateUrl: './job-station-creator.component.html',
	styleUrl: './job-station-creator.component.css',
})
export class JobStationCreatorComponent {
	roleNameMaxLength = 100;

	focusRoleName = false;
	focusRoleNameEng = false;

	errorMessage: string | null = null;
	companyId: number | null = null;

	jobStationCreatorForm: FormGroup;
	jobStationCreatorRequest: JobStationCreatorRequest = {
		companyId: 0,
		userRequestedId: '',
		roleName: '',
		roleNameEng: '',
	};

	constructor(
		private router: Router,
		private blockUI: BlockUIService,
		private route: ActivatedRoute,
		private jobStation: JobStationService,
		private translate: TranslateService,
		private alertService: AlertService
	) {
		this.companyId = this.route.snapshot.parent?.params['id'];
		if (this.companyId == undefined || this.companyId == null) {
			//ToDo client logger
			this.router.navigate(['Internal-Error']);
			this.blockUI.stop();
		}

		this.jobStationCreatorForm = new FormGroup({
			roleName: new FormControl(this.jobStationCreatorRequest.roleName, {
				validators: [Validators.required, Validators.maxLength(this.roleNameMaxLength)],
			}),
			roleNameEng: new FormControl(this.jobStationCreatorRequest.roleNameEng, {
				validators: [Validators.required, Validators.maxLength(this.roleNameMaxLength)],
			}),
		});
	}

	sendCreator(): void {
		if (!this.jobStationCreatorForm.valid) {
			return;
		}

		this.blockUI.start();
		this.jobStationCreatorRequest = this.jobStationCreatorForm.value;
		this.jobStationCreatorRequest.companyId = this.companyId!;

		this.jobStation
			.create(this.jobStationCreatorRequest)
			.pipe(first())
			.subscribe({
				next: async (response: JobStationCreatorResponse) => {
					if (response.isSucceeded) {
						const jobStationCreatedMessage = await firstValueFrom(
							this.translate.get('JobStation_JobStationCreate_Created')
						);
						this.alertService.showAlert(jobStationCreatedMessage);
						this.blockUI.stop();
						this.router.navigate(['/CompanyMenu', this.companyId, 'JobStationList']);
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

	goBack(): void {
		if (window.history.length > 1) {
			window.history.back();
		} else {
			this.router.navigateByUrl('/Dashboard');
		}
	}
}
