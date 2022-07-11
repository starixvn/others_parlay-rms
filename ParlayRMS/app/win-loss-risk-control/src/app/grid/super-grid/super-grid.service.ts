import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { HttpService } from 'nap-components/http';
import { GridService } from '../grid.service';
import { Observable } from 'rxjs/Observable';
import { Helper } from 'nap-components/helper';
import { WinLossModel } from 'src/app/common/win-loss.model';
import * as _ from 'lodash';
import { SuperGridModel } from './super-grid.model';
import { MasterGridModel } from '../master-grid/master-grid.model';

@Injectable()
export class SuperGridService {
    public model: SuperGridModel;
    constructor(
        private readonly httpService: HttpService,
        private readonly gridService: GridService){
    }

    public query(): Observable<WinLossModel> {
        const queryModel = this.createQueryModel();
        const api = `${this.gridService.netcoreApi}api/win-loss-risk-control/company-win-loss`;
        return this.httpService.post<WinLossModel>(api, queryModel);
    }

    private createQueryModel() {
        const filterModel = this.gridService.filterModel;
        return {
            breadCrumb: this.gridService.breadCrumb,
            fromDate: filterModel.fromDate,
            toDate: filterModel.toDate,
            company: this.model.username,
            isDeposit: false,
            parlayType: filterModel.parlayType,
            sportType: filterModel.sportType,
            selection: filterModel.selection,
            liveStatus: filterModel.liveStatus
        };
    }

    public createMasterGridModel(username: string, custId: number, site: string): MasterGridModel {
        return {
            username,
            custId,
            site
        };
    }

    exportExcel() {
      const queryModel = this.createQueryModel();
      const fromDate = moment(queryModel.fromDate).format('YYYY_MM_DD');
      const toDate = moment(queryModel.toDate).format('YYYY_MM_DD');
      const api = `${this.gridService.netcoreApi}api/win-loss-risk-control/company-win-loss-excel`;
      this.httpService.download(api, queryModel).subscribe(response => {
          const fileName = `Super_Win_Loss_From_${fromDate}_To_${toDate}.xlsx`;
          Helper.downloadFile(response, fileName);
      });
    }
}
