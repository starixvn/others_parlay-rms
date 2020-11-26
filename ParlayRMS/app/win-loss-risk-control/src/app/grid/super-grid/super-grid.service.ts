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
        private readonly gridService: GridService
    ) { }

    public query(): Observable<WinLossModel> {
        const queryModel = this.createQueryModel();
        const api = `${this.gridService.filterModel.netcoreApiUrl}api/win-loss-risk-control/company-win-loss`;
        return this.httpService.post<WinLossModel>(api, queryModel);
    }

    private createQueryModel() {
        const filterModel = this.gridService.filterModel;
        return {
            fromDate: filterModel.fromDate,
            toDate: filterModel.toDate,
            company: this.model.username,
            isDeposit: false,
            parlayType: filterModel.parlayType,
            sportType: filterModel.sportType,
        };
    }

    public createMasterGridModel(username: string, custId: number): MasterGridModel {
        return {
            username,
            custId
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
