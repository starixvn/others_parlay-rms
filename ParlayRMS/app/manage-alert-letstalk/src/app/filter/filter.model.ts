export interface ItemModel {
    name: string;
    id: string;
    checked: boolean;
}

export interface FilterModel {
    fromDate: string;
    toDate: string;
    alertTypes: string;
    member: string;
    checkedBy: string;
    isNone: boolean;
    action: string;
    ticketStatus: string;
  }