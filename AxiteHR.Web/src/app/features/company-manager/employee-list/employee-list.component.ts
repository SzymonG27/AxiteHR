import { Component, OnInit } from '@angular/core';
import { CompanyManagerListService } from '../../../core/services/company-manager/company-manager-list.service';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { EmployeeListItem } from '../../../core/models/company-manager/employee-list/EmployeeListItem';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { firstValueFrom, Observable, of, switchMap, take, zip } from 'rxjs';
import { NgxPaginationModule } from 'ngx-pagination';
import { Pagination } from '../../../shared/models/Pagination';

@Component({
	selector: 'app-employee-list',
	imports: [CommonModule, RouterModule, TranslateModule, NgxPaginationModule],
	templateUrl: './employee-list.component.html',
	styleUrl: './employee-list.component.css',
})
export class EmployeeListComponent implements OnInit {
	errorMessage: string | null = null;
	employeeList: EmployeeListItem[] = [];
	companyId: number | null = null;
	errorPage: string | null = null;
	forkHelper: unknown[] = [];

	//Pagination
	pagination: Pagination = new Pagination();

	constructor(
		private companyManagerListService: CompanyManagerListService,
		private blockUIService: BlockUIService,
		private translate: TranslateService,
		private route: ActivatedRoute,
		private router: Router
	) {}

	ngOnInit() {
		this.blockUIService.start();

		this.companyId = this.route.snapshot.parent?.params['id'];
		if (this.companyId == undefined || this.companyId == null) {
			//ToDo client logger
			this.router.navigate(['Internal-Error']);
			this.blockUIService.stop();
		}

		zip(
			this.getEmployeeListCount(this.companyId!),
			this.getEmployeeListViewPage(
				this.companyId!,
				this.pagination.pageNumber - 1,
				this.pagination.pageSize
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

	pageChanged(event: number) {
		this.blockUIService.start();

		this.pagination.pageNumber = event;

		zip(
			this.getEmployeeListViewPage(
				this.companyId!,
				this.pagination.pageNumber - 1,
				this.pagination.pageSize
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

	private getEmployeeListViewPage(
		passedCompanyId: number,
		currentPage: number,
		pageSize: number
	): Observable<void> {
		return this.companyManagerListService
			.getEmployeeListView(passedCompanyId, currentPage, pageSize)
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

	private getEmployeeListCount(passedCompanyId: number): Observable<void> {
		return this.companyManagerListService.getEmployeeListCount(passedCompanyId).pipe(
			take(1),
			switchMap(response => {
				this.pagination.totalItems = response;
				return of(void 0);
			})
		);
	}
}
