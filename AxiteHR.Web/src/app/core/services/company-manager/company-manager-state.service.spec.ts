import { TestBed } from '@angular/core/testing';

import { CompanyManagerStateService } from './company-manager-state.service';

describe('CompanyManagerStateService', () => {
	let service: CompanyManagerStateService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(CompanyManagerStateService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
