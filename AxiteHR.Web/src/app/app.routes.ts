import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/authentication/login/login.component';
import { RegisterComponent } from './features/authentication/register/register.component';

export const routes: Routes = [
	{
		path: '',
		component: HomeComponent,
		title: 'Home page'
	},
	{
		path: 'Login',
		component: LoginComponent,
		title: 'Login page'
	},
	{
		path: 'Register',
		component: RegisterComponent,
		title: 'Register page'
	}
];
