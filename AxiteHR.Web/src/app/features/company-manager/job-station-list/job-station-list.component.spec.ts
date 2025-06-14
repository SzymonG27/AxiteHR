import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobStationListComponent } from './job-station-list.component';

describe('JobStationListComponent', () => {
	let component: JobStationListComponent;
	let fixture: ComponentFixture<JobStationListComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [JobStationListComponent],
		}).compileComponents();

		fixture = TestBed.createComponent(JobStationListComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
