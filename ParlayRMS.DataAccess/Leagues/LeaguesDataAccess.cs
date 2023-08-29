// --------------------------------------------------------------------------
// <copyright file="LeaguesDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Leagues
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Leagues;
    using ParlayRMS.Utility.Data;

    public class LeaguesDataAccess : BaseDataAccess
    {
        public List<RMS_LeagueSel> RMS_LeagueSel(string leagueName, int groupId, int sportType)
        {
            var result = new List<RMS_LeagueSel>();

            using (var db = new ObjectDb("RMS_LeagueSel"))
            {
                db.AddInParameter("leaguename", DbType.String, leagueName ?? string.Empty);
                db.AddInParameter("groupid", DbType.Int32, groupId);
                db.AddInParameter("SportType", DbType.Int32, sportType);
                using (var reader = db.ExecuteReader())
                {
                    int index = 1;
                    while (reader.Read())
                    {
                        result.Add(new RMS_LeagueSel
                        {
                            No = index++,
                            LeagueId = reader.ToInt32("leagueid"),
                            LeagueName = reader.ToString("leaguename"),
                            GroupId = reader.ToInt32("GroupId"),
                            GroupName = reader.ToString("GroupName"),
                        });
                    }
                }
            }

            return result;
        }

        public Result RMS_LeagueIns(string leagueName, int groupId, int adminId, string ip)
        {
            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("leaguename", leagueName ?? string.Empty);
            param.Add("groupid", groupId);
            param.Add("ip", ip);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            var r = ExecSp("RMS_LeagueIns", param);

            return r.ToResult();
        }

        public Result RMS_LeagueUpd(int leagueId, int groupId, int adminId, string ip)
        {
            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("leagueId", leagueId);
            param.Add("groupId", groupId);
            param.Add("ip", ip);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            var r = ExecSp("RMS_LeagueUpd", param);

            return r.ToResult();
        }
    }
}