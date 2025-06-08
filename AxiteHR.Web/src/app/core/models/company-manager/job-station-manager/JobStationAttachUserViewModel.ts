import { EmployeeListItem } from '../employee-list/EmployeeListItem';

export interface JobStationAttachUserViewModel {
	isSucceed: boolean;
	errorMessage: string;
	employeeList: EmployeeListItem[];
}
