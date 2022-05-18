import { Pipe, PipeTransform } from '@angular/core';
import * as _ from 'lodash';
import { Company } from './filter.model';
import { TreeviewItem } from 'ngx-treeview';

@Pipe({
  name: 'companyTreeview'
})

export class CompanyTreeviewPipe implements PipeTransform {
  transform(company: Company[]): TreeviewItem[] {
    const mbcGroup = new TreeviewItem({ text: 'MBC Group', value: 0 });
    const licenseeGroup = new TreeviewItem({ text: 'Licensee Group', value: 1 });

    if (_.isNil(company)) {
      return undefined;
    }

    company.forEach(item => {
      const itemCompany = new TreeviewItem({ text: item.site, value: item.site, checked: item.checked });
      let groupTreeviewItem: TreeviewItem;

      switch (item.isDeposit) {
        case true:
          groupTreeviewItem = licenseeGroup;
          break;
        default:
          groupTreeviewItem = mbcGroup;
          break;
      }

      if (_.isNil(groupTreeviewItem.children)) {
        groupTreeviewItem.children = [itemCompany];
      } else {
        groupTreeviewItem.children.push(itemCompany);
      }
    });

    mbcGroup.correctChecked();
    licenseeGroup.correctChecked();

    const siteTreeItems = [
      mbcGroup,
      licenseeGroup
    ];

    return siteTreeItems.filter(item => !_.isNil(item.children) && item.children.length > 0);
  }
}
