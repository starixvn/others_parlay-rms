import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FilterModel, ItemModel } from './filter.model';
import { TreeviewConfig } from 'ngx-treeview';
import { CountdownComponent, CountdownConfig, CountdownEvent } from 'ngx-countdown';
import { DateHelper, DateFormat } from 'nap-components/date';
import { CONFIG } from '../common/config';
import * as moment from 'moment';
import { FilterService } from './filter.service';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent implements OnInit {
  @Input() apiUrl: string = '';
  @Output() generated = new EventEmitter<FilterModel>();
  @ViewChild('countdown', { static: false }) countdown!: CountdownComponent;

  alertTypeItems: ItemModel[] = [];
  selectedAlertTypes:string[] = [];
  member: string = '';
  checkedBy: string = '';
  chkNone: boolean = false;
  actionItems: ItemModel[] = [];
  selectedActions: string[] = [];
  ticketStatusItems: ItemModel[] = [];
  selectedTicketStatuses: string[] = [];
  itemConfig = TreeviewConfig.create({
    hasAllCheckBox: true,
    hasCollapseExpand: false,
    maxHeight: 500
  });

  refreshItems: ItemModel[] = [];
  selectedRefresh: string = '60';
  checkedRefresh: boolean = false;
  isClickedSubmit: boolean = false;
  countdownConfig: CountdownConfig = {
    leftTime: 60,
    formatDate: ({ date }) => `${date / 1000}`,
    demand: true,
  };

  fromDate = DateHelper.convertMomentToDateStruct(CONFIG.date.today);
  toDate = DateHelper.convertMomentToDateStruct(CONFIG.date.today);

  minStartDate = DateHelper.convertMomentToDateStruct(CONFIG.date.today.add(-1, 'month'));
  maxEndDate = DateHelper.convertMomentToDateStruct(CONFIG.date.today);

  constructor(private filterService: FilterService) { }

  ngOnInit(): void {
    this.alertTypeItems = this.filterService.getAlertTypes();
    this.actionItems = this.filterService.getActions();
    this.ticketStatusItems = this.filterService.getTicketStatuses();
    this.refreshItems = this.filterService.getRefreshs();
  }

  private createModelCountdown(time: number, demand: boolean): void {
    this.countdownConfig = {
      leftTime: time,
      formatDate: ({ date }) => `${date / 1000}`,
      demand: demand
    };
  }

  changeRefreshnow(): void {
    this.createModelCountdown(Number(this.selectedRefresh), false);
    this.countdown.restart();
    this.checkedRefresh = false;
  }

  pauseCountdown(): void {
    this.checkedRefresh ? this.countdown.pause() : this.countdown.resume();
  }

  handleCountdown(e: CountdownEvent) {
    if (e.left === 0) {
      this.getData();
      this.countdown.restart()
    }
  }

  getData(): void {
    this.createModelCountdown(Number(this.selectedRefresh), false);
    this.isClickedSubmit = true;
    this.checkedRefresh = false;
    const model = this.createQueryModel();
    this.generated.emit(model);
  }

  onFromDateChange() {
    let fromDate: moment.Moment;
    fromDate = DateHelper.convertDateStructToMoment(this.fromDate);
    fromDate = moment.utc(fromDate);
    this.fromDate = DateHelper.convertMomentToDateStruct(fromDate);

    const maxEndDate = fromDate.add(1, 'month');
    this.maxEndDate = DateHelper.convertMomentToDateStruct(maxEndDate);
  }

  onToDateChange() {
    let toDate: moment.Moment;
    toDate = DateHelper.convertDateStructToMoment(this.toDate);
    toDate = moment.utc(toDate);
    this.toDate = DateHelper.convertMomentToDateStruct(toDate);

    const minStartDate = toDate.subtract(1, 'month');
    this.minStartDate = DateHelper.convertMomentToDateStruct(minStartDate);
  }

  private createQueryModel(): FilterModel {
    return {
      fromDate: '',
      toDate: '',
      action: '',
      alertTypes: '',
      checkedBy: '',
      isNone: false,
      member: '',
      ticketStatus: ''
    };
  }
}
