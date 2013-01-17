// -----------------------------------------------------------------------
// <copyright file="ISimilarBetsDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DataAccess.Interface.SimilarBets
{
    using ParlayRMS.DTO.SimilarBets;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISimilarBetsDataAccess
    {
        /// <summary>
        /// RMs the s_ similar_ parlay_ betlist.
        /// </summary>
        /// <param name="transId">The trans id.</param>
        /// <returns></returns>
        RMS_Similar_Parlay_Betlist RMS_Similar_Parlay_Betlist(long transId);
    }
}