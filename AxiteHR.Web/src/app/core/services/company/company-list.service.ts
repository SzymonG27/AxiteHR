import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JWTTokenService } from '../authentication/jwttoken.service';
import { Observable, catchError, map, of } from 'rxjs';
import { Environment } from '../../../environment/Environment';
import { ApiPaths } from '../../../environment/ApiPaths';
import { CompanyListItem } from '../../models/company/company-list/CompanyListItem';
import { CompanyListViewModel } from '../../models/company/company-list/CompanyListViewModel';

@Injectable({
	providedIn: 'root'
})
export class CompanyListService {

	constructor(private http: HttpClient, private jwtToken: JWTTokenService) { }

	getCompanyListView(): Observable<CompanyListViewModel> {
		var companyListViewModel = new CompanyListViewModel();
		var decodedToken = this.jwtToken.getDecodedToken();
		if (!decodedToken) {
			companyListViewModel.isSucceed = false;
			//ToDo Error message
			return of(companyListViewModel);
		}

		return this.http.get<CompanyListItem[]>(
			`${Environment.gatewayApiUrl}${ApiPaths.CompanyList}/${decodedToken.sub}`
		).pipe(
			map(data => {
			  companyListViewModel.isSucceed = true;
			  companyListViewModel.companyList = data;
			  return companyListViewModel;
			}),
			catchError(error => {
			  companyListViewModel.isSucceed = false;
			  companyListViewModel.errorMessage = error.message;
			  return of(companyListViewModel);
			})
		);
	}
}
