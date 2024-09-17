import { HttpClient, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { RegisterRequest } from '../../../core/models/authentication/RegisterRequest';
import { Environment } from '../../../environment/Environment';
import { ApiPaths } from '../../../environment/ApiPaths';
import { LoginRequest } from '../../../core/models/authentication/LoginRequest';
import { LoginResponse } from '../../../core/models/authentication/LoginResponse';
import { AuthDictionary } from '../../../shared/dictionary/AuthDictionary';
import { AuthStateService } from './auth-state.service';
import { TempPasswordChangeRequest } from '../../models/authentication/TempPasswordChangeRequest';
import { TempPasswordChangeResponse } from '../../models/authentication/TempPasswordChangeResponse';

@Injectable({
	providedIn: 'root'
})
export class AuthenticationService {
	constructor(private http: HttpClient, private authState: AuthStateService) {}

	public Register(register: RegisterRequest): Observable<HttpEvent<unknown>> {
		return this.http.post<HttpEvent<unknown>>(
			`${Environment.authApiUrl}${ApiPaths.Register}`,
			register
		);
	}

	public Login(login: LoginRequest): Observable<LoginResponse> {
		return this.http.post<LoginResponse>(
			`${Environment.authApiUrl}${ApiPaths.Login}`,
			login
		);
	}

	public LogOut() {
		const token = localStorage.getItem(AuthDictionary.Token);
		if (token) {
			localStorage.removeItem(AuthDictionary.Token);
			this.authState.setLoggedIn(false);
		}
	}

	public TempPasswordChange(tempPasswordChange: TempPasswordChangeRequest) {
		return this.http.post<TempPasswordChangeResponse>(
			`${Environment.authApiUrl}${ApiPaths.TempPasswordChange}`,
			tempPasswordChange	
		).pipe(take(1));
	}
}
