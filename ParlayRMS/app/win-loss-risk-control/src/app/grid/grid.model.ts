import { Observable } from "rxjs/Observable";

export enum GridType {
    Site,
    Super,
    Master,
    Agent,
    Member
}

export interface Grid {
    hasExcel: boolean;
    title: string;
    query(): Observable<any>;
    exportExcel(): void;
}
