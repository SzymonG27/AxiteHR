import { Component, OnInit } from '@angular/core';
import { JobStationState } from '../../../core/models/company-manager/job-station/JobStationState';
import { CompanyManagerStateService } from '../../../core/services/company-manager/company-manager-state.service';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { DropListComponent } from '../../../shared/components/drop-list/drop-list.component';
import { ModalComponent } from '../../../shared/components/modal/modal.component';
import { Pagination } from '../../../shared/models/Pagination';
import { JobStationManagerService } from '../../../core/services/company-manager/job-station-manager.service';
import { firstValueFrom, Observable, of, switchMap, take, zip } from 'rxjs';
import { EmployeeListItem } from '../../../core/models/company-manager/employee-list/EmployeeListItem';
import { NgxPaginationModule } from 'ngx-pagination';
import { CommonModule } from '@angular/common';

@Component({
	selector: 'app-job-station-manager',
	imports: [
		CommonModule,
		DropListComponent,
		TranslateModule,
		RouterModule,
		ModalComponent,
		NgxPaginationModule,
	],
	templateUrl: './job-station-manager.component.html',
	styleUrl: './job-station-manager.component.css',
})
export class JobStationManagerComponent implements OnInit {
	jobStationState: JobStationState | null = null;
	employeeList: EmployeeListItem[] = [];
	errorMessage: string | null = null;
	isModalAddEmployeeOpen = false;

	paginationAddEmployee: Pagination = new Pagination();

	constructor(
		private companyManagerStateService: CompanyManagerStateService,
		private jobStationManagerService: JobStationManagerService,
		private blockUIService: BlockUIService,
		private translate: TranslateService,
		private router: Router
	) {}

	ngOnInit(): void {
		this.blockUIService.start();

		this.jobStationState = this.companyManagerStateService.getStateJobStationManager();

		if (!this.jobStationState) {
			this.blockUIService.stop();
			this.router.navigate(['Internal-Error']);
		}

		this.blockUIService.stop();
	}

	getTranslatedCompanyRoleSettingsTitle() {
		return this.translate.instant('JobStation_Manager_CompanyRoleSettingsTitle');
	}

	getTranslatedCompanyRoleUserSettingsTitle() {
		return this.translate.instant('JobStation_Manager_CompanyRoleUserSettingsTitle');
	}

	//#region Modal Add Employee
	getTranslatedModalAddEmployeeTitle() {
		return this.translate.instant('JobStation_Manager_AddEmployeeToCompanyRole');
	}

	openModalAddEmployee() {
		this.blockUIService.start();

		this.isModalAddEmployeeOpen = true;
		this.searchEmployeesToAdd();

		this.blockUIService.stop();
	}

	closeModalAddEmployee() {
		this.isModalAddEmployeeOpen = false;
	}

	pageChangedAddEmployee(event: number) {
		this.paginationAddEmployee.pageNumber = event;

		this.searchEmployeesToAdd();
	}

	addEmployeeToJobStation(employee: EmployeeListItem) {
		this.blockUIService.start();

		this.jobStationManagerService
			.addEmployeeToJobStation(
				this.jobStationState!.companyId,
				this.jobStationState!.roleCompanyId,
				employee.companyUserId
			)
			.pipe(take(1))
			.subscribe({
				next: () => {
					//ToDo notification and router to manage page of employee
					this.blockUIService.stop();
				},
				error: async err => {
					this.errorMessage =
						err.message ||
						(await firstValueFrom(this.translate.get('Global_UnknownError')));
					this.blockUIService.stop();
				},
			});
	}

	private searchEmployeesToAdd() {
		zip(
			this.getCountOfEmployeesToAdd(this.jobStationState!.companyId),
			this.getListOfEmployeesToAdd(
				this.jobStationState!.companyId,
				this.paginationAddEmployee.pageNumber - 1,
				this.paginationAddEmployee.pageSize
			)
		)
			.pipe(take(1))
			.subscribe({
				next: () => {
					this.blockUIService.stop();
				},
				error: async err => {
					this.errorMessage =
						err.message ||
						(await firstValueFrom(this.translate.get('Global_UnknownError')));
					this.blockUIService.stop();
				},
			});
	}

	private getListOfEmployeesToAdd(
		companyId: number,
		currentPage: number,
		pageSize: number
	): Observable<void> {
		return this.jobStationManagerService
			.getListOfEmployeesToAdd(companyId, currentPage, pageSize)
			.pipe(
				take(1),
				switchMap(response => {
					if (!response.isSucceed) {
						//ToDo error handler
						this.errorMessage = response.errorMessage;
					}
					this.employeeList = response.employeeList;
					return of(void 0);
				})
			);
	}

	private getCountOfEmployeesToAdd(companyId: number): Observable<void> {
		return this.jobStationManagerService.getCountOfEmployeesToAdd(companyId).pipe(
			take(1),
			switchMap(response => {
				this.paginationAddEmployee.totalItems = response;
				return of(void 0);
			})
		);
	}
	//#endregion Modal Add Employee
}
