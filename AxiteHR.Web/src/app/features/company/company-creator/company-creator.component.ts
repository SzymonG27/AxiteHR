import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CompanyCreatorRequest } from '../../../core/models/company/company-creator/CompanyCreatorRequest';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { CompanyService } from '../../../core/services/company/company.service';
import { CompanyCreatorResponse } from '../../../core/models/company/company-creator/CompanyCreatorResonse';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { first, take } from 'rxjs';

@Component({
	selector: 'app-company-creator',
	standalone: true,
	imports: [
		CommonModule,
		TranslateModule,
		ReactiveFormsModule,
		RouterModule,
		FormsModule
	],
	templateUrl: './company-creator.component.html',
	styleUrl: './company-creator.component.css'
})
export class CompanyCreatorComponent {
	companyStateName: boolean = false;
	companyCreatorForm: FormGroup;
	errorMessage: string | null = null;
	companyCreatorModel: CompanyCreatorRequest = new CompanyCreatorRequest();

	constructor(private blockUIService: BlockUIService,
		private companyService: CompanyService,
		private translate: TranslateService,
		private router: Router) {
		this.companyCreatorForm = new FormGroup( {
			CompanyName: new FormControl(this.companyCreatorModel.companyName, {
				validators: [Validators.required]
			})
		});
	}

	createNewCompany() {
		if (!this.companyCreatorForm.valid) {
			return;
		}
		this.blockUIService.start();
		this.companyCreatorModel = this.companyCreatorForm.value;
		this.companyService.createNewCompany(this.companyCreatorModel).pipe(take(1)).subscribe({
			next: () => {
				this.router.navigate(['/Company/List']);
				this.blockUIService.stop();
			},
			error: (error: HttpErrorResponse) => {
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
					this.translate.get('Authentication_Login_UnexpectedError')
						.pipe(first())
						.subscribe((translation: string) => {
							this.errorMessage = '*' + translation;
						});
				}
				this.blockUIService.stop();
			}
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
