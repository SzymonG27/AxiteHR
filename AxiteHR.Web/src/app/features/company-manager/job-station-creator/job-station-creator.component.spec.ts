import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobStationCreatorComponent } from './job-station-creator.component';

describe('JobStationCreatorComponent', () => {
  let component: JobStationCreatorComponent;
  let fixture: ComponentFixture<JobStationCreatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JobStationCreatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JobStationCreatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
