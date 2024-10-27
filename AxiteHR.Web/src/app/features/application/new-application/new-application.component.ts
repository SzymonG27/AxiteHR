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
import { animate, style, transition, trigger } from '@angular/animations';
import { greaterThan } from '../../../shared/validators/greater-than.validator';
import { maxPeriodDifference } from '../../../shared/validators/max-period-difference.validator';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { requiredIfFalse } from '../../../shared/validators/required-if-false.validator';
import { Subject, takeUntil } from 'rxjs';

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

	private destroy$ = new Subject<void>();

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
					validators: [requiredIfFalse('isFullDay')],
				}),
				hoursTo: new FormControl(this.applicationFormCreatorRequest.hoursTo, {
					validators: [requiredIfFalse('isFullDay')],
				}),
			},
			{
				validators: [
					dateGreaterThanOrEqualsTo('periodFrom', 'periodTo'),
					maxPeriodDifference('hoursFrom', 'hoursTo', this.maxHours),
					greaterThan('hoursFrom', 'hoursTo'),
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
		this.applicationCreatorForm.markAllAsTouched();
		this.applicationCreatorForm.updateValueAndValidity();

		if (!this.applicationCreatorForm.valid) {
			const invalidFields = Object.keys(this.applicationCreatorForm.controls)
				.filter(name => this.applicationCreatorForm.controls[name].invalid)
				.map(name => {
					const controlErrors = this.applicationCreatorForm.controls[name].errors;
					return {
						field: name,
						errors: controlErrors ? Object.keys(controlErrors) : [],
					};
				});

			console.log('Invalid fields and their errors:', invalidFields);
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

		this.applicationCreatorForm
			.get('isFullDay')
			?.valueChanges.pipe(takeUntil(this.destroy$))
			.subscribe((isFullDay: boolean) => {
				const hoursFromControl = this.applicationCreatorForm.get('hoursFrom');
				const hoursToControl = this.applicationCreatorForm.get('hoursTo');

				if (isFullDay === false) {
					hoursFromControl?.setValidators([
						requiredIfFalse('isFullDay'),
						Validators.min(this.workingHoursFrom),
						Validators.max(this.workingHoursTo),
					]);

					hoursToControl?.setValidators([
						requiredIfFalse('isFullDay'),
						Validators.min(this.workingHoursFrom),
						Validators.max(this.workingHoursTo),
					]);
				} else {
					hoursFromControl?.setValidators([requiredIfFalse('isFullDay')]);
					hoursToControl?.setValidators([requiredIfFalse('isFullDay')]);
				}

				hoursFromControl?.updateValueAndValidity();
				hoursToControl?.updateValueAndValidity();
			});
	}

	ngOnDestroy(): void {
		document.removeEventListener('click', this.handleClickOutside);

		this.destroy$.next();
		this.destroy$.complete();
	}
}
