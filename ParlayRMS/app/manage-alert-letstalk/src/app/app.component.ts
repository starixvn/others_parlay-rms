import { Component, ElementRef, ViewChild } from '@angular/core';
import { FilterModel } from './filter/filter.model';
import { DataModel } from './grid/grid.model';
import { WebInfoModel } from './app.model';
import { FilterComponent } from './filter/filter.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  @ViewChild('appFilter') appFilter!: FilterComponent;
  title = 'manage-alert-letstalk';
  gridModel!: DataModel;
  webInfoModel: WebInfoModel;
  filterModel!: FilterModel;

  constructor(private readonly elementRef: ElementRef<HTMLElement>) {
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

  startCountdown(): void {
    this.appFilter.startCountdown();
  }
}
