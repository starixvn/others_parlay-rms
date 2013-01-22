// -----------------------------------------------------------------------
// <copyright file="TopWinnersService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Implement.TopWinners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ParlayRMS.DataAccess.Implement.TopWinners;
    using ParlayRMS.DataAccess.Interface.TopWinners;
    using ParlayRMS.DTO.TopWinners;
    using ParlayRMS.Services.Implement.Common;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Interface.TopWinners;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TopWinnersService : BaseService, ITopWinnersService
    {
        private readonly ITopWinnersDataAccess topwinnersDataAccess = new TopWinnersDataAccess();
        private readonly ICommonService commonService = new CommonService();

        /// <summary>
        /// RMs the s_ RPT_ parlay_ top winner.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="sportsType">Type of the sports.</param>
        /// <param name="selectTop">The select top.</param>
        /// <returns></returns>
        public IEnumerable<RMS_Rpt_Parlay_TopWinner> GetTopWinners(DateTime fromDate, DateTime toDate, int sportsType, int selectTop)
        {
            var info = commonService.GetBalanceUpDayInfo();
            return this.topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners(fromDate, toDate, info.MoveBetsDay, info.BalanceUpDay, sportsType, selectTop);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="sportsType"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> GetTopWinnerDetails(long custId, DateTime fromDate, DateTime toDate, int sportsType)
        {
            var info = commonService.GetBalanceUpDayInfo();

            if (info.BalanceUpDay.CompareTo(fromDate) <= 0 && info.BalanceUpDay.CompareTo(toDate) <= 0)
            {
                return this.topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details(custId, fromDate, toDate, info.MoveBetsDay, sportsType);
            }
            else if (info.BalanceUpDay.CompareTo(fromDate) > 0 && info.BalanceUpDay.CompareTo(toDate) > 0)
            {
                return (IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail>)this.topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details_old(custId, fromDate, toDate, sportsType);
            }
            else
            {
                IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> listEntity_old = null;
                IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> listEntity = null;

                listEntity_old = this.topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details_old(custId, fromDate, info.BalanceUpDay.AddDays(-1), sportsType);
                listEntity = this.topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details(custId, info.BalanceUpDay, toDate, info.MoveBetsDay, sportsType);

                if (listEntity_old.Count() != 0 && listEntity.Count() != 0)
                {
                    return listEntity.Union(listEntity_old);
                }
                else if (listEntity_old.Count() != 0 && listEntity.Count() <= 0)
                {
                    return listEntity_old;
                }
                else
                {
                    return listEntity;
                }
            }
        }
    }
}