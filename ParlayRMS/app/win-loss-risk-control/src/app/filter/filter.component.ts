import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import * as moment from 'moment';
import { CONFIG } from '../common/config';
import { FilterModel, SelectItem } from './filter.model';
import { FilterService } from './filter.service';
import { DateHelper, DateFormat } from 'nap-components/date';
import { MessageType, ToastService } from 'nap-components/toast';
import { Company, Sport } from './filter.model';
import { TreeviewConfig } from 'ngx-treeview';
import * as _ from 'lodash';
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
  @Input() userId: string = '-1';
  @Output() generated = new EventEmitter<FilterModel>();
  public readonly favoriteCompany: string = 'favoriteCompany';
  public company: Company[];
  public companyConfig = TreeviewConfig.create({
    hasFilter: true,
    hasAllCheckBox: true,
    hasCollapseExpand: false,
    maxHeight: 500
  });
  public selectedCompany: string[] = [];

  public parlayType: SelectItem[];
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

  public comboType: SelectItem[];
  public comboTypeConfig = TreeviewConfig.create({
    hasFilter: false,
    hasAllCheckBox: true,
    hasCollapseExpand: false,
    maxHeight: 250
  });
  public selectedcomboType: string[] = [];

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
    this.getCombo();
  }

  private getCompany() {
    this.filterService.getCompany(this.netcoreApiUrl).subscribe((company: Company[]) => this.initCompany(company));
  }

  private getSports() {
    this.filterService.getSports(this.netcoreApiUrl).subscribe((sports: Sport[]) => this.sportsType = sports);
  }

  private getParlay() {
    this.filterService.getParlay(this.netcoreApiUrl).subscribe((parlay: SelectItem[]) => this.parlayType = parlay);
  }

  private getCombo() {
    this.filterService.getCombo(this.netcoreApiUrl).subscribe((combo: SelectItem[]) => this.comboType = combo);
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
      selection: this.selectedcomboType.join(',')
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

    if (this.selectedcomboType.length === 0) {
      this.toastService.activate(`Please select at least one combo!`);
      return false;
    }

    return true;
  }

  private initCompany(company: Company[]): void {
    const cachedCompanyString = localStorage.getItem(`${this.favoriteCompany}-${this.userId}`);
    let cachedCompany: string[] = [];
    if (cachedCompanyString !== null) {
      cachedCompany = cachedCompanyString.split(',');
      _.forEach(company, x => {
        if (_.indexOf(cachedCompany, x.site) > -1) {
          x.checked = true;
        } else {
          x.checked = false;
        }
      })
    }
    this.company = company;
  }

  public addFavorite(): void {
    const companyList = this.selectedCompany.join(',');
    if (companyList === '' || companyList === null)
    {
      localStorage.removeItem(`${this.favoriteCompany}-${this.userId}`);
    } else {
      localStorage.setItem(`${this.favoriteCompany}-${this.userId}`, companyList);
    }
    this.toastService.activate('Added', MessageType.Success, 3);
  }
}
