import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { filter } from 'rxjs';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { AuthStateService } from '../../services/authentication/auth-state.service';
import { AuthDictionary } from '../../../shared/dictionary/AuthDictionary';
import { UserRole } from '../../models/authentication/UserRole';

@Component({
	selector: 'app-nav-bar',
	standalone: true,
	imports: [
		CommonModule,
		RouterModule,
		TranslateModule
	],
	templateUrl: './nav-bar.component.html',
	styleUrl: './nav-bar.component.css',
	animations: [
		trigger('menuAnimation', [
			state('closed', style({
			  maxHeight: '0px',
			  opacity: 0
			})),
			state('open', style({
			  maxHeight: '500px',
			  opacity: 1
			})),
			transition('closed <=> open', animate('300ms ease-in-out'))
		])
	]
})
export class NavBarComponent {
	isMenuOpen: boolean = false;
	isLoggedIn: boolean = false;
	currentUrl: string = "";
	isLoginPage: boolean = false;
	isRegisterPage: boolean = false;
	UserRole = UserRole;
	userRoles: string[] = [];

	languages = [
		{ code: 'en', label: 'English', flag: 'assets/flags/us.webp' },
		{ code: 'pl', label: 'Polski', flag: 'assets/flags/pl.webp' }
	];
	currentLanguage: string = 'en';
	isLanguageMenuOpen: boolean = false;
	isLanguageFlagPressed: boolean = false;

	constructor(
		private router: Router,
		private authService: AuthenticationService,
		private authState: AuthStateService,
		private translate: TranslateService)
	{
		this.currentLanguage = this.translate.currentLang || 'en';
	}

	ngOnInit() {
		this.authState.isLoggedIn.subscribe((status: boolean) => {
			this.isLoggedIn = status;
			this.mapUserRoles(this.isLoggedIn);
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

	private mapUserRoles(isLoggedIn: boolean) {
		if (isLoggedIn) {
			this.userRoles = this.authState.getUserRoles(localStorage.getItem(AuthDictionary.Token));
			return;
		}

		//Delete user roles when user is not logged in
		this.userRoles = [];
	}

	isInRole(role: string) : boolean {
		return this.userRoles.includes(role);
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

	switchLanguage(language: string) {
		this.translate.use(language);
		localStorage.setItem('language', language);
		this.currentLanguage = language;
		this.isLanguageMenuOpen = false;
	}
	
	toggleLanguageMenu(event: MouseEvent) {
		event.stopPropagation();
		this.isLanguageMenuOpen = !this.isLanguageMenuOpen;
		if (!this.isLanguageMenuOpen) {
			this.isLanguageFlagPressed = false;
		}
	}

	@HostListener('document:click', ['$event'])
	closeLanguageMenuOnClickingOutside(event: MouseEvent) {
		if (!this.isLanguageMenuOpen) {
			return;
		}
		this.isLanguageMenuOpen = false;
	}
	
	getCurrentLanguageFlag() {
		const currentLang = this.languages.find(lang => lang.code === this.currentLanguage);
		return currentLang ? currentLang.flag : this.languages[0].flag;
	}
}