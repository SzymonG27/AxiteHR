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
import { ApplicationType } from '../../../core/models/application/ApplicationType';
import { dateGreaterThanOrEqualsTo } from '../../../shared/validators/date-greater-than-or-equals-to.validator';
import { NewApplicationFormRequest } from '../../../core/models/application/new-application/NewApplicationFormRequest';
import { requiredIfTrue } from '../../../shared/validators/required-if-true.validator';
import { animate, style, transition, trigger } from '@angular/animations';
import { greaterThan } from '../../../shared/validators/greater-than.validator';
import { maxPeriodDifference } from '../../../shared/validators/max-period-difference.validator';
import { BlockUIService } from '../../../core/services/block-ui.service';

@Component({
	selector: 'app-new-application',
	standalone: true,
	imports: [CommonModule, FormsModule, ReactiveFormsModule, TranslateModule],
	animations: [
		trigger('slideDownUp', [
			transition(':enter', [
				style({ height: '0', opacity: 0, paddingTop: 0, paddingBottom: 0 }),
				animate(
					'300ms ease-out',
					style({ height: '*', opacity: 1, paddingTop: '*', paddingBottom: '*' })
				),
			]),
			transition(':leave', [
				animate(
					'300ms ease-in',
					style({ height: '0', opacity: 0, paddingTop: 0, paddingBottom: 0 })
				),
			]),
		]),
	],
	templateUrl: './new-application.component.html',
	styleUrl: './new-application.component.css',
})
export class NewApplicationComponent implements OnDestroy, OnInit {
	workingHoursFrom = 8;
	workingHoursTo = 16;
	maxHours = this.workingHoursTo - this.workingHoursFrom;
	reasonMaxLength = 200;

	errorMessage: string | null = null;
	applicationCreatorForm: FormGroup;
	applicationFormCreatorRequest: NewApplicationFormRequest = {
		newApplicationRequest: {
			companyUserId: 0,
			applicationType: ApplicationType.Vacation,
			periodFrom: new Date(),
			periodTo: new Date(),
			reason: '',
		},
		isFullDay: true,
		hoursFrom: null,
		hoursTo: null,
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

	constructor(
		private router: Router,
		private blockUI: BlockUIService
	) {
		this.applicationCreatorForm = new FormGroup(
			{
				//Request fields
				applicationType: new FormControl(
					this.applicationFormCreatorRequest.newApplicationRequest.applicationType,
					{
						validators: [Validators.required],
					}
				),
				periodFrom: new FormControl(
					this.applicationFormCreatorRequest.newApplicationRequest.periodFrom,
					{
						validators: [Validators.required],
					}
				),
				periodTo: new FormControl(
					this.applicationFormCreatorRequest.newApplicationRequest.periodTo,
					{
						validators: [Validators.required],
					}
				),
				reason: new FormControl(
					this.applicationFormCreatorRequest.newApplicationRequest.reason,
					{
						validators: [Validators.maxLength(this.reasonMaxLength)],
					}
				),
				//Additional fields
				isFullDay: new FormControl(this.applicationFormCreatorRequest.isFullDay),
				hoursFrom: new FormControl(this.applicationFormCreatorRequest.hoursFrom, {
					validators: [
						requiredIfTrue('isFullDay'),
						Validators.min(this.workingHoursFrom),
						Validators.max(this.workingHoursTo),
					],
				}),
				hoursTo: new FormControl(this.applicationFormCreatorRequest.hoursTo, {
					validators: [
						requiredIfTrue('isFullDay'),
						Validators.min(this.workingHoursFrom),
						Validators.max(this.workingHoursTo),
						greaterThan('hoursFrom'),
					],
				}),
			},
			{
				validators: [
					dateGreaterThanOrEqualsTo('periodFrom', 'periodTo'),
					maxPeriodDifference('hoursFrom', 'hoursTo', this.maxHours),
				],
			}
		);
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

	sendApplication(): void {
		if (!this.applicationCreatorForm.valid) {
			return;
		}

		this.blockUI.start();
		this.applicationFormCreatorRequest = this.applicationCreatorForm.value;
		this.blockUI.stop();
	}

	removeFocus(elementId: string): void {
		const element = document.getElementById(elementId);
		element?.blur();
	}

	open(elementId: string): void {
		const element = document.getElementById(elementId);
		element?.focus();
	}

	ngOnInit(): void {
		document.addEventListener('click', this.handleClickOutside);
	}

	ngOnDestroy(): void {
		document.removeEventListener('click', this.handleClickOutside);
	}
}
