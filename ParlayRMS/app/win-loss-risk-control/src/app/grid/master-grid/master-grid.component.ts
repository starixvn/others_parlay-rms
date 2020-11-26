import { Component } from '@angular/core';
import * as _ from 'lodash';
import { Grid, GridType } from '../grid.model';
import { GridService } from '../grid.service';
import { MasterGridService } from './master-grid.service';
import { WinLossModel } from 'src/app/common/win-loss.model';
import { Observable } from 'rxjs/Observable';
import { AgentGridService } from '../agent-grid/agent-grid.service';

@Component({
    selector: 'app-master-grid',
    templateUrl: './master-grid.component.html',
    styleUrls: ['./master-grid.component.scss']
})
export class MasterGridComponent implements Grid {

    public winLoss: WinLossModel;

    constructor(
        private gridService: GridService,
        private masterGridService: MasterGridService,
        private agentGridService: AgentGridService,
    ) { }

    query() {
        return new Observable(observer => {
            this.masterGridService.query().subscribe(data => {
                this.winLoss = data;
                observer.complete();
            });
        });
    }

    exportExcel() {
      //this.ticketService.exportExcel();
    }

    get hasExcel(): boolean {
        return this.hasData;
    }

    get hasData(): boolean {
        return !_.isNil(this.winLoss) && this.winLoss.rows.length > 0;
    }

    get title(): string {
      return this.masterGridService.model.username;
    }

    public showAgent(username: string, custId: number) {
      this.agentGridService.model = this.masterGridService.createAgentGridModel(username, custId);
      this.gridService.generate(GridType.Agent);
    }
}
