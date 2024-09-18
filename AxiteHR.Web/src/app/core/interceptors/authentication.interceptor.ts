import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { JwtPayloadClient } from '../../core/models/authentication/JwtPayloadClient';
import { Router } from '@angular/router';
import { AuthDictionary } from '../../shared/dictionary/AuthDictionary';
import { DataBehaviourService } from '../services/data/data-behaviour.service';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {
	constructor(
		private dataService: DataBehaviourService,
		private router: Router
	) {}

	intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
		const token = localStorage.getItem(AuthDictionary.Token);

		if (token) {
			const decodedToken: JwtPayloadClient = jwtDecode<JwtPayloadClient>(token);
			const isExpired: boolean =
				decodedToken && decodedToken.exp ? decodedToken.exp < Date.now() / 1000 : false;

			if (isExpired) {
				localStorage.removeItem(AuthDictionary.Token);
				this.dataService.setIsTokenExpired(true);
				this.router.navigate(['/Login']);
			}

			req = req.clone({
				setHeaders: { Authorization: `${AuthDictionary.Bearer} ${token}` },
			});
		}

		return next.handle(req);
	}
}
