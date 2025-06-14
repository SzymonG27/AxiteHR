import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AuthDictionary } from '../../../shared/dictionary/AuthDictionary';
import { JWTTokenService } from './jwttoken.service';

@Injectable({
	providedIn: 'root',
})
export class AuthStateService {
	private loggedIn = new BehaviorSubject<boolean>(this.hasToken());

	constructor(private jwtTokenService: JWTTokenService) {}

	private hasToken(): boolean {
		return !!localStorage.getItem(AuthDictionary.Token);
	}

	get isLoggedIn() {
		return this.loggedIn.asObservable();
	}

	setLoggedIn(isLoggedIn: boolean) {
		this.loggedIn.next(isLoggedIn);
	}

	getUserRoles(): string[] {
		const decoded = this.jwtTokenService.getDecodedToken();
		if (decoded) {
			if (Array.isArray(decoded.role)) {
				return decoded.role;
			} else if (decoded.role) {
				return [decoded.role]; // Converts a single role into an array
			}
		}
		return []; // Returns an empty array if no roles are present or token not provided
	}

	getLoggedUserId(): string {
		const decoded = this.jwtTokenService.getDecodedToken();
		if (decoded && decoded.sub != null) {
			return decoded.sub;
		}
		return '';
	}

	hasRole(role: string) {
		const userRoles = this.getUserRoles();
		return userRoles.includes(role);
	}

	hasAnyRole(roles: string[]) {
		return roles.some(role => this.hasRole(role));
	}

	setTempPasswordUserId(userId: string): void {
		localStorage.setItem(AuthDictionary.TempPasswordUserIdStorageKey, userId);
	}

	getTempPasswordUserId(): string | null {
		return localStorage.getItem(AuthDictionary.TempPasswordUserIdStorageKey);
	}

	removeTempPasswordUserId(): void {
		localStorage.removeItem(AuthDictionary.TempPasswordUserIdStorageKey);
	}

	private registeredSource = new BehaviorSubject<boolean>(false);
	currentRegistered = this.registeredSource.asObservable();
	setRegistered(value: boolean) {
		this.registeredSource.next(value);
	}

	private isTokenExpiredSource = new BehaviorSubject<boolean>(false);
	isTokenExpired = this.isTokenExpiredSource.asObservable();
	setIsTokenExpired(value: boolean) {
		this.isTokenExpiredSource.next(value);
	}

	private tempPasswordErrorSource = new BehaviorSubject<string>('');
	tempPasswordError = this.tempPasswordErrorSource.asObservable();
	setTempPasswordError(value: string) {
		this.tempPasswordErrorSource.next(value);
	}

	private tempPasswordSuccessSource = new BehaviorSubject<string>('');
	tempPasswordSuccess = this.tempPasswordSuccessSource.asObservable();
	setTempPasswordSuccess(value: string) {
		this.tempPasswordSuccessSource.next(value);
	}
}
