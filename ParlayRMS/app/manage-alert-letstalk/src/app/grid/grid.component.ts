import { ApplicationRef, Component, ComponentFactoryResolver, ComponentRef, EmbeddedViewRef, EventEmitter, Injector, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { AlertDetailModel, BetDetails, DataModel, DataModelItem, DetailDataComponent, TakeActionModel } from './grid.model';
import { AlertTypeConstants, PotentialWinningLevelConstants } from '../common/constants';
import { SimilarTicketComponent } from './similar-ticket/similar-ticket.component';
import { GridService } from './grid.service';
import { PotentialWinningComponent } from './potential-winning/potential-winning.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LogComponent } from './log/log.component';
import { FilterModel, ItemModel } from '../filter/filter.model';
import { TreeviewConfig } from 'ngx-treeview';
import * as _ from 'lodash';
import { BetDetailsComponent } from './bet-details/bet-details.component';
import { WebInfoModel } from '../app.model';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { PaginationService as NgxPaginationService, PaginatePipe } from 'ngx-pagination';
import { PaginationInstance, PaginationService } from 'nap-components/pagination';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss']
})
export class GridComponent implements OnChanges {
  @Input() filterModel!: FilterModel;
  @Input() webInfoModel!: WebInfoModel;
  @Output() onGetData = new EventEmitter();

  data!: DataModel;
  displayedData: DataModelItem[] = [];
  isPageChanged = false;
  originalPageSize = 100;
  body!: SafeHtml;
  componentsRef: ComponentRef<DetailDataComponent>[] = [];
  actionItems: ItemModel[] = [];
  itemConfig = TreeviewConfig.create({
    hasAllCheckBox: true,
    hasCollapseExpand: false,
    maxHeight: 500
  });
  checkedBy: boolean = true;
  paginationInstance!: PaginationInstance;
  private paginatePipe: PaginatePipe;
  cachedAlertId = '';

  constructor(
    private readonly renderer: Renderer2,
    private readonly componentFactoryResolver: ComponentFactoryResolver,
    private readonly injector: Injector,
    private readonly applicationRef: ApplicationRef,
    private readonly gridService: GridService,
    private readonly modalService: NgbModal,
    private readonly sanitized: DomSanitizer,
    private readonly paginationService: PaginationService,
    private ngxPaginationService: NgxPaginationService
    ) {
      this.paginatePipe = new PaginatePipe(this.ngxPaginationService);
    }

  ngOnChanges(changes: SimpleChanges): void {
    this.destroyComponents();
    this.actionItems = this.gridService.getActions();
    if (this.filterModel != null) {
      this.gridService.getAlerts(this.webInfoModel.apiUrl, this.filterModel).subscribe(data => {
        this.data = data;
        this.paginationInstance = this.paginationService.createPaginationInstance(this.data.totalItems);
        this.isPageChanged = false;
        this.onPageChange();
        this.onGetData.emit();
      })
    }
  }

  getColorViaAlertType(type: string): string {
    if (type === AlertTypeConstants.SIMILAR_BET) {
      return AlertTypeConstants.SIMILAR_BET_COLOR;
    }
    if (type === AlertTypeConstants.SMALL_ODDS) {
      return AlertTypeConstants.SMALL_ODDS_COLOR;
    }
    if (type === AlertTypeConstants.LAST_TICKET_PENDING) {
      return AlertTypeConstants.LAST_TICKET_PENDING_COLOR;
    }
    if (type === AlertTypeConstants.SMALL_LEAGUE_ODDS) {
      return AlertTypeConstants.SMALL_LEAGUE_ODDS_COLOR;
    }
    return AlertTypeConstants.POTENTIAL_WINNING_COLOR;
  }

  getColorViaSportType(sportType: string, alertType: string): string {
    if (alertType === AlertTypeConstants.POTENTIAL_WINNING) {
      if (sportType.toLowerCase().includes('red')) {
        return PotentialWinningLevelConstants.RED;
      }
      if (sportType.toLowerCase().includes('orange')) {
        return PotentialWinningLevelConstants.ORANGE;
      }
      if (sportType.toLowerCase().includes('yellow')) {
        return PotentialWinningLevelConstants.YELLOW;
      }
    }
    return PotentialWinningLevelConstants.WHITE;
  }

  openNewTabOfSimilarBet(betId: number) {
    window.open(`${this.webInfoModel.webUrl}SimilarBets?betid=${betId}`, '_blank');
  }

  openNewTabOfTotalOutstanding(user: string) {
    window.open(`${this.webInfoModel.webUrl}TotalOutstanding?username=${user}`);
  }

  isDisplayDetailData(type: string): boolean {
    if (type === AlertTypeConstants.SIMILAR_BET || type === AlertTypeConstants.POTENTIAL_WINNING) {
      return true;
    }

    return false;
  }

  showLog(id: string, alertType: string, userName: string, betId: string): void {
    const modalRef = this.modalService.open(LogComponent, { backdrop: 'static', windowClass: 'modal-auto-size' });
    const componentInstance: LogComponent = modalRef.componentInstance;
    this.gridService.getLogs(this.webInfoModel.apiUrl, id).subscribe(data => componentInstance.data = data);
    componentInstance.alertType = alertType;
    componentInstance.user = userName;
    componentInstance.betId = betId === null ? '' : ` - ${betId}`;
  }

