import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CompanyCreatorRequest } from '../../models/company/company-creator/CompanyCreatorRequest';
import { Observable, take } from 'rxjs';
import { CompanyCreatorResponse } from '../../models/company/company-creator/CompanyCreatorResonse';
import { Environment } from '../../../environment/Environment';
import { ApiPaths } from '../../../environment/ApiPaths';
import { AuthStateService } from '../authentication/auth-state.service';
import { AuthDictionary } from '../../../shared/dictionary/AuthDictionary';

@Injectable({
	providedIn: 'root'
})
export class CompanyService {

	constructor(private http: HttpClient, private authStateService: AuthStateService) { }

	public createNewCompany(newCompany: CompanyCreatorRequest) : Observable<CompanyCreatorResponse> {
		newCompany.creatorId = this.authStateService.getLoggedUserId(localStorage.getItem(AuthDictionary.Token));
		
		return this.http.post<CompanyCreatorResponse>(
			`${Environment.gatewayApiUrl}${ApiPaths.CompanyCreator}`,
			newCompany
		).pipe(take(1));
	}
}
