import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/authentication/login/login.component';
import { RegisterComponent } from './features/authentication/register/register.component';
import { CompanyListComponent } from './features/company/company-list/company-list.component';
import { IsLoggedInGuard } from './core/guards/auth/is-logged-in-guard.service';
import { NoAccessComponent } from './core/components/no-access/no-access.component';
import { IsInRoleGuard } from './core/guards/auth/is-in-role-guard.service';
import { UserRole } from './core/models/authentication/UserRole';
import { IsNotLoggedInGuard } from './core/guards/auth/is-not-logged-in-guard.service';
import { CompanyCreatorComponent } from './features/company/company-creator/company-creator.component';
import { PageNotFoundComponent } from './core/components/page-not-found/page-not-found.component';
import { CompanyManagerComponent } from './shared/components/company-manager/company-manager.component';
import { MainComponent } from './features/company-manager/main/main.component';
import { EmployeeListComponent } from './features/company-manager/employee-list/employee-list.component';

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
		canActivate: [ IsNotLoggedInGuard ],
		data: { title: 'LOGIN_PAGE_TITLE' }
	},
	{
		path: 'Register',
		component: RegisterComponent,
		canActivate: [ IsNotLoggedInGuard ],
		data: { title: 'REGISTER_PAGE_TITLE' }
	},

	//Company
	{
		path: 'Company/List',
		component: CompanyListComponent,
		canActivate: [ IsLoggedInGuard, IsInRoleGuard ],
		data: { title: 'COMPANY_LIST_TITLE_PAGE', requiredRoles: [UserRole.Admin, UserRole.User] }
	},
	{
		path: 'Company/Create',
		component: CompanyCreatorComponent,
		canActivate: [ IsLoggedInGuard, IsInRoleGuard ],
		data: { title: 'COMPANY_CREATE_TITLE_PAGE', requiredRoles: [UserRole.Admin, UserRole.User] }
	},
	{
		path: 'Manager/:id',
		component: CompanyManagerComponent,
		children: [
			{
				path: 'Dashboard',
				component: MainComponent,
				data: { title: 'MANAGER_MAIN_TITLE_PAGE' }
			},
			{
				path: 'EmployeeList',
				component: EmployeeListComponent,
				data: { title: 'MANAGER_EMPLOYEES_TITLE_PAGE' }
			}
		]
	},

	{
		path: '**',
		component: PageNotFoundComponent,
		data: { title: 'NO_ACCESS_TITLE' }
	},
];