  updateCheckedUser(data: DataModelItem): void {
    const model: TakeActionModel = {
      actionId: Number(_.find(this.actionItems, { name: data.action })?.id),
      alertId: data.alertId,
      userId: Number(this.webInfoModel.userId),
      isChecked: true
    };
    this.gridService.takeAction(this.webInfoModel.apiUrl, model).subscribe(() => data.checkedBy = this.webInfoModel.userName);
  }

  writeActionLog(data: DataModelItem): void {
    if (data.action !== _.first(this.actionItems)?.name) {
      const model: TakeActionModel = {
        actionId: Number(_.find(this.actionItems, { name: data.action })?.id),
        alertId: data.alertId,
        userId: Number(this.webInfoModel.userId),
        isChecked: false
      };
      this.gridService.takeAction(this.webInfoModel.apiUrl, model).subscribe();
    }
  }

  public showBetDetails(data: DataModelItem): void {
    const modalRef = this.modalService.open(BetDetailsComponent, {
      backdrop: 'static',
      windowClass: 'modal-bet-detail'
     });
    const componentInstance: BetDetailsComponent = modalRef.componentInstance;
    const model: BetDetails = {
      transId: data.betId,
      refNo: data.refNo,
      userName: data.userName,
      custId: data.custId,
      betType: data.betType
    }
    this.gridService.getBetDetail(model).subscribe((body) => {
      this.body = this.sanitized.bypassSecurityTrustHtml(body);
      componentInstance.body = this.body;
    });
  }

  public showDetailData(index: number, item: DataModelItem): void {
    this.destroyComponents();
    if (item.isViewedDetail && item.alertId === this.cachedAlertId) {
      item.isViewedDetail = false;
      return;
    }
    let newEle!: HTMLElement;
    if (item.alertType === AlertTypeConstants.SIMILAR_BET) {
      newEle = this.renderHtmlForSimilarTickets(item.alertId);
    }
    if (item.alertType === AlertTypeConstants.POTENTIAL_WINNING) {
      newEle = this.renderHtmlForPotentialWinning(item.alertId);
    }
    const element = this.renderer.selectRootElement(`tr#detail-data-${index}`, true) as HTMLElement;
    this.renderer.insertBefore(element.parentNode, newEle, element.nextSibling);
    item.isViewedDetail = true;
    this.cachedAlertId = item.alertId;
  }

  getCheckedBy(checkedBy: string): string {
    if (_.isEmpty(checkedBy)) {
      return 'None';
    }
    return checkedBy;
  }

  private renderHtmlForSimilarTickets(id: string): HTMLElement {
    const factory = this.componentFactoryResolver.resolveComponentFactory(SimilarTicketComponent);
    const component = factory.create(this.injector);
    this.componentsRef.push(component);
    const alertDetail: AlertDetailModel ={
      alertId: id,
      selection: this.filterModel.selection,
      odds: this.filterModel.odds,
      ticketStatus: this.filterModel.ticketStatus
    }
    component.instance.apiUrl = this.webInfoModel.apiUrl;
    component.instance.alertDetail = alertDetail;
    this.gridService.getSimilarTickets(this.webInfoModel.apiUrl, alertDetail).subscribe(data => component.instance.data = data);
    this.applicationRef.attachView(component.hostView);
    const element = (component.hostView as EmbeddedViewRef<any>).rootNodes[0] as HTMLElement;
    return element;
  }

  private renderHtmlForPotentialWinning(id: string): HTMLElement {
    const factory = this.componentFactoryResolver.resolveComponentFactory(PotentialWinningComponent);
    const component = factory.create(this.injector);
    this.componentsRef.push(component);
    const alertDetail: AlertDetailModel ={
      alertId: id,
      selection: this.filterModel.selection,
      odds: this.filterModel.odds,
      ticketStatus: this.filterModel.ticketStatus
    }
    component.instance.webInfoModel = this.webInfoModel;
    component.instance.alertDetail = alertDetail;
    this.gridService.getPotentialWinning(this.webInfoModel.apiUrl, alertDetail).subscribe(data => component.instance.data = data);
    this.applicationRef.attachView(component.hostView);
    const element = (component.hostView as EmbeddedViewRef<any>).rootNodes[0] as HTMLElement;
    return element;
  }

  private destroyComponents(): void {
    this.componentsRef.forEach(component => {
      component.destroy();
    });
  }

  public isNoAction(action: string): boolean {
    return action === 'No Action';
  }

  onPageChange() {
    this.createPaging();
  }

  private createPaging() {
    if (this.isPageChanged) {
        this.destroyComponents();
        this.paginationInstance.currentPage = this.paginationInstance.itemsPerPage === this.originalPageSize ?
         this.paginationInstance.currentPage : 1;
        this.filterModel.pageIndex = this.paginationInstance.currentPage;
        this.filterModel.pageSize = this.paginationInstance.itemsPerPage;

        this.gridService.getAlerts(this.webInfoModel.apiUrl, this.filterModel).subscribe(data => {
        this.data = data;
        this.createPagingTransform();
        });
    }

    this.createPagingTransform();
  }

  private createPagingTransform() {
    const paginateData = this.paginatePipe.transform(this.data.items, this.paginationInstance);
    this.displayedData = _.cloneDeep(paginateData);
    this.isPageChanged = true;
    this.originalPageSize = this.filterModel.pageSize;
  }

  removeDetailComponent(): void {
    this.destroyComponents();
  }

  getIndex(index: number): number {
    return index + (this.paginationInstance.currentPage - 1) * this.paginationInstance.itemsPerPage;
  }

  exportExcel(): void {
    this.gridService.exportExcel(this.webInfoModel.apiUrl, this.filterModel);
  }
}
