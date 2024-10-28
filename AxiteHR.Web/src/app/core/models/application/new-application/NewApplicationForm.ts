import { ApplicationType } from '../ApplicationType';

export interface NewApplicationForm {
	applicationType: ApplicationType;
	periodFrom: Date | null;
	periodTo: Date | null;
	isFullDay: boolean;
	hoursFrom: number | null;
	hoursTo: number | null;
	reason?: string;
}
