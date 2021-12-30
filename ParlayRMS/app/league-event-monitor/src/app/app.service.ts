import { Injectable } from '@angular/core';
import { HttpService } from 'nap-components/http';
import { Observable } from 'rxjs';
import { FilterModel } from './filter/filter.model';
import { LeagueDataModel } from './league/league.model';

@Injectable({
  providedIn: 'root'
})
export class AppService {

  private api = 'api/league-event-control/leagueevent/';
  constructor(private readonly httpService: HttpService)
  { }

  public getLeaguesData(netcoreApiUrl: string, query: FilterModel): Observable<LeagueDataModel[]> {
    return this.httpService.post(`${netcoreApiUrl}${this.api}league-data`, query);
  }
}
