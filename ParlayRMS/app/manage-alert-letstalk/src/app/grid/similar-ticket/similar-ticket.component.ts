import { Component, Input, OnInit } from '@angular/core';
import { SimilarTicketModel } from './similar-ticket.model';

@Component({
  selector: 'app-similar-ticket',
  templateUrl: './similar-ticket.component.html',
  styleUrls: ['./similar-ticket.component.scss']
})
export class SimilarTicketComponent implements OnInit {
  @Input() data: SimilarTicketModel[] = [];

  constructor() { }

  ngOnInit(): void {
  }

}
