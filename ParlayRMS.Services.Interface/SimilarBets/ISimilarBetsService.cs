// -----------------------------------------------------------------------
// <copyright file="ISimilarBetsService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Interface.SimilarBets
{
    using ParlayRMS.DTO.SimilarBets;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISimilarBetsService
    {
        /// <summary>
        /// RMs the s_ similar_ parlay_ betlist.
        /// </summary>
        /// <param name="transId">The trans id.</param>
        /// <returns></returns>
        RMS_Similar_Parlay_Betlist GetSimilarBets(long transId);
    }
}