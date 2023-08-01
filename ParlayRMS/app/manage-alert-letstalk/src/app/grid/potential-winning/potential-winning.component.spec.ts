import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PotentialWinningComponent } from './potential-winning.component';

describe('PotentialWinningComponent', () => {
  let component: PotentialWinningComponent;
  let fixture: ComponentFixture<PotentialWinningComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PotentialWinningComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PotentialWinningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
