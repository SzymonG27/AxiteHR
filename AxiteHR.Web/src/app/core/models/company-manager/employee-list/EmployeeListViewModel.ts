import { EmployeeListItem } from "./EmployeeListItem";

export class EmployeeListViewModel {
	isSucceed: boolean = false;
	errorMessage: string = '';
	employeeList: EmployeeListItem[] = [];
}