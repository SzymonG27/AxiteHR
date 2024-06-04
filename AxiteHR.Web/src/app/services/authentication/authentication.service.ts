import { HttpClient, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { RegisterRequest } from '../../models/authentication/RegisterRequest';
import { Environment } from '../../core/environment/Environment';
import { ApiPaths } from '../../core/environment/ApiPaths';
import { LoginRequest } from '../../models/authentication/LoginRequest';
import { LoginResponse } from '../../models/authentication/LoginResponse';
import { AuthDictionary } from '../../core/environment/dictionary/AuthDictionary';
import { AuthStateService } from './auth-state.service';

@Injectable({
	providedIn: 'root'
})
export class AuthenticationService {
	constructor(private http: HttpClient, private authState: AuthStateService) {}

	public Register(register: RegisterRequest): Observable<HttpEvent<any>> {
		return this.http.post<HttpEvent<any>>(
			`${Environment.authApiBaseUrl}${ApiPaths.Register}`,
			register
		);
	}

	public Login(login: LoginRequest): Observable<LoginResponse> {
		return this.http.post<{ value: LoginResponse }>(
			`${Environment.authApiBaseUrl}${ApiPaths.Login}`,
			login
		).pipe(
			map(response => response.value)
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
