import { NewApplicationRequest } from './NewApplicationRequest';

export interface NewApplicationFormRequest {
	newApplicationRequest: NewApplicationRequest;
	isFullDay: boolean;
	hoursFrom: number | null;
	hoursTo: number | null;
}
