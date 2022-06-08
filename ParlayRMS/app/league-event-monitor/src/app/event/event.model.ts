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
    bettype: number;
    awayBettype: number;
}

export interface EventGroupModel {
    matchId: number;
    eventList: EventModel[],
    fixedBettypes: number;
}

export interface MatchBettypeModel {
    matchId: number;
    event: string;
    bettype: number;
    betChoice: number;
    bettypeName: string;
}
