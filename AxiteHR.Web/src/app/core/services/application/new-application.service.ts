import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NewApplicationFormRequest } from '../../models/application/new-application/NewApplicationFormRequest';
import { NewApplicationForm } from '../../models/application/new-application/NewApplicationForm';
import { NewApplicationRequest } from '../../models/application/new-application/NewApplicationRequest';
import { NewApplicationResponse } from '../../models/application/new-application/NewApplicationResponse';

@Injectable({
	providedIn: 'root',
})
export class NewApplicationService {
	mapFormToRequest(formGroup: FormGroup): NewApplicationFormRequest {
		const form = formGroup.value as NewApplicationForm;
		return {
			newApplicationRequest: {
				companyUserId: 0,
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

	createNewApplication(newApplicationRequest: NewApplicationRequest): Observable<NewApplicationResponse> {
		
	}
}
