import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobStationManagerComponent } from './job-station-manager.component';

describe('JobStationManagerComponent', () => {
	let component: JobStationManagerComponent;
	let fixture: ComponentFixture<JobStationManagerComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [JobStationManagerComponent],
		}).compileComponents();

		fixture = TestBed.createComponent(JobStationManagerComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
