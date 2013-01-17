namespace ParlayRMS.DataAccess.Interface.Groups
{
    using System.Collections.Generic;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DTO.Groups;
    using ParlayRMS.DTO;

    public interface IGroupsDataAccess
    {
        IEnumerable<RMS_GroupInfoSel> RMS_GroupInfoSel();

        IEnumerable<RMS_GroupInfoSelAllGroup> RMS_GroupInfoSelAllGroup();

        IEnumerable<RMS_GroupPolicySel> RMS_GroupPolicySel(int groupId);

        RMS_GroupInfoSelByGroupID RMS_GroupInfoSelByGroupID(int groupId);

        Result RMS_GroupInfoIns(string groupName, string description, int adminId, string ip, out int groupId);

        Result RMS_GroupInfoUpd(string groupName, string description, int adminId, string ip, int groupId);

        Result RMS_GroupPolicyUpd(string groupPolicy, int adminId, string ip, int groupId);
    }
}