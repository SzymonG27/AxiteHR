import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { filter } from 'rxjs';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
import { AuthStateService } from '../../../services/authentication/auth-state.service';

@Component({
	selector: 'app-nav-bar',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule
	],
	templateUrl: './nav-bar.component.html',
	styleUrl: './nav-bar.component.css',
	animations: [
		trigger('menuAnimation', [
			state('closed', style({
				height: '0px'
			})),
			state('open', style({
				height: '*'
			})),
			transition('closed => open', [
				animate('0.5s ease-in-out')
			]),
			transition('open => closed', [
				animate('0.5s ease-in-out')
			])
		])
	]
})
export class NavBarComponent {
	isMenuOpen: boolean = false;
	isLoggedIn: boolean = false;
	currentUrl: string = "";
	isLoginPage: boolean = false;
	isRegisterPage: boolean = false;

	constructor(private router: Router, private authService: AuthenticationService, private authState: AuthStateService) { }

	ngOnInit() {
		this.authState.isLoggedIn.subscribe((status: boolean) => {
			this.isLoggedIn = status;
		});
		this.router.events
			.pipe(
				filter(event => event instanceof NavigationEnd)
			)
			.subscribe((event) => {
				const navEndEvent = event as NavigationEnd;
				this.currentUrl = navEndEvent.urlAfterRedirects;
				this.checkUrl();
			});
	}

	toggleMenu() {
		this.isMenuOpen = !this.isMenuOpen;
	}

	checkUrl() {
		this.isLoginPage = this.currentUrl.includes('Login');
		this.isRegisterPage = this.currentUrl.includes('Register');
	}

	logOut() {
		this.authService.LogOut();
	}
}