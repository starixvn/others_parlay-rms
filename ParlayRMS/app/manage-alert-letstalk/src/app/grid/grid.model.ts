export interface DataModelItem {
    alertTime: string;
    alertId: string;
    betId: string,
    refNo: string,
    custId: number,
    betType: number,
    alertType: string;
    sportType: string;
    userName: string;
    similarTickets: string;
    selection: number;
    odds: number;
    totalBets: string;
    memberStake: number;
    potentialWinning: number;
    companyLose: number;
    checkedBy: string;
    action: string;
}

export interface DataModel {
    items: DataModelItem[];
    totalItems: number;
}

export interface DetailDataComponent {
    data: any;
}

export interface BetDetails {
    transId: string;
    userName: string;
    refNo: string;
    betType: number;
    custId: number;
}

export interface TakeActionModel {
    alertId: string;
    userId: number;
    isChecked: boolean;
    actionId: number;
}
