import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { NavManagerComponent } from '../../../core/components/nav-manager/nav-manager.component';
import { RouterOutlet } from '@angular/router';

@Component({
	selector: 'app-company-manager',
	imports: [NavManagerComponent, RouterOutlet],
	templateUrl: './company-manager.component.html',
	styleUrl: './company-manager.component.css',
})
export class CompanyManagerComponent implements AfterViewInit, OnDestroy {
	@ViewChild('mainContent', { static: true }) mainContent!: ElementRef;
	@ViewChild('navManager', { static: true }) navManager!: NavManagerComponent;

	private resizeObserver!: ResizeObserver;

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
	}

	setSidebarHeight() {
		if (this.navManager && this.mainContent) {
			const contentHeight = this.mainContent.nativeElement.scrollHeight;
			const totalHeight = contentHeight;
			this.navManager.setHeight(totalHeight);
		}
	}
}
