import { Component, Input, OnInit } from '@angular/core';
import { BetDetails } from '../grid.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-bet-details',
  templateUrl: './bet-details.component.html',
  styleUrls: ['./bet-details.component.scss']
})
export class BetDetailsComponent {
  @Input() data!: BetDetails;
  userName: string = '';
  transId: string = '';
  
  constructor(public readonly activeModal: NgbActiveModal) {}
}
