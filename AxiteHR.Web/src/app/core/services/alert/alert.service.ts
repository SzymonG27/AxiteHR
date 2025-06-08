import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Alert } from '../../models/alert/Alert';

@Injectable({
	providedIn: 'root',
})
export class AlertService {
	private alertsSubject = new BehaviorSubject<Alert[]>([]);
	private alertId = 0;

	alerts$ = this.alertsSubject.asObservable();

	showAlert(
		message: string,
		type: 'success' | 'error' | 'warning' = 'success',
		duration = 5000
	): number {
		const id = this.alertId++;
		const alert: Alert = { id, message, type, duration };
		const currentAlerts = this.alertsSubject.value;

		this.alertsSubject.next([...currentAlerts, alert]);

		setTimeout(() => this.removeAlert(id), duration);

		return id;
	}

	removeAlert(id: number) {
		const updatedAlerts = this.alertsSubject.value.filter(alert => alert.id !== id);
		this.alertsSubject.next(updatedAlerts);
	}
}
