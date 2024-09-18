import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';

@Injectable()
export class LanguageInterceptor implements HttpInterceptor {
	constructor(private translate: TranslateService) {}

	intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
		const currentLang = this.translate.currentLang || 'en';

		const clonedRequest = req.clone({
			setHeaders: {
				'Accept-Language': currentLang,
			},
		});
		return next.handle(clonedRequest);
	}
}
