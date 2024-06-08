import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavBarComponent } from './core/components/nav-bar/nav-bar.component';
import { BlockUIModule } from 'ng-block-ui';
import { TranslateService } from '@ngx-translate/core';
import { TitleService } from './core/services/title-translate.service';

@Component({
	selector: 'app-root',
	standalone: true,
	imports: [
		//Components
		NavBarComponent,
		BlockUIModule,
		//Others
		RouterOutlet
	],
	templateUrl: './app.component.html',
	styleUrl: './app.component.css'
})
export class AppComponent {
	constructor(private translate: TranslateService, private titleService: TitleService) {

		//Translate configure
		this.translate.addLangs(['en', 'pl']);
		const savedLanguage = localStorage.getItem('language') || 'en';
    	this.translate.setDefaultLang(savedLanguage);
    	this.translate.use(savedLanguage);

		this.titleService.init();
	}
}
