import { Component, EventEmitter, Input, Output } from '@angular/core';
import { LeagueDataModel } from './league.model';

@Component({
  selector: 'app-league',
  templateUrl: './league.component.html',
  styleUrls: ['./league.component.scss']
})
export class LeagueComponent {
  @Input() data: LeagueDataModel[] = [];
  @Output() clickedOnLeague = new EventEmitter<number>();

  constructor() { }

  public showEvent(leagueId: number): void {
    this.clickedOnLeague.emit(leagueId);
  }
}
