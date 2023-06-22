import { Injectable } from '@angular/core';
import { HttpService } from 'nap-components/http';
import { ItemModel } from './filter.model';

@Injectable({
  providedIn: 'root'
})
export class FilterService {
  private api = 'api/win-loss-risk-control/filter/';
  constructor(private httpService: HttpService) { }

  public getAlertTypes(): ItemModel[] {
    return [
      { id: '0', name: 'Similar Bet', checked: true },
      { id: '1', name: 'Small Odds', checked: true },
      { id: '2', name: 'Potential Winning', checked: true },
      { id: '3', name: 'Last Pending Tickets', checked: true },
      { id: '4', name: 'Small League & Odds', checked: true }
    ]
  }

  public getActions(): ItemModel[] {
    return [
      { id: '0', name: 'Send S&V', checked: true },
      { id: '1', name: 'Send OS', checked: true },
      { id: '2', name: 'Take Note', checked: true }
    ]
  }

  public getTicketStatuses(): ItemModel[] {
    return [
      { id: '0', name: 'Won', checked: true },
      { id: '1', name: 'Lose', checked: true },
      { id: '2', name: 'Running', checked: true }
    ]
  }

  public getRefreshs(): ItemModel[]
  {
    return [
      { id: '60', name: '60', checked: true },
      { id: '120', name: '120', checked: false }
    ]
  }
}
