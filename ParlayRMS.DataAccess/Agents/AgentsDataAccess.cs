namespace ParlayRMS.DataAccess.Agents
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Agents;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Agent data access.
    /// </summary>
    public class AgentsDataAccess : BaseDataAccess
    {
        /// <summary>
        /// Gets dangerous agent list.
        /// </summary>
        /// <param name="username">The user name that is used to filter agent.</param>
        /// <returns>Dangerous agent list.</returns>
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

        /// <summary>
        /// Inserts a new dangerous agent.
        /// </summary>
        /// <param name="userName">The user name of the dangerous agent.</param>
        /// <param name="adminId">The ID of administrator who adds the agent.</param>
        /// <param name="ip">IP of machine which adds the agent.</param>
        /// <returns>Whether or not the agent is added successfully.</returns>
        public Result RMS_DangerousAgentIns(string userName, int adminId, string ip)
        {
            Hashtable param = new Hashtable();
            param.Add("userid", adminId);
            param.Add("username", userName);
            param.Add("ip", ip);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            var r = ExecSp("RMS_DangerousAgentIns", param);

            return r.ToResult();
        }

        /// <summary>
        /// Deletes an existing dangerous agent.
        /// </summary>
        /// <param name="agentId">The ID of the dangerous agent.</param>
        /// <param name="userName">The user name of the dangerous agent.</param>
        /// <param name="adminId">The ID of administrator who deletes the agent.</param>
        /// <param name="ip">IP of machine which deletes the agent.</param>
        /// <returns>Whether or not the agent is deleted successfully.</returns>
        public Result RMS_DangerousAgentDel(int agentId, string userName, int adminId, string ip)
        {
            Hashtable param = new Hashtable();
            param.Add("userid", adminId);
            param.Add("agentid", agentId);
            param.Add("username", userName);
            param.Add("ip", ip);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            var r = ExecSp("RMS_DangerousAgentDel", param);

            return r.ToResult();
        }
    }
}