import { Injectable } from '@angular/core';
import { HttpService } from 'nap-components/http';
import { Helper } from 'nap-components/helper';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { SimilarTicketModel } from './similar-ticket/similar-ticket.model';
import { PotentialWinningModel } from './potential-winning/potential-winning.model';
import { LogModel } from './log/log.model';
import { FilterModel, ItemModel } from '../filter/filter.model';
import { BetDetails, DataModel, TakeActionModel } from './grid.model';
import { HttpClient } from '@angular/common/http';
import { SpinnerService } from 'nap-components/spinner';

@Injectable({
  providedIn: 'root'
})
export class GridService {
  private api = 'api/manage-alert/';

  constructor(
    private httpService: HttpService,
    private readonly httpClient: HttpClient,
    private readonly spinnerService: SpinnerService
    ) { }

  getAlerts(apiUrl: string, filterModel: FilterModel): Observable<DataModel> {
    return this.httpService.post(`${apiUrl}${this.api}alerts`, filterModel);
  }
  
  getSimilarTickets(apiUrl: string, id: string): Observable<SimilarTicketModel[]> {
    return this.httpService.post(`${apiUrl}${this.api}similar-detail`, { alertId : id });
  }

  getPotentialWinning(apiUrl: string, id: string): Observable<PotentialWinningModel[]> {
    return this.httpService.post(`${apiUrl}${this.api}potential-winning-detail`, { alertId : id });
  }

  getLogs(apiUrl: string, id: string): Observable<LogModel[]> {
    return this.httpService.post(`${apiUrl}${this.api}log`, { alertId : id });
  }

  takeAction(apiUrl: string, model: TakeActionModel): Observable<LogModel[]> {
    return this.httpService.post(`${apiUrl}${this.api}take-action`, model);
  }

  getActions(): ItemModel[] {
    return [
      { id: '4', name: 'Choose Action', checked: true },
      { id: '1', name: 'Send S&V', checked: false },
      { id: '2', name: 'Send OS', checked: false },
      { id: '3', name: 'Take Note', checked: false }
    ]
  }

  getBetDetail(betDetails: BetDetails): Observable<string> {
    let url = '';
    let model;
    switch(betDetails.betType) { 
      case 38: { 
         url = 'BetList/SingleMatchParlayDetailPopup',
         model = {
          transId: betDetails.transId,
          custId: betDetails.custId,
          username: betDetails.userName
         }
         break; 
      } 
      case 376: { 
        url = 'BetList/PenaltyShootoutParlayDetailPopup',
        model = {
         transId: betDetails.transId,
         username: betDetails.userName
        } 
         break; 
      } 
      default: { 
        url = 'BetList/BetDetails',
         model = {
          transId: betDetails.transId,
          refNo: betDetails.refNo,
          username: betDetails.userName
         }
         break; 
      }
    }
    this.spinnerService.show();
    return this.httpClient.post(url, model, { responseType: 'text' })
            .pipe(tap(() => this.spinnerService.hide()));
  }

  public exportExcel(apiUrl: string, filterModel: FilterModel): void {
    const api = `${apiUrl}${this.api}export-excel-alerts`;
    this.httpService.download(api, filterModel).subscribe(response => {
        const fileName = `Manage-Alerts.xlsx`;
        Helper.downloadFile(response, fileName);
    });
  }

  public exportExcelForSimilarBet(apiUrl: string, id: string): void {
    const api = `${apiUrl}${this.api}export-excel-similar-bet`;
    this.httpService.download(api, { alertId : id }).subscribe(response => {
        const fileName = `Similar-Bet.xlsx`;
        Helper.downloadFile(response, fileName);
    });
  }

  public exportExcelForPotentialWinning(apiUrl: string, id: string): void {
    const api = `${apiUrl}${this.api}export-excel-potential-winning`;
    this.httpService.download(api, { alertId : id }).subscribe(response => {
        const fileName = `Potential-Winning.xlsx`;
        Helper.downloadFile(response, fileName);
    });
  }
}
