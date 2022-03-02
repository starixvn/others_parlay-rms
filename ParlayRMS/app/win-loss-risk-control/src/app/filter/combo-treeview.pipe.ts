import { Pipe, PipeTransform } from '@angular/core';
import * as _ from 'lodash';
import { TreeviewItem } from 'ngx-treeview';
import { SelectItem } from './filter.model';

@Pipe({
  name: 'comboTreeview'
})
export class ComboTreeviewPipe implements PipeTransform {

  transform(comboes: SelectItem[]): TreeviewItem[] {
    const treeItems: TreeviewItem[] = [];

    if (_.isNil(comboes)) {
      return undefined;
    }

    comboes.forEach(combo => {
      const item = new TreeviewItem({ text: combo.name, value: combo.id });
      treeItems.push(item);
    });

    return treeItems;
  }

}
