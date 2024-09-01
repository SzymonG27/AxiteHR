import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CalendarModule } from 'angular-calendar';

@Component({
	selector: 'app-calendar',
	standalone: true,
	imports: [
		CommonModule,
		CalendarModule
	],
	templateUrl: './calendar.component.html',
	styleUrl: './calendar.component.css'
})
export class CalendarComponent {
	view: string = 'month';
	viewDate: Date = new Date();

	events: any[] = [
		{
			start: new Date(),
			title: 'Example Event',
		},
	];
}
