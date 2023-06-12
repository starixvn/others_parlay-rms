import * as moment from "moment";

export class DateConfig {
    constructor(private day: moment.Moment) { }

    get today(): moment.Moment {
        return moment.utc(this.day);
    }

    get yesterday(): moment.Moment {
        return moment.utc(this.day).add(-1, 'day');
    }
}

export let CONFIG = {
    date: new DateConfig(moment.utc().startOf('day')),
    dateFormatString: 'MMM DD, YYYY',
};