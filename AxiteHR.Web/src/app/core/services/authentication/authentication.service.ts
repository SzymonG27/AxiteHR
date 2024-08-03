import { HttpClient, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { RegisterRequest } from '../../../core/models/authentication/RegisterRequest';
import { Environment } from '../../../environment/Environment';
import { ApiPaths } from '../../../environment/ApiPaths';
import { LoginRequest } from '../../../core/models/authentication/LoginRequest';
import { LoginResponse } from '../../../core/models/authentication/LoginResponse';
import { AuthDictionary } from '../../../shared/dictionary/AuthDictionary';
import { AuthStateService } from './auth-state.service';

@Injectable({
	providedIn: 'root'
})
export class AuthenticationService {
	constructor(private http: HttpClient, private authState: AuthStateService) {}

	public Register(register: RegisterRequest): Observable<HttpEvent<any>> {
		return this.http.post<HttpEvent<any>>(
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
		let token = localStorage.getItem(AuthDictionary.Token);
		if (token) {
			localStorage.removeItem(AuthDictionary.Token);
			this.authState.setLoggedIn(false);
		}
	}
}
