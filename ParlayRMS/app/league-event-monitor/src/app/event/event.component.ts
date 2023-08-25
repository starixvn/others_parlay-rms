import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { EventGroupModel, EventModel, MatchBettypeModel } from './event.model';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss']
})
export class EventComponent implements OnInit {
  @Input() data: EventGroupModel[] = [];
  @Input() isClickedOnleague: boolean = false;
  @Output() showBackLeague = new EventEmitter();
  @Output() clickedOnEvent = new EventEmitter<MatchBettypeModel>();
  @Output() clickedExportExcel = new EventEmitter<MatchBettypeModel>();
  constructor() { }

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
}
