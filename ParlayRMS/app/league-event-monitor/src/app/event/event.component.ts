import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss']
})
export class EventComponent implements OnInit {

  @Output() showBackLeague = new EventEmitter();
  constructor() { }

  ngOnInit(): void {
  }

  public backToLeague(): void {
    this.showBackLeague.emit();
  }
}
