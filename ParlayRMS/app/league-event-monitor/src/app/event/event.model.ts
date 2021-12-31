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
}

export interface EventGroupModel {
    matchId: number;
    eventList: EventModel[],
    fixedBettypes: number;
}
