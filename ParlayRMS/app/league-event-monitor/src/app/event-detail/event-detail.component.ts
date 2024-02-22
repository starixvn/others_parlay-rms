import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatchBettypeModel } from '../event/event.model';
import { EventDetailGroupModel } from './event-detail.model';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-event-detail',
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.scss'],
  providers: [DecimalPipe]
})
export class EventDetailComponent implements OnInit {
  @Input() data: EventDetailGroupModel | undefined;
  @Input() isClickedOnEvent: boolean = false;
  @Output() showBackEvent = new EventEmitter();
  @Output() clickedExportExcel = new EventEmitter<MatchBettypeModel>();
  
  constructor(private readonly decimalPipe: DecimalPipe) { }

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

  public formatDecimal(value: number) {
    const digitsInfo = value < 10 ? '1.2-2' : '1.0-0';
    return this.decimalPipe.transform(value, digitsInfo);
  }
}
