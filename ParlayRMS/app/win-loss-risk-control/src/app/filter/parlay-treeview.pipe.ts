import { Pipe, PipeTransform } from '@angular/core';
import * as _ from 'lodash';
import { SelectItem } from './filter.model';
import { TreeviewItem } from 'ngx-treeview';

@Pipe({
  name: 'parlayTreeview'
})

export class ParlayTreeviewPipe implements PipeTransform {
  transform(parlays: SelectItem[]): TreeviewItem[] {
    const treeItems: TreeviewItem[] = [];

    if (_.isNil(parlays)) {
      return undefined;
    }

    parlays.forEach(parlay => {
      const item = new TreeviewItem({ text: parlay.name, value: parlay.id });
      treeItems.push(item);
    });

    return treeItems;
  }
}

