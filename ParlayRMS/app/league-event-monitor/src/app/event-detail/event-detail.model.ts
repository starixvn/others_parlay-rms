export interface EventDetailModel {
    market: string;
    avgOdds: number;
    stake: number;
    ticket: number;
    bettype: number;
}

export interface EventDetailGroupModel {
    matchId: number;
    match: string;
    eventList: EventDetailModel[],
    fixedBettypes: number;
}