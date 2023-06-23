import { Injectable } from '@angular/core';
import { HttpService } from 'nap-components/http';
import * as moment from 'moment';
import { Observable, of } from 'rxjs';
import { FilterModel } from './filter/filter.model';
import { DataModel } from './grid/grid.model';

@Injectable({
  providedIn: 'root'
})
export class AppService {
  
  constructor(private readonly httpService: HttpService) { }

  testDataModelList: DataModel[] = [
    {
      no: 1,
      alertTime: '4/21/2023 16:07:00 PM',
      alertType: 'Similar Bet',
      betId: '124723475158925312',
      odds: 3.7,
      potentialCompanyLoseAmount: 4.7,
      sportType: 'Soccer Only',
      totalBets: 4,
      totalMemberStake: 4.7,
      totalPotentialWinningAmount: 4.7,
      totalSelection: 7,
      totalSimilarTickets: 4,
      userName: 'HAIFATB011706',
      checkedBy: 'sa',
      action: '1'
    },
    {
      no: 2,
      alertTime: '4/21/2023 16:07:00 PM',
      alertType: 'Small Odds',
      betId: '124414563931127808',
      odds: 3.7,
      potentialCompanyLoseAmount: 4.7,
      sportType: 'Basketball',
      totalBets: 4,
      totalMemberStake: 4.7,
      totalPotentialWinningAmount: 4.7,
      totalSelection: 7,
      totalSimilarTickets: 4,
      userName: 'M999RM00F31',
      checkedBy: 'sa',
      action: '1'
    },
    {
      no: 3,
      alertTime: '4/21/2023 16:07:00 PM',
      alertType: 'Small League & Odds',
      betId: '124414563931127808',
      odds: 3.7,
      potentialCompanyLoseAmount: 4.7,
      sportType: 'Basketball',
      totalBets: 4,
      totalMemberStake: 4.7,
      totalPotentialWinningAmount: 4.7,
      totalSelection: 7,
      totalSimilarTickets: 4,
      userName: 'HAIFATB011706',
      checkedBy: 'sa',
      action: '1'
    },
    {
      no: 4,
      alertTime: '4/21/2023 16:07:00 PM',
      alertType: 'Potential Winning',
      betId: '124414563931127808',
      odds: 3.7,
      potentialCompanyLoseAmount: 4.7,
      sportType: 'Basketball',
      totalBets: 4,
      totalMemberStake: 4.7,
      totalPotentialWinningAmount: 4.7,
      totalSelection: 7,
      totalSimilarTickets: 4,
      userName: 'HAIFATB011706',
      checkedBy: '',
      action: '0'
    }
  ];

  getToday(): Observable<moment.Moment> {
    const api = `ServerTime/today`;
    return this.httpService.post<moment.Moment>(api);
  }

  getData(filterModel: FilterModel): Observable<DataModel[]> {
    return of(this.testDataModelList);
  }
}
