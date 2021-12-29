import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from 'nap-components/http';
import { TreeviewModule } from 'ngx-treeview';

import { AppComponent } from './app.component';
import { FilterComponent } from './filter/filter.component';
import { ItemTreeviewPipe } from './filter/item-treeview.pipe';
import { LeagueTreeviewPipe } from './filter/league-treeview.pipe';

@NgModule({
  declarations: [
    AppComponent,
    FilterComponent,
    ItemTreeviewPipe,
    LeagueTreeviewPipe
  ],
  imports: [
    BrowserModule,
    TreeviewModule.forRoot(),
    HttpModule.forRoot(),
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
