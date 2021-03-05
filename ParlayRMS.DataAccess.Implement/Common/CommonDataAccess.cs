// --------------------------------------------------------------------------
// <copyright file="CommonDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement.Common
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DTO.Common;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Common Data Access.
    /// </summary>
    public class CommonDataAccess : BaseDataAccess
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

        /// <summary>
        /// Get bet type belong bet radar.
        /// </summary>
        /// <returns>Return the bet type list.</returns>
        public List<int> GetBettypeBelongsBetRadar()
        {
            var bettypes = new List<int>();
            using (var db = new ObjectDb("Acc_SportBettypeMatrix_Sel"))
            {
                db.AddInParameter("mode", DbType.Int16, 4);
                db.AddInParameter("subproduct", DbType.String, "BetRadar");

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bettypes.Add(reader.ToInt32("bettype"));
                    }
                }
            }

            return bettypes;
        }

        /// <summary>
        /// Get currencies.
        /// </summary>
        /// <returns>Return currency list.</returns>
        public IEnumerable<Currency> GetCurrencies()
        {
            var currencies = new List<Currency>();
            using (var db = new ObjectDb("RMS_CurrencySel"))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currencies.Add(new Currency { Id = reader.ToInt32("currency"), CurrencyCode = reader.ToString("currencyName") });
                    }
                }
            }

            return currencies;
        }
    }
}