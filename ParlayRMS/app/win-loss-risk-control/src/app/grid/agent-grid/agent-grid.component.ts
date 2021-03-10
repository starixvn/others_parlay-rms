import { Component } from '@angular/core';
import * as _ from 'lodash';
import { Grid, GridType } from '../grid.model';
import { GridService } from '../grid.service';
import { AgentGridService } from './agent-grid.service';
import { WinLossModel } from 'src/app/common/win-loss.model';
import { Observable } from 'rxjs/Observable';
import { MemberGridService } from '../member-grid/member-grid.service';

@Component({
    selector: 'app-agent-grid',
    templateUrl: './agent-grid.component.html',
    styleUrls: ['./agent-grid.component.scss']
})
export class AgentGridComponent implements Grid {

    public winLoss: WinLossModel;

    constructor(
        private gridService: GridService,
        private agentGridService: AgentGridService,
        private memberGridService: MemberGridService,
    ) { }

    query() {
        return new Observable(observer => {
            this.agentGridService.query().subscribe(data => {
                this.winLoss = data;
                observer.complete();
            });
        });
    }

    exportExcel() {
      this.agentGridService.exportExcel();
    }

    get hasExcel(): boolean {
        return this.hasData;
    }

    get hasData(): boolean {
        return !_.isNil(this.winLoss) && this.winLoss.rows.length > 0;
    }

    get title(): string {
      return this.agentGridService.model.username;
    }

    public showMember(username: string, custId: number, site: string) {
        this.memberGridService.model = this.agentGridService.createMemberGridModel(username, custId, site);
        this.gridService.generate(GridType.Member);
    }
}
