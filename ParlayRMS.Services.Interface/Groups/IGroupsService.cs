namespace ParlayRMS.Services.Interface.Groups
{
    using System.Collections.Generic;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Common.Models;
    using ParlayRMS.Models.Groups;
    using ParlayRMS.DTO;

    public interface IGroupsService
    {
        IEnumerable<Group> GetGroups();

        IEnumerable<Group> GetGroupNames();

        IEnumerable<Function> GetFunctionsByGroupId(int groupId);

        Group GetGroupById(int groupId);

        Result CreateGroup(string groupName, string description, int adminId, string ip, out int groupId);

        Result EditGroup(string groupName, string description, int adminId, string ip, int groupId);

        Result EditGroupPolicy(string groupPolicy, int adminId, string ip, int groupId);
    }
}