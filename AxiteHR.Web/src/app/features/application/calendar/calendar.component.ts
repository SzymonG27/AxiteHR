import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CalendarModule, CalendarEvent, CalendarView, CalendarMonthViewDay } from 'angular-calendar';
import {
	addDays,
	subDays,
	isSameDay,
	isSameMonth,
} from 'date-fns';
import { FormsModule } from '@angular/forms';

@Component({
	selector: 'app-calendar',
	standalone: true,
	imports: [
		CommonModule,
		CalendarModule,
		FormsModule, // Dodaj FormsModule dla ngModel
	],
	templateUrl: './calendar.component.html',
	styleUrls: ['./calendar.component.css']
})
export class CalendarComponent {
	// Określenie aktualnego widoku kalendarza
	view: CalendarView = CalendarView.Month;

	CalendarView = CalendarView;

	viewDate: Date = new Date();

	// Przykładowe wydarzenia
	events: CalendarEvent[] = [
		{
			start: subDays(new Date(), 1),
			title: 'Wydarzenie wczorajsze',
			color: { primary: '#ad2121', secondary: '#FAE3E3' },
		},
		{
			start: new Date(),
			title: 'Dzisiaj',
			color: { primary: '#1e90ff', secondary: '#D1E8FF' },
		},
		{
			start: addDays(new Date(), 1),
			title: 'Jutro',
			color: { primary: '#e3bc08', secondary: '#FDF1BA' },
		},
	];

	// Zarządzanie aktywnym dniem
	activeDay: Date | null = null;

	prevMonth(): void {
		this.viewDate = subDays(this.viewDate, 30);
	}

	nextMonth(): void {
		this.viewDate = addDays(this.viewDate, 30);
	}

	today(): void {
		this.viewDate = new Date();
	}

	dayClicked({ day, sourceEvent }: { day: CalendarMonthViewDay; sourceEvent: MouseEvent | KeyboardEvent }): void {
		const date = day.date;
		const events = day.events;

		if (isSameMonth(date, this.viewDate)) {
			if (isSameDay(this.viewDate, date)) {
				this.activeDay = this.activeDay ? null : date;
			} else {
				this.activeDay = date;
			}
			this.viewDate = date;

			// Przykładowa logika: otwieranie alertu z liczbą wydarzeń
			if (events.length > 0) {
				alert(`Liczba wydarzeń w tym dniu: ${events.length}`);
			}
		}
	}

	eventClicked({ event }: { event: CalendarEvent }): void {
		alert(`Kliknięto wydarzenie: ${event.title}`);
	}

	// Dodanie nowego wydarzenia
	newEvent: { title: string; start: string } = { title: '', start: '' };

	addEvent(): void {
		if (this.newEvent.title && this.newEvent.start) {
			this.events = [
				...this.events,
				{
					title: this.newEvent.title,
					start: new Date(this.newEvent.start),
					color: { primary: '#e3bc08', secondary: '#FDF1BA' },
				},
			];
			// Resetowanie formularza
			this.newEvent = { title: '', start: '' };
		}
	}
}