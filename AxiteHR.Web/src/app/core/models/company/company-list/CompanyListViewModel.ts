import { CompanyListItem } from "./CompanyListItem";

export type CompanyListViewModel = {
	isSucceed: boolean;
	errorMessage: string;
	companyList: CompanyListItem[];
}