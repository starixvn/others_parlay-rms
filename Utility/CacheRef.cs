namespace ParlayRMS.Utility
{
    using System.Threading;
    using RemotingRef;

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

        public static string GetBettype(object bettypeId)
        {
            return GetBettype(bettypeId, null);
        }

        public static string GetBettype(object bettypeId, object betID)
        {
            // resourceId remove first character
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

        public static string GetRouletteBetItem(object itemId)
        {
            return RefData.GetRouletteBetItem(itemId.ToString(), CurrentTWLanguage, string.Empty);
        }

        public static string GetCasinoBetItem(object betStatusId)
        {
            return RefData.GetCasinoBetItem(betStatusId.ToString(), CurrentTWLanguage, string.Empty);
        }

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

        public static string GetFncIndexName(object indexId)
        {
            return RefData.GetFncIndexName(indexId.ToString(), CurrentTWLanguage);
        }

        public static string GetHorseTeamName(object codeId)
        {
            return RefData.GetHorseTeamName(codeId.ToString(), CurrentTWLanguage);
        }

        public static string GetOddsTypeName(object oddsTypeId)
        {
            return RefData.GetOddsTypeName(oddsTypeId.ToString(), CurrentTWLanguage);
        }
    }
}