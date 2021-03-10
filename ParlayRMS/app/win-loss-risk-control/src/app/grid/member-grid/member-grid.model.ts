import { WinLossModel } from '../../common/win-loss.model';

export interface MemberWinLossModel extends WinLossModel {
   totalRecords: number;
}

export interface MemberGridModel {
   username: string;
   custId: number;
   site: string;
}
