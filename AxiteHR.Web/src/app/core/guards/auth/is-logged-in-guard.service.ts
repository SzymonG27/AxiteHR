import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthStateService } from '../../services/authentication/auth-state.service';
import { firstValueFrom } from 'rxjs';

export const IsLoggedInGuard: CanActivateFn = async route => {
	const authStateService = inject(AuthStateService);
	const router = inject(Router);

	const isLoggedIn = await firstValueFrom(authStateService.isLoggedIn);

	if (!isLoggedIn) {
		const returnUrl = '/' + route.url.map(segment => segment.path).join('/');
		router.navigate(['/Login'], { queryParams: { returnUrl: returnUrl } });
		return false;
	}
	return true;
};
