import { Injectable } from '@angular/core';
import { ItemModel } from './filter.model';

@Injectable({
  providedIn: 'root'
})
export class FilterService {
  constructor() { }

  public getAlertTypes(): ItemModel[] {
    return [
      { id: '1', name: 'Similar Bet', checked: true },
      { id: '2', name: 'Small Odds', checked: true },
      { id: '3', name: 'Potential Winning', checked: true },
      { id: '4', name: 'Last Pending Tickets', checked: true },
      { id: '5', name: 'Small League & Odds', checked: true }
    ]
  }

  public getActions(): ItemModel[] {
    return [
      { id: '0', name: 'No Action', checked: true },
      { id: '1', name: 'Send S&V', checked: true },
      { id: '2', name: 'Send OS', checked: true },
      { id: '3', name: 'Take Note', checked: true }
    ]
  }

  public getTicketStatuses(): ItemModel[] {
    return [
      { id: '112', name: 'Won', checked: true },
      { id: '113', name: 'Lose', checked: true },
      { id: '0', name: 'Running', checked: true }
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
