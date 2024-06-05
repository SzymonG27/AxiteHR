import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavBarComponent } from './core/components/nav-bar/nav-bar.component';
import { BlockUIModule } from 'ng-block-ui';

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
	title = 'AxiteHR.Web';
}
