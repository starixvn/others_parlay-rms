import { Component, ElementRef, TemplateRef, ViewChild, ViewContainerRef } from '@angular/core';
import * as _ from 'lodash';
import { AppService } from './app.service';
import { ReportTypeConsts } from './common/constants';
import { EventDetailGroupModel, EventDetailModel } from './event-detail/event-detail.model';
import { EventGroupModel, MatchBettypeModel } from './event/event.model';
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
  @ViewChild("containerRef", { read: ViewContainerRef }) containerRef: ViewContainerRef | undefined;
  @ViewChild("contentRef", {read: TemplateRef}) contentRef: TemplateRef<any> | undefined;
  title = 'league-event-monitor';
  netcoreApiUrl: string;
  filterModel!: FilterModel;
  leagueData: LeagueDataModel[] = [];
  eventData: EventGroupModel[] = [];
  eventDetailData: EventDetailGroupModel | undefined;
  isShowLeague: boolean = true;
  isShowDetail: boolean = false;
  isClickedOnleague: boolean = false;
  isClickedOnEvent: boolean = false;
  cachedLeagueItems: LeagueModel[] = [];
  cachedTop: string = '0';
  betChoiceText: string = '';
  constructor(
    private readonly elementRef: ElementRef<HTMLElement>,
    private readonly appService: AppService
  ) {
    this.netcoreApiUrl = this.elementRef.nativeElement.getAttribute('netcore-api-url')??'';
  }

  public generate(filterModel: FilterModel): void {
    this.containerRef?.clear();
    this.containerRef?.createEmbeddedView(this.contentRef!);
    this.filterModel = filterModel;
    this.isClickedOnleague = false;
    this.isClickedOnEvent = false;
    this.isShowDetail = false;
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
    this.isShowDetail = false;
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

  public showEventDetail(matchBettype: MatchBettypeModel): void {
    this.isShowLeague = false;
    this.isShowDetail = true;
    this.isClickedOnEvent = true;
    const filterDetailModel = _.assign({}, this.filterModel,
      { matchId: matchBettype.matchId, betChoice: matchBettype.betChoice, detailBettype: matchBettype.bettype});

    this.appService.getEventDetail(this.netcoreApiUrl, filterDetailModel).subscribe(
      (data: EventDetailModel[]) => this.eventDetailData = {
        match: matchBettype.event, eventList: data, fixedBettypes: data.length, matchId: matchBettype.matchId
      }
    );
  }

  public exportExcel(matchBettype: MatchBettypeModel): void {
    switch(matchBettype.betChoice) { 
      case 1: { 
         this.betChoiceText = ' - [Home (O)]'; 
         break; 
      } 
      case 0: { 
         this.betChoiceText = ' - [Away (U)]'; 
         break; 
      } 
      default: { 
        this.betChoiceText = ''; 
         break; 
      } 
   }
    const exportExcelModel = _.assign({}, this.filterModel,
      { matchId: matchBettype.matchId,
        betChoice: matchBettype.betChoice,
        detailBettype: matchBettype.bettype,
        title: `Match [${matchBettype.event}] - [${matchBettype.bettypeName}]${this.betChoiceText}`
      });
    this.appService.exportExcel(this.netcoreApiUrl, exportExcelModel);
  }

  public showBackLeague(): void {
    this.isShowLeague = true;
    this.filterComponent.leagueItems = this.cachedLeagueItems;
    this.filterComponent.selectedTop = this.cachedTop;
    this.filterComponent.reportType = ReportTypeConsts.LEAGUE;
  }

  public showBackEvent(): void {
    this.isShowDetail = false;
    this.isShowLeague = false;
  }
}
