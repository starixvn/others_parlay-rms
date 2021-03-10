export interface WinLossModel {
  username: string;
  isDeposit: boolean;
  noMember: number;
  betCount: number;
  memberWinLoss: number;
  smaWinLoss: number;
  extraPT: number;
  commission: number;
  realIncome: number;
  realTurnover: number;
  margin: number;
  rows: WinLossModel[];
  isCollapsed: boolean;
  site: string;
}
