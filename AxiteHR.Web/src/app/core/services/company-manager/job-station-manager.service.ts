import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, of } from 'rxjs';
import { JobStationListUserViewModel } from '../../models/company-manager/job-station-manager/JobStationListUserViewModel';
import { AuthStateService } from '../authentication/auth-state.service';
import { Environment } from '../../../environment/Environment';
import { ApiPaths } from '../../../environment/ApiPaths';
import { EmployeeListItem } from '../../models/company-manager/employee-list/EmployeeListItem';
import { JobStationAddEmployeeResponse } from '../../models/company-manager/job-station/JobStationAddEmployeeResponse';
import { JobStationAddEmployeeRequest } from '../../models/company-manager/job-station/JobStationAddEmployeeRequest';

@Injectable({
	providedIn: 'root',
})
export class JobStationManagerService {
	constructor(
		private http: HttpClient,
		private authStateService: AuthStateService
	) {}

	getListOfEmployeesInJobStation(
		companyId: number,
		companyRoleCompanyId: number,
		currentPage: number,
		pageSize: number
	): Observable<JobStationListUserViewModel> {
		const userId = this.authStateService.getLoggedUserId();

		const jobStationAttachUserViewModel: JobStationListUserViewModel = {
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
			>(`${Environment.gatewayApiUrl}${ApiPaths.CompanyRoleEmployeeList}/${companyId}/${companyRoleCompanyId}/${userId}?Page=${currentPage}&ItemsPerPage=${pageSize}`)
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

	getCountOfEmployeesInJobStation(
		companyId: number,
		companyRoleCompanyId: number
	): Observable<number> {
		let listOfEmployeesCount = 0;
		const userId = this.authStateService.getLoggedUserId();

		if (userId.length === 0) {
			return of(0);
		}

		return this.http
			.get<number>(
				`${Environment.gatewayApiUrl}${ApiPaths.CompanyRoleEmployeeCount}/${companyId}/${companyRoleCompanyId}/${userId}`
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

	getListOfEmployeesToAdd(
		companyId: number,
		currentPage: number,
		pageSize: number
	): Observable<JobStationListUserViewModel> {
		const userId = this.authStateService.getLoggedUserId();

		const jobStationAttachUserViewModel: JobStationListUserViewModel = {
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

	addEmployeeToJobStation(
		companyId: number,
		roleCompanyId: number,
		companyUserToAttachId: number
	): Observable<JobStationAddEmployeeResponse> {
		const userId = this.authStateService.getLoggedUserId();

		const jobStationAddEmployeeResponse: JobStationAddEmployeeResponse = {
			isSucceeded: false,
			errorMessage: '',
			jobStationName: '',
			employeeName: '',
		};

		if (userId.length === 0) {
			return of(jobStationAddEmployeeResponse);
		}

		const jobStationAddEmployeeRequest: JobStationAddEmployeeRequest = {
			companyId: companyId,
			companyRoleCompanyId: roleCompanyId,
			companyUserToAttachId: companyUserToAttachId,
			userRequestedId: userId,
		};

		return this.http.post<JobStationAddEmployeeResponse>(
			`${Environment.gatewayApiUrl}${ApiPaths.AttachUserAsync}`,
			jobStationAddEmployeeRequest
		);
	}
}
