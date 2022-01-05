import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { TreeviewConfig } from 'ngx-treeview';
import { FilterModel, ItemModel, LeagueModel } from './filter.model';
import { FilterService } from './filter.service';
import { SelectItem } from 'nap-components/dropdown-select';
import { ReportTypeConsts } from '../common/constants';
import { CountdownComponent, CountdownConfig, CountdownEvent } from 'ngx-countdown';
import { ToastService } from 'nap-components/toast';
import * as _ from 'lodash';

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
  public isClickedSubmit: boolean = false;

  public itemConfig = TreeviewConfig.create({
    hasFilter: true,
    hasAllCheckBox: true,
    hasCollapseExpand: false,
    maxHeight: 500
  });

  public countdownConfig: CountdownConfig = {
    leftTime: 60,
    formatDate: ({ date }) => `${date / 1000}`,
    demand: true,
  };

  public reportTypeOptions: SelectItem[] = [];
  public reportType = ReportTypeConsts.LEAGUE;
  
  constructor(
    private readonly filterService: FilterService,
    private toastService: ToastService
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
    this.createModelCountdown(Number(this.selectedRefresh), false);
    this.countdown.restart();
    this.checkedRefresh = false;
  }

  public pauseCountdown(): void {
    this.checkedRefresh ? this.countdown.pause() : this.countdown.resume();
  }

  public getData(): void {
    this.createModelCountdown(60, false);
    this.isClickedSubmit = true;
    this.checkedRefresh = false;
    const model = this.createModel();
    if (!this.isValid) {
      return;
    }
    this.generated.emit(model);
  }

  private createModel(): FilterModel {
    console.log(this.customerClass);
    return {
      selection: this.selectedComboes.join(','),
      parlayType: this.selectedParlayTypes.join(','),
      isViewDetail: this.reportType === ReportTypeConsts.EVENT || this.selectedLeagues.length === 1,
      leagueIds: this.selectedLeagues.join(','),
      limitRowCount: Number(this.selectedTop),
      level: this.selectedLevels.join(','),
      customerClass: this.customerClass === '' || this.customerClass === null ? undefined : Number(this.customerClass)
    };
  }

  public handleCountdown(e: CountdownEvent) {
    if (e.left === 0) {
      this.getData();
      this.countdown.restart()
    }
  }

  private get isValid(): boolean {
    if (this.selectedComboes.length === 0) {
      this.toastService.activate(`Please select at least one Combo!`);
      return false;
    }

    if (this.selectedParlayTypes.length === 0) {
      this.toastService.activate(`Please select at least one parlay!`);
      return false;
    }

    if (this.selectedLeagues.length === 0) {
      this.toastService.activate(`Please select at least one league!`);
      return false;
    }

    if (this.selectedLevels.length === 0) {
      this.toastService.activate(`Please select at least one level!`);
      return false;
    }

    return true;
  }

  private createModelCountdown(time: number, demand: boolean): void {
    this.countdownConfig = {
      leftTime: time,
      formatDate: ({ date }) => `${date / 1000}`,
      demand: demand
    };
  }
}
