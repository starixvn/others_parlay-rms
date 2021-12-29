import { Component, Input, OnInit } from '@angular/core';
import { TreeviewConfig } from 'ngx-treeview';
import { ItemModel, LeagueModel } from './filter.model';
import { FilterService } from './filter.service';
import { SelectItem } from 'nap-components/dropdown-select';
import { ReportTypeConsts } from '../common/constants';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss'],
})
export class FilterComponent implements OnInit {
  @Input() netcoreApiUrl: string = '';

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

}
