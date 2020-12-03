import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { HttpService } from 'nap-components/http';
import { GridService } from '../grid.service';
import { Observable } from 'rxjs/Observable';
import { Helper } from 'nap-components/helper';
import { MemberGridModel, MemberWinLossModel } from './member-grid.model';
import { Role } from 'src/app/common/role.model';

@Injectable()
export class MemberGridService {
    public model: MemberGridModel;
    constructor(
        private readonly httpService: HttpService,
        private readonly gridService: GridService
    ) { }

    public query(): Observable<MemberWinLossModel> {
        const queryModel = this.createQueryModel();
        const api = `${this.gridService.netcoreApi}api/win-loss-risk-control/agent-win-loss`;
        return this.httpService.post<MemberWinLossModel>(api, queryModel);
    }

    private createQueryModel() {
        const filterModel = this.gridService.filterModel;
        return {
            breadCrumb: this.gridService.breadCrumb,
            fromDate: filterModel.fromDate,
            toDate: filterModel.toDate,
            custId: this.model.custId,
            roleId: Role.agent,
            parlayType: filterModel.parlayType,
            sportType: filterModel.sportType,
            pageSize: filterModel.pageSize,
            pageIndex: filterModel.pageIndex
        };
    }
    exportExcel() {
        const queryModel = this.createQueryModel();
        const fromDate = moment(queryModel.fromDate).format('YYYY_MM_DD');
        const toDate = moment(queryModel.toDate).format('YYYY_MM_DD');
        const api = `${this.gridService.netcoreApi}api/win-loss-risk-control/member-win-loss-excel`;
        this.httpService.download(api, queryModel).subscribe(response => {
            const fileName = `Member_Win_Loss_From_${fromDate}_To_${toDate}.xlsx`;
            Helper.downloadFile(response, fileName);
        });
    }
}
