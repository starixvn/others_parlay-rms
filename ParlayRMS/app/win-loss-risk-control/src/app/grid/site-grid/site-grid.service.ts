import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { HttpService } from 'nap-components/http';
import { GridService } from '../grid.service';
import { Observable } from 'rxjs/Observable';
import { WinLossModel } from 'src/app/common/win-loss.model';
import { SuperGridModel } from '../super-grid/super-grid.model';
import { AgentGridModel } from '../agent-grid/agent-grid.model';

@Injectable()
export class SiteGridService {
    public model: SuperGridModel;
    constructor(
        private readonly httpService: HttpService,
        private readonly gridService: GridService
    ) { }

    public query(): Observable<WinLossModel> {
        const queryModel = this.gridService.filterModel;
        const api = `${queryModel.netcoreApiUrl}api/win-loss-risk-control/get-site`;
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
    // exportExcel() {
    //     const queryModel = this.gridService.filterModel;
    //     const winlostDate = moment(queryModel.winlostDate).format('YYYY_MM_DD');
    //     this.httpService.download('api/voiding-settled-tickets/products-excel', queryModel).subscribe(response => {
    //         const fileName = `Voiding_Settled_Tickets_Report_For_${winlostDate}.xlsx`;
    //         Helper.downloadFile(response, fileName);
    //     });
    // }
}
