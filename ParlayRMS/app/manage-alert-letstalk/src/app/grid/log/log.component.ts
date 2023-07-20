import { Component, Input } from '@angular/core';
import { LogModel } from './log.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-log',
  templateUrl: './log.component.html',
  styleUrls: ['./log.component.scss']
})
export class LogComponent {
  @Input() data: LogModel[] = [];
  user: string = '';
  betId: string = '';
  alertType: string = '';

  constructor(public readonly activeModal: NgbActiveModal) { }

}
