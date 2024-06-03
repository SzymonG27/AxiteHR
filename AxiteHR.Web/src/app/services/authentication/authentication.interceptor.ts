import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable } from "rxjs";
import { jwtDecode } from "jwt-decode";
import { JwtPayloadClient } from "../../models/authentication/JwtPayloadClient";
import { Router } from "@angular/router";
import { DataBehaviourService } from "../data/data-behaviour.service";

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {
	constructor(private dataService: DataBehaviourService) {}

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		let router = inject(Router);

		const token = localStorage.getItem('authToken');

		if (token) {
			let decodedToken: JwtPayloadClient = jwtDecode<JwtPayloadClient>(token);
			let isExpired: boolean = decodedToken && decodedToken.exp
				? decodedToken.exp < Date.now() / 1000
				: false;

			if (isExpired) {
				localStorage.removeItem('authToken');
				this.dataService.setIsTokenExpired(true);
				router.navigate(['/Login']);
			}

			req = req.clone({
				setHeaders: { Authorization: `Bearer ${token}` }
			});
		}

		return next.handle(req);
	}
}