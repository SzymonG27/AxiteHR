import { Injectable } from '@angular/core';
import { JwtPayloadClient } from '../../../core/models/authentication/JwtPayloadClient';
import { jwtDecode } from 'jwt-decode';
import { AuthDictionary } from '../../../shared/dictionary/AuthDictionary';

@Injectable({
	providedIn: 'root',
})
export class JWTTokenService {
	getDecodedToken(): JwtPayloadClient {
		const token = localStorage.getItem(AuthDictionary.Token);
		if (!token) {
			return new JwtPayloadClient();
		}

		return jwtDecode<JwtPayloadClient>(token);
	}

	getToken(): string {
		return localStorage.getItem(AuthDictionary.Token) ?? '';
	}
}
