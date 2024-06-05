import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';

@Injectable()
export class LanguageInterceptor implements HttpInterceptor {
	constructor(private translate: TranslateService) { }

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		const clonedRequest = req.clone({
			setHeaders: {
			  'Accept-Language': this.translate.currentLang
			}
		});
		return next.handle(clonedRequest);
	}
}