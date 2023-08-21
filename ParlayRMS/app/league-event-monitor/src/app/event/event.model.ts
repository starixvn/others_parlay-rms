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
    leagueName: string;
    event: string;
}

export interface EventGroupModel {
    matchId: number;
    eventList: EventModel[];
    fixedBettypes: number;
    rowsTotal: number;
    totalStake: number;
}

export interface MatchBettypeModel {
    matchId: number;
    event: string;
    bettype: number;
    betChoice: number;
    bettypeName: string;
}
