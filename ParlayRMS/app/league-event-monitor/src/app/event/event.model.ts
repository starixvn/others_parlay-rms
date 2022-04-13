export interface EventModel {
    matchId: number;
    homeName: string;
    market: string;
    homeAvgOdds: number;
    homeTotalStake: number;
    homeTotalTicket: number;
    awayName: string;
    awayAvgOdds: number;
    awayTotalStake: number;
    awayTotalTicket: number;
    isOther: boolean;
    awayMarket: string;
}

export interface EventGroupModel {
    matchId: number;
    eventList: EventModel[],
    fixedBettypes: number;
}
