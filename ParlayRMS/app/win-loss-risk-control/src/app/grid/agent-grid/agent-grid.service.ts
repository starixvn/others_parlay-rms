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

@Injectable()
export class AgentGridService {
    public model: AgentGridModel;
    constructor(
        private readonly httpService: HttpService,
        private readonly gridService: GridService
    ) { }

    public query(): Observable<WinLossModel> {
        const queryModel = this.createQueryModel();
        const api = (this.model.custId < 1 && this.model.isDeposit)
                ? 'api/win-loss-risk-control/company-win-loss'
                : 'api/win-loss-risk-control/agent-win-loss';
        return this.httpService.post<WinLossModel>(`${this.gridService.filterModel.netcoreApiUrl}${api}`, queryModel);
    }

    private createQueryModel() {
        const filterModel = this.gridService.filterModel;
        if (this.model.custId < 1 && this.model.isDeposit) {
            return {
                fromDate: filterModel.fromDate,
                toDate: filterModel.toDate,
                isDeposit: true,
                company: this.model.username,
                parlayType: filterModel.parlayType,
                sportType: filterModel.sportType,
            };
        } else {
            return {
                netcoreApiUrl: filterModel.netcoreApiUrl,
                fromDate: filterModel.fromDate,
                toDate: filterModel.toDate,
                custId: this.model.custId,
                roleId: Role.master,
                parlayType: filterModel.parlayType,
                sportType: filterModel.sportType,
            };
        }
    }

    public createMemberGridModel(username: string, custId: number): MemberGridModel {
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
