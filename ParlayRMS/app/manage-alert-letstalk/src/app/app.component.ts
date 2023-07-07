import { Component, ElementRef } from '@angular/core';
import { FilterModel } from './filter/filter.model';
import { DataModel } from './grid/grid.model';
import { AppService } from './app.service';
import { WebInfoModel } from './app.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'manage-alert-letstalk';
  gridModel!: DataModel;
  webInfoModel: WebInfoModel;
  filterModel!: FilterModel;

  constructor(private readonly elementRef: ElementRef<HTMLElement>,
    private readonly appService: AppService) {
      this.webInfoModel = {
        apiUrl: this.elementRef.nativeElement.getAttribute('api-url')??'',
        webUrl: this.elementRef.nativeElement.getAttribute('web-url')??'',
        userId: this.elementRef.nativeElement.getAttribute('userid')??'-1',
        userName: this.elementRef.nativeElement.getAttribute('username')??''
      }
  }

  generate(filterModel: FilterModel): void {
    this.filterModel = filterModel;
  }
}
