import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { TreeviewConfig } from 'ngx-treeview';
import { FilterModel, ItemModel, LeagueModel } from './filter.model';
import { FilterService } from './filter.service';
import { SelectItem } from 'nap-components/dropdown-select';
import { ReportTypeConsts } from '../common/constants';
import { CountdownComponent, CountdownConfig } from 'ngx-countdown';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss'],
})
export class FilterComponent implements OnInit {
  @Input() netcoreApiUrl: string = '';
  @Output() generated = new EventEmitter<FilterModel>();
  @ViewChild('countdown', { static: false }) countdown!: CountdownComponent;

  public comboItems: ItemModel[] = [];
  public selectedComboes:string[] = [];
  public parlayTypeItems: ItemModel[] = [];
  public selectedParlayTypes:string[] = [];
  public leagueItems: LeagueModel[] = [];
  public selectedLeagues:string[] = [];
  public topItems: ItemModel[] = [];
  public selectedTop: string = '200';
  public levelItems: ItemModel[] = [];
  public selectedLevels:string[] = [];
  public customerClass: string = '';
  public refreshItems: ItemModel[] = [];
  public selectedRefresh: string = '60';
  public checkedRefresh: boolean = false;

  public itemConfig = TreeviewConfig.create({
    hasFilter: true,
    hasAllCheckBox: true,
    hasCollapseExpand: false,
    maxHeight: 500
  });

  public countdownConfig: CountdownConfig = {
    leftTime: 60,
    formatDate: ({ date }) => `${date / 1000}`,
  };

  public reportTypeOptions: SelectItem[] = [];
  public reportType = ReportTypeConsts.LEAGUE;
  
  constructor(
    private readonly filterService: FilterService
  ) { }

  ngOnInit(): void {
    this.comboItems = this.filterService.getComboes();
    this.parlayTypeItems = this.filterService.getParlayTypes();
    this.reportTypeOptions = this.filterService.getReportTypeOptions();
    this.filterService.getLeagues(this.netcoreApiUrl).subscribe((leagues: LeagueModel[]) => this.leagueItems = leagues);
    this.topItems = this.filterService.getTops();
    this.levelItems = this.filterService.getLevels();
    this.refreshItems = this.filterService.getRefreshs();
  }

  public changeRefreshnow(): void {
    this.countdownConfig = {
      leftTime: Number(this.selectedRefresh),
      formatDate: ({ date }) => `${date / 1000}`,
    };
    this.countdown.restart();
    this.checkedRefresh = false;
  }

  public pauseCountdown(): void {
    this.checkedRefresh ? this.countdown.pause() : this.countdown.resume();
  }

  public getData(): void {
    const model = this.createModel();
    this.generated.emit(model);
  }

  private createModel(): FilterModel {
    return {
      selection: this.selectedComboes.join(','),
      parlayType: this.selectedParlayTypes.join(','),
      isViewDetail: this.reportType === ReportTypeConsts.EVENT,
      leagueIds: this.selectedLeagues.join(','),
      limitRowCount: Number(this.selectedTop),
      level: this.selectedLevels.join(','),
      customerClass: Number(this.customerClass)
    };
  }
}
