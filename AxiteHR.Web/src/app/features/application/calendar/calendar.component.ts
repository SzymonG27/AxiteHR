import { CommonModule } from '@angular/common';
import {
	Component,
	OnDestroy,
	AfterViewChecked,
	Renderer2,
	ViewChild,
	ElementRef,
} from '@angular/core';
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
import { Subscription, take } from 'rxjs';
import { ModalComponent } from '../../../shared/components/modal/modal.component';
import { ActivatedRoute, Router } from '@angular/router';
import { DataBehaviourService } from '../../../core/services/data/data-behaviour.service';

@Component({
	selector: 'app-calendar',
	standalone: true,
	imports: [CommonModule, CalendarModule, FormsModule, TranslateModule, ModalComponent],
	templateUrl: './calendar.component.html',
	styleUrls: ['./calendar.component.css'],
})
export class CalendarComponent implements OnDestroy, AfterViewChecked {
	@ViewChild('calendar', { static: false }) calendar!: ElementRef;

	isModalEventOpen = false;

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

	constructor(
		private translate: TranslateService,
		private renderer: Renderer2,
		private el: ElementRef,
		private router: Router,
		private dataService: DataBehaviourService,
		private route: ActivatedRoute
	) {
		registerLocaleData(localePl);
		registerLocaleData(localeEn);

		this.currentLang = translate.currentLang;

		this.langChangeSubscription = translate.onLangChange.subscribe((event: LangChangeEvent) => {
			this.currentLang = event.lang;
			this.setNewApplicationButtonText();
		});
		this.updateCalendarLocale();
		this.setNewApplicationButtonText();
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

	dayClicked({
		day,
		sourceEvent,
	}: {
		day: CalendarMonthViewDay;
		sourceEvent: MouseEvent | KeyboardEvent;
	}): void {
		const date = day.date;

		if (isSameMonth(date, this.viewDate)) {
			if (isSameDay(this.viewDate, date)) {
				this.activeDay = this.activeDay ? null : date;
			} else {
				this.activeDay = date;
			}
			this.viewDate = date;
		}

		if (this.activeDay && this.calendar) {
			const activeElements = this.calendar.nativeElement.querySelectorAll('.cal-open');
			activeElements.forEach((element: HTMLElement) => {
				this.renderer.removeClass(element, 'cal-open');
			});
		}

		const targetElement = sourceEvent.target as HTMLElement;
		const closestCell = targetElement.closest('.cal-cell');
		if (closestCell && !closestCell.classList.contains('cal-out-month')) {
			this.renderer.addClass(targetElement.closest('.cal-cell'), 'cal-open');
			this.renderer.addClass(targetElement.closest('.cal-cell'), 'cal-has-events');
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

	//#region Modal
	getTranslatedModalTitle() {
		return this.translate.instant('Calendar_EventModal_Title');
	}

	closeEventModal(): void {
		this.isModalEventOpen = false;
	}

	submitApplicationModal(): void {
		if (!this.activeDay) {
			this.isModalEventOpen = false;
			return;
		}

		this.isModalEventOpen = false;

		this.dataService.setSelectedDate(this.activeDay);

		const currentFullPath = this.router.url.split('/');
		currentFullPath[currentFullPath.length - 1] = 'NewApplication';
		this.router.navigate(currentFullPath);
	}
	//#endregion Modal

	setNewApplicationButtonText(): void {
		this.translate
			.get('Nav_Manager_Applications_NewApplication')
			.pipe(take(1))
			.subscribe((translatedText: string) => {
				const spanElement = this.el.nativeElement.querySelector('.button-new-application');
				if (spanElement) {
					this.renderer.setProperty(spanElement, 'textContent', '');
					const buttonText = this.renderer.createText(translatedText);
					this.renderer.appendChild(spanElement, buttonText);
				}
			});
	}

	ngAfterViewChecked(): void {
		const openDayEventsElements = document.querySelectorAll('.cal-open-day-events');

		openDayEventsElements.forEach(element => {
			if (!element.querySelector('.add-event-button')) {
				const button = this.renderer.createElement('button');
				const addIcon = this.renderer.createElement('span');
				const buttonTextSpan = this.renderer.createElement('span');

				const buttonText = this.renderer.createText('Dodaj wydarzenie');
				const iconText = this.renderer.createText('add');

				this.renderer.addClass(button, 'add-event-button');
				this.renderer.addClass(button, 'add-event-button-container');
				this.renderer.addClass(addIcon, 'material-icons');
				this.renderer.addClass(buttonTextSpan, 'textbutton-line-hover');
				this.renderer.addClass(buttonTextSpan, 'button-new-application');

				this.renderer.appendChild(addIcon, iconText);
				this.renderer.appendChild(button, addIcon);

				this.renderer.appendChild(buttonTextSpan, buttonText);
				this.renderer.appendChild(button, buttonTextSpan);

				this.renderer.listen(button, 'click', () => {
					this.isModalEventOpen = true;
					this.addEvent();
				});

				const firstChild = element.firstChild;
				this.renderer.insertBefore(element, button, firstChild);
			}
		});
	}

	ngOnDestroy(): void {
		if (this.langChangeSubscription) {
			this.langChangeSubscription.unsubscribe();
		}
	}
}
