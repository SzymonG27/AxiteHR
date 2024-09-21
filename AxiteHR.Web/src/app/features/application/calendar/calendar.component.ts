import { CommonModule } from '@angular/common';
import { Component, OnDestroy } from '@angular/core';
import {
	CalendarModule,
	CalendarEvent,
	CalendarView,
	CalendarMonthViewDay,
} from 'angular-calendar';
import { addDays, subDays, isSameDay, isSameMonth } from 'date-fns';
import { FormsModule } from '@angular/forms';
import { LangChangeEvent, TranslateModule, TranslateService } from '@ngx-translate/core';
import { registerLocaleData } from '@angular/common';
import localePl from '@angular/common/locales/pl';
import localeEn from '@angular/common/locales/en';
import { Subscription } from 'rxjs';
import { ModalComponent } from '../../../shared/components/modal/modal.component';

@Component({
	selector: 'app-calendar',
	standalone: true,
	imports: [CommonModule, CalendarModule, FormsModule, TranslateModule, ModalComponent],
	templateUrl: './calendar.component.html',
	styleUrls: ['./calendar.component.css'],
})
export class CalendarComponent implements OnDestroy {
	isModalApplicationOpen = false;

	view: CalendarView = CalendarView.Month;
	CalendarView = CalendarView;
	viewDate: Date = new Date();

	currentLang = 'en';
	langChangeSubscription: Subscription;

	activeDay: Date | null = null;

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

	constructor(private translate: TranslateService) {
		registerLocaleData(localePl);
		registerLocaleData(localeEn);

		this.currentLang = translate.currentLang;

		this.langChangeSubscription = translate.onLangChange.subscribe((event: LangChangeEvent) => {
			this.currentLang = event.lang;
			this.updateCalendarLocale();
		});
	}

	prevMonth(): void {
		this.viewDate = subDays(this.viewDate, 30);
		this.activeDay = null;
	}

	nextMonth(): void {
		this.viewDate = addDays(this.viewDate, 30);
		this.activeDay = null;
	}

	today(): void {
		this.viewDate = new Date();
	}

	getTranslatedMonth(): string {
		const monthIndex = this.viewDate.getMonth();
		const monthNames = [
			'JANUARY',
			'FEBRUARY',
			'MARCH',
			'APRIL',
			'MAY',
			'JUNE',
			'JULY',
			'AUGUST',
			'SEPTEMBER',
			'OCTOBER',
			'NOVEMBER',
			'DECEMBER',
		];

		return this.translate.instant(`Calendar_Months.${monthNames[monthIndex]}`);
	}

	dayClicked({ day }: { day: CalendarMonthViewDay }): void {
		const date = day.date;

		if (isSameMonth(date, this.viewDate)) {
			if (isSameDay(this.viewDate, date)) {
				this.activeDay = this.activeDay ? null : date;
			} else {
				this.activeDay = date;
			}
			this.viewDate = date;
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

	updateCalendarLocale(): void {
		this.viewDate = new Date();
	}

	closeApplicationModal(): void {
		this.isModalApplicationOpen = false;
	}

	submitApplicationModal(): void {
		//Application action
		this.isModalApplicationOpen = false;
	}

	ngOnDestroy(): void {
		if (this.langChangeSubscription) {
			this.langChangeSubscription.unsubscribe();
		}
	}
}
