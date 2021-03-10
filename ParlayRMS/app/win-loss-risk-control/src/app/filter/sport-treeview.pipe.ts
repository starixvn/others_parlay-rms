import { Pipe, PipeTransform } from '@angular/core';
import * as _ from 'lodash';
import { Sport } from './filter.model';
import { TreeviewItem } from 'ngx-treeview';

@Pipe({
  name: 'sportTreeview'
})

export class SportTreeviewPipe implements PipeTransform {
  transform(sports: Sport[]): TreeviewItem[] {
    const treeItems: TreeviewItem[] = [];
    const otherSports = new TreeviewItem({ text: 'All Other Sports', value: 0 });

    if (_.isNil(sports)) {
      return undefined;
    }

    sports.forEach(sport => {
      const item = new TreeviewItem({ text: sport.sportName, value: sport.sportId });
      let groupTreeviewItem: TreeviewItem;
      switch (sport.sportGroup) {
        case 1:
          groupTreeviewItem = otherSports;
          if (_.isNil(groupTreeviewItem.children)) {
            groupTreeviewItem.children = [item];
          } else {
            groupTreeviewItem.children.push(item);
          }
          break;
        default:
          treeItems.push(item);
          break;
      }
    });

    treeItems.push(otherSports);
    return treeItems;
  }
}

