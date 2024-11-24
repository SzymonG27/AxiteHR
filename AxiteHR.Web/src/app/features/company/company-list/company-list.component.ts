import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CompanyListService } from '../../../core/services/company/company-list.service';
import { CompanyListViewModel } from '../../../core/models/company/company-list/CompanyListViewModel';
import { CompanyListItem } from '../../../core/models/company/company-list/CompanyListItem';
import { firstValueFrom, take } from 'rxjs';
import { BlockUIService } from '../../../core/services/block-ui.service';

@Component({
	selector: 'app-company-list',
	imports: [CommonModule, RouterModule, TranslateModule],
	templateUrl: './company-list.component.html',
	styleUrl: './company-list.component.css',
})
export class CompanyListComponent implements OnInit {
	isLoadingTableError = false;
	errorMessage: string | null = null;
	companyList: CompanyListItem[] = [];
	constructor(
		private companyListService: CompanyListService,
		private blockUIService: BlockUIService,
		private translate: TranslateService
	) {}

	ngOnInit() {
		this.blockUIService.start();
		this.companyListService
			.getCompanyListView()
			.pipe(take(1))
			.subscribe({
				next: (response: CompanyListViewModel) => {
					if (!response.isSucceed) {
						this.isLoadingTableError = true;
						this.errorMessage = response.errorMessage;
					}
					this.companyList = response.companyList;
					this.blockUIService.stop();
				},
				error: async () => {
					//ToDo message
					this.isLoadingTableError = true;
					this.errorMessage = await firstValueFrom(
						this.translate.get('Global_ErrorFetchingData')
					);
					this.blockUIService.stop();
				},
			});
	}
}
