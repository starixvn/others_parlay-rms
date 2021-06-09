import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GridComponent } from './grid.component';
import { GridService } from './grid.service';
import { SiteGridService } from './site-grid/site-grid.service';
import { SiteGridComponent } from './site-grid/site-grid.component';
import { NumberFormatModule } from 'nap-components/number-format';
import { BreadcrumbModule } from 'nap-components/breadcrumb';
import { PaginationService, PaginationConfig, PaginationModule } from 'nap-components/pagination';
import { SuperGridComponent } from './super-grid/super-grid.component';
import { SuperGridService } from './super-grid/super-grid.service';
import { MemberGridComponent } from './member-grid/member-grid.component';
import { MemberGridService } from './member-grid/member-grid.service';
import { CustomPaginationConfig } from '../common/config';
import { MasterGridComponent } from './master-grid/master-grid.component';
import { AgentGridComponent } from './agent-grid/agent-grid.component';
import { MasterGridService } from './master-grid/master-grid.service';
import { AgentGridService } from './agent-grid/agent-grid.service';
import { SortDirective } from '../directive/sort.directive';

@NgModule({
    imports: [
        CommonModule,
        NumberFormatModule,
        BreadcrumbModule,
        PaginationModule
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ],
    declarations: [
        GridComponent,
        SiteGridComponent,
        SuperGridComponent,
        MasterGridComponent,
        AgentGridComponent,
        MemberGridComponent,
        SortDirective
    ],
    exports: [
        GridComponent
    ],
    providers: [
        GridService,
        SiteGridService,
        SuperGridService,
        MasterGridService,
        AgentGridService,
        MemberGridService,
        PaginationService,
        PaginationConfig,
       { provide: PaginationConfig, useClass: CustomPaginationConfig },
    ]
})

export class GridModule { }
