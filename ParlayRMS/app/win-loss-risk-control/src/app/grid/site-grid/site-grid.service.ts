import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { HttpService } from 'nap-components/http';
import { GridService } from '../grid.service';
import { Observable } from 'rxjs/Observable';
import { WinLossModel } from 'src/app/common/win-loss.model';
import { SuperGridModel } from '../super-grid/super-grid.model';
import { AgentGridModel } from '../agent-grid/agent-grid.model';
import { Helper } from 'nap-components/helper';

@Injectable()
export class SiteGridService {
    public model: SuperGridModel;
    constructor(
        private readonly httpService: HttpService,
        private readonly gridService: GridService
    ) { }

    public query(): Observable<WinLossModel> {
        const queryModel = this.gridService.filterModel;
        const api = `${this.gridService.netcoreApi}api/win-loss-risk-control/get-site`;
        return this.httpService.post<WinLossModel>(api, queryModel);
    }

    public createSuperGridModel(username: string): SuperGridModel {
        return {
            username,
            isDeposit: false
        };
    }

    public createAgentGridModel(username: string): AgentGridModel {
        return {
            username,
            isDeposit: true,
            custId: 0
        };
    }
    exportExcel() {
      const queryModel = this.gridService.filterModel;
      const fromDate = moment(queryModel.fromDate).format('YYYY_MM_DD');
      const toDate = moment(queryModel.toDate).format('YYYY_MM_DD');
      const api = `${this.gridService.netcoreApi}api/win-loss-risk-control/site-win-loss-excel`;
      this.httpService.download(api, queryModel).subscribe(response => {
          const fileName = `Site_Win_Loss_From_${fromDate}_To_${toDate}.xlsx`;
          Helper.downloadFile(response, fileName);
      });
    }
}
