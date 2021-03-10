import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import * as moment from 'moment';
import { CONFIG } from '../common/config';
import { FilterModel } from './filter.model';
import { FilterService } from './filter.service';
import { NapDateStruct, DateHelper, DateFormat } from 'nap-components/date';
import { ToastService } from 'nap-components/toast';
import { Company, ParlayType, Sport } from './filter.model';
import { TreeviewConfig } from 'ngx-treeview';
@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss'],
  providers: [
    FilterService
  ]
})
export class FilterComponent implements OnInit {
  @Input() netcoreApiUrl: string;
  @Output() generated = new EventEmitter<FilterModel>();
  public company: Company[];
  public companyConfig = TreeviewConfig.create({
    hasFilter: true,
    hasAllCheckBox: true,
    hasCollapseExpand: false,
    maxHeight: 500
  });
  public selectedCompany: string[] = [];

  public parlayType: ParlayType[];
  public parlayTypeConfig = TreeviewConfig.create({
    hasFilter: false,
    hasAllCheckBox: true,
    hasCollapseExpand: false,
    maxHeight: 200
  });
  public selectedParlayType: string[] = [];

  public sportsType: Sport[];
  public sportsTypeConfig = TreeviewConfig.create({
    hasFilter: false,
    hasAllCheckBox: true,
    hasCollapseExpand: false,
    maxHeight: 500
  });
  public selectedSportsType: string[] = [];

  fromDate = DateHelper.convertMomentToDateStruct(CONFIG.date.today);
  toDate = DateHelper.convertMomentToDateStruct(CONFIG.date.today);

  minStartDate = DateHelper.convertMomentToDateStruct(CONFIG.date.today.add(-1, 'month'));
  maxEndDate = DateHelper.convertMomentToDateStruct(CONFIG.date.today);
  constructor(
    private readonly filterService: FilterService,
    private toastService: ToastService
  ) { }

  ngOnInit() {
    this.getCompany();
    this.getSports();
    this.getParlay();
  }

  private getCompany() {
    this.filterService.getCompany(this.netcoreApiUrl).subscribe((company: Company[]) => this.company = company);
  }

  private getSports() {
    this.filterService.getSports(this.netcoreApiUrl).subscribe((sports: Sport[]) => this.sportsType = sports);
  }

  private getParlay() {
    this.filterService.getParlay(this.netcoreApiUrl).subscribe((parlay: ParlayType[]) => this.parlayType = parlay);
  }

  public onFromDateChange() {
    let fromDate: moment.Moment;
    fromDate = DateHelper.convertDateStructToMoment(this.fromDate);
    fromDate = moment.utc(fromDate);
    this.fromDate = DateHelper.convertMomentToDateStruct(fromDate);

    const maxEndDate = fromDate.add(1, 'month');
    this.maxEndDate = DateHelper.convertMomentToDateStruct(maxEndDate);
  }

  public onToDateChange() {
    let toDate: moment.Moment;
    toDate = DateHelper.convertDateStructToMoment(this.toDate);
    toDate = moment.utc(toDate);
    this.toDate = DateHelper.convertMomentToDateStruct(toDate);

    const minStartDate = toDate.subtract(1, 'month');
    this.minStartDate = DateHelper.convertMomentToDateStruct(minStartDate);
  }

  public onSubmit() {
    const model = this.createModel();

    if (!this.isValid) {
      return;
    }

    this.generated.emit(model);
  }

  private createModel(): FilterModel {
    const fromDate = DateHelper.convertDateStructToMoment(this.fromDate);
    const toDate = DateHelper.convertDateStructToMoment(this.toDate);

    const model: FilterModel = {
      custId: 0,
      roleId: 0,
      fromDate: DateHelper.formatMomentDate(fromDate, DateFormat.Date),
      toDate: DateHelper.formatMomentDate(toDate, DateFormat.Date),
      company: this.selectedCompany.join(','),
      sportType: this.selectedSportsType.join(','),
      parlayType: this.selectedParlayType.join(','),
    };

    return model;
  }

  private get isValid(): boolean {
    if (this.selectedCompany.length === 0) {
      this.toastService.activate(`Please select at least one company!`);
      return false;
    }

    if (this.selectedParlayType.length === 0) {
      this.toastService.activate(`Please select at least one parlay!`);
      return false;
    }

    if (this.selectedSportsType.length === 0) {
      this.toastService.activate(`Please select at least one sport!`);
      return false;
    }

    return true;
  }
}
