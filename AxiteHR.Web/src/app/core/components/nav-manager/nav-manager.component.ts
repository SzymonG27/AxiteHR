import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
	selector: 'app-nav-manager',
	standalone: true,
	imports: [
		CommonModule,
		TranslateModule,
		RouterModule
	],
	templateUrl: './nav-manager.component.html',
	styleUrl: './nav-manager.component.css'
})
export class NavManagerComponent {
	isMenuOpen: boolean = false;
  	isTeamsExpanded: boolean = false;
  	isProjectsExpanded: boolean = false;
	companyId: string | null = null;

	constructor(private route: ActivatedRoute) { }

	ngOnInit(): void {
		this.route.paramMap.subscribe(params => {
			this.companyId = params.get('id');
		});
	}

	toggleMenu() {
		this.isMenuOpen = !this.isMenuOpen;
	}

	toggleDropdown(itemName: string) {
		if (itemName === "Teams") {
			this.isTeamsExpanded = !this.isTeamsExpanded;
		} else if (itemName === 'Projects') {
			this.isProjectsExpanded = !this.isProjectsExpanded;
		}
	}
}
