import { EmployeeListItem } from "./EmployeeListItem";

export type EmployeeListViewModel = {
	isSucceed: boolean;
	errorMessage: string;
	employeeList: EmployeeListItem[];
}