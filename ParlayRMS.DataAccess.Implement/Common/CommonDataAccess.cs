// -----------------------------------------------------------------------
// <copyright file="CommonDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DataAccess.Implement.Common
{
    using System;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.Common;
    using ParlayRMS.DTO.Common;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommonDataAccess : BaseDataAccess, ICommonDataAccess
    {
        #region Balance Up Day

        public RMS_SelBalanceUpDay RMS_SelBalanceUpDay()
        {
            try
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
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return null;
        }

        #endregion Balance Up Day
    }
}