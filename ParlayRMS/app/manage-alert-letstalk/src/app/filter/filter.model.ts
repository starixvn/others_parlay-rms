export interface ItemModel {
    name: string;
    id: string;
    checked: boolean;
}

export interface FilterModel {
    fromDate: string;
    toDate: string;
    alertType: string;
    member: string;
    checkBy: string;
    isNoChecked: boolean;
    actionType: string;
    ticketStatus: string;
    pageSize: number;
    pageIndex: number;
}

export interface SportModel {
    sportId: number;
    sportName: string;
    sportGroup: number;
}