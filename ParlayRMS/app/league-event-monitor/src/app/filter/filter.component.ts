import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { TreeviewConfig } from 'ngx-treeview';
import { BettypeModel, FilterModel, ItemModel, LeagueModel } from './filter.model';
import { FilterService } from './filter.service';
import { SelectItem } from 'nap-components/dropdown-select';
import { ReportTypeConsts, SportTypeConsts } from '../common/constants';
import { ToastService } from 'nap-components/toast';
import * as _ from 'lodash';
import { Subscription, interval } from 'rxjs';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss'],
})
export class FilterComponent implements OnInit {
  @Input() netcoreApiUrl: string = '';
  @Output() generated = new EventEmitter<FilterModel>();

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
  public bettypeItems: BettypeModel[] = [];
  public selectedBettypes:string[] = [];
  public smaMember: string = '';
  public refreshItems: ItemModel[] = [];
  public selectedRefresh: string = '60';
  public checkedRefresh: boolean = false;
  public isClickedSubmit: boolean = false;
  public sportItems: ItemModel[] = [];
  public selectedSport: string = SportTypeConsts.ESPORT; 

  public itemConfig = TreeviewConfig.create({
    hasFilter: true,
    hasAllCheckBox: true,
    hasCollapseExpand: false,
    maxHeight: 500
  });

  timer = '60'
  seconds = 60;
  selectedSeconds = 60;
  countDownSubcription!: Subscription;

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
    this.filterService.getLeagues(this.netcoreApiUrl, Number(SportTypeConsts.ESPORT)).subscribe(
      (leagues: LeagueModel[]) => this.leagueItems = leagues);
    this.topItems = this.filterService.getTops();
    this.levelItems = this.filterService.getLevels();
    this.refreshItems = this.filterService.getRefreshs();
    this.sportItems = this.filterService.getSports();
    this.filterService.getBettypes(this.netcoreApiUrl, Number(this.selectedSport)).subscribe(
      (bettypes: BettypeModel[]) => this.bettypeItems = bettypes);
  }

  public changeRefreshnow(): void {
    this.seconds = Number(this.selectedRefresh);
    this.selectedSeconds = Number(this.selectedRefresh);
    this.checkedRefresh = false;
    this.clearTimer();
    if (this.isClickedSubmit) {
      this.startTimer();
    } else {
      this.timer = this.selectedRefresh;
    }
  }

  public getData(): void {
    this.emitData();
    this.startCountdown();
  }

  private emitData(): void {
    this.isClickedSubmit = true;
    this.checkedRefresh = false;
    const model = this.createModel();
    if (!this.isValid) {
      return;
    }
    this.generated.emit(model);
  }

  private createModel(): FilterModel {
    return {
      selection: this.selectedComboes.join(','),
      parlayType: this.selectedParlayTypes.join(','),
      isViewDetail: this.reportType === ReportTypeConsts.EVENT || this.selectedLeagues.length === 1,
      leagueIds: this.selectedLeagues.join(','),
      limitRowCount: Number(this.selectedTop),
      level: this.selectedLevels.join(','),
      customerClass: this.customerClass === '' || this.customerClass === null ? undefined : Number(this.customerClass),
      sportType: Number(this.selectedSport),
      betTypes: this.selectedBettypes.join(','),
      userName: this.smaMember
    };
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

  public getLeagueAndBettypeBySport(): void {
    this.filterService.getLeagues(this.netcoreApiUrl, Number(this.selectedSport)).subscribe(
      (leagues: LeagueModel[]) => this.leagueItems = leagues);
    this.filterService.getBettypes(this.netcoreApiUrl, Number(this.selectedSport)).subscribe(
      (bettypes: BettypeModel[]) => this.bettypeItems = bettypes);
  }

  private startTimer(): void {
    this.timer = this.selectedSeconds.toString();
    this.countDownSubcription = interval(1000).subscribe(() => {
      this.selectedSeconds--;
      if (this.selectedSeconds === 0) {
        this.emitData();
        this.selectedSeconds = Number(this.selectedRefresh);
      }
      this.timer = this.selectedSeconds.toString();
    })
  }

  private clearTimer(): void {
    if (this.countDownSubcription !== undefined) {
      this.countDownSubcription.unsubscribe();
    }
  }

  startCountdown(): void {
    this.clearTimer();
    this.startTimer();
  }

  pauseTimer(): void {
    if (this.checkedRefresh) {
      this.selectedSeconds = Number(this.timer);
      this.clearTimer();
    } else {
      this.startTimer();
    }
  }
}
