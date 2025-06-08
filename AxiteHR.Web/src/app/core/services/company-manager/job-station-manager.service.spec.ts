import { TestBed } from '@angular/core/testing';

import { JobStationManagerService } from './job-station-manager.service';

describe('JobStationManagerService', () => {
	let service: JobStationManagerService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(JobStationManagerService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
