import { Pipe, PipeTransform } from '@angular/core';
import { TreeviewItem } from 'ngx-treeview';
import { SportModel } from './filter.model';
import * as _ from 'lodash';

@Pipe({
  name: 'sportTreeview'
})
export class SportTreeviewPipe implements PipeTransform {

  transform(sports: SportModel[]): TreeviewItem[] {
    const treeItems: TreeviewItem[] = [];
    const otherSports = new TreeviewItem({ text: 'All Other Sports', value: 0 });

    if (_.isNil(sports)) {
      return [];
    }

    sports.forEach(sport => {
      const item = new TreeviewItem({ text: sport.sportName, value: sport.sportId });
      switch (sport.sportGroup) {
        case 1:
          if (_.isNil(otherSports.children)) {
            otherSports.children = [item];
          } else {
            otherSports.children.push(item);
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
