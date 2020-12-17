import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpService } from 'nap-components/http';
import { Observable } from 'rxjs';
import { Company, ParlayType, Sport } from './filter.model';

@Injectable()
export class FilterService {
  private api = 'api/win-loss-risk-control/filter/';
  constructor(private httpService: HttpService)
  { }

  public getSports(netcoreApiUrl: string): Observable<Sport[]> {
    return this.httpService.post(`${netcoreApiUrl}${this.api}sports`);
  }

  public getParlay(netcoreApiUrl: string): Observable<ParlayType[]> {
    return this.httpService.post(`${netcoreApiUrl}${this.api}parlay-types`);
  }

  public getCompany(netcoreApiUrl: string): Observable<Company[]> {
    return this.httpService.post(`${netcoreApiUrl}${this.api}company`);
  }
}
