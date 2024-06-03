import { Injectable } from '@angular/core';
import { JwtPayloadClient } from '../../models/authentication/JwtPayloadClient';
import { jwtDecode } from 'jwt-decode';

@Injectable({
	providedIn: 'root'
})
export class JWTTokenService {
	constructor() { }

	getDecodedToken(): JwtPayloadClient {
		const token = localStorage.getItem('authToken');
		if (!token) {
			return new JwtPayloadClient();
		}

		return jwtDecode<JwtPayloadClient>(token);
	}
}
