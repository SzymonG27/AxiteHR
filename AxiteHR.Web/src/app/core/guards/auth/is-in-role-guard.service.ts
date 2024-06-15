import { inject } from "@angular/core"
import { CanActivateFn, Router } from "@angular/router"
import { AuthStateService } from "../../services/authentication/auth-state.service"

export const IsInRoleGuardService: CanActivateFn = (route) => {
	const authStateService = inject(AuthStateService);
	const router = inject(Router);

	const requiredRoles: string[] = route.data['requiredRoles'];
	if (requiredRoles.length === 0) {
		return true;
	}

	const hasRole = requiredRoles.some(role => authStateService.hasRole(role));
	if (!hasRole) {
		router.navigate(['No-Access']);
		return false;
	}
	return true;
}
