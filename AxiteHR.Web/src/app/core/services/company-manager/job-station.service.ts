import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { JobStationListViewModel } from '../../models/company-manager/job-station/JobStationListViewModel';
import { JobStationListItem } from '../../models/company-manager/job-station/JobStationListItem';
import { Environment } from '../../../environment/Environment';
import { ApiPaths } from '../../../environment/ApiPaths';

@Injectable({
	providedIn: 'root',
})
export class JobStationService {
	constructor(private http: HttpClient) {}

	getList(
		companyId: number,
		page: number,
		itemsPerPage: number
	): Observable<JobStationListViewModel> {
		const jobStationListViewModel: JobStationListViewModel = {
			isSucceed: false,
			errorMessage: '',
			jobStationList: [],
		};

		return this.http
			.get<
				JobStationListItem[]
			>(`${Environment.gatewayApiUrl}${ApiPaths.JobStationList}?CompanyId=${companyId}&Page=${page}&ItemsPerPage=${itemsPerPage}`)
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

	getCountList(companyId: number) {
		let employeeListCount = 0;

		return this.http
			.get<number>(
				`${Environment.gatewayApiUrl}${ApiPaths.JobStationListCount}?CompanyId=${companyId}`
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
}
