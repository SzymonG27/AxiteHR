import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CompanyCreatorRequest } from '../../models/company/company-creator/CompanyCreatorRequest';
import { first, map, Observable, of, take } from 'rxjs';
import { CompanyCreatorResponse } from '../../models/company/company-creator/CompanyCreatorResonse';
import { Environment } from '../../../environment/Environment';
import { ApiPaths } from '../../../environment/ApiPaths';
import { AuthStateService } from '../authentication/auth-state.service';
import { TranslateService } from '@ngx-translate/core';
import { CompanyForEmployee } from '../../models/company/CompanyForEmployee';

@Injectable({
	providedIn: 'root',
})
export class CompanyService {
	constructor(
		private http: HttpClient,
		private authStateService: AuthStateService,
		private translate: TranslateService
	) {}

	public createNewCompany(companyName: string): Observable<CompanyCreatorResponse> {
		const newCompany: CompanyCreatorRequest = {
			companyName: companyName,
			creatorId: this.authStateService.getLoggedUserId(),
		};
		if (newCompany.creatorId.length === 0) {
			const responseError: CompanyCreatorResponse = {
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

		return this.http
			.post<CompanyCreatorResponse>(
				`${Environment.gatewayApiUrl}${ApiPaths.CompanyCreator}`,
				newCompany
			)
			.pipe(take(1));
	}

	public isUserInCompany(userId: string, companyId: number): Observable<boolean> {
		if (userId === '' || companyId === 0) {
			return of(false);
		}

		return this.http
			.get<boolean>(
				`${Environment.gatewayApiUrl}${ApiPaths.IsUserInCompany}/${userId}/${companyId}`
			)
			.pipe(take(1));
	}

	public getCompanyForEmployee(employeeId: string): Observable<CompanyForEmployee> {
		if (employeeId === '') {
			return of({
				companyId: 0,
				companyName: '',
			} as CompanyForEmployee);
		}

		return this.http
			.get<CompanyForEmployee>(
				`${Environment.gatewayApiUrl}${ApiPaths.GetCompanyForEmployee}/${employeeId}`
			)
			.pipe(take(1));
	}
}
