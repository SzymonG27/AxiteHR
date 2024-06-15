import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AuthDictionary } from '../../../shared/dictionary/AuthDictionary';
import { JwtPayloadClient } from '../../models/authentication/JwtPayloadClient';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthStateService {
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());

	private hasToken(): boolean {
		return !!localStorage.getItem(AuthDictionary.Token);
	}
	
	get isLoggedIn() {
		return this.loggedIn.asObservable();
	}

	setLoggedIn(isLoggedIn: boolean) {
		this.loggedIn.next(isLoggedIn);
	}

	private decodeToken(token: string): JwtPayloadClient {
		return jwtDecode<JwtPayloadClient>(token);
	}

	getUserRoles(token: string | null): string[] {
		if (token) {
			const decoded = this.decodeToken(token);
			if (Array.isArray(decoded.role)) {
			  return decoded.role;
			} else if (decoded.role) {
			  return [decoded.role]; // Converts a single role into an array
			}
		}
		return []; // Returns an empty array if no roles are present or token not provided
	}

	hasRole(role: string) {
		const userRoles = this.getUserRoles(localStorage.getItem(AuthDictionary.Token))
		return userRoles.includes(role);
	}
}
