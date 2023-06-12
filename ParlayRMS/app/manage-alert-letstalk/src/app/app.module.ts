import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { FilterComponent } from './filter/filter.component';
import { TreeviewModule } from 'ngx-treeview';
import { HttpModule } from 'nap-components/http';
import { FormsModule } from '@angular/forms';
import { CountdownModule } from 'ngx-countdown';
import { SpinnerModule } from 'nap-components/spinner';
import { ToastModule, ToastService } from 'nap-components/toast';
import { ItemTreeviewPipe } from './filter/item-treeview.pipe';
import { DateModule } from 'nap-components/date';
import * as moment from 'moment';
import { AppService } from './app.service';
import { CONFIG, DateConfig } from './common/config';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CustomHttpRequestInterceptor } from './custom-httprequest-interceptor';
import { GridComponent } from './grid/grid.component';
import { NumberFormatModule } from 'nap-components/number-format';

@NgModule({
  declarations: [
    AppComponent,
    FilterComponent,
    ItemTreeviewPipe,
    GridComponent
  ],
  imports: [
    BrowserModule,
    TreeviewModule.forRoot(),
    HttpModule.forRoot(),
    DateModule,
    FormsModule,
    CountdownModule,
    NumberFormatModule,
    SpinnerModule,
    ToastModule
  ],
  providers: [
    [
      { provide: HTTP_INTERCEPTORS, useClass: CustomHttpRequestInterceptor, multi: true }
    ],
    ToastService,
    AppService,
    {
      provide: APP_INITIALIZER,
      useFactory: initConfiguration,
      deps: [AppService],
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function initConfiguration(service: AppService) {
  return () => new Promise(resolve => {
    service.getToday().subscribe(model => {
      const today = moment.utc(model);
      CONFIG.date = new DateConfig(today);
      resolve(true);
    });
  });
}
