import { Component } from '@angular/core';
import {
	FormControl,
	FormGroup,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { JobStationCreatorRequest } from '../../../core/models/company-manager/job-station/JobStationCreatorRequest';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

@Component({
	selector: 'app-job-station-creator',
	imports: [CommonModule, FormsModule, ReactiveFormsModule, TranslateModule],
	templateUrl: './job-station-creator.component.html',
	styleUrl: './job-station-creator.component.css',
})
export class JobStationCreatorComponent {
	roleNameMaxLength = 100;

	focusRoleName = false;
	focusRoleNameEng = false;

	errorMessage: string | null = null;
	companyId: number | null = null;

	jobStationCreatorForm: FormGroup;
	jobStationCreatorRequest: JobStationCreatorRequest = {
		companyId: 0,
		userRequestedId: '',
		roleName: '',
		roleNameEng: '',
	};

	constructor(
		private router: Router,
		private blockUI: BlockUIService,
		private route: ActivatedRoute
	) {
		this.companyId = this.route.snapshot.parent?.params['id'];
		if (this.companyId == undefined || this.companyId == null) {
			//ToDo client logger
			this.router.navigate(['Internal-Error']);
			this.blockUI.stop();
		}

		this.jobStationCreatorForm = new FormGroup({
			roleName: new FormControl(this.jobStationCreatorRequest.roleName, {
				validators: [Validators.required, Validators.maxLength(this.roleNameMaxLength)],
			}),
			roleNameEng: new FormControl(this.jobStationCreatorRequest.roleNameEng, {
				validators: [Validators.required, Validators.maxLength(this.roleNameMaxLength)],
			}),
		});
	}

	sendCreator(): void {}

	goBack(): void {
		if (window.history.length > 1) {
			window.history.back();
		} else {
			this.router.navigateByUrl('/Dashboard');
		}
	}
}
