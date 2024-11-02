import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NewApplicationFormRequest } from '../../models/application/new-application/NewApplicationFormRequest';
import { NewApplicationForm } from '../../models/application/new-application/NewApplicationForm';
import { NewApplicationRequest } from '../../models/application/new-application/NewApplicationRequest';
import { NewApplicationResponse } from '../../models/application/new-application/NewApplicationResponse';
import { AuthStateService } from '../authentication/auth-state.service';
import { TranslateService } from '@ngx-translate/core';
import { first, map, Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Environment } from '../../../environment/Environment';
import { ApiPaths } from '../../../environment/ApiPaths';
import { ApplicationType } from '../../models/application/ApplicationType';

@Injectable({
	providedIn: 'root',
})
export class NewApplicationService {
	constructor(
		private http: HttpClient,
		private authStateService: AuthStateService,
		private translate: TranslateService
	) {}

	mapFormToRequest(formGroup: FormGroup, companyId: number): NewApplicationFormRequest {
		const form = formGroup.value as NewApplicationForm;
		return {
			newApplicationRequest: {
				companyId: companyId,
				userId: '',
				applicationType: form.applicationType,
				periodFrom: form.periodFrom,
				periodTo: form.periodTo,
				reason: form.reason,
			},
			isFullDay: form.isFullDay,
			hoursFrom: form.hoursFrom,
			hoursTo: form.hoursTo,
		};
	}

	createNewApplication(
		newApplicationRequest: NewApplicationRequest
	): Observable<NewApplicationResponse> {
		const userId = this.authStateService.getLoggedUserId();
		if (userId.length === 0) {
			const responseError: NewApplicationResponse = {
				isSucceeded: false,
				errorMessage: null,
			};

			this.translate
				.get('Global_UserNotLogged')
				.pipe(
					first(),
					map((translation: string) => translation)
				)
				.subscribe(message => (responseError.errorMessage = message));

			return of(responseError);
		}

		newApplicationRequest.userId = userId;
		newApplicationRequest.applicationType = this.convertToApplicationType(
			newApplicationRequest.applicationType
		);

		return this.http.post<NewApplicationResponse>(
			`${Environment.gatewayApiUrl}${ApiPaths.NewApplicationCreator}`,
			newApplicationRequest
		);
	}

	private convertToApplicationType(value: string | number): ApplicationType {
		const parsedValue = typeof value === 'string' ? parseInt(value, 10) : value;

		if (Object.values(ApplicationType).includes(parsedValue)) {
			return parsedValue as ApplicationType;
		} else {
			throw new Error(`Invalid ApplicationType: ${value}`);
		}
	}
}
