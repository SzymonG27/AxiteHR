import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EmployeeCreatorRequest } from '../../models/company-manager/employee-creator/EmployeeCreatorRequest';
import { first, map, Observable, of, take } from 'rxjs';
import { EmployeeCreatorResponse } from '../../models/company-manager/employee-creator/EmployeeCreatorResponse';
import { TranslateService } from '@ngx-translate/core';
import { AuthStateService } from '../authentication/auth-state.service';
import { ApiPaths } from '../../../environment/ApiPaths';
import { Environment } from '../../../environment/Environment';

@Injectable({
	providedIn: 'root',
})
export class EmployeeService {
	constructor(
		private http: HttpClient,
		private authStateService: AuthStateService,
		private translate: TranslateService
	) {}

	createNewEmployee(
		newEmployeeRequest: EmployeeCreatorRequest
	): Observable<EmployeeCreatorResponse> {
		newEmployeeRequest.insUserId = this.authStateService.getLoggedUserId();
		if (newEmployeeRequest.insUserId.length === 0) {
			const responseError: EmployeeCreatorResponse = {
				isSucceeded: false,
				errorMessage: null,
				employeeId: '',
			};

			responseError.isSucceeded = false;
			this.translate
				.get('Global_UserNotLogged')
				.pipe(
					first(),
					map((translation: string) => translation)
				)
				.subscribe(message => (responseError.errorMessage = message));

			return of(responseError);
		}

		return this.http
			.post<EmployeeCreatorResponse>(
				`${Environment.gatewayApiUrl}${ApiPaths.EmployeeCreator}`,
				newEmployeeRequest
			)
			.pipe(take(1));
	}
}
