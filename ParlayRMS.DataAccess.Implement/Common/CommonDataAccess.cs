// --------------------------------------------------------------------------
// <copyright file="CommonDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement.Common
{
    using System;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.Common;
    using ParlayRMS.DTO.Common;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Common Data Access.
    /// </summary>
    public class CommonDataAccess : BaseDataAccess, ICommonDataAccess
    {
        #region Balance Up Day

        /// <summary>
        /// Gets balance up day.
        /// </summary>
        /// <returns>Balance up day.</returns>
        public RMS_SelBalanceUpDay RMS_SelBalanceUpDay()
        {
            using (var db = new ObjectDb("RMS_SelBalanceUpDay"))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new RMS_SelBalanceUpDay
                        {
                            BalanceUpDay = reader.ToDateTime("BalanceUpDay"),
                            MoveBetsDay = reader.ToDateTime("MoveBetsDay")
                        };
                    }
                }
            }

            return null;
        }

        #endregion Balance Up Day

        /// <summary>
        /// Gets min TRANSDATE from RMBETTRANS table.
        /// </summary>
        /// <returns>Min TRANSDATE from RMBETTRANS.</returns>
        public RMS_Rpt_Parlay_SelMinTransdate RMS_Rpt_Parlay_SelMinTransdate()
        {
            using (var db = new ObjectDb("RMS_Rpt_Parlay_SelMinTransdate"))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new RMS_Rpt_Parlay_SelMinTransdate
                        {
                            MinTransdate = reader.ToDateTime("transdate_min")
                        };
                    }
                }
            }

            return null;
        }
    }
}