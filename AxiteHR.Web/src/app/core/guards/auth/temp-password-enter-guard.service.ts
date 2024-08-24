import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthStateService } from "../../services/authentication/auth-state.service";

export const TempPasswordEnterGuard: CanActivateFn = () => {
	const authStateService = inject(AuthStateService);
    const router = inject(Router);

    const tempPassword = authStateService.getTempPasswordUserId();
    if (tempPassword === null || tempPassword.length === 0) {
        router.navigate(['Login']);
		return false;
    }
	return true;
};