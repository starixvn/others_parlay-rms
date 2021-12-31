import { Component, ElementRef } from '@angular/core';
import { AppService } from './app.service';
import { EventGroupModel } from './event/event.model';
import { FilterModel } from './filter/filter.model';
import { LeagueDataModel } from './league/league.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'league-event-monitor';
  netcoreApiUrl: string;
  filterModel!: FilterModel;
  leagueData: LeagueDataModel[] = [];
  eventData: EventGroupModel[] = [];
  isShowLeague: boolean = true;
  constructor(
    private readonly elementRef: ElementRef<HTMLElement>,
    private readonly appService: AppService
  ) {
    this.netcoreApiUrl = this.elementRef.nativeElement.getAttribute('netcore-api-url')??'';
  }

  public generate(filterModel: FilterModel): void {
    this.filterModel = filterModel;
    if (this.filterModel.isViewDetail) {
      this.isShowLeague = false;
      this.appService.getEvents(this.netcoreApiUrl, this.filterModel).subscribe((data: EventGroupModel[]) => this.eventData = data);
    } else {
      this.isShowLeague = true;
      this.appService.getLeaguesData(this.netcoreApiUrl, this.filterModel).subscribe((data: LeagueDataModel[]) => this.leagueData = data);
    }
  }

  public showEvent(): void {
    this.isShowLeague = false;
  }

  public showBackLeague(): void {
    this.isShowLeague = true;
  }
}
