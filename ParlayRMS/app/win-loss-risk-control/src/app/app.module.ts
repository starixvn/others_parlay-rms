import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FilterComponent } from './filter/filter.component';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { ToastService, ToastModule } from 'nap-components/toast';
import { DateModule } from 'nap-components/date';
import { NavbarModule } from 'nap-components/navbar';
import { SpinnerModule } from 'nap-components/spinner';
import { HttpModule } from 'nap-components/http';
import { TreeviewModule } from 'ngx-treeview';
import { CompanyTreeviewPipe } from './filter/company-treeview.pipe';
import { SportTreeviewPipe } from './filter/sport-treeview.pipe';
import { ParlayTreeviewPipe } from './filter/parlay-treeview.pipe';
import { GridModule } from './grid/grid.module';
import * as moment from 'moment';
import { AppService } from './app.service';
import { CONFIG, DateConfig } from './common/config';

@NgModule({
  declarations: [
    AppComponent,
    FilterComponent,
    CompanyTreeviewPipe,
    SportTreeviewPipe,
    ParlayTreeviewPipe
  ],
  imports: [
    BrowserModule,
    FormsModule,
    DateModule,
    HttpModule,
    NavbarModule,
    SpinnerModule,
    GridModule,
    ToastModule,
    TreeviewModule.forRoot()
  ],
  providers: [
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
