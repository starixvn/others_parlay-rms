import { Injectable } from '@angular/core';
import { FilterModel } from '../filter/filter.model';
import { Grid, GridType } from './grid.model';

@Injectable()
export class GridService {
    filterModel: FilterModel;
    activeGrid: Grid;
    breadCrumb: string;
    netcoreApi: string;
    generate: (gridType: GridType) => void;
}
