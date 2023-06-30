import { ApplicationRef, Component, ComponentFactoryResolver, ComponentRef, EmbeddedViewRef, Injector, Input, OnChanges, Renderer2, SimpleChanges } from '@angular/core';
import { BetDetails, DataModel, DetailDataComponent, TakeActionModel } from './grid.model';
import { AlertTypeConstants } from '../common/constants';
import { SimilarTicketComponent } from './similar-ticket/similar-ticket.component';
import { GridService } from './grid.service';
import { PotentialWinningComponent } from './potential-winning/potential-winning.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LogComponent } from './log/log.component';
import { ItemModel } from '../filter/filter.model';
import { TreeviewConfig } from 'ngx-treeview';
import * as _ from 'lodash';
import { BetDetailsComponent } from './bet-details/bet-details.component';
import { WebInfoModel } from '../app.model';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss']
})
export class GridComponent implements OnChanges {
  @Input() data: DataModel[] = [];
  @Input() webInfoModel!: WebInfoModel;

  body!: SafeHtml;
  componentsRef: ComponentRef<DetailDataComponent>[] = [];
  actionItems: ItemModel[] = [];
  itemConfig = TreeviewConfig.create({
    hasAllCheckBox: true,
    hasCollapseExpand: false,
    maxHeight: 500
  });
  selectedAction: string = '0';
  checkedBy: boolean = true;

  constructor(
    private readonly renderer: Renderer2,
    private readonly componentFactoryResolver: ComponentFactoryResolver,
    private readonly injector: Injector,
    private readonly applicationRef: ApplicationRef,
    private readonly gridService: GridService,
    private readonly modalService: NgbModal,
    private readonly sanitized: DomSanitizer
    ) { }

  ngOnChanges(changes: SimpleChanges): void {
    this.destroyComponents();
    this.actionItems = this.gridService.getActions();
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

  showLog(id: string, userName: string, betId: string): void {
    const modalRef = this.modalService.open(LogComponent, { backdrop: 'static', windowClass: 'modal-auto-size' });
    const componentInstance: LogComponent = modalRef.componentInstance;
    this.gridService.getLogs(this.webInfoModel.apiUrl, id).subscribe(data => componentInstance.data = data);
    componentInstance.user = userName;
    componentInstance.betId = betId;
  }

  updateCheckedUser(data: DataModel): void {
    const model: TakeActionModel = {
      actionId: Number(_.find(this.actionItems, { name: data.action })?.id),
      alertId: data.alertId,
      userId: Number(this.webInfoModel.userId),
      isChecked: true
    };
    this.gridService.takeAction(this.webInfoModel.apiUrl, model).subscribe(() => data.checkedBy = this.webInfoModel.userName);
  }

  writeActionLog(data: DataModel): void {
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

  public showBetDetails(data: DataModel): void {
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

  public showDetailData(index: number, id: string, type: string): void {
    this.destroyComponents();
    let newEle!: HTMLElement;
    if (type === AlertTypeConstants.SIMILAR_BET) {
      newEle = this.renderHtmlForSimilarTickets(id);
    }
    if (type === AlertTypeConstants.POTENTIAL_WINNING) {
      newEle = this.renderHtmlForPotentialWinning(id);
    }
    const element = this.renderer.selectRootElement(`tr#detail-data-${index}`, true) as HTMLElement;
    this.renderer.insertBefore(element.parentNode, newEle, element.nextSibling);
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
    this.gridService.getSimilarTickets(this.webInfoModel.apiUrl, id).subscribe(data => component.instance.data = data);
    this.applicationRef.attachView(component.hostView);
    const element = (component.hostView as EmbeddedViewRef<any>).rootNodes[0] as HTMLElement;
    return element;
  }

  private renderHtmlForPotentialWinning(id: string): HTMLElement {
    const factory = this.componentFactoryResolver.resolveComponentFactory(PotentialWinningComponent);
    const component = factory.create(this.injector);
    this.componentsRef.push(component);
    component.instance.webInfoModel = this.webInfoModel;
    this.gridService.getPotentialWinning(this.webInfoModel.apiUrl, id).subscribe(data => component.instance.data = data);
    this.applicationRef.attachView(component.hostView);
    const element = (component.hostView as EmbeddedViewRef<any>).rootNodes[0] as HTMLElement;
    return element;
  }

  private destroyComponents(): void {
    this.componentsRef.forEach(component => {
      component.destroy();
    });
  }
}
