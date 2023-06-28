import { ApplicationRef, Component, ComponentFactoryResolver, ComponentRef, EmbeddedViewRef, Injector, Input, OnChanges, Renderer2, SimpleChanges } from '@angular/core';
import { DataModel, DetailDataComponent } from './grid.model';
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

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss']
})
export class GridComponent implements OnChanges {
  @Input() data: DataModel[] = [];
  @Input() webInfoModel!: WebInfoModel;

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
    private readonly modalService: NgbModal
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

  showData(index: number, id: string, type: string): void{
    if (this.isDisplayDetailData(type)) {
      this.showDetailData(index, id, type);
    } else {
      this.showBetDetails();
    }
  }

  showLog(id: string, userName: string, betId: string): void {
    const modalRef = this.modalService.open(LogComponent, { backdrop: 'static', windowClass: 'modal-auto-size' });
    const componentInstance: LogComponent = modalRef.componentInstance;
    this.gridService.getLogs(this.webInfoModel.apiUrl, id).subscribe(data => componentInstance.data = data);
    componentInstance.user = userName;
    componentInstance.betId = betId;
  }

  updateCheckedUser(): void {
    console.log('abc');
  }

  private showBetDetails(): void {
    const modalRef = this.modalService.open(BetDetailsComponent, { backdrop: 'static', windowClass: 'modal-auto-size' });
    const componentInstance: BetDetailsComponent = modalRef.componentInstance;
    this.gridService.getBetDetail().subscribe(data => componentInstance.data = data);
    componentInstance.userName = 'HAIFATB011706';
    componentInstance.transId = '123456789';
  }

  private showDetailData(index: number, id: string, type: string): void {
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
