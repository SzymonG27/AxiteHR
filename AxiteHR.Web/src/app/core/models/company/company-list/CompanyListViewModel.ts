import { CompanyListItem } from './CompanyListItem';

export interface CompanyListViewModel {
	isSucceed: boolean;
	errorMessage: string;
	companyList: CompanyListItem[];
}
