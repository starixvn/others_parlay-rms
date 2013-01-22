// -----------------------------------------------------------------------
// <copyright file="ITopWinnersDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DataAccess.Interface.TopWinners
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.TopWinners;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITopWinnersDataAccess
    {
        /// <summary>
        /// RMs the s_ RPT_ parlay_ top winner.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="moveBetsDay">The move bets date.</param>
        /// <param name="sportsType">Type of the sports.</param>
        /// <param name="selectTop">The select top.</param>
        /// <returns></returns>
        IEnumerable<RMS_Rpt_Parlay_TopWinner> RMS_Rpt_Parlay_TopWinners(DateTime fromDate, DateTime toDate, DateTime moveBetsDay, DateTime balanceUpDay, int sportsType, int selectTop);

        /// <summary>
        /// RMs the s_ RPT_ parlay_ top winners_ details.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="moveBetsDay">The move bets date.</param>
        /// <param name="balanceUpDay">The balance up day.</param>
        /// <param name="sportsType">Type of the sports.</param>
        /// <returns></returns>
        IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details(long custId, DateTime fromDate, DateTime toDate, DateTime moveBetsDay, int sportsType);

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="sportsType"></param>
        /// <returns></returns>
        IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details_old(long custId, DateTime fromDate, DateTime toDate, int sportsType);
    }
}