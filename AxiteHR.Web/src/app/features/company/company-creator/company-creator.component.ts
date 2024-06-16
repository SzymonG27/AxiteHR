import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { CompanyCreatorRequest } from '../../../core/models/company/CompanyCreatorRequest';
import { CommonModule } from '@angular/common';

@Component({
	selector: 'app-company-creator',
	standalone: true,
	imports: [
		CommonModule,
		TranslateModule,
		ReactiveFormsModule,
		FormsModule
	],
	templateUrl: './company-creator.component.html',
	styleUrl: './company-creator.component.css'
})
export class CompanyCreatorComponent {
	companyCreatorForm: FormGroup;
	companyCreatorModel: CompanyCreatorRequest = new CompanyCreatorRequest();

	constructor() {
		this.companyCreatorForm = new FormGroup( {
			CompanyName: new FormControl(this.companyCreatorModel.companyName, {
				validators: [Validators.required]
			})
		});
	}
}
