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
  
  getToday(): Observable<moment.Moment> {
    const api = `ServerTime/today`;
    return this.httpService.post<moment.Moment>(api);
  }
}
