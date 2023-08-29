namespace ParlayRMS.DataAccess.Groups
{
    using System.Collections;
    using System.Collections.Generic;
    using DTO;
    using ParlayRMS.DTO.Groups;
    using Utility;
    using Utility.Data;

    public class GroupsDataAccess : BaseDataAccess
    {
        public IEnumerable<RMS_GroupInfoSel> RMS_GroupInfoSel()
        {
            var result = new List<RMS_GroupInfoSel>();

            using (var reader = ExecSpDataReader("RMS_GroupInfoSel"))
            {
                while (reader.Read())
                {
                    result.Add(new RMS_GroupInfoSel
                    {
                        GroupId = reader.ToInt32("groupid"),
                        GroupName = reader.ToString("groupname"),
                        NoUsers = reader.ToInt32("nousers"),
                        Description = reader.ToString("description"),
                        Creator = reader.ToString("creator"),
                        CreatedDate = reader.ToDateTime("createddate")
                    });
                }
            }

            return result;
        }

        public RMS_GroupInfoSelByGroupID RMS_GroupInfoSelByGroupID(int groupId)
        {
            var param = new Hashtable { { "groupid", groupId } };
            using (var reader = ExecSpDataReader("RMS_GroupInfoSelByGroupID", param))
            {
                while (reader.Read())
                {
                    return new RMS_GroupInfoSelByGroupID
                    {
                        GroupName = reader.ToString("groupname"),
                        Description = reader.ToString("Description")
                    };
                }
            }

            return null;
        }

        public Result RMS_GroupInfoIns(string groupName, string description, int adminId, string ip, out int groupId)
        {
            groupId = 0;
            var result = Result.Empty;

            var param = new Hashtable
            {
                { "adminid", adminId },
                { "groupname", groupName },
                { "description", description },
                { "ip", ip },
                { "groupid", 0 },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_GroupInfoIns", param);
            result = r.ToResult();
            if (result.IsSuccessful)
            {
                groupId = DbConverter.ToInt32(r.Outputs["groupid"]);
            }

            return result;
        }

        public Result RMS_GroupInfoUpd(string groupName, string description, int adminId, string ip, int groupId)
        {
            var param = new Hashtable
            {
                { "adminid", adminId },
                { "groupid", groupId },
                { "groupname", groupName },
                { "description", description },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_GroupInfoUpd", param);

            return r.ToResult();
        }

        public IEnumerable<RMS_GroupInfoSelAllGroup> RMS_GroupInfoSelAllGroup()
        {
            var result = new List<RMS_GroupInfoSelAllGroup>();

            using (var reader = ExecSpDataReader("RMS_GroupInfoSelAllGroup"))
            {
                while (reader.Read())
                {
                    result.Add(new RMS_GroupInfoSelAllGroup
                    {
                        GroupId = reader.ToInt32("groupid"),
                        GroupName = reader.ToString("groupname"),
                    });
                }
            }

            return result;
        }

        public IEnumerable<RMS_GroupPolicySel> RMS_GroupPolicySel(int groupId)
        {
            var result = new List<RMS_GroupPolicySel>();

            var param = new Hashtable { { "groupid", groupId } };
            using (var reader = ExecSpDataReader("RMS_GroupPolicySel", param))
            {
                while (reader.Read())
                {
                    result.Add(new RMS_GroupPolicySel
                    {
                        FuncId = reader.ToInt32("funcid"),
                        FuncKey = reader.ToString("funckey"),
                        FuncName = reader.ToString("funcname"),
                        DisplayOrder = reader.ToInt32("displayorder"),
                        ParentId = reader.ToInt32("parentid"),
                        ParentName = reader.ToString("parentname"),
                        Policy = reader.ToInt32("policy")
                    });
                }
            }

            return result;
        }

        public Result RMS_GroupPolicyUpd(string groupPolicy, int adminId, string ip, int groupId)
        {
            var param = new Hashtable
            {
                { "adminid", adminId },
                { "groupid", groupId },
                { "grouppolicy", groupPolicy },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_GroupPolicyUpd", param);

            return r.ToResult();
        }
    }
}
