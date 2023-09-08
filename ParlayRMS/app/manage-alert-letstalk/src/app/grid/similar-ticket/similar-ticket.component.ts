import { Component, Input } from '@angular/core';
import { SimilarTicketModel } from './similar-ticket.model';
import { BetDetails, DetailDataComponent } from '../grid.model';
import { GridService } from '../grid.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { BetDetailsComponent } from '../bet-details/bet-details.component';

@Component({
  selector: 'app-similar-ticket',
  templateUrl: './similar-ticket.component.html',
  styleUrls: ['./similar-ticket.component.scss']
})
export class SimilarTicketComponent implements DetailDataComponent {
  @Input() data: SimilarTicketModel[] = [];
  @Input() alertId: string = '0';
  @Input() apiUrl: string = '';
  @Input() combos: string = '';
  @Input() odds?: number;

  body!: SafeHtml;

  constructor(
    private readonly gridService: GridService,
    private readonly modalService: NgbModal,
    private readonly sanitized: DomSanitizer
  ){}

  public showBetDetails(data: SimilarTicketModel): void {
    const modalRef = this.modalService.open(BetDetailsComponent, {
      backdrop: 'static',
      windowClass: 'modal-bet-detail'
     });
    const componentInstance: BetDetailsComponent = modalRef.componentInstance;
    const model: BetDetails = {
      transId: data.transId,
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
  
  exportExcel(): void {
    this.gridService.exportExcelForSimilarBet(this.apiUrl, this.alertId, this.combos, this.odds);
  }
}
