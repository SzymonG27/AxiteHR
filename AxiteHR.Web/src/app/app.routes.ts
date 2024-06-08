import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/authentication/login/login.component';
import { RegisterComponent } from './features/authentication/register/register.component';

export const routes: Routes = [
	{
		path: '',
		component: HomeComponent,
		data: { title: 'HOME_PAGE_TITLE' }
	},
	{
		path: 'Login',
		component: LoginComponent,
		data: { title: 'LOGIN_PAGE_TITLE' }
	},
	{
		path: 'Register',
		component: RegisterComponent,
		data: { title: 'REGISTER_PAGE_TITLE' }
	}
];
