import { Pipe, PipeTransform } from '@angular/core';
import { TreeviewItem } from 'ngx-treeview';
import { ItemModel } from './filter.model';
import * as _ from 'lodash';

@Pipe({
  name: 'itemTreeview'
})
export class ItemTreeviewPipe implements PipeTransform {

  transform(items: ItemModel[]): TreeviewItem[] {
    const treeItems: TreeviewItem[] = [];

    if (_.isNil(items)) {
      return [];
    }

    items.forEach(item => {
      const value = new TreeviewItem({ text: item.name, value: item.id, checked: item.checked });
      treeItems.push(value);
    });

    return treeItems;
  }

}
