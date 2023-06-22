import { ApplicationRef, Component, ComponentFactoryResolver, EmbeddedViewRef, Injector, Input, OnInit, Renderer2 } from '@angular/core';
import { DataModel } from './grid.model';
import { AlertTypeConstants } from '../common/constants';
import { SimilarTicketComponent } from './similar-ticket/similar-ticket.component';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss']
})
export class GridComponent implements OnInit {
  @Input() data: DataModel[] = [];
  @Input() webUrl: string = '';

  constructor(
    private readonly renderer: Renderer2,
    private readonly componentFactoryResolver: ComponentFactoryResolver,
    private readonly injector: Injector,
    private readonly applicationRef: ApplicationRef
    ) { }

  ngOnInit(): void {
    
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
    window.open(`${this.webUrl}SimilarBets?betid=${betId}`, '_blank');
  }

  openNewTabOfTotalOutstanding(user: string) {
    window.open(`${this.webUrl}TotalOutstanding?username=${user}`);
  }

  isDisplayDetailData(type: string): boolean {
    if (type === AlertTypeConstants.SIMILAR_BET || type === AlertTypeConstants.POTENTIAL_WINNING) {
      return true;
    }

    return false;
  }

  viewDetailData(id: number): void{
    const element = this.renderer.selectRootElement(`tr#detail-data-${id}`, true) as HTMLElement;
    const test = this.renderer.createElement('tr') as HTMLElement;
    const newEle = this.renderHtmlForSimilarTickets();
    this.renderer.insertBefore(element.parentNode, newEle, element.nextSibling);
  }

  renderHtmlForSimilarTickets(): HTMLElement {
    const factory = this.componentFactoryResolver.resolveComponentFactory(SimilarTicketComponent);
    const component = factory.create(this.injector);
    this.applicationRef.attachView(component.hostView);
    const element = (component.hostView as EmbeddedViewRef<any>).rootNodes[0] as HTMLElement;
    return element;
  }
}
