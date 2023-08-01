import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { BetDetails } from '../grid.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'app-bet-details',
  templateUrl: './bet-details.component.html',
  styleUrls: ['./bet-details.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BetDetailsComponent {
  @Input() body!: SafeHtml;
  
  constructor(public readonly activeModal: NgbActiveModal) {}
}
