import { Component, Input, OnInit } from '@angular/core';
import { PotentialWinningModel } from './potential-winning.model';
import { DetailDataComponent } from '../grid.model';

@Component({
  selector: 'app-potential-winning',
  templateUrl: './potential-winning.component.html',
  styleUrls: ['./potential-winning.component.scss']
})
export class PotentialWinningComponent implements DetailDataComponent {
  @Input() data: PotentialWinningModel[] = [];
}
