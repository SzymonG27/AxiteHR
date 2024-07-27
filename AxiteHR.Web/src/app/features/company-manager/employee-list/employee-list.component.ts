import { Component } from '@angular/core';
import { CompanyManagerListService } from '../../../core/services/company-manager/company-manager-list.service';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { EmployeeListItem } from '../../../core/models/company-manager/employee-list/EmployeeListItem';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { first, switchMap, take } from 'rxjs';
import { EmployeeListViewModel } from '../../../core/models/company-manager/employee-list/EmployeeListViewModel';
import { NgxPaginationModule } from 'ngx-pagination';
import { Pagination } from '../../../shared/models/Pagination';

@Component({
	selector: 'app-employee-list',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule,
		TranslateModule,
		NgxPaginationModule
	],
	templateUrl: './employee-list.component.html',
	styleUrl: './employee-list.component.css'
})
export class EmployeeListComponent {
	isLoadingTableError: boolean = false;
	errorMessage: string | null = null;
	employeeList: EmployeeListItem[] = [];
	companyId: number | null = null;

	//Pagination
	pagination: Pagination = new Pagination();

	constructor(
		private companyManagerListService: CompanyManagerListService,
		private blockUIService: BlockUIService,
		private translate: TranslateService,
		private route: ActivatedRoute
	) { }

	ngOnInit() {
		this.blockUIService.start();
	
		this.route.params.pipe(
		  first(),
		  switchMap(params => {
			this.companyId = params['id'];
			if (this.companyId == null) {
			  this.errorMessage = 'Error while getting parameters from URL';
			  this.blockUIService.stop();
			  throw new Error(this.errorMessage); // ToDo: Proper error handling
			}
			return this.getEmployeeListViewPage(this.companyId, this.pagination.pageNumber - 1, this.pagination.pageSize);
		  })
		).subscribe({
		  next: () => this.blockUIService.stop(),
		  error: (err) => {
			this.errorMessage = err.message || 'Unknown error';
			this.blockUIService.stop();
		  }
		});
	  }
	
	  pageChanged(event: number) {
		this.pagination.pageNumber = event;
		this.getEmployeeListViewPage(this.companyId!, this.pagination.pageNumber - 1, this.pagination.pageSize)
		  .subscribe({
			next: () => {},
			error: (err) => {
			  this.errorMessage = err.message || 'Unknown error';
			  this.blockUIService.stop();
			}
		  });
	  }
	
	  getEmployeeListViewPage(passedCompanyId: number, currentPage: number, pageSize: number) {
		return this.companyManagerListService.getEmployeeListView(passedCompanyId, currentPage, pageSize).pipe(
		  take(1),
		  switchMap(response => {
			if (!response.isSucceed) {
			  this.isLoadingTableError = true;
			  this.errorMessage = response.errorMessage;
			  this.blockUIService.stop();
			  throw new Error(this.errorMessage); // ToDo: Proper error handling
			}
			this.employeeList = response.employeeList;
			this.blockUIService.stop();
			return [];
		  })
		);
	  }
}
