export interface ItemModel {
    name: string;
    id: string;
    checked: boolean;
}

export interface LeagueModel {
    leagueId: number;
    leagueName: string;
    checked: boolean;
}

export interface FilterModel {
    selection: string;
    parlayType: string;
    isViewDetail: boolean;
    leagueIds: string;
    limitRowCount: number;
    level: string;
    customerClass?: number;
    sportType: number;
  }
