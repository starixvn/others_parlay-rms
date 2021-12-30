import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from 'nap-components/http';
import { CountdownModule } from 'ngx-countdown';
import { TreeviewModule } from 'ngx-treeview';

import { AppComponent } from './app.component';
import { FilterComponent } from './filter/filter.component';
import { ItemTreeviewPipe } from './filter/item-treeview.pipe';
import { LeagueTreeviewPipe } from './filter/league-treeview.pipe';
import { LeagueComponent } from './league/league.component';
import { EventComponent } from './event/event.component';
import { NumberFormatModule } from 'nap-components/number-format';
import { SpinnerModule } from 'nap-components/spinner';

@NgModule({
  declarations: [
    AppComponent,
    FilterComponent,
    ItemTreeviewPipe,
    LeagueTreeviewPipe,
    LeagueComponent,
    EventComponent
  ],
  imports: [
    BrowserModule,
    TreeviewModule.forRoot(),
    HttpModule.forRoot(),
    FormsModule,
    CountdownModule,
    NumberFormatModule,
    SpinnerModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
