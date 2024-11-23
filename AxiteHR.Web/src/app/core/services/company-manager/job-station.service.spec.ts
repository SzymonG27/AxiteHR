import { TestBed } from '@angular/core/testing';

import { JobStationService } from './job-station.service';

describe('JobStationService', () => {
  let service: JobStationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JobStationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
