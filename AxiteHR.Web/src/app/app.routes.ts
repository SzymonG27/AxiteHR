import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/authentication/login/login.component';
import { RegisterComponent } from './features/authentication/register/register.component';
import { CompanyListComponent } from './features/company-manager/company-list/company-list.component';

export const routes: Routes = [
	//Home
	{
		path: '',
		component: HomeComponent,
		data: { title: 'HOME_PAGE_TITLE' }
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
		data: { title: 'COMPANY_LIST_TITLE_PAGE' }
	}
];
