using System.Threading;
using Fanex.RemotingRef;

namespace ParlayRMS.Utility
{
    public class CacheRef
    {
        public static RemotingRefService RemotingRefService = new RemotingRefService();

        public static string Language
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

        public static string GetBettype(object bettypeId)
        {
            try
            {
                return RemotingRefService.GetBetTypeName(bettypeId.ToString(), Language);
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
                return RemotingRefService.GetSportName(sportId.ToString(), Language);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetSportNameDropDown(int sportId)
        {
            switch (sportId)
            {
                case -1:
                    return "Combined other sports";

                case -2:
                    return "Other sports";

                case -3:
                    return "Combined sports";

                default:
                    return GetSportName(sportId);
            }
        }

        public static string GetTeamName(object teamId)
        {
            try
            {
                return RemotingRefService.GetTeamName(teamId.ToString(), Language);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetRouletteBetItem(object itemId)
        {
            return RemotingRefService.GetRouletteBetItem(itemId.ToString(), Language);
        }

        public static string GetCasinoBetItem(object betStatusId)
        {
            return RemotingRefService.GetCasinoBetItem(betStatusId.ToString(), Language);
        }

        public static string GetLeagueName(object leagueId)
        {
            try
            {
                return RemotingRefService.GetLeagueName(leagueId.ToString(), Language);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetHorseTeamName(object codeId)
        {
            return RemotingRefService.GetHorseTeamName(codeId.ToString(), Language);
        }

        public static string GetOddsTypeName(object oddsTypeId)
        {
            return RemotingRefService.GetOddsTypeName(oddsTypeId.ToString(), Language);
        }

        public static string[] GetCombinationTypeCombo(string betTeam, string betCheck, int hdp1, int hdp2)
        {
            betCheck = betCheck.Replace(".00", string.Empty);
            return RemotingRefService.GetCombinationTypeCombo(betTeam, betCheck, hdp1, hdp2, Language);
        }
    }
}