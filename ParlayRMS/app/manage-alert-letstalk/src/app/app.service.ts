import { Injectable } from '@angular/core';
import { HttpService } from 'nap-components/http';
import * as moment from 'moment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppService {
  
  constructor(private httpService: HttpService) { }

  getToday(): Observable<moment.Moment> {
      const api = `ServerTime/today`;
      return this.httpService.post<moment.Moment>(api);
  }
}
