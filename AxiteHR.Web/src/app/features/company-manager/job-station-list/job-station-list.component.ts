import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Pagination } from '../../../shared/models/Pagination';
import { NgxPaginationModule } from 'ngx-pagination';
import { JobStationListItem } from '../../../core/models/company-manager/job-station/JobStationListItem';
import { JobStationService } from '../../../core/services/company-manager/job-station.service';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { firstValueFrom, Observable, of, switchMap, take, zip } from 'rxjs';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { JobStationListRequest } from '../../../core/models/company-manager/job-station/JobStationListRequest';
import { FormsModule } from '@angular/forms';
import { ListFilterComponent } from '../../../shared/components/list-filter/list-filter.component';

@Component({
	selector: 'app-job-station-list',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule,
		TranslateModule,
		NgxPaginationModule,
		FormsModule,
		ListFilterComponent,
	],
	templateUrl: './job-station-list.component.html',
	styleUrl: './job-station-list.component.css',
})
export class JobStationListComponent implements OnInit {
	//Pagination
	pagination: Pagination = new Pagination();

	jobStationList: JobStationListItem[] = [];
	companyId: number | null = null;
	errorMessage: string | null = null;
	isFilterVisible = false;

	jobStationListRequest: JobStationListRequest = {
		companyId: 0,
		roleName: '',
	};

	constructor(
		private jobStationService: JobStationService,
		private blockUIService: BlockUIService,
		private translate: TranslateService,
		private route: ActivatedRoute,
		private router: Router
	) {}

	ngOnInit(): void {
		this.blockUIService.start();

		this.companyId = this.route.snapshot.parent?.params['id'];
		if (this.companyId == undefined || this.companyId == null) {
			this.router.navigate(['Internal-Error']);
			this.blockUIService.stop();
		}

		this.jobStationListRequest.companyId = this.companyId!;

		this.searchJobStation();
	}

	pageChanged(event: number) {
		this.blockUIService.start();

		this.pagination.pageNumber = event;
		this.jobStationListRequest.companyId = this.companyId!;

		zip(
			this.getJobStationList(
				this.jobStationListRequest,
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

	toggleFilter() {
		this.isFilterVisible = !this.isFilterVisible;
	}

	searchByFilter() {
		this.blockUIService.start();
		this.searchJobStation();
	}

	clearFilter() {
		this.blockUIService.start();
		this.jobStationListRequest.roleName = '';
		this.searchJobStation();
	}

	private searchJobStation() {
		zip(
			this.getJobStationListCount(this.jobStationListRequest),
			this.getJobStationList(
				this.jobStationListRequest,
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

	private getJobStationList(
		requestModel: JobStationListRequest,
		currentPage: number,
		pageSize: number
	): Observable<void> {
		return this.jobStationService.getList(requestModel, currentPage, pageSize).pipe(
			take(1),
			switchMap(response => {
				if (!response.isSucceed) {
					//ToDo error handler
					this.errorMessage = response.errorMessage;
				}
				this.jobStationList = response.jobStationList;
				return of(void 0);
			})
		);
	}

	private getJobStationListCount(requestModel: JobStationListRequest): Observable<void> {
		return this.jobStationService.getCountList(requestModel).pipe(
			take(1),
			switchMap(response => {
				this.pagination.totalItems = response;
				return of(void 0);
			})
		);
	}
}
