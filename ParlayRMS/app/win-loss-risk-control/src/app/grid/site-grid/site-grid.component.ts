import { Component } from '@angular/core';
import * as _ from 'lodash';
import { Grid, GridType } from '../grid.model';
import { GridService } from '../grid.service';
import { SiteGridService } from './site-grid.service';
import { WinLossModel } from 'src/app/common/win-loss.model';
import { Observable } from 'rxjs/Observable';
import { SuperGridService } from '../super-grid/super-grid.service';
import { AgentGridService } from '../agent-grid/agent-grid.service';

@Component({
    selector: 'app-site-grid',
    templateUrl: './site-grid.component.html',
    styleUrls: ['./site-grid.component.scss']
})
export class SiteGridComponent implements Grid {

    public siteWinLoss: WinLossModel;
    public sortData: WinLossModel[][] = [];

    constructor(
        private gridService: GridService,
        private siteGridService: SiteGridService,
        private superGridService: SuperGridService,
        private agentGridService: AgentGridService
    ) { }

    query() {
        return new Observable(observer => {
            this.siteGridService.query().subscribe(data => {
                this.siteWinLoss = data;
                this.getSortData(data);
                observer.complete();
            });
        });
    }

    exportExcel() {
        this.siteGridService.exportExcel();
    }

    get hasExcel(): boolean {
        return this.hasData;
    }

    get hasData(): boolean {
        return !_.isNil(this.siteWinLoss) && this.siteWinLoss.rows.length > 0;
    }

    get title(): string {
        return `Site`;
    }

    public showSuperOrAgent(username: string, isDeposit: boolean) {
      if (isDeposit) {
        this.agentGridService.model = this.siteGridService.createAgentGridModel(username);
        this.gridService.generate(GridType.Agent);
      } else {
        this.superGridService.model = this.siteGridService.createSuperGridModel(username);
        this.gridService.generate(GridType.Super);
      }
    }

    public onCollapsed(site: WinLossModel) {
      site.isCollapsed = !site.isCollapsed;
    }

    public getSortData(site: WinLossModel) {
        if (site.rows.length > 0) {
            _.forEach(site.rows, item => {
                this.sortData.push(item.rows);
            })
        }
    }
}
