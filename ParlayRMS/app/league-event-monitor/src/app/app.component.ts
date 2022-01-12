import { Component, ElementRef, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { AppService } from './app.service';
import { ReportTypeConsts } from './common/constants';
import { EventGroupModel } from './event/event.model';
import { FilterComponent } from './filter/filter.component';
import { FilterModel, LeagueModel } from './filter/filter.model';
import { LeagueDataModel } from './league/league.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  @ViewChild('filterComponent', { static: false }) filterComponent!: FilterComponent;
  title = 'league-event-monitor';
  netcoreApiUrl: string;
  filterModel!: FilterModel;
  leagueData: LeagueDataModel[] = [];
  eventData: EventGroupModel[] = [];
  isShowLeague: boolean = true;
  isClickedOnleague: boolean = false;
  cachedLeagueItems: LeagueModel[] = [];
  cachedTop: string = '0';
  constructor(
    private readonly elementRef: ElementRef<HTMLElement>,
    private readonly appService: AppService
  ) {
    this.netcoreApiUrl = this.elementRef.nativeElement.getAttribute('netcore-api-url')??'';
  }

  public generate(filterModel: FilterModel): void {
    this.filterModel = filterModel;
    this.isClickedOnleague = false;
    if (this.filterModel.isViewDetail) {
      this.isShowLeague = false;
      this.appService.getEvents(this.netcoreApiUrl, this.filterModel).subscribe((data: EventGroupModel[]) => this.eventData = data);
    } else {
      this.isShowLeague = true;
      this.appService.getLeaguesData(this.netcoreApiUrl, this.filterModel).subscribe((data: LeagueDataModel[]) => this.leagueData = data);
    }
  }

  public showEvent(leagueId: number): void {
    this.isShowLeague = false;
    this.isClickedOnleague = true;
    this.cachedLeagueItems = _.cloneDeep(this.filterComponent.leagueItems);
    const tempLeagueItems = _.clone(this.filterComponent.leagueItems);
    this.cachedTop = this.filterComponent.selectedTop;
    _.map(tempLeagueItems, x => {
      if (x.leagueId === leagueId)
      {
        x.checked = true;
      } else {
        x.checked = false;
      }
    });
    this.filterComponent.leagueItems = tempLeagueItems;
    this.filterComponent.selectedTop = '0';
    this.filterComponent.reportType = ReportTypeConsts.EVENT;
    this.filterModel.limitRowCount = 0;
    this.filterModel.isViewDetail = true;
    this.filterModel.leagueIds = leagueId.toString();
    this.appService.getEvents(this.netcoreApiUrl, this.filterModel).subscribe((data: EventGroupModel[]) => this.eventData = data);
  }

  public showBackLeague(): void {
    this.isShowLeague = true;
    this.filterComponent.leagueItems = this.cachedLeagueItems;
    this.filterComponent.selectedTop = this.cachedTop;
    this.filterComponent.reportType = ReportTypeConsts.LEAGUE;
  }
}
