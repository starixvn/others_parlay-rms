import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { PaginationConfig } from 'nap-components/pagination';

export class DateConfig {
    constructor(private day: moment.Moment) { }

    get today(): moment.Moment {
        return this.day;
    }

    get yesterday(): moment.Moment {
        return moment.utc(this.day).add(-1, 'day');
    }
}

export let CONFIG = {
    date: new DateConfig(moment.utc().startOf('day')),
    dateFormatString: 'MMM DD, YYYY',
};

@Injectable()
export class CustomPaginationConfig extends PaginationConfig {
    pageSizes = [1000, 2000, 3000, 4000, 5000];
    itemsPerPage = 1000;
}
