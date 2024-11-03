import { TestBed } from '@angular/core/testing';
import { AlertService } from './alert.service';
import { first } from 'rxjs';

describe('AlertService', () => {
	let service: AlertService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(AlertService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});

	it('should add a new alert when showAlert is called', () => {
		const message = 'Test alert';
		const type = 'success';
		service.showAlert(message, type);

		service.alerts$.pipe(first()).subscribe(alerts => {
			expect(alerts.length).toBe(1);
			expect(alerts[0].message).toBe(message);
			expect(alerts[0].type).toBe(type);
		});
	});

	it('should remove alert by id when removeAlert is called', () => {
		const message = 'Test alert';
		const type = 'success';
		service.showAlert(message, type);

		service.alerts$.pipe(first()).subscribe(alerts => {
			const alertId = alerts[0].id;
			service.removeAlert(alertId);

			service.alerts$.pipe(first()).subscribe(updatedAlerts => {
				expect(updatedAlerts.length).toBe(0);
			});
		});
	});

	it('should automatically remove alert after specified duration', done => {
		const message = 'Auto-remove alert';
		const type = 'warning';
		const duration = 1000;

		service.showAlert(message, type, duration);

		// Sprawdź początkowo, czy alert został dodany
		service.alerts$.pipe(first()).subscribe(initialAlerts => {
			expect(initialAlerts.length).toBe(1);
		});

		// Czekamy na usunięcie alertu po `duration`
		setTimeout(() => {
			service.alerts$.pipe(first()).subscribe(finalAlerts => {
				expect(finalAlerts.length).toBe(0);
				done();
			});
		}, duration + 50);
	});
});
