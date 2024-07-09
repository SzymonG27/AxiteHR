import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyCreatorComponent } from './company-creator.component';

describe('CompanyCreatorComponent', () => {
  let component: CompanyCreatorComponent;
  let fixture: ComponentFixture<CompanyCreatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompanyCreatorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompanyCreatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
