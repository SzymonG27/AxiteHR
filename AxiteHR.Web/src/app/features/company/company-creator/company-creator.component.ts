import { Component } from '@angular/core';
import {
	FormControl,
	FormGroup,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { CompanyService } from '../../../core/services/company/company.service';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { firstValueFrom, take } from 'rxjs';
import { AlertService } from '../../../core/services/alert/alert.service';

@Component({
	selector: 'app-company-creator',
	imports: [CommonModule, TranslateModule, ReactiveFormsModule, RouterModule, FormsModule],
	templateUrl: './company-creator.component.html',
	styleUrl: './company-creator.component.css',
})
export class CompanyCreatorComponent {
	companyStateName = false;
	companyCreatorForm: FormGroup;
	errorMessage: string | null = null;
	companyName = '';

	constructor(
		private blockUIService: BlockUIService,
		private companyService: CompanyService,
		private translate: TranslateService,
		private router: Router,
		private alertService: AlertService
	) {
		this.companyCreatorForm = new FormGroup({
			CompanyName: new FormControl(this.companyName, {
				validators: [Validators.required],
			}),
		});
	}

	createNewCompany() {
		if (!this.companyCreatorForm.valid) {
			return;
		}
		this.blockUIService.start();
		this.companyName = this.companyCreatorForm.get('CompanyName')?.value;
		this.companyService
			.createNewCompany(this.companyName)
			.pipe(take(1))
			.subscribe({
				next: async () => {
					const companyCreatorSuccess: string = await firstValueFrom(
						this.translate.get('Company_Creator_Success')
					);
					this.alertService.showAlert(companyCreatorSuccess);
					this.router.navigate(['/Company/List']);
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
					this.blockUIService.stop();
				},
			});
	}

	toggleCompanyNameState(state: boolean) {
		if (state) {
			this.companyStateName = true;
			return;
		}
		this.companyStateName = false;
	}
}
