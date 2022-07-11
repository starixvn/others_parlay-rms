import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { HttpService } from 'nap-components/http';
import { GridService } from '../grid.service';
import { Observable } from 'rxjs/Observable';
import { WinLossModel } from 'src/app/common/win-loss.model';
import * as _ from 'lodash';
import { AgentGridModel } from './agent-grid.model';
import { MemberGridModel } from '../member-grid/member-grid.model';
import { Role } from 'src/app/common/role.model';
import { Helper } from 'nap-components/helper';

@Injectable()
export class AgentGridService {
    public model: AgentGridModel;
    constructor(
        private readonly httpService: HttpService,
        private readonly gridService: GridService
    ) { }

    public query(): Observable<WinLossModel> {
        const queryModel = this.createQueryModel();
        const api = `${this.gridService.netcoreApi}api/win-loss-risk-control/agent-win-loss`;
        return this.httpService.post<WinLossModel>(api, queryModel);
    }

    private createQueryModel() {
        const filterModel = this.gridService.filterModel;
        return {
            breadCrumb: this.gridService.breadCrumb,
            fromDate: filterModel.fromDate,
            toDate: filterModel.toDate,
            custId: this.model.custId,
            site: this.model.site,
            isDeposit: this.model.isDeposit,
            company: this.model.username,
            roleId: Role.master,
            parlayType: filterModel.parlayType,
            sportType: filterModel.sportType,
            selection: filterModel.selection,
            liveStatus: filterModel.liveStatus
        };
    }

    public createMemberGridModel(username: string, custId: number, site: string): MemberGridModel {
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
      const api = `${this.gridService.netcoreApi}api/win-loss-risk-control/agent-win-loss-excel`;
      this.httpService.download(api, queryModel).subscribe(response => {
          const fileName = `Agent_Win_Loss_From_${fromDate}_To_${toDate}.xlsx`;
          Helper.downloadFile(response, fileName);
      });
    }
}
