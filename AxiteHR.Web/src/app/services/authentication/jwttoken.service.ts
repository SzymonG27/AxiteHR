import { Injectable } from '@angular/core';
import { JwtPayloadClient } from '../../models/authentication/JwtPayloadClient';
import { jwtDecode } from 'jwt-decode';
import { AuthDictionary } from '../../core/environment/dictionary/AuthDictionary';

@Injectable({
	providedIn: 'root'
})
export class JWTTokenService {
	constructor() { }

	getDecodedToken(): JwtPayloadClient {
		const token = localStorage.getItem(AuthDictionary.Token);
		if (!token) {
			return new JwtPayloadClient();
		}

		return jwtDecode<JwtPayloadClient>(token);
	}
}
