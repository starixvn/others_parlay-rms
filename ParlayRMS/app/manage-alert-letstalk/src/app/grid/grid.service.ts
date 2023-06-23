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

  constructor(private httpService: HttpService) { }

  testSimilarTicketList: SimilarTicketModel[] = [
    {
      no: 1,
      betId: '124723475158925312',
      site: 'Site',
      userName: 'sa',
      customerClass: '1',
      stake: 4.7,
      winningAmount: 4.7,
      houseAmount: 7,
      status: 'Running'
    },
    {
      no: 2,
      betId: '124723475158925312',
      site: 'Site',
      userName: 'sa',
      customerClass: '1',
      stake: 4.7,
      winningAmount: 4.7,
      houseAmount: 7,
      status: 'Running'
    },
    {
      no: 3,
      betId: '124723475158925312',
      site: 'Site',
      userName: 'sa',
      customerClass: '1',
      stake: 4.7,
      winningAmount: 4.7,
      houseAmount: 7,
      status: 'Running'
    },
    {
      no: 4,
      betId: '124723475158925312',
      site: 'Site',
      userName: 'sa',
      customerClass: '1',
      stake: 4.7,
      winningAmount: 4.7,
      houseAmount: 7,
      status: 'Running'
    }
  ];

  testPotentialWinningList: PotentialWinningModel[] = [
    {
      no: 1,
      betId: '124723475158925312',
      transTime: '04/22/2023 14:49:33PM',
      odds: 2.25,
      stake: 4.7,
      winningAmount: 4,
      houseAmount: 7,
      ip: '127.0.0.1',
      housePt: 0.15,
      totalComboBets: '1 bet',
      totalSelections: 2,
      sportType: 'Other sports',
      status: 'Running'
    },
    {
      no: 2,
      betId: '124723475158925312',
      transTime: '04/22/2023 14:49:33PM',
      odds: 2.25,
      stake: 4.7,
      winningAmount: 4,
      houseAmount: 7,
      ip: '127.0.0.1',
      housePt: 0.15,
      totalComboBets: '1 bet',
      totalSelections: 2,
      sportType: 'Other sports',
      status: 'Running'
    },
    {
      no: 3,
      betId: '124723475158925312',
      transTime: '04/22/2023 14:49:33PM',
      odds: 2.25,
      stake: 4.7,
      winningAmount: 4,
      houseAmount: 7,
      ip: '127.0.0.1',
      housePt: 0.15,
      totalComboBets: '1 bet',
      totalSelections: 2,
      sportType: 'Other sports',
      status: 'Running'
    },
    {
      no: 4,
      betId: '124723475158925312',
      transTime: '04/22/2023 14:49:33PM',
      odds: 2.25,
      stake: 4.7,
      winningAmount: 4,
      houseAmount: 7,
      ip: '127.0.0.1',
      housePt: 0.15,
      totalComboBets: '1 bet',
      totalSelections: 2,
      sportType: 'Other sports',
      status: 'Running'
    }
  ];

  testLogModelList: LogModel[] = [
    {
      dateTime: '04/22/2023 14:49:33PM',
      user: 'sa',
      action: 'Send OS'
    },
    {
      dateTime: '04/22/2023 14:49:33PM',
      user: 'sa',
      action: 'Send OS'
    },
    {
      dateTime: '04/22/2023 14:49:33PM',
      user: 'sa',
      action: 'Send OS'
    }
  ];

  getSimilarTickets(): Observable<SimilarTicketModel[]> {
    return of(this.testSimilarTicketList);
  }

  getPotentialWinning(): Observable<PotentialWinningModel[]> {
    return of(this.testPotentialWinningList);
  }

  getLogs(): Observable<LogModel[]> {
    return of(this.testLogModelList);
  }

  getActions(): ItemModel[] {
    return [
      { id: '0', name: 'Choose Action', checked: true },
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
