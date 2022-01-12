import { Pipe, PipeTransform } from '@angular/core';
import * as _ from 'lodash';
import { TreeviewItem } from 'ngx-treeview';
import { LeagueModel } from './filter.model';

@Pipe({
  name: 'leagueTreeview'
})
export class LeagueTreeviewPipe implements PipeTransform {

  transform(items: LeagueModel[]): TreeviewItem[] {
    const treeItems: TreeviewItem[] = [];

    if (_.isNil(items)) {
      return [];
    }

    items.forEach(item => {
      const value = new TreeviewItem({ text: item.leagueName, value: item.leagueId, checked: item.checked });
      treeItems.push(value);
    });
    return treeItems;
  }

}
