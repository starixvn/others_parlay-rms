namespace ParlayRMS.DataAccess.Agents
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using DTO;
    using ParlayRMS.DTO.Agents;
    using Utility.Data;

    public class AgentsDataAccess : BaseDataAccess
    {
        public IEnumerable<RMS_DangerousAgentSel> RMS_DangerousAgentSel(string username)
        {
            var result = new List<RMS_DangerousAgentSel>();
            var counter = 0;

            using (var db = new ObjectDb("RMS_DangerousAgentSel"))
            {
                if (!string.IsNullOrEmpty(username))
                {
                    db.AddInParameter("username", DbType.String, username);
                }

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_DangerousAgentSel
                        {
                            No = ++counter,
                            AgentId = reader.ToInt32("AgentId"),
                            UserName = reader.ToString("UserName")
                        });
                    }
                }
            }

            return result;
        }

        public Result RMS_DangerousAgentIns(string userName, int adminId, string ip)
        {
            var param = new Hashtable
            {
                { "userid", adminId },
                { "username", userName },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_DangerousAgentIns", param);

            return r.ToResult();
        }

        public Result RMS_DangerousAgentDel(int agentId, string userName, int adminId, string ip)
        {
            var param = new Hashtable
            {
                { "userid", adminId },
                { "agentid", agentId },
                { "username", userName },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_DangerousAgentDel", param);

            return r.ToResult();
        }
    }
}
