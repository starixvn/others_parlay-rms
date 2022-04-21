import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatchBettypeModel } from '../event/event.model';
import { EventDetailGroupModel } from './event-detail.model';

@Component({
  selector: 'app-event-detail',
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.scss']
})
export class EventDetailComponent implements OnInit {
  @Input() data: EventDetailGroupModel | undefined;
  @Input() isClickedOnEvent: boolean = false;
  @Output() showBackEvent = new EventEmitter();
  @Output() clickedExportExcel = new EventEmitter<MatchBettypeModel>();
  
  constructor() { }

  ngOnInit(): void {
  }

  public backToEvent(): void {
    this.showBackEvent.emit();
  }

  public exportExcel(event: string, matchId: number, betChoice: number, bettypeName: string, bettype: number): void {
    const matchBettype: MatchBettypeModel = {
      matchId: matchId,
      event: event,
      bettype: bettype,
      betChoice: betChoice,
      bettypeName: bettypeName
    };
    this.clickedExportExcel.emit(matchBettype);
  }
}
