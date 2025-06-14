import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AlertComponent } from './alert.component';
import { Alert } from '../../../core/models/alert/Alert';

describe('AlertComponent', () => {
	let component: AlertComponent;
	let fixture: ComponentFixture<AlertComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [AlertComponent],
		}).compileComponents();

		fixture = TestBed.createComponent(AlertComponent);
		component = fixture.componentInstance;
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});

	it('should apply correct classes based on alert type', () => {
		const alertTypes: {
			type: Alert['type'];
			expectedClass: keyof typeof component.alertClasses;
		}[] = [
			{ type: 'success', expectedClass: 'bg-green-100 text-green-700 border-green-300' },
			{ type: 'error', expectedClass: 'bg-red-100 text-red-700 border-red-300' },
			{ type: 'warning', expectedClass: 'bg-yellow-100 text-yellow-700 border-yellow-300' },
		];

		for (const { type, expectedClass } of alertTypes) {
			component.alert = { id: 1, message: 'Test message', type, duration: 5000 };
			fixture.detectChanges();

			const alertClasses = component.alertClasses;
			expect(alertClasses[expectedClass as keyof typeof alertClasses]).toBeTrue();
		}
	});

	it('should add fadingOut class shortly before removal', fakeAsync(() => {
		const duration = 5000;
		component.alert = { id: 1, message: 'Test message', type: 'success', duration };
		fixture.detectChanges();

		expect(component.fadingOut).toBeFalse();

		tick(duration - 1000);
		expect(component.fadingOut).toBeTrue();
	}));

	it('should set combinedClasses correctly based on fadingIn and fadingOut states', fakeAsync(() => {
		component.alert = { id: 1, message: 'Test message', type: 'success', duration: 5000 };
		fixture.detectChanges();

		// Na początku fadingIn i fadingOut są false
		let combinedClasses = component.combinedClasses;
		expect(combinedClasses['opacity-0']).toBeTrue();
		expect(combinedClasses['opacity-100']).toBeFalse();

		// Po włączeniu fadingIn
		tick(10);
		combinedClasses = component.combinedClasses;
		expect(combinedClasses['opacity-0']).toBeFalse();
		expect(combinedClasses['opacity-100']).toBeTrue();

		// Po włączeniu fadingOut
		tick(5000 - 1000); // Przeskocz do momentu, w którym fadingOut powinno się włączyć
		combinedClasses = component.combinedClasses;
		expect(combinedClasses['opacity-0']).toBeTrue();
		expect(combinedClasses['opacity-100']).toBeFalse();
	}));
});
