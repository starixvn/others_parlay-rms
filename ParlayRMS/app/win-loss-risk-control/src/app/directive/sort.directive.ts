import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { Sort } from '../util/sort';

@Directive({
  selector: '[appSort]'
})
export class SortDirective {
  @Input() appSort = Array<Array<any>>();
  public sortedColClass = "sorted-col";
  public sortIconUpHtml = '<em class="fa fa-sort-up"></em>';
  public sortIconDownHtml = '<em class="fa fa-sort-down"></em>';
  public sortIconHtml = `${this.sortIconUpHtml}${this.sortIconDownHtml}`;

  constructor(private targetElem: ElementRef) {}

  @HostListener("click")
  sortData(){
    const sort = new Sort();
    const elem = this.targetElem.nativeElement;
    const order = elem.getAttribute("data-order");
    const property = elem.getAttribute("data-prop");
    const sortIconElem = elem.querySelector("span");
    this.resetSortedIcon(elem);
    this.appSort.forEach(item => {
      item.sort(sort.startSort(property, order));
    });
    sortIconElem.setAttribute("class", this.sortedColClass);
    if (order === "desc") {
      elem.setAttribute("data-order", "asc");
      sortIconElem.innerHTML = this.sortIconDownHtml;
    } else {
      elem.setAttribute("data-order", "desc");
      sortIconElem.innerHTML = '<em class="fa fa-sort-up sort-icon"></em>';
    }
  }

  private resetSortedIcon(element: HTMLElement)
  {
    const childElements = element.parentElement.children;
    for (let i = 0; i < childElements.length; i++) {
      const iconElement = childElements[i].querySelector("span");
      if (iconElement !== null && iconElement.className === this.sortedColClass)
      {
        iconElement.innerHTML = this.sortIconHtml;
        iconElement.setAttribute("class", "");
        break;
      }
    }
  }
}
