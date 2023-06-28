import { Injectable } from '@angular/core';
import { HttpService } from 'nap-components/http';
import { Observable, of } from 'rxjs';
import { SimilarTicketModel } from './similar-ticket/similar-ticket.model';
import { PotentialWinningModel } from './potential-winning/potential-winning.model';
import { LogModel } from './log/log.model';
import { ItemModel } from '../filter/filter.model';
import { BetDetails } from './grid.model';

@Injectable({
  providedIn: 'root'
})
export class GridService {
  private api = 'api/manage-alert/';

  constructor(private httpService: HttpService) { }

  getSimilarTickets(apiUrl: string, id: string): Observable<SimilarTicketModel[]> {
    return this.httpService.post(`${apiUrl}${this.api}similar-detail`, { alertId : id });
  }

  getPotentialWinning(apiUrl: string, id: string): Observable<PotentialWinningModel[]> {
    return this.httpService.post(`${apiUrl}${this.api}potential-winning-detail`, { alertId : id });
  }

  getLogs(apiUrl: string, id: string): Observable<LogModel[]> {
    return this.httpService.post(`${apiUrl}${this.api}log`, { alertId : id });
  }

  getActions(): ItemModel[] {
    return [
      { id: '-1', name: 'Choose Action', checked: true },
      { id: '1', name: 'Send S&V', checked: false },
      { id: '2', name: 'Send OS', checked: false },
      { id: '3', name: 'Take Note', checked: false }
    ]
  }

  getBetDetail(): Observable<BetDetails> {
    const data = {
      ticketDetail: '',
      comboTypes: ['Double (1 Bet) - 50,000']
    }
    return of(data);
  }
}
