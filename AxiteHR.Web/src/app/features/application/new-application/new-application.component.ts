import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, AfterViewInit } from '@angular/core';
import {
	FormControl,
	FormGroup,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ApplicationType } from '../../../core/models/application/ApplicationType';
import { dateGreaterThanOrEqualsTo } from '../../../shared/validators/date-greater-than-or-equals-to.validator';
import { NewApplicationFormRequest } from '../../../core/models/application/new-application/NewApplicationFormRequest';
import { animate, style, transition, trigger } from '@angular/animations';
import { greaterThan } from '../../../shared/validators/greater-than.validator';
import { maxPeriodDifference } from '../../../shared/validators/max-period-difference.validator';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { requiredIfFalse } from '../../../shared/validators/required-if-false.validator';
import { first, firstValueFrom, Subject, take, takeUntil } from 'rxjs';
import { DataBehaviourService } from '../../../core/services/data/data-behaviour.service';
import { NewApplicationService } from '../../../core/services/application/new-application.service';
import { NewApplicationResponse } from '../../../core/models/application/new-application/NewApplicationResponse';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { AlertService } from '../../../core/services/alert/alert.service';

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
export class NewApplicationComponent implements OnDestroy, OnInit, AfterViewInit {
	workingHoursFrom = 8;
	workingHoursTo = 16;
	maxHours = this.workingHoursTo - this.workingHoursFrom;
	reasonMaxLength = 200;

	private destroy$ = new Subject<void>();

	errorMessage: string | null = null;
	applicationCreatorForm: FormGroup;
	applicationFormCreatorRequest: NewApplicationFormRequest = {
		newApplicationRequest: {
			companyId: 0,
			userId: '',
			applicationType: ApplicationType.Vacation,
			periodFrom: null,
			periodTo: null,
			reason: '',
		},
		isFullDay: true,
		hoursFrom: null,
		hoursTo: null,
	};

	hoursFromBeforeFullDayDisabled: number | null = null;
	hoursToBeforeFullDayDisabled: number | null = null;

	companyId: number | null = null;

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
		private blockUI: BlockUIService,
		private dataService: DataBehaviourService,
		private newApplicationService: NewApplicationService,
		private route: ActivatedRoute,
		private translate: TranslateService,
		private alertService: AlertService
	) {
		this.companyId = this.route.snapshot.parent?.params['id'];
		if (this.companyId == undefined || this.companyId == null) {
			//ToDo client logger
			this.router.navigate(['Internal-Error']);
			this.blockUI.stop();
		}

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
			return;
		}

		this.blockUI.start();

		this.applicationFormCreatorRequest = this.newApplicationService.mapFormToRequest(
			this.applicationCreatorForm,
			this.companyId!
		);

		const periodFrom = this.applicationFormCreatorRequest.newApplicationRequest.periodFrom
			? new Date(this.applicationFormCreatorRequest.newApplicationRequest.periodFrom)
			: null;
		const periodTo = this.applicationFormCreatorRequest.newApplicationRequest.periodTo
			? new Date(this.applicationFormCreatorRequest.newApplicationRequest.periodTo)
			: null;

		const isFullDayControlValue = this.applicationCreatorForm.get('isFullDay')
			?.value as boolean;

		if (isFullDayControlValue) {
			periodFrom?.setHours(this.workingHoursFrom);
			periodTo?.setHours(this.workingHoursTo);
		} else {
			periodFrom?.setHours(this.applicationFormCreatorRequest.hoursFrom!);
			periodTo?.setHours(this.applicationFormCreatorRequest.hoursTo!);
		}
		this.applicationFormCreatorRequest.newApplicationRequest.periodFrom = periodFrom;
		this.applicationFormCreatorRequest.newApplicationRequest.periodTo = periodTo;

		this.newApplicationService
			.createNewApplication(this.applicationFormCreatorRequest.newApplicationRequest)
			.pipe(first())
			.subscribe({
				next: async (response: NewApplicationResponse) => {
					if (response.isSucceeded) {
						const newApplicationCreatedSuccessfully = await firstValueFrom(
							this.translate.get('Application_SendApplication_Success')
						);
						this.alertService.showAlert(newApplicationCreatedSuccessfully);
						this.blockUI.stop();
						this.router.navigate(['/CompanyMenu', this.companyId, 'Calendar']);
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
							this.translate.get('Application_NewApplicationCreator_UnexpectedError')
						);
						this.errorMessage = null;
						this.alertService.showAlert(unexpectedErrorTranslation, 'error');
					}
					this.blockUI.stop();
				},
			});
	}

	removeFocus(elementId: string): void {
		const element = document.getElementById(elementId);
		element?.blur();
	}

	open(elementId: string): void {
		const element = document.getElementById(elementId);
		element?.focus();
	}

	toggleFullDayDisabled(): void {
		const fullDayElement = document.getElementById('isFullDay') as HTMLInputElement;

		const periodFromToValid = this.applicationCreatorForm.get('periodFrom')
			?.value as Date | null;
		const periodToToValid = this.applicationCreatorForm.get('periodTo')?.value as Date | null;

		const hoursFromControl = this.applicationCreatorForm.get('hoursFrom');
		const hoursToControl = this.applicationCreatorForm.get('hoursTo');
		const isFullDayControl = this.applicationCreatorForm.get('isFullDay');

		const disableFullDayControls = () => {
			this.hoursFromBeforeFullDayDisabled = hoursFromControl?.value ?? null;
			this.hoursToBeforeFullDayDisabled = hoursToControl?.value ?? null;
			hoursFromControl?.setValue(null);
			hoursToControl?.setValue(null);
			isFullDayControl?.setValue(true);
			fullDayElement.disabled = true;
		};

		if (!periodFromToValid || !periodToToValid) {
			disableFullDayControls();
			return;
		}

		if (periodFromToValid !== periodToToValid) {
			disableFullDayControls();
			return;
		}

		fullDayElement.disabled = false;
		if (this.hoursFromBeforeFullDayDisabled && this.hoursFromBeforeFullDayDisabled) {
			hoursFromControl?.setValue(this.hoursFromBeforeFullDayDisabled);
			hoursToControl?.setValue(this.hoursToBeforeFullDayDisabled);
		}
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

	ngAfterViewInit(): void {
		this.dataService.selectedDate.pipe(take(1)).subscribe(async (value: Date | null) => {
			if (value) {
				const year = value.getFullYear();
				const month = String(value.getMonth() + 1).padStart(2, '0');
				const day = String(value.getDate()).padStart(2, '0');

				const formattedDate = `${year}-${month}-${day}`;
				this.applicationCreatorForm.get('periodFrom')?.setValue(formattedDate);
				this.applicationCreatorForm.get('periodTo')?.setValue(formattedDate);
			}
		});
		this.toggleFullDayDisabled();
	}

	ngOnDestroy(): void {
		document.removeEventListener('click', this.handleClickOutside);

		this.destroy$.next();
		this.destroy$.complete();
	}
}
