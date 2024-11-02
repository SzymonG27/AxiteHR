import { ApplicationType } from '../ApplicationType';

export interface NewApplicationRequest {
	companyId: number;
	userId: string;
	applicationType: ApplicationType;
	periodFrom: Date | null;
	periodTo: Date | null;
	reason?: string;
}
