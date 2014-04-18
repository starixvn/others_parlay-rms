// --------------------------------------------------------------------------
// <copyright file="ViewIPInfoDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement.ViewIPInfo
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.ViewIPInfo;
    using ParlayRMS.DTO.ViewIPInfo;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// ViewIPInfo Data Access.
    /// </summary>
    public class ViewIPInfoDataAccess : BaseDataAccess, IViewIPInfoDataAccess
    {
        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> RMS_Rpt_Parlay_BetsStatisticsByIP(string ip, DateTime date)
        {
            var result = new List<RMS_Rpt_Parlay_ViewIPInfo>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_BetsStatisticsByIP"))
            {
                db.AddInParameter("ip", DbType.String, ip);
                db.AddInParameter("winlostdate", DbType.DateTime, date);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_ViewIPInfo
                        {
                            Username = reader.ToString("username"),
                            Soccer = reader.ToInt32("cnt_soccer"),
                            Others = reader.ToInt32("cnt_others"),
                        });
                    }
                }
            }

            return result;
        }
    }
}