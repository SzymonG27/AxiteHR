import { ApplicationType } from '../ApplicationType';

export interface NewApplicationRequest {
	companyUserId: number;
	applicationType: ApplicationType;
	periodFrom: Date;
	periodTo: Date;
	reason?: string;
}
