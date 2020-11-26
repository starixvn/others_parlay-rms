import { WinLossModel } from "src/app/common/win-loss.model";

export interface MemberWinLossModel extends WinLossModel {
   totalRecords: number;
}

export interface MemberGridModel {
   username: string;
   custId: number;
}