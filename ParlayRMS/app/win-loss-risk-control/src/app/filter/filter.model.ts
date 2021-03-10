export interface FilterModel {
  fromDate: string;
  toDate: string;
  custId: number;
  roleId: number;
  company: string;
  parlayType: string;
  sportType: string;
  pageSize?: number;
  pageIndex?: number;
  isPageChanged?: boolean;
}

export interface Sport {
  sportId: number;
  sportName: string;
  sportGroup: number;
}

export interface Company {
  site: string;
  isDeposit: boolean;
}

export interface ParlayType {
  name: string;
  id: number;
}
