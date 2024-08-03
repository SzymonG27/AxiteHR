import { CompanyListItem } from "./CompanyListItem";

export class CompanyListViewModel {
	isSucceed: boolean = false;
	errorMessage: string = '';
	companyList: CompanyListItem[] = [];
}