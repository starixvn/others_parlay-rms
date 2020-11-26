import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { HttpService } from 'nap-components/http';
import { GridService } from '../grid.service';
import { Observable } from 'rxjs/Observable';
import { Helper } from 'nap-components/helper';
import { WinLossModel } from 'src/app/common/win-loss.model';
import * as _ from 'lodash';
import { MasterGridModel } from './master-grid.model';
import { Role } from 'src/app/common/role.model';
import { AgentGridModel } from '../agent-grid/agent-grid.model';

@Injectable()
export class MasterGridService {
    public model: MasterGridModel;
    constructor(
        private readonly httpService: HttpService,
        private readonly gridService: GridService
    ) { }

    public query(): Observable<WinLossModel> {
        const queryModel = this.createQueryModel();
        const api = `${this.gridService.filterModel.netcoreApiUrl}api/win-loss-risk-control/agent-win-loss`;
        return this.httpService.post<WinLossModel>(api, queryModel);
    }

    private createQueryModel() {
        const filterModel = this.gridService.filterModel;
        return {
            fromDate: filterModel.fromDate,
            toDate: filterModel.toDate,
            custId: this.model.custId,
            roleId: Role.super,
            parlayType: filterModel.parlayType,
            sportType: filterModel.sportType,
        };
    }

    public createAgentGridModel(username: string, custId: number): AgentGridModel {
        return {
            username: username,
            custId: custId,
            isDeposit: false
        };
    }

    // exportExcel() {
    //     const queryModel = this.gridService.filterModel;
    //     const winlostDate = moment(queryModel.winlostDate).format('YYYY_MM_DD');
    //     this.httpService.download('api/voiding-settled-tickets/products-excel', queryModel).subscribe(response => {
    //         const fileName = `Voiding_Settled_Tickets_Report_For_${winlostDate}.xlsx`;
    //         Helper.downloadFile(response, fileName);
    //     });
    // }
}
