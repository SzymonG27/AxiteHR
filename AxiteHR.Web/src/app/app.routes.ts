import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/authentication/login/login.component';
import { RegisterComponent } from './features/authentication/register/register.component';
import { CompanyListComponent } from './features/company/company-list/company-list.component';
import { IsLoggedInGuard } from './core/guards/auth/is-logged-in-guard.service';
import { NoAccessComponent } from './core/components/no-access/no-access/no-access.component';

export const routes: Routes = [
	//Home
	{
		path: '',
		component: HomeComponent,
		data: { title: 'HOME_PAGE_TITLE' }
	},
	//Shared
	{
		path: 'No-Access',
		component: NoAccessComponent,
		data: { title: 'NO_ACCESS_TITLE' }
	},
	//Auth
	{
		path: 'Login',
		component: LoginComponent,
		data: { title: 'LOGIN_PAGE_TITLE' }
	},
	{
		path: 'Register',
		component: RegisterComponent,
		data: { title: 'REGISTER_PAGE_TITLE' }
	},
	//Company
	{
		path: 'Company/List',
		component: CompanyListComponent,
		canActivate: [ IsLoggedInGuard ],
		data: { title: 'COMPANY_LIST_TITLE_PAGE' }
	}
];
