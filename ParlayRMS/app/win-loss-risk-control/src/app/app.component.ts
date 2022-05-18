import { Component, ElementRef } from '@angular/core';
import { FilterModel } from './filter/filter.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  filterModel: FilterModel;
  userId: string;
  netcoreApiUrl: string;
  constructor(
    private readonly elementRef: ElementRef<HTMLElement>
  ) {
    this.netcoreApiUrl = this.elementRef.nativeElement.getAttribute('netcore-api-url');
    this.userId = this.elementRef.nativeElement.getAttribute('user-id')??'-1';
  }

  generate(filterModel: FilterModel) {
    this.filterModel = filterModel;
  }
}
