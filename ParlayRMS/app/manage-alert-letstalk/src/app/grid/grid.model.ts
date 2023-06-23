export interface DataModel {
    no: number;
    alertTime: string;
    betId: string,
    alertType: string;
    sportType: string;
    userName: string;
    totalSimilarTickets: number;
    totalSelection: number;
    odds: number;
    totalBets: number;
    totalMemberStake: number;
    totalPotentialWinningAmount: number;
    potentialCompanyLoseAmount: number;
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
