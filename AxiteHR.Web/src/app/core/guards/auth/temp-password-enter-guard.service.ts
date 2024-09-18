import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthStateService } from '../../services/authentication/auth-state.service';

export const TempPasswordEnterGuard: CanActivateFn = () => {
	const authStateService = inject(AuthStateService);
	const router = inject(Router);

	const tempPasswordUserId = authStateService.getTempPasswordUserId();
	if (tempPasswordUserId === null || tempPasswordUserId.length === 0) {
		router.navigate(['Login']);
		return false;
	}
	return true;
};
