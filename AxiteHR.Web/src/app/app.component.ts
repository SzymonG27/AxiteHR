import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavBarComponent } from './core/components/nav-bar/nav-bar.component';
import { BlockUIModule } from 'ng-block-ui';
import { TranslateService } from '@ngx-translate/core';
import { TitleService } from './core/services/title-translate.service';
import { CommonModule } from '@angular/common';
import { Alert } from './core/models/alert/Alert';
import { AlertService } from './core/services/alert/alert.service';
import { AlertComponent } from './shared/components/alert/alert.component';
import { Subject, takeUntil } from 'rxjs';

@Component({
	selector: 'app-root',
	standalone: true,
	imports: [
		//Components
		CommonModule,
		NavBarComponent,
		BlockUIModule,
		//Others
		RouterOutlet,
		AlertComponent,
	],
	templateUrl: './app.component.html',
	styleUrl: './app.component.css',
})
export class AppComponent {
	alerts: Alert[] = [];
	private destroy$ = new Subject<void>();

	constructor(
		private translate: TranslateService,
		private titleService: TitleService,
		private alertService: AlertService
	) {
		//Translate configure
		this.translate.addLangs(['en', 'pl']);
		const savedLanguage = localStorage.getItem('language') || 'en';
		this.translate.setDefaultLang(savedLanguage);
		this.translate.use(savedLanguage);

		this.titleService.init();

		this.alertService.alerts$
			.pipe(takeUntil(this.destroy$))
			.subscribe(alerts => (this.alerts = alerts));
	}
}
