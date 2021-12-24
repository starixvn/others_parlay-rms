import { Injectable } from '@angular/core';
import { SelectItem } from 'nap-components/dropdown-select';
import { HttpService } from 'nap-components/http';
import { Observable } from 'rxjs';
import { ReportTypeConsts } from '../common/constants';
import { ItemModel } from './filter.model';

@Injectable({
  providedIn: 'root'
})
export class FilterService {
  private api = 'api/league-event-monitor/filter/';
  // constructor(private httpService: HttpService)
  // { }

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
}
