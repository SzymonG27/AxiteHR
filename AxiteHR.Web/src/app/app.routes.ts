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
import { InternalErrorComponent } from './core/components/internal-error/internal-error.component';
import { EmployeeCreatorComponent } from './features/company-manager/employee-creator/employee-creator.component';
import { TempPasswordChangeComponent } from './features/authentication/temp-password-change/temp-password-change.component';
import { TempPasswordLeaveGuard } from './core/guards/auth/temp-password-leave-guard.service';
import { TempPasswordEnterGuard } from './core/guards/auth/temp-password-enter-guard.service';
import { IsInCompanyGuard } from './core/guards/company/is-in-company-guard.service';
import { CalendarComponent } from './features/application/calendar/calendar.component';
import { NewApplicationComponent } from './features/application/new-application/new-application.component';
import { JobStationListComponent } from './features/company-manager/job-station-list/job-station-list.component';
import { JobStationCreatorComponent } from './features/company-manager/job-station-creator/job-station-creator.component';
import { JobStationManagerComponent } from './features/company-manager/job-station-manager/job-station-manager.component';

export const routes: Routes = [
	//Home
	{
		path: '',
		component: HomeComponent,
		data: { title: 'HOME_PAGE_TITLE' },
	},

	//Shared
	{
		path: 'No-Access',
		component: NoAccessComponent,
		data: { title: 'NO_ACCESS_TITLE' },
	},
	{
		path: 'Internal-Error',
		component: InternalErrorComponent,
		data: { title: 'INTERNAL_ERROR_TITLE' },
	},

	//Auth
	{
		path: 'Login',
		component: LoginComponent,
		canActivate: [IsNotLoggedInGuard],
		data: { title: 'LOGIN_PAGE_TITLE' },
	},
	{
		path: 'Register',
		component: RegisterComponent,
		canActivate: [IsNotLoggedInGuard],
		data: { title: 'REGISTER_PAGE_TITLE' },
	},
	{
		path: 'ChangeTempPassword',
		component: TempPasswordChangeComponent,
		canActivate: [IsNotLoggedInGuard, TempPasswordEnterGuard],
		canDeactivate: [TempPasswordLeaveGuard],
		data: { title: 'CHANGE_TEMP_PASSWORD_PAGE_TITLE' },
	},

	//Company
	{
		path: 'Company/List',
		component: CompanyListComponent,
		canActivate: [IsLoggedInGuard, IsInRoleGuard],
		data: { title: 'COMPANY_LIST_TITLE_PAGE', requiredRoles: [UserRole.Admin, UserRole.User] },
	},
	{
		path: 'Company/Create',
		component: CompanyCreatorComponent,
		canActivate: [IsLoggedInGuard, IsInRoleGuard],
		data: {
			title: 'COMPANY_CREATE_TITLE_PAGE',
			requiredRoles: [UserRole.Admin, UserRole.User],
		},
	},
	{
		path: 'CompanyMenu/:id',
		component: CompanyManagerComponent,
		canActivate: [IsLoggedInGuard, IsInRoleGuard, IsInCompanyGuard],
		data: { requiredRoles: [UserRole.Admin, UserRole.User, UserRole.UserFromCompany] },
		children: [
			//Main
			{
				path: 'Dashboard',
				component: MainComponent,
				data: { title: 'MANAGER_MAIN_TITLE_PAGE' },
			},
			//Company
			{
				path: 'EmployeeList',
				component: EmployeeListComponent,
				data: { title: 'MANAGER_EMPLOYEES_TITLE_PAGE' },
			},
			{
				path: 'EmployeeCreator',
				component: EmployeeCreatorComponent,
				data: { title: 'EMPLOYEE_CREATE_TITLE_PAGE' },
			},
			{
				path: 'JobStationList',
				component: JobStationListComponent,
				data: { title: 'WORKSTATION_LIST_TITLE_PAGE' },
			},
			{
				path: 'JobStationCreator',
				component: JobStationCreatorComponent,
				data: { title: 'NEW_JOBSTATION_TITLE_PAGE' },
			},
			{
				path: 'JobStationManager',
				component: JobStationManagerComponent,
				data: { title: 'JOBSTATION_MANAGER_TITLE_PAGE' },
			},
			//Application
			{
				path: 'Calendar',
				component: CalendarComponent,
				data: { title: 'CALENDAR_TITLE_PAGE' },
			},
			{
				path: 'NewApplication',
				component: NewApplicationComponent,
				data: { title: 'NEW_APPLICATION_TITLE_PAGE' },
			},
		],
	},

	{
		path: '**',
		component: PageNotFoundComponent,
		data: { title: 'NO_ACCESS_TITLE' },
	},
];
