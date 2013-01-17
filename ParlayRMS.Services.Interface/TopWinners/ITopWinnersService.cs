// -----------------------------------------------------------------------
// <copyright file="ITopWinnersService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Interface.TopWinners
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.TopWinners;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITopWinnersService
    {
        /// <summary>
        /// RMs the s_ RPT_ parlay_ top winner.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="sportsType">Type of the sports.</param>
        /// <param name="selectTop">The select top.</param>
        /// <returns></returns>
        IEnumerable<RMS_Rpt_Parlay_TopWinner> GetTopWinners(DateTime fromDate, DateTime toDate, int sportsType, int selectTop);

        /// <summary>
        /// Gets the top winner_ details.
        /// </summary>
        /// <param name="custId">The cust id.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="sportsType">Type of the sports.</param>
        /// <returns></returns>
        IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> GetTopWinnerDetails(long custId, DateTime fromDate, DateTime toDate, int sportsType);
    }
}