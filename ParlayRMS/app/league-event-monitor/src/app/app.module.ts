import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from 'nap-components/http';
import { TreeviewModule } from 'ngx-treeview';

import { AppComponent } from './app.component';
import { FilterComponent } from './filter/filter.component';
import { ItemTreeviewPipe } from './filter/item-treeview.pipe';
import { LeagueTreeviewPipe } from './filter/league-treeview.pipe';
import { LeagueComponent } from './league/league.component';
import { EventComponent } from './event/event.component';
import { NumberFormatModule } from 'nap-components/number-format';
import { SpinnerModule } from 'nap-components/spinner';
import { ToastModule, ToastService } from 'nap-components/toast';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CustomHttpRequestInterceptor } from './custom-httprequest-interceptor';
import { AppService } from './app.service';
import { BettypeTreeviewPipe } from './filter/bettype-treeview.pipe';
import { EventDetailComponent } from './event-detail/event-detail.component';
import { SortDirective } from './directive/sort.directive';

@NgModule({
  declarations: [
    AppComponent,
    FilterComponent,
    ItemTreeviewPipe,
    LeagueTreeviewPipe,
    LeagueComponent,
    EventComponent,
    BettypeTreeviewPipe,
    EventDetailComponent,
    SortDirective
  ],
  imports: [
    BrowserModule,
    TreeviewModule.forRoot(),
    HttpModule.forRoot(),
    FormsModule,
    NumberFormatModule,
    SpinnerModule,
    ToastModule
  ],
  providers: [
    [
        { provide: HTTP_INTERCEPTORS, useClass: CustomHttpRequestInterceptor, multi: true }
    ],
    ToastService,
    AppService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
