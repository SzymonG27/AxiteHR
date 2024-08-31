import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { CompanyService } from "../../services/company/company.service";
import { AuthStateService } from "../../services/authentication/auth-state.service";
import { catchError, map, of } from "rxjs";

export const IsInCompanyGuard: CanActivateFn = (route) => {
	const authStateService = inject(AuthStateService);
	const companyService = inject(CompanyService);
	const router = inject(Router);

	const userId = authStateService.getLoggedUserId();
	const companyId = Number(route.paramMap.get('id'));
	return companyService.isUserInCompany(userId, companyId).pipe(
		map((isInCompany: boolean) => {
			if (!isInCompany) {
				router.navigate(['No-Access']);
				return false;
			}
			return true;
		}),
		catchError(() => {
			router.navigate(['No-Access']);
			return of(false);
		})
	);
}