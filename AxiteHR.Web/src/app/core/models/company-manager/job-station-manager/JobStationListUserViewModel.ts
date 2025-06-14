import { EmployeeListItem } from '../employee-list/EmployeeListItem';

export interface JobStationListUserViewModel {
	isSucceed: boolean;
	errorMessage: string;
	employeeList: EmployeeListItem[];
}
