import { Component, Input, OnInit } from '@angular/core';
import { DataModel } from './grid.model';
import { AlertTypeConstants } from '../common/constants';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss']
})
export class GridComponent implements OnInit {
  @Input() data: DataModel[] = [];
  @Input() webUrl: string = '';

  constructor() { }

  ngOnInit(): void {
  }

  getColorViaAlertType(type: string): string {
    if (type === AlertTypeConstants.SIMILAR_BET) {
      return '#bdd7ee';
    }
    if (type === AlertTypeConstants.SMALL_ODDS) {
      return '#ffff00';
    }
    if (type === AlertTypeConstants.LAST_TICKET_PENDING) {
      return '#c6e0b4';
    }
    if (type === AlertTypeConstants.SMALL_LEAGUE_ODDS) {
      return '#f9cb9c';
    }
    return '#ffffff';
  }

  openNewTabOfSimilarBet(betId: number) {
    window.open(`${this.webUrl}SimilarBets?betid=${betId}`, '_blank');
  }

  openNewTabOfTotalOutstanding(user: string) {
    window.open(`${this.webUrl}TotalOutstanding?username=${user}`);
  }
}
