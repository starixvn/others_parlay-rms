import { MemberGridModel } from "../member-grid/member-grid.model";

export interface AgentGridModel extends MemberGridModel {
    isDeposit: boolean;
}
