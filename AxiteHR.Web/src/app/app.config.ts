import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { AuthenticationInterceptor } from './core/interceptors/authentication.interceptor';
import { BlockUIModule, BlockUIService } from 'ng-block-ui';

export const appConfig: ApplicationConfig = {
	providers: [
		provideZoneChangeDetection({ eventCoalescing: true }),
		provideRouter(routes),
		provideAnimations(),
		importProvidersFrom(BlockUIModule.forRoot()),
		provideHttpClient(),
		{
			provide: HTTP_INTERCEPTORS,
			useClass: AuthenticationInterceptor,
			multi: true
		},
	]
};
