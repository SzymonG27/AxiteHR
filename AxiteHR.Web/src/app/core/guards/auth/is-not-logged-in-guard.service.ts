import { CanActivateFn, Router } from '@angular/router';
import { AuthStateService } from '../../services/authentication/auth-state.service';
import { inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

export const IsNotLoggedInGuard: CanActivateFn = async () => {
	const authStateService = inject(AuthStateService);
	const router = inject(Router);

	const isLoggedIn = await firstValueFrom(authStateService.isLoggedIn);

	if (isLoggedIn) {
		router.navigate(['']);
		return false;
	}

	return true;
};
