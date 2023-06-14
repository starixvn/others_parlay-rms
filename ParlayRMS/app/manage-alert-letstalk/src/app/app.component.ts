import { Component, ElementRef } from '@angular/core';
import { FilterModel } from './filter/filter.model';
import { DataModel } from './grid/grid.model';
import { AppService } from './app.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'manage-alert-letstalk';
  apiUrl: string;
  webUrl: string;
  gridModel: DataModel[] = [];
  filterModel!: FilterModel;

  constructor(private readonly elementRef: ElementRef<HTMLElement>,
    private readonly appService: AppService) {
    this.apiUrl = this.elementRef.nativeElement.getAttribute('api-url')??'';
    this.webUrl = this.elementRef.nativeElement.getAttribute('web-url')??'';
  }

  generate(filterModel: FilterModel): void {
    this.filterModel = filterModel;
    this.appService.getData(filterModel).subscribe(data => this.gridModel = data);
  }
}
