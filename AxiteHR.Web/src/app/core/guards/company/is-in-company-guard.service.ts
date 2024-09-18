import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CompanyService } from '../../services/company/company.service';
import { AuthStateService } from '../../services/authentication/auth-state.service';
import { catchError, map, of } from 'rxjs';
import { BlockUIService } from '../../services/block-ui.service';

export const IsInCompanyGuard: CanActivateFn = route => {
	const authStateService = inject(AuthStateService);
	const blockUIService = inject(BlockUIService);
	const companyService = inject(CompanyService);
	const router = inject(Router);

	blockUIService.start();

	const userId = authStateService.getLoggedUserId();
	const companyId = Number(route.paramMap.get('id'));
	return companyService.isUserInCompany(userId, companyId).pipe(
		map((isInCompany: boolean) => {
			if (!isInCompany) {
				blockUIService.stop();
				router.navigate(['No-Access']);
				return false;
			}
			blockUIService.stop();
			return true;
		}),
		catchError(() => {
			blockUIService.stop();
			router.navigate(['No-Access']);
			return of(false);
		})
	);
};
