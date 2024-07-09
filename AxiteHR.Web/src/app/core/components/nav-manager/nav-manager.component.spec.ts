import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavManagerComponent } from './nav-manager.component';

describe('NavManagerComponent', () => {
  let component: NavManagerComponent;
  let fixture: ComponentFixture<NavManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavManagerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(NavManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
