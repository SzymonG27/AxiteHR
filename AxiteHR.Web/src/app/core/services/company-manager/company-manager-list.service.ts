import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JWTTokenService } from '../authentication/jwttoken.service';
import { EmployeeListViewModel } from '../../models/company-manager/employee-list/EmployeeListViewModel';
import { catchError, map, Observable, of } from 'rxjs';
import { ApiPaths } from '../../../environment/ApiPaths';
import { EmployeeListItem } from '../../models/company-manager/employee-list/EmployeeListItem';
import { Environment } from '../../../environment/Environment';

@Injectable({
  providedIn: 'root'
})
export class CompanyManagerListService {

  constructor(private http: HttpClient, private jwtToken: JWTTokenService) { }

  getEmployeeListView(companyId: number, page: number, itemsPerPage: number): Observable<EmployeeListViewModel> {
		var employeeListViewModel = new EmployeeListViewModel();
		var decodedToken = this.jwtToken.getDecodedToken();
		if (!decodedToken) {
			employeeListViewModel.isSucceed = false;
			//ToDo Error message
			return of(employeeListViewModel);
		}

		return this.http.get<EmployeeListItem[]>(
			`${Environment.gatewayApiUrl}${ApiPaths.EmployeeList}/${companyId}/${decodedToken.sub}?Page=${page}&ItemsPerPage=${itemsPerPage}`
		).pipe(
			map(data => {
				employeeListViewModel.isSucceed = true;
				employeeListViewModel.employeeList = data;
				return employeeListViewModel;
			}),
			catchError(error => {
				employeeListViewModel.isSucceed = false;
				employeeListViewModel.errorMessage = error.message;
				return of(employeeListViewModel);
			})
		);
	}
}
