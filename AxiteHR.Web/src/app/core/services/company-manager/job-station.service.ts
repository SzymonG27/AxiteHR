import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, first, map, Observable, of, take } from 'rxjs';
import { JobStationListViewModel } from '../../models/company-manager/job-station/JobStationListViewModel';
import { JobStationListItem } from '../../models/company-manager/job-station/JobStationListItem';
import { Environment } from '../../../environment/Environment';
import { ApiPaths } from '../../../environment/ApiPaths';
import { AuthStateService } from '../authentication/auth-state.service';
import { JobStationCreatorRequest } from '../../models/company-manager/job-station/JobStationCreatorRequest';
import { JobStationCreatorResponse } from '../../models/company-manager/job-station/JobStationCreatorResponse';
import { TranslateService } from '@ngx-translate/core';
import { JobStationListRequest } from '../../models/company-manager/job-station/JobStationListRequest';

@Injectable({
	providedIn: 'root',
})
export class JobStationService {
	constructor(
		private http: HttpClient,
		private authStateService: AuthStateService,
		private translate: TranslateService
	) {}

	getList(
		requestModel: JobStationListRequest,
		page: number,
		itemsPerPage: number
	): Observable<JobStationListViewModel> {
		const userId = this.authStateService.getLoggedUserId();

		const jobStationListViewModel: JobStationListViewModel = {
			isSucceed: false,
			errorMessage: '',
			jobStationList: [],
		};

		if (userId.length === 0) {
			return of(jobStationListViewModel);
		}

		return this.http
			.get<
				JobStationListItem[]
			>(`${Environment.gatewayApiUrl}${ApiPaths.JobStationList}?CompanyId=${requestModel.companyId}&RoleName=${requestModel.roleName}&UserRequestedId=${userId}&Page=${page}&ItemsPerPage=${itemsPerPage}`)
			.pipe(
				map(data => {
					jobStationListViewModel.isSucceed = true;
					jobStationListViewModel.jobStationList = data;
					return jobStationListViewModel;
				}),
				catchError(error => {
					jobStationListViewModel.isSucceed = false;
					jobStationListViewModel.errorMessage = error.message;
					return of(jobStationListViewModel);
				})
			);
	}

	getCountList(requestModel: JobStationListRequest) {
		let employeeListCount = 0;
		const userId = this.authStateService.getLoggedUserId();

		if (userId.length === 0) {
			return of(0);
		}

		return this.http
			.get<number>(
				`${Environment.gatewayApiUrl}${ApiPaths.JobStationListCount}?CompanyId=${requestModel.companyId}&RoleName=${requestModel.roleName}&UserRequestedId=${userId}`
			)
			.pipe(
				map(data => {
					employeeListCount = data;
					return employeeListCount;
				}),
				catchError(() => {
					return of(employeeListCount);
				})
			);
	}

	create(request: JobStationCreatorRequest): Observable<JobStationCreatorResponse> {
		request.userRequestedId = this.authStateService.getLoggedUserId();

		const jobStationCreatorResponse: JobStationCreatorResponse = {
			isSucceeded: false,
			errorMessage: '',
			companyRoleId: 0,
			companyRoleCompanyId: 0,
		};

		if (request.userRequestedId.length === 0) {
			this.translate
				.get('Global_UserNotLogged')
				.pipe(
					first(),
					map((translation: string) => translation)
				)
				.subscribe(message => (jobStationCreatorResponse.errorMessage = message));

			return of(jobStationCreatorResponse);
		}

		return this.http
			.post<JobStationCreatorResponse>(
				`${Environment.gatewayApiUrl}${ApiPaths.JobStationCreate}`,
				request
			)
			.pipe(take(1));
	}
}
