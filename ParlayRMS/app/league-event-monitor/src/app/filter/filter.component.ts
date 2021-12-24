import { Component, OnInit } from '@angular/core';
import { TreeviewConfig } from 'ngx-treeview';
import { ItemModel } from './filter.model';
import { FilterService } from './filter.service';
import { SelectItem } from 'nap-components/dropdown-select';
import { ReportTypeConsts } from '../common/constants';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss'],
})
export class FilterComponent implements OnInit {

  public comboItems: ItemModel[] = [];
  public selectedComboes:string[] = [];
  public parlayTypeItems: ItemModel[] = [];
  public selectedParlayTypes:string[] = [];
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
  }

}
