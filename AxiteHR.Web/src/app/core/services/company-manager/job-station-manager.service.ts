import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, of } from 'rxjs';
import { JobStationAttachUserViewModel } from '../../models/company-manager/job-station-manager/JobStationAttachUserViewModel';
import { AuthStateService } from '../authentication/auth-state.service';
import { Environment } from '../../../environment/Environment';
import { ApiPaths } from '../../../environment/ApiPaths';
import { EmployeeListItem } from '../../models/company-manager/employee-list/EmployeeListItem';

@Injectable({
	providedIn: 'root',
})
export class JobStationManagerService {
	constructor(
		private http: HttpClient,
		private authStateService: AuthStateService
	) {}

	getListOfEmployeesToAdd(
		companyId: number,
		currentPage: number,
		pageSize: number
	): Observable<JobStationAttachUserViewModel> {
		const userId = this.authStateService.getLoggedUserId();

		const jobStationAttachUserViewModel: JobStationAttachUserViewModel = {
			isSucceed: false,
			errorMessage: '',
			employeeList: [],
		};

		if (userId.length === 0) {
			return of(jobStationAttachUserViewModel);
		}

		return this.http
			.get<
				EmployeeListItem[]
			>(`${Environment.gatewayApiUrl}${ApiPaths.ListEmployeesToAttach}/${companyId}/${userId}?Page=${currentPage}&ItemsPerPage=${pageSize}`)
			.pipe(
				map(data => {
					jobStationAttachUserViewModel.isSucceed = true;
					jobStationAttachUserViewModel.employeeList = data;
					return jobStationAttachUserViewModel;
				}),
				catchError(error => {
					jobStationAttachUserViewModel.isSucceed = false;
					jobStationAttachUserViewModel.errorMessage = error.message;
					return of(jobStationAttachUserViewModel);
				})
			);
	}

	getCountOfEmployeesToAdd(companyId: number): Observable<number> {
		let listOfEmployeesCount = 0;
		const userId = this.authStateService.getLoggedUserId();

		if (userId.length === 0) {
			return of(0);
		}

		return this.http
			.get<number>(
				`${Environment.gatewayApiUrl}${ApiPaths.CountEmployeesToAttach}/${companyId}/${userId}`
			)
			.pipe(
				map(data => {
					listOfEmployeesCount = data;
					return listOfEmployeesCount;
				}),
				catchError(() => {
					return of(listOfEmployeesCount);
				})
			);
	}
}
