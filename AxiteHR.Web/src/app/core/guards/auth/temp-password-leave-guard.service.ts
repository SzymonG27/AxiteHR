import { inject } from '@angular/core';
import { AuthStateService } from '../../services/authentication/auth-state.service';
import { CanDeactivateFn } from '@angular/router';

export const TempPasswordLeaveGuard: CanDeactivateFn<unknown> = () => {
	const authStateService = inject(AuthStateService);
	authStateService.removeTempPasswordUserId();
	return true;
};
