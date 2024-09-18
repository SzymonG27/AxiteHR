import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TempPasswordChangeComponent } from './temp-password-change.component';

describe('TempPasswordChangeComponent', () => {
	let component: TempPasswordChangeComponent;
	let fixture: ComponentFixture<TempPasswordChangeComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [TempPasswordChangeComponent],
		}).compileComponents();

		fixture = TestBed.createComponent(TempPasswordChangeComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
