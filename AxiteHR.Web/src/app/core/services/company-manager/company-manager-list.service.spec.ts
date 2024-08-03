import { TestBed } from '@angular/core/testing';

import { CompanyManagerListService } from './company-manager-list.service';

describe('CompanyManagerListService', () => {
	let service: CompanyManagerListService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(CompanyManagerListService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
