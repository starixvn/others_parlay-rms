// --------------------------------------------------------------------------
// <copyright file="CacheRef.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Utility
{
    using System.Threading;
    using RemotingRef;

    /// <summary>
    /// Cache reference.
    /// </summary>
    public class CacheRef
    {
        public static string CurrentTWLanguage
        {
            get
            {
                switch (Thread.CurrentThread.CurrentCulture.Name)
                {
                    case "en-US": return "en";
                    case "zh-TW": return "ch"; // Traditional
                    case "zh-CN": return "cs"; // Simplified
                    case "ja-JP": return "jp";
                    case "th-TH": return "th"; // Thai
                    case "ko-KR": return "ko"; // Korean
                    case "vi-VN": return "vi"; // Vietnamese
                    default: return "en";
                }
            }
        }

        private static bool IsWebService
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets bet type name by ID.
        /// </summary>
        /// <param name="bettypeId">Bet type ID.</param>
        /// <returns>Bet type name associated with the passed ID.</returns>
        public static string GetBettype(object bettypeId)
        {
            return GetBettype(bettypeId, null);
        }

        /// <summary>
        /// Gets bet type name by bet type ID and resource ID.
        /// </summary>
        /// <param name="bettypeId">Bet type ID.</param>
        /// <param name="betID">Resource ID.</param>
        /// <returns>Bet type name associated with the passed bet type ID and resource ID.</returns>
        public static string GetBettype(object bettypeId, object betID)
        {
            // ResourceId remove first character
            string resourceId = string.Empty;
            if (betID != null && betID.ToString().Length > 1)
            {
                resourceId = betID.ToString().Substring(1);
            }

            try
            {
                return RefData.GetBetTypeName(bettypeId.ToString(), CurrentTWLanguage, string.Empty, resourceId);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets sport name by ID.
        /// </summary>
        /// <param name="sportId">Sport ID value.</param>
        /// <returns>Sport name associated with the ID.</returns>
        public static string GetSportName(object sportId)
        {
            try
            {
                return RefData.GetSportName(sportId.ToString(), CurrentTWLanguage, string.Empty);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets team name by ID.
        /// </summary>
        /// <param name="teamId">Team ID value.</param>
        /// <returns>Team name associated with the ID.</returns>
        public static string GetTeamName(object teamId)
        {
            try
            {
                return RefData.GetTeamName(teamId.ToString(), CurrentTWLanguage, string.Empty);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets roulette bet name by ID.
        /// </summary>
        /// <param name="itemId">Item ID value.</param>
        /// <returns>Roulette bet name associated with the ID.</returns>
        public static string GetRouletteBetItem(object itemId)
        {
            return RefData.GetRouletteBetItem(itemId.ToString(), CurrentTWLanguage, string.Empty);
        }

        /// <summary>
        /// Gets casino bet name by ID.
        /// </summary>
        /// <param name="betStatusId">Bet status ID value.</param>
        /// <returns>Casino bet name associated with the ID.</returns>
        public static string GetCasinoBetItem(object betStatusId)
        {
            return RefData.GetCasinoBetItem(betStatusId.ToString(), CurrentTWLanguage, string.Empty);
        }

        /// <summary>
        /// Gets league name by ID.
        /// </summary>
        /// <param name="leagueId">League ID value.</param>
        /// <returns>League name associated with the ID.</returns>
        public static string GetLeagueName(object leagueId)
        {
            try
            {
                return RefData.GetLeagueName(leagueId.ToString(), CurrentTWLanguage, string.Empty);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets FNC Index name by ID.
        /// </summary>
        /// <param name="indexId">Index ID value.</param>
        /// <returns>FNC Index name associated with the ID.</returns>
        public static string GetFncIndexName(object indexId)
        {
            return RefData.GetFncIndexName(indexId.ToString(), CurrentTWLanguage);
        }

        /// <summary>
        /// Gets horse team name by ID.
        /// </summary>
        /// <param name="codeId">Code ID value.</param>
        /// <returns>Horse team name associated with the ID.</returns>
        public static string GetHorseTeamName(object codeId)
        {
            return RefData.GetHorseTeamName(codeId.ToString(), CurrentTWLanguage);
        }

        /// <summary>
        /// Gets odd type name by ID.
        /// </summary>
        /// <param name="oddsTypeId">Odd type ID value.</param>
        /// <returns>Odd type name associated with the ID.</returns>
        public static string GetOddsTypeName(object oddsTypeId)
        {
            return RefData.GetOddsTypeName(oddsTypeId.ToString(), CurrentTWLanguage);
        }
    }
}