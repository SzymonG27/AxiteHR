import { CommonModule } from '@angular/common';
import { Component, OnInit, OnDestroy, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { Subject, takeUntil } from 'rxjs';
import { AuthStateService } from '../../services/authentication/auth-state.service';
import { UserRole } from '../../models/authentication/UserRole';

@Component({
	selector: 'app-nav-manager',
	standalone: true,
	imports: [CommonModule, TranslateModule, RouterModule],
	templateUrl: './nav-manager.component.html',
	styleUrl: './nav-manager.component.css',
})
export class NavManagerComponent implements OnInit, OnDestroy {
	@ViewChild('sidebar', { static: true }) sidebar!: ElementRef;
	private destroy$ = new Subject<void>();

	isMenuOpen = false;
	isTeamsExpanded = false;
	isProjectsExpanded = false;
	isApplicationsExpanded = false;

	companyId: string | null = null;

	UserRole = UserRole;
	userRoles: string[] = [];

	constructor(
		private route: ActivatedRoute,
		private authState: AuthStateService
	) {}

	ngOnInit(): void {
		this.route.paramMap.pipe(takeUntil(this.destroy$)).subscribe(params => {
			this.companyId = params.get('id');
		});

		this.userRoles = this.authState.getUserRoles();
	}

	ngOnDestroy(): void {
		this.destroy$.next();
		this.destroy$.complete();
	}

	toggleMenu() {
		this.isMenuOpen = !this.isMenuOpen;
	}

	toggleDropdown(itemName: string) {
		switch (itemName) {
			case 'Teams':
				this.isTeamsExpanded = !this.isTeamsExpanded;
				break;
			case 'Projects':
				this.isProjectsExpanded = !this.isProjectsExpanded;
				break;
			case 'Applications':
				this.isApplicationsExpanded = !this.isApplicationsExpanded;
				break;
		}
	}

	setHeight(height: number) {
		if (this.sidebar) {
			this.sidebar.nativeElement.style.height = `${height}px`;
		}
	}
}
