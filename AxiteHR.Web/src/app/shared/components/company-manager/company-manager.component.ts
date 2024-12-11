import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy, OnInit } from '@angular/core';
import { NavManagerComponent } from '../../../core/components/nav-manager/nav-manager.component';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { first, Subject, takeUntil } from 'rxjs';
import { AuthStateService } from '../../../core/services/authentication/auth-state.service';
import { CompanyService } from '../../../core/services/company/company.service';
import { NotificationService } from '../../../core/services/signalr/notification.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
	selector: 'app-company-manager',
	imports: [NavManagerComponent, RouterOutlet],
	templateUrl: './company-manager.component.html',
	styleUrl: './company-manager.component.css',
})
export class CompanyManagerComponent implements AfterViewInit, OnDestroy, OnInit {
	@ViewChild('mainContent', { static: true }) mainContent!: ElementRef;
	@ViewChild('navManager', { static: true }) navManager!: NavManagerComponent;

	private destroy$ = new Subject<void>();
	private resizeObserver!: ResizeObserver;
	private companyId: number | null = null;

	constructor(
		private route: ActivatedRoute,
		private authState: AuthStateService,
		private companyService: CompanyService,
		private notificationService: NotificationService
	) {}

	ngOnInit() {
		this.route.paramMap.pipe(takeUntil(this.destroy$)).subscribe(params => {
			this.companyId = Number.parseInt(params.get('id') ?? '0');
		});

		this.companyService
			.getCompanyUserId(this.authState.getLoggedUserId(), this.companyId!)
			.pipe(first())
			.subscribe({
				next: (companyUserIdResponse: number) => {
					if (companyUserIdResponse !== 0) {
						this.notificationService.startConnection(companyUserIdResponse.toString());
					}
				},
				error: async (error: HttpErrorResponse) => {
					console.log(error);
				},
			});
	}

	ngAfterViewInit() {
		this.setSidebarHeight();

		this.resizeObserver = new ResizeObserver(() => {
			this.setSidebarHeight();
		});

		this.resizeObserver.observe(this.mainContent.nativeElement);
	}

	ngOnDestroy() {
		if (this.resizeObserver) {
			this.resizeObserver.disconnect();
		}

		this.destroy$.next();
		this.destroy$.complete();
	}

	setSidebarHeight() {
		if (this.navManager && this.mainContent) {
			const contentHeight = this.mainContent.nativeElement.scrollHeight;
			const totalHeight = contentHeight;
			this.navManager.setHeight(totalHeight);
		}
	}
}
