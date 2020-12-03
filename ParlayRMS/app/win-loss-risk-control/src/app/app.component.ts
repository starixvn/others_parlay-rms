import { Component, ElementRef } from '@angular/core';
import { FilterModel } from './filter/filter.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  filterModel: FilterModel;
  netcoreApiUrl: string;
  constructor(
    private readonly elementRef: ElementRef<HTMLElement>
  ) {
    this.netcoreApiUrl = this.elementRef.nativeElement.getAttribute('netcore-api-url');
  }

  generate(filterModel: FilterModel) {
    this.filterModel = filterModel;
  }
}
