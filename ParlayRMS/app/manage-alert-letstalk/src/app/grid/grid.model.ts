export interface DataModel {
    alertTime: string;
    alertId: string;
    betId: string,
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

export interface DetailDataComponent {
    data: any;
}

export interface BetDetails {
    ticketDetail: string;
    comboTypes: string[];
}
