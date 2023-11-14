import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JoinElectionComponent } from './join-election.component';

describe('JoinElectionComponent', () => {
  let component: JoinElectionComponent;
  let fixture: ComponentFixture<JoinElectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JoinElectionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(JoinElectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
