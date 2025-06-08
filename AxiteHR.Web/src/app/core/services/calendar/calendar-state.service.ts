import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class CalendarStateService {
	private selectedDateSource = new BehaviorSubject<Date | null>(null);
	selectedDate = this.selectedDateSource.asObservable();
	setSelectedDate(value: Date) {
		this.selectedDateSource.next(value);
	}
}
