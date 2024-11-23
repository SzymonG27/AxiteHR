import { JobStationListItem } from './JobStationListItem';

export interface JobStationListViewModel {
	isSucceed: boolean;
	errorMessage: string;
	jobStationList: JobStationListItem[];
}
