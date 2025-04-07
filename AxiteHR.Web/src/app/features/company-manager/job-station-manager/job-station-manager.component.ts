import { Component, OnInit } from '@angular/core';
import { JobStationState } from '../../../core/models/company-manager/job-station/JobStationState';
import { CompanyManagerStateService } from '../../../core/services/company-manager/company-manager-state.service';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { DropListComponent } from '../../../shared/components/drop-list/drop-list.component';

@Component({
	selector: 'app-job-station-manager',
	imports: [DropListComponent],
	templateUrl: './job-station-manager.component.html',
	styleUrl: './job-station-manager.component.css',
})
export class JobStationManagerComponent implements OnInit {
	jobStationState: JobStationState | null = null;

	constructor(
		private companyManagerStateService: CompanyManagerStateService,
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
}
