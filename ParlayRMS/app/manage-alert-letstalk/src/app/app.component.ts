import { Component, ElementRef } from '@angular/core';
import { FilterModel } from './filter/filter.model';
import { DataModel } from './grid/grid.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'manage-alert-letstalk';
  netcoreApiUrl: string;
  gridModel: DataModel[] = [];
  filterModel!: FilterModel;

  constructor(private readonly elementRef: ElementRef<HTMLElement>) {
    this.netcoreApiUrl = this.elementRef.nativeElement.getAttribute('netcore-api-url')??'';
  }

  generate(filterModel: FilterModel): void {
    this.filterModel = filterModel;
    this.gridModel = [];
  }
}
