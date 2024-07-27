import { Component } from '@angular/core';
import { CompanyManagerListService } from '../../../core/services/company-manager/company-manager-list.service';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { EmployeeListItem } from '../../../core/models/company-manager/employee-list/EmployeeListItem';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { first, take } from 'rxjs';
import { EmployeeListViewModel } from '../../../core/models/company-manager/employee-list/EmployeeListViewModel';

@Component({
	selector: 'app-employee-list',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule,
		TranslateModule
	],
	templateUrl: './employee-list.component.html',
	styleUrl: './employee-list.component.css'
})
export class EmployeeListComponent {
	isLoadingTableError: boolean = false;
	errorMessage: string | null = null;
	employeeList: EmployeeListItem[] = [];

	constructor(
		private companyManagerListService: CompanyManagerListService,
		private blockUIService: BlockUIService,
		private translate: TranslateService,
		private route: ActivatedRoute
	) { }

	ngOnInit() {
		let companyId: number | null = null;
		this.blockUIService.start();

		this.route.params.pipe(first()).subscribe(params => {
			companyId = params['id'];
		});

		if (companyId == null) {
			this.errorMessage = "Error while getting parameters from url"; //ToDo Error
			return;
		}
		
		this.companyManagerListService.getEmployeeListView(companyId).pipe(
			take(1)
		).subscribe({
			next: (response: EmployeeListViewModel) => {
				if (!response.isSucceed) {
					this.isLoadingTableError = true;
					this.errorMessage = response.errorMessage;
				}
				this.employeeList = response.employeeList;
				this.blockUIService.stop();
			},
			error: () => {
				this.isLoadingTableError = true;
				this.translate.get('Global_ErrorFetchingData')
					.pipe(first())
					.subscribe((translation: string) => {
						this.errorMessage = translation;
					});
				this.blockUIService.stop();
			}
		});
	}
}
