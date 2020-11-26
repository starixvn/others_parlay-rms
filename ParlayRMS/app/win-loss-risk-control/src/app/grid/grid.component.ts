import {
  Component,
  OnInit,
  Input,
  ViewContainerRef,
  ViewChild,
  Renderer2,
  ComponentFactoryResolver,
  ComponentRef,
  ComponentFactory,
  OnDestroy,
  OnChanges,
} from '@angular/core';
import { FilterModel } from '../filter/filter.model';
import { Grid, GridType } from './grid.model';
import { GridService } from './grid.service';
import { SiteGridComponent } from './site-grid/site-grid.component';
import { SuperGridComponent } from './super-grid/super-grid.component';
import { MemberGridComponent } from './member-grid/member-grid.component';
import { MasterGridComponent } from './master-grid/master-grid.component';
import { AgentGridComponent } from './agent-grid/agent-grid.component';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss'],
  entryComponents: [
    SiteGridComponent,
    SuperGridComponent,
    MasterGridComponent,
    AgentGridComponent,
    MemberGridComponent
  ]
})
export class GridComponent implements OnInit, OnChanges, OnDestroy {
  @Input() filterModel: FilterModel;
  @ViewChild('container', { read: ViewContainerRef, static: true }) container: ViewContainerRef;
  gridType = GridType.Site;
  breadcrumbItems: string[];
  private components: ComponentRef<Grid>[] = [];

  constructor(
    private componentFactoryResolver: ComponentFactoryResolver,
    private renderer: Renderer2,
    private gridService: GridService) { }

  ngOnInit() {
    this.gridService.generate = (gridType: GridType) => this.generate(gridType);
  }

  ngOnChanges() {
    this.breadcrumbItems = [];
    while (this.components.length) {
      const component = this.components.pop();
      component.destroy();
    }

    this.gridType = GridType.Site;
    this.gridService.filterModel = this.filterModel;
    this.createGridComponent();
  }

  ngOnDestroy() {
    this.container.clear();
  }

  private generate(gridType: GridType) {
    this.gridType = gridType;
    this.createGridComponent();
  }

  private getComponentFactory(): ComponentFactory<Grid> {
    switch (this.gridType) {
      case GridType.Site:
        return this.componentFactoryResolver.resolveComponentFactory(SiteGridComponent);
      case GridType.Super:
        return this.componentFactoryResolver.resolveComponentFactory(SuperGridComponent);
      case GridType.Master:
        return this.componentFactoryResolver.resolveComponentFactory(MasterGridComponent);
      case GridType.Agent:
        return this.componentFactoryResolver.resolveComponentFactory(AgentGridComponent);
      case GridType.Member:
        return this.componentFactoryResolver.resolveComponentFactory(MemberGridComponent);

      default:
        throw new Error('Not implemented yet');
    }
  }

  private createGridComponent() {
    const factory = this.getComponentFactory();
    const component = this.container.createComponent(factory);
    this.components.push(component);

    const grid = component.instance;
    this.gridService.activeGrid = grid;
    grid.query().subscribe({
      complete: () => this.showGridAndBreadcrumb()
    });
  }

  private destroyComponentsFrom(index: number) {
    while (this.components.length - 1 > index) {
      const component = this.components.pop();
      component.destroy();
    }

    this.gridService.activeGrid = this.components[this.components.length - 1].instance;
    this.showGridAndBreadcrumb();
  }

  private showGridAndBreadcrumb() {
    this.updateBreadcrumb();
    this.showOnlyLastGrid();
  }

  private updateBreadcrumb() {
    this.breadcrumbItems = [];
    this.components.forEach(component => {
      const componentTitle = component.instance.title;
      this.breadcrumbItems.push(componentTitle);
    });
  }

  private showOnlyLastGrid() {
    const length = this.components.length;
    const currentComponent = this.components[length - 1];
    this.renderer.removeAttribute(currentComponent.location.nativeElement, 'hidden');

    if (length > 1) {
      const lastComponent = this.components[length - 2];
      this.renderer.setAttribute(lastComponent.location.nativeElement, 'hidden', 'hidden');
    }
  }

  onBreadcrumbItemClick(event: number) {
    this.destroyComponentsFrom(event);
    this.showGridAndBreadcrumb();
  }

  exportExcel() {
    if (this.components.length === 0) {
      return;
    }

    const component = this.components[this.components.length - 1];
    const grid = component.instance;
    grid.exportExcel();
  }
}
