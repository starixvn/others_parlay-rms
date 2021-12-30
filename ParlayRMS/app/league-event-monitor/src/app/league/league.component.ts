import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LeagueDataModel } from './league.model';

@Component({
  selector: 'app-league',
  templateUrl: './league.component.html',
  styleUrls: ['./league.component.scss']
})
export class LeagueComponent implements OnInit {
  @Input() data: LeagueDataModel[] = [];
  @Output() clickedOnLeague = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  public showEvent(): void {
    this.clickedOnLeague.emit();
  }
}
