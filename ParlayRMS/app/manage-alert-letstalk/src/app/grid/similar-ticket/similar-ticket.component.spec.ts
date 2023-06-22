import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SimilarTicketComponent } from './similar-ticket.component';

describe('SimilarTicketComponent', () => {
  let component: SimilarTicketComponent;
  let fixture: ComponentFixture<SimilarTicketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SimilarTicketComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SimilarTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
