import { Component, Input } from '@angular/core';
import { SimilarTicketModel } from './similar-ticket.model';
import { DetailDataComponent } from '../grid.model';

@Component({
  selector: 'app-similar-ticket',
  templateUrl: './similar-ticket.component.html',
  styleUrls: ['./similar-ticket.component.scss']
})
export class SimilarTicketComponent implements DetailDataComponent {
  @Input() data: SimilarTicketModel[] = [];
}
