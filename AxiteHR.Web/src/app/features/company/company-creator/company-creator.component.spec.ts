import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { CompanyCreatorComponent } from './company-creator.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { provideRouter, Router, RouterLink } from '@angular/router';
import { provideLocationMocks } from '@angular/common/testing';
import { BlockUIService } from '../../../core/services/block-ui.service';
import { CompanyService } from '../../../core/services/company/company.service';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { of, throwError } from 'rxjs';
import { CompanyCreatorResponse } from '../../../core/models/company/company-creator/CompanyCreatorResonse';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { MockComponent } from '../../../tests/utils/MockComponent';

describe('CompanyCreatorComponent', () => {
  let component: CompanyCreatorComponent;
  let fixture: ComponentFixture<CompanyCreatorComponent>;
  let companyService: jasmine.SpyObj<CompanyService>;
  let blockUIService: jasmine.SpyObj<BlockUIService>;
  let router: jasmine.SpyObj<Router>;
  let translateServiceSpy: jasmine.SpyObj<TranslateService>;

  beforeEach(async () => {
    const companyServiceSpy = jasmine.createSpyObj('CompanyService', ['createNewCompany']);
    const blockUIServiceSpy = jasmine.createSpyObj('BlockUIService', ['start', 'stop']);
    translateServiceSpy = jasmine.createSpyObj('TranslateService', ['get', 'stream', 'instant']);

    translateServiceSpy.get.and.returnValue(of(''));
    translateServiceSpy.instant.and.returnValue('');
    translateServiceSpy.stream.and.returnValue(of(''));

    Object.defineProperty(translateServiceSpy, 'onLangChange', { value: of() });
    Object.defineProperty(translateServiceSpy, 'onTranslationChange', { value: of() });
    Object.defineProperty(translateServiceSpy, 'onDefaultLangChange', { value: of() });

    await TestBed.configureTestingModule({
      imports: [
        CompanyCreatorComponent,
        FormsModule,
        ReactiveFormsModule,
        TranslateModule.forRoot(),
        RouterLink,
      ],
      providers: [
          provideRouter([
            { path: 'Company/List', component: MockComponent },
          ]),
        provideLocationMocks(),
        { provide: CompanyService, useValue: companyServiceSpy },
        { provide: BlockUIService, useValue: blockUIServiceSpy },
        { provide: TranslateService, useValue: translateServiceSpy },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();

    fixture = TestBed.createComponent(CompanyCreatorComponent);
    component = fixture.componentInstance;
    companyService = TestBed.inject(CompanyService) as jasmine.SpyObj<CompanyService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    blockUIService = TestBed.inject(BlockUIService) as jasmine.SpyObj<BlockUIService>;

    fixture.detectChanges();
  });

  it('Should create component', () => {
    expect(component).toBeTruthy();
  });

    it('Sholud initialize component with field CompanyName', () => {
    expect(component.companyCreatorForm.contains('CompanyName')).toBeTrue();
  });

  it('Field CompanyName should be required', () => {
    const control = component.companyCreatorForm.get('CompanyName');
    control!.setValue('');
    expect(control!.valid).toBeFalse();

    control!.setValue('Test company');
    expect(control!.valid).toBeTrue();
  });

  it('Should not proceed if the form is invalid', () => {
    component.companyCreatorForm.get('CompanyName')!.setValue('');
    component.createNewCompany();
    expect(blockUIService.start).not.toHaveBeenCalled();
    expect(companyService.createNewCompany).not.toHaveBeenCalled();
  });

  it('Should call createNewCompany when the form is valid', () => {
    component.companyCreatorForm.get('CompanyName')!.setValue('Test Company');

    const companyCreatorResponse: CompanyCreatorResponse = {
      isSucceeded: true,
      errorMessage: null
    }

    companyService.createNewCompany.and.returnValue(of(companyCreatorResponse));
    component!.createNewCompany();

    expect(blockUIService.start).toHaveBeenCalled();
    expect(companyService.createNewCompany).toHaveBeenCalledWith('Test Company');
  });

  it('Should navigate to /Company/List on success', fakeAsync(() => {
    const companyCreatorResponse: CompanyCreatorResponse = {
      isSucceeded: true,
      errorMessage: null
    }
    component.companyCreatorForm.get('CompanyName')!.setValue('Test company');
    companyService.createNewCompany.and.returnValue(of(companyCreatorResponse));
    spyOn(router, 'navigate');
    component.createNewCompany();
    tick();
    expect(router.navigate).toHaveBeenCalledWith(['/Company/List']);
    expect(blockUIService.stop).toHaveBeenCalled();
  }));

  it('Should set errorMessage when it receives a BadRequest with errorMessage', fakeAsync(() => {
    component.companyCreatorForm.get('CompanyName')!.setValue('Test company');
    const errorResponse = new HttpErrorResponse({
      status: HttpStatusCode.BadRequest,
      error: { errorMessage: 'Test error message' },
    });
    companyService.createNewCompany.and.returnValue(throwError(() => errorResponse));
    component.createNewCompany();
    tick();
    expect(component.errorMessage).toBe('Test error message');
    expect(blockUIService.stop).toHaveBeenCalled();
  }));

  it('Should set errorMessage when it receives BadRequest with errors', fakeAsync(() => {
    component.companyCreatorForm.get('CompanyName')!.setValue('Test company');
    const errorResponse = new HttpErrorResponse({
      status: HttpStatusCode.BadRequest,
      error: { errors: { CompanyName: ['Name is required', 'Name must be unique'] } },
    });
    companyService.createNewCompany.and.returnValue(throwError(() => errorResponse));
    component.createNewCompany();
    tick();
    expect(component.errorMessage).toBe('Name is required\n*Name must be unique');
    expect(blockUIService.stop).toHaveBeenCalled();
  }));

  it('Should set errorMessage to the translated message when an unexpected error occurs', fakeAsync(() => {
    component.companyCreatorForm.get('CompanyName')!.setValue('Test company');
    const errorResponse = new HttpErrorResponse({
      status: HttpStatusCode.InternalServerError,
      error: {},
    });
    companyService.createNewCompany.and.returnValue(throwError(() => errorResponse));
    translateServiceSpy.get.and.returnValue(of('An unexpected error occurred'));
    component.createNewCompany();
    tick();
    expect(component.errorMessage).toBe('*An unexpected error occurred');
    expect(blockUIService.stop).toHaveBeenCalled();
  }));

  it('Should set companyStateName to true when toggleCompanyNameState is called with true', () => {
    component.toggleCompanyNameState(true);
    expect(component.companyStateName).toBeTrue();
  });

  it('Should set companyStateName to false when toggleCompanyNameState is called with false', () => {
    component.toggleCompanyNameState(false);
    expect(component.companyStateName).toBeFalse();
  });
});