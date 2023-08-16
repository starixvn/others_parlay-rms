import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FilterModel, ItemModel, SportModel } from './filter.model';
import { TreeviewConfig } from 'ngx-treeview';
import { DateHelper, DateFormat } from 'nap-components/date';
import { CONFIG } from '../common/config';
import * as moment from 'moment';
import { FilterService } from './filter.service';
import { ToastService } from 'nap-components/toast';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent implements OnInit {
  @Input() apiUrl: string = '';
  @Output() generated = new EventEmitter<FilterModel>();

  alertTypeItems: ItemModel[] = [];
  selectedAlertTypes:string[] = [];
  member: string = '';
  checkedBy: string = '';
  chkNone: boolean = false;
  actionItems: ItemModel[] = [];
  selectedActions: string[] = [];
  ticketStatusItems: ItemModel[] = [];
  selectedTicketStatuses: string[] = [];
  comboItems: ItemModel[] = [];
  selectedCombos: string[] = [];
  odds: string = '';
  sportItems: SportModel[] = [];
  selectedSport: string[] = [];
  itemConfig = TreeviewConfig.create({
    hasAllCheckBox: true,
    hasCollapseExpand: false,
    maxHeight: 500
  });

  refreshItems: ItemModel[] = [];
  selectedRefresh: string = '60';
  checkedRefresh: boolean = false;
  isClickedSubmit: boolean = false;

  fromDate = DateHelper.convertMomentToDateStruct(CONFIG.date.yesterday);
  toDate = DateHelper.convertMomentToDateStruct(CONFIG.date.today);

  minStartDate = DateHelper.convertMomentToDateStruct(CONFIG.date.today.add(-1, 'month'));
  maxEndDate = DateHelper.convertMomentToDateStruct(CONFIG.date.today);

  timer = '60'
  seconds = 60;
  selectedSeconds = 60;
  intervalId = -1;

  constructor(private filterService: FilterService, private toastService: ToastService) { }

  ngOnInit(): void {
    this.alertTypeItems = this.filterService.getAlertTypes();
    this.actionItems = this.filterService.getActions();
    this.ticketStatusItems = this.filterService.getTicketStatuses();
    this.refreshItems = this.filterService.getRefreshs();
    this.comboItems = this.filterService.getCombos();
    this.filterService.getSports(this.apiUrl).subscribe(data => this.sportItems = data);
  }

  getData(): void {
    if (!this.isValid()) {
      return;
    }
    const model = this.createQueryModel();
    this.generated.emit(model);
  }

  startCountdown(): void {
    this.clearTimer();
    this.startTimer();
    this.isClickedSubmit = true;
    this.checkedRefresh = false;
  }

  pauseTimer(): void {
    if (this.checkedRefresh) {
      this.selectedSeconds = Number(this.timer);
      this.clearTimer();
    } else {
      this.startTimer();
    }
  }

  changeRefreshnow(): void {
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

  onFromDateChange() {
    let fromDate: moment.Moment;
    fromDate = DateHelper.convertDateStructToMoment(this.fromDate);
    fromDate = moment.utc(fromDate);
    this.fromDate = DateHelper.convertMomentToDateStruct(fromDate);
  }

  onToDateChange() {
    let toDate: moment.Moment;
    toDate = DateHelper.convertDateStructToMoment(this.toDate);
    toDate = moment.utc(toDate);
    this.toDate = DateHelper.convertMomentToDateStruct(toDate);
  }

  private createQueryModel(): FilterModel {
    const fromDate = DateHelper.convertDateStructToMoment(this.fromDate);
    const toDate = DateHelper.convertDateStructToMoment(this.toDate);

    return {
      fromDate: DateHelper.formatMomentDate(fromDate, DateFormat.Date),
      toDate: DateHelper.formatMomentDate(toDate, DateFormat.Date),
      actionType: this.selectedActions.length === this.actionItems.length ? '-1' : this.selectedActions.join(','),
      alertType: this.selectedAlertTypes.length === this.alertTypeItems.length ? '-1' : this.selectedAlertTypes.join(','),
      checkBy: this.chkNone ? '' : this.checkedBy,
      isNoChecked: this.chkNone,
      member: this.member,
      ticketStatus: this.selectedTicketStatuses.length === this.ticketStatusItems.length ? '-1' : this.selectedTicketStatuses.join(','),
      pageIndex: 1,
      pageSize: 100
    };
  }

  private isValid(): boolean {
    if (this.selectedAlertTypes.length === 0) {
      this.toastService.activate(`Please select at least one Alert type!`);
      return false;
    }

    if (this.selectedActions.length === 0) {
      this.toastService.activate(`Please select at least one Action!`);
      return false;
    }

    if (this.selectedTicketStatuses.length === 0) {
      this.toastService.activate(`Please select at least one Ticket Status!`);
      return false;
    }

    return true;
  }

  private startTimer(): void {
    this.timer = this.selectedSeconds.toString();
    this.intervalId = window.setInterval(() => {
      this.selectedSeconds--;
      if (this.selectedSeconds === 0) {
        this.getData();
        this.selectedSeconds = Number(this.selectedRefresh);
      }
      this.timer = this.selectedSeconds.toString();
    }, 1000)
  }

  private clearTimer(): void {
    window.clearInterval(this.intervalId);
  }
}