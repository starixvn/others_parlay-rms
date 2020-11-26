import { Component } from '@angular/core';
import * as _ from 'lodash';
import { Grid, GridType } from '../grid.model';
import { GridService } from '../grid.service';
import { SuperGridService } from './super-grid.service';
import { WinLossModel } from 'src/app/common/win-loss.model';
import { Observable } from 'rxjs/Observable';
import { MasterGridService } from '../master-grid/master-grid.service';

@Component({
    selector: 'app-super-grid',
    templateUrl: './super-grid.component.html',
    styleUrls: ['./super-grid.component.scss']
})
export class SuperGridComponent implements Grid {

    public winLoss: WinLossModel;

    constructor(
        private gridService: GridService,
        private superGridService: SuperGridService,
        private masterGridService: MasterGridService,
    ) { }

    query() {
        return new Observable(observer => {
            this.superGridService.query().subscribe(data => {
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
      return this.superGridService.model.username;
    }

    public showMaster(username: string, custId: number) {
        this.masterGridService.model = this.superGridService.createMasterGridModel(username, custId);
        this.gridService.generate(GridType.Master);
    }
}
