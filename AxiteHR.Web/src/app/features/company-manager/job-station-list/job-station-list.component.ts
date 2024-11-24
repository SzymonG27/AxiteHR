import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Pagination } from '../../../shared/models/Pagination';
import { NgxPaginationModule } from 'ngx-pagination';
import { JobStationListItem } from '../../../core/models/company-manager/job-station/JobStationListItem';
import { JobStationService } from '../../../core/services/company-manager/job-station.service';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { firstValueFrom, Observable, of, switchMap, take, zip } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
	selector: 'app-job-station-list',
	standalone: true,
	imports: [CommonModule, TranslateModule, NgxPaginationModule],
	templateUrl: './job-station-list.component.html',
	styleUrl: './job-station-list.component.css',
})
export class JobStationListComponent implements OnInit {
	//Pagination
	pagination: Pagination = new Pagination();
	jobStationList: JobStationListItem[] = [];
	companyId: number | null = null;
	errorMessage: string | null = null;

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

		zip(
			this.getJobStationListCount(this.companyId!),
			this.getJobStationList(
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
			this.getJobStationList(
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

	private getJobStationList(
		passedCompanyId: number,
		currentPage: number,
		pageSize: number
	): Observable<void> {
		return this.jobStationService.getList(passedCompanyId, currentPage, pageSize).pipe(
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

	private getJobStationListCount(passedCompanyId: number): Observable<void> {
		return this.jobStationService.getCountList(passedCompanyId).pipe(
			take(1),
			switchMap(response => {
				this.pagination.totalItems = response;
				return of(void 0);
			})
		);
	}
}
