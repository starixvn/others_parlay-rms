import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { EventGroupModel, EventModel, MatchBettypeModel } from './event.model';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss'],
  providers: [DecimalPipe]
})
export class EventComponent implements OnInit {
  @Input() data: EventGroupModel[] = [];
  @Input() isClickedOnleague: boolean = false;
  @Output() showBackLeague = new EventEmitter();
  @Output() clickedOnEvent = new EventEmitter<MatchBettypeModel>();
  @Output() clickedExportExcel = new EventEmitter<MatchBettypeModel>();
  constructor(private readonly decimalPipe: DecimalPipe) { }

  ngOnInit(): void {
  }

  public backToLeague(): void {
    this.showBackLeague.emit();
  }

  public isNormalRow(model: EventModel): boolean {
    return model.homeName !== null && model.homeName !== 'Others' && model.homeName !== 'Total';
  }

  public showEventDetail(event: string, matchId: number, betChoice: number): void {
    const matchBettype: MatchBettypeModel = {
      matchId: matchId,
      event: event,
      bettype: 0,
      betChoice: betChoice,
      bettypeName: ''
    };
    this.clickedOnEvent.emit(matchBettype);
  }

  public exportExcel(model: EventModel, matchId: number, betChoice: number, bettypeName: string): void {
    const matchBettype: MatchBettypeModel = {
      matchId: matchId,
      event: model.isOther ? model.event : `${model.homeName} - vs - ${model.awayName}`,
      bettype: betChoice === 0 ? model.awayBettype : model.bettype,
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
