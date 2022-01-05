import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { EventGroupModel, EventModel } from './event.model';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss']
})
export class EventComponent implements OnInit {
  @Input() data: EventGroupModel[] = [];
  @Input() isClickedOnleague: boolean = false;
  @Output() showBackLeague = new EventEmitter();
  constructor() { }

  ngOnInit(): void {
  }

  public backToLeague(): void {
    this.showBackLeague.emit();
  }

  public isNormalColumn(model: EventModel): boolean {
    return model.homeName !== null && model.homeName !== 'Others' && model.homeName !== 'Total';
  }
}
