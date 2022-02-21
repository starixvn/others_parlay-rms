import { Injectable } from '@angular/core';
import { SelectItem } from 'nap-components/dropdown-select';
import { HttpService } from 'nap-components/http';
import { Observable } from 'rxjs';
import { ReportTypeConsts, SportTypeConsts } from '../common/constants';
import { ItemModel, LeagueModel } from './filter.model';

@Injectable({
  providedIn: 'root'
})
export class FilterService {
  private api = 'api/league-event-control/filter/';
  constructor(private readonly httpService: HttpService)
  { }

  public getComboes(): ItemModel[] {
    let result: ItemModel[] = [];
    for (let i = 2; i < 10; i++) {
      result.push({ name: i.toString(), id: i.toString(), checked: i <= 6 });
    }
    result.push({ name: '10 UP', id: '10', checked: false });
    return result;
  }

  public getParlayTypes(): ItemModel[] {
    return [
      { name: 'Mix Parlay', id: '1', checked: true },
      { name: 'Combo Parlay', id: '2', checked: true },
    ]
  }

  public getReportTypeOptions(): SelectItem[] {
    return [
      { text: 'League', value: ReportTypeConsts.LEAGUE },
      { text: 'Event', value: ReportTypeConsts.EVENT }
    ]
  }

  public getLeagues(netcoreApiUrl: string, sportId: number): Observable<LeagueModel[]> {
    return this.httpService.post(`${netcoreApiUrl}${this.api}league`, { sportType: sportId });
  }

  public getTops(): ItemModel[]
  {
    return [
      { id: '0', name: 'All', checked: false },
      { id: '10', name: '10', checked: false },
      { id: '20', name: '20', checked: false },
      { id: '50', name: '50', checked: false },
      { id: '100', name: '100', checked: false },
      { id: '200', name: '200', checked: true },
      { id: '1000', name: '1000', checked: false },
      { id: '2000', name: '2000', checked: false },
      { id: '3000', name: '3000', checked: false }
    ]
  }

  public getLevels(): ItemModel[] {
    let result: ItemModel[] = [];
    result.push({ name: 'None', id: '0', checked: true });
    for (let i = 1; i <= 10; i++) {
      result.push({ name: i.toString(), id: i.toString(), checked: true });
    }
    return result;
  }

  public getRefreshs(): ItemModel[]
  {
    return [
      { id: '60', name: '60', checked: true },
      { id: '120', name: '120', checked: false }
    ]
  }

  public getSports(): ItemModel[]
  {
    return [
      { id: SportTypeConsts.ESPORT, name: 'E-Sports', checked: true },
      { id: SportTypeConsts.BASKETBALL, name: 'Basketball ', checked: false },
    ]
  }
}
