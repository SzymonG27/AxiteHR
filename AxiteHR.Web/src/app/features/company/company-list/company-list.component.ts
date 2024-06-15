import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { CompanyListService } from '../../../core/services/company/company-list.service';
import { CompanyListViewModel } from '../../../core/models/company/CompanyListViewModel';
import { CompanyListItem } from '../../../core/models/company/CompanyListItem';
import { take } from 'rxjs';

@Component({
	selector: 'app-company-list',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule,
		TranslateModule
	],
	templateUrl: './company-list.component.html',
	styleUrl: './company-list.component.css'
})
export class CompanyListComponent {
	isLoadingTableError: boolean = false;
	errorMessage: string | null = null;
	companyList: CompanyListItem[] = [];
	constructor(private companyListService: CompanyListService) { }

	ngOnInit() {
		this.companyListService.getCompanyListView().pipe(
			take(1)
		).subscribe({
			next: (response: CompanyListViewModel) => {
				if (!response.isSucceed) {
					this.isLoadingTableError = true;
					this.errorMessage = response.errorMessage;
				}
				this.companyList = response.companyList;
			},
			error: () => {
				//ToDo message
				this.isLoadingTableError = true;
				this.errorMessage = "An error occurred while fetching data";
			}
		});
	}
}
