import { Component } from '@angular/core';
import * as _ from 'lodash';
import { Grid } from '../grid.model';
import { GridService } from '../grid.service';
import { MemberGridService } from './member-grid.service';
import { PaginatePipe, PaginationInstance, PaginationService as NgxPaginationService } from 'ngx-pagination';
import { PaginationService } from 'nap-components/pagination';
import { MemberWinLossModel } from './member-grid.model';
import { CustomPaginationConfig } from 'src/app/common/config';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'app-member-grid',
    templateUrl: './member-grid.component.html',
    styleUrls: ['./member-grid.component.scss']
})
export class MemberGridComponent implements Grid {

    public winLoss: MemberWinLossModel;
    paginationInstance: PaginationInstance;
    private paginatePipe: PaginatePipe;

    constructor(
        private gridService: GridService,
        private memberGridService: MemberGridService,
        private readonly paginationService: PaginationService,
        private readonly ngxPaginationService: NgxPaginationService) {
        this.paginatePipe = new PaginatePipe(this.ngxPaginationService);
    }

    query() {
        const pagingConfig = new CustomPaginationConfig();
        this.gridService.filterModel.pageIndex = 1;
        this.gridService.filterModel.pageSize = pagingConfig.itemsPerPage;

        return new Observable(observer => {
            this.memberGridService.query().subscribe(data => {
                this.winLoss = data;
                this.paginationInstance = this.paginationService.createPaginationInstance(this.winLoss.totalRecords);
                this.onPageChange();
                observer.complete();
            });
        });
    }

    exportExcel() {
      this.memberGridService.exportExcel();
    }

    get hasExcel(): boolean {
        return this.hasData;
    }

    get hasData(): boolean {
        return !_.isNil(this.winLoss) && this.winLoss.rows.length > 0;
    }

    get title(): string {
      return this.memberGridService.model.username;
    }

    onPageChange() {
        this.createPaging();
    }

    private createPaging() {
        if (this.gridService.filterModel.isPageChanged) {
            this.gridService.filterModel.pageIndex = this.paginationInstance.currentPage;
            this.gridService.filterModel.pageSize = this.paginationInstance.itemsPerPage;

            this.memberGridService.query().subscribe(data => {
            this.winLoss = data;
            this.createPagingTransform();
            });
        }

        this.createPagingTransform();
    }

    private createPagingTransform() {
        const paginateData = this.paginatePipe.transform(this.winLoss.rows, this.paginationInstance);
        this.winLoss.rows = _.cloneDeep(paginateData);
        this.gridService.filterModel.isPageChanged = true;
    }
}
