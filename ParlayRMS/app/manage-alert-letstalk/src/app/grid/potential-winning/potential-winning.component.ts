import { Component, Input } from '@angular/core';
import { PotentialWinningModel } from './potential-winning.model';
import { BetDetails, DetailDataComponent } from '../grid.model';
import { GridService } from '../grid.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { BetDetailsComponent } from '../bet-details/bet-details.component';
import { WebInfoModel } from 'src/app/app.model';

@Component({
  selector: 'app-potential-winning',
  templateUrl: './potential-winning.component.html',
  styleUrls: ['./potential-winning.component.scss']
})
export class PotentialWinningComponent implements DetailDataComponent {
  @Input() data: PotentialWinningModel[] = [];
  @Input() webInfoModel!: WebInfoModel;
  @Input() alertId: string = '0';
  @Input() combos: string = '';
  @Input() odds?: number;

  body!: SafeHtml;

  constructor(
    private readonly gridService: GridService,
    private readonly modalService: NgbModal,
    private readonly sanitized: DomSanitizer
  ){}

  public showBetDetails(data: PotentialWinningModel): void {
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

  openNewTabOfSimilarBet(betId: number) {
    window.open(`${this.webInfoModel.webUrl}SimilarBets?betid=${betId}`, '_blank');
  }

  openNewTabOfIpInfo(ip: string, date: string) {
    window.open(`${this.webInfoModel.webUrl}ViewIPInfo?ip=${ip}&date=${date}`, '_blank');
  }

  exportExcel(): void{
    this.gridService.exportExcelForPotentialWinning(this.webInfoModel.apiUrl, this.alertId, this.combos, this.odds);
  }
}
