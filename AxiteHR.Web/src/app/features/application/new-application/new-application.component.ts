import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import {
	FormControl,
	FormGroup,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { NewApplicationRequest } from '../../../core/models/application/new-application/NewApplicationRequest';
import { ApplicationType } from '../../../core/models/application/ApplicationType';

@Component({
	selector: 'app-new-application',
	standalone: true,
	imports: [CommonModule, FormsModule, ReactiveFormsModule, TranslateModule],
	templateUrl: './new-application.component.html',
	styleUrl: './new-application.component.css',
})
export class NewApplicationComponent implements OnDestroy, OnInit {
	errorMessage: string | null = null;
	applicationCreatorForm: FormGroup;
	applicationCreatorRequest: NewApplicationRequest = {
		companyUserId: 0,
		applicationType: ApplicationType.Vacation,
		periodFrom: new Date(),
		periodTo: new Date(),
		reason: '',
	};

	applicationTypeOptions = Object.values(ApplicationType).filter(
		value => typeof value === 'number'
	) as ApplicationType[];

	applicationTypeTranslationKeys = {
		[ApplicationType.Vacation]: 'Application_Types.Vacation',
		[ApplicationType.Inaccessibility]: 'Application_Types.Inaccessibility',
		[ApplicationType.UnpaidBreak]: 'Application_Types.UnpaidBreak',
		[ApplicationType.HomeWork]: 'Application_Types.HomeWork',
	};

	constructor(private router: Router) {
		this.applicationCreatorForm = new FormGroup({
			applicationType: new FormControl(this.applicationCreatorRequest.applicationType, {
				validators: [Validators.required],
			}),
			periodFrom: new FormControl(this.applicationCreatorRequest.periodFrom, {
				validators: [Validators.required],
			}),
			periodTo: new FormControl(this.applicationCreatorRequest.periodTo, {
				validators: [Validators.required],
			}),
			reason: new FormControl(this.applicationCreatorRequest.reason, {
				validators: [Validators.maxLength(2000)],
			}),
		});
	}

	goBack(): void {
		if (window.history.length > 1) {
			window.history.back();
		} else {
			this.router.navigateByUrl('/Dashboard');
		}
	}

	getApplicationTypeTranslation(type: ApplicationType): string {
		return this.applicationTypeTranslationKeys[type];
	}

	handleClickOutside = (event: MouseEvent) => {
		const selectElement = document.getElementById('applicationTypeSelect');
		if (
			selectElement &&
			event.target !== selectElement &&
			!selectElement.contains(event.target as Node)
		) {
			selectElement.blur();
		}
	};

	removeFocus(elementId: string): void {
		const element = document.getElementById(elementId);
		element?.blur();
	}

	ngOnInit(): void {
		document.addEventListener('click', this.handleClickOutside);
	}

	ngOnDestroy(): void {
		document.removeEventListener('click', this.handleClickOutside);
	}
}
