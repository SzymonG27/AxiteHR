import { EmployeeListItem } from "./EmployeeListItem";

export interface EmployeeListViewModel {
	isSucceed: boolean;
	errorMessage: string;
	employeeList: EmployeeListItem[];
}