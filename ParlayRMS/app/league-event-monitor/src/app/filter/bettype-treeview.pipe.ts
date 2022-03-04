import { Pipe, PipeTransform } from '@angular/core';
import * as _ from 'lodash';
import { TreeviewItem } from 'ngx-treeview';
import { BettypeGroupConsts } from '../common/constants';
import { BettypeModel } from './filter.model';

@Pipe({
  name: 'bettypeTreeview'
})
export class BettypeTreeviewPipe implements PipeTransform {

  transform(bettypes: BettypeModel[]): TreeviewItem[] {
    const treeItems: TreeviewItem[] = [];
    const majorGroup = new TreeviewItem({ text: BettypeGroupConsts.Major, value: 0 });
    const minorGroup = new TreeviewItem({ text: BettypeGroupConsts.Minor, value: 0 });

    if (_.isNil(bettypes)) {
      return [];
    }

    bettypes.forEach(bettype => {
      const item = new TreeviewItem({ text: bettype.typeNamee, value: bettype.bettype });
      let groupTreeviewItem: TreeviewItem;
      switch (bettype.groupId) {
        case 1:
          groupTreeviewItem = majorGroup;
          if (_.isNil(groupTreeviewItem.children)) {
            groupTreeviewItem.children = [item];
          } else {
            groupTreeviewItem.children.push(item);
          }
          break;
        default:
          groupTreeviewItem = minorGroup;
          if (_.isNil(groupTreeviewItem.children)) {
            groupTreeviewItem.children = [item];
          } else {
            groupTreeviewItem.children.push(item);
          }
          break;
      }
    });

    treeItems.push(majorGroup);
    treeItems.push(minorGroup);
    return treeItems;
  }

}
