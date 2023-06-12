import { Component, Input, OnInit } from '@angular/core';
import { DataModel } from './grid.model';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss']
})
export class GridComponent implements OnInit {
  @Input() data: DataModel[] = [];

  constructor() { }

  ngOnInit(): void {
  }

}
