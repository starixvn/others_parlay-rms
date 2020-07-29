using ParlayRMS.Models;
using ParlayRMS.Models.Members;
using ParlayRMS.Utility;
using System;
using System.Collections.Generic;

namespace ParlayRMS.Common.Models
{
    public static class ModelsHelper
    {
        private static List<MemberLevel> memberLevels = new List<MemberLevel>();

        static ModelsHelper()
        {
            var levelDict = new Dictionary<int, string>();
            levelDict.Add(10, "level10");
            levelDict.Add(9, "level9");
            levelDict.Add(8, "level8");
            levelDict.Add(7, "level7");
            levelDict.Add(6, "level6");
            levelDict.Add(5, "level5");
            levelDict.Add(4, "level4");
            levelDict.Add(3, "level3");
            levelDict.Add(2, "level2");
            levelDict.Add(1, "level1");
            levelDict.Add(0, "levelNone");

            foreach (var entry in levelDict)
            {
                memberLevels.Add(new MemberLevel
                {
                    LevelId = DbConverter.ToInt32(entry.Key),
                    LevelColor = entry.Value,
                    LevelValue = DbConverter.ToInt32(entry.Key) == 0 ? "None" : ViewLang.Level + " " + entry.Key.ToString()
                });
            }
        }

        public static Dictionary<int, string> LeagueGroupColors = new Dictionary<int, string>
        {
            { 0, "#000000" },
            { 1, "#5b9ad5" },
            { 2, "#f4b083" },
            { 3, "#548235" },
            { 4, "#cf51c9" },
            { 5, "#843c0c" },
            { 6, "#6f30a0" },
            { 7, "#bf9000" },
            { 9, "#2244bf"},
            { 8, "#ff0000" },
        };
        public static string GetColorByLeagueGroup(int groupId)
        {
            return LeagueGroupColors.ContainsKey(groupId) ? LeagueGroupColors[groupId] : string.Empty;
        }

        public static Dictionary<int, string> LeagueGroupNames = new Dictionary<int, string>
        {
            { 0, "All Group" },
            { 1, "MJ" },
            { 2, "M1" },
            { 3, "M2" },
            { 4, "M3" },
            { 5, "M4 (a)" },
            { 6, "M4 (b)" },
            { 7, "M4 (x)" },
            { 9, "Virtual"},
            { 8, "Blacklisted" },
        };
        public static string GetNameByLeagueGroup(int groupId)
        {
            return LeagueGroupNames.ContainsKey(groupId) ? LeagueGroupNames[groupId] : string.Empty;
        }

        /// <summary>
        /// Gets Color Code By Level.
        /// The level list: 10, 9, 8, 7, 6, 5, 4, 3, 2, 1
        /// The color list: #ED1C24, #669999, #CCCC99, #333399, #006600, #CC6666, #FF9900, #CC6699, #7F7F7F, #653232.
        /// </summary>
        /// <param name="levelId">Level ID value.</param>
        /// <returns>Level value.</returns>
        public static string GetClassByMemberLevel(int levelId)
        {
            foreach (var level in memberLevels)
            {
                if (level.LevelId == levelId)
                {
                    return level.LevelColor;
                }
            }

            return string.Empty;
        }

        public static object GetReportType(ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.Combo:
                    return "Combo";

                case ReportType.SportType:
                    return "Sport type";

                default:
                    throw new NotImplementedException("Report type :" + reportType);
            }
        }

        public static List<MemberLevel> GetMemberLevels()
        {
            return memberLevels;
        }

        public static string GetSportType(string sportTypes)
        {
            var list = sportTypes.Split(',');
            if (list.Length == 14)
            {
                return ViewLang.All;
            }
            else if (list.Length == 1)
            {
                var sportType = Convert.ToInt32(list[0]);
                return GetSportType(sportType);
            }
            else
            {
                return string.Format("{0} options", list.Length);
            }
        }

        public static string GetSportType(int sportType)
        {
            switch (sportType)
            {
                case -1:
                    return "Combined other sports";

                case -2:
                    return "Other sports";

                case -3:
                    return "Combined sports";

                default:
                    return CacheRef.GetSportName(sportType);
            }
        }

        public static string GetSportType(SportsType sportType)
        {
            switch (sportType)
            {
                case SportsType.AllSports:
                    return ViewLang.Allsports;

                case SportsType.SoccerOnly:
                    return ViewLang.Socceronly;

                case SportsType.OtherSports:
                    return ViewLang.Othersports;

                case SportsType.CombinedSports:
                    return ViewLang.Combinedsports;
            }

            return "Undefined";
        }

        public static string GetParlayTitle(string parlayTypes)
        {
            if (parlayTypes == "1,2,3,4")
            {
                return ViewLang.All;
            }

            return parlayTypes.Replace("1", ViewLang.MixparLay)
                .Replace("2", ViewLang.ComboParlay)
                .Replace("3", ViewLang.SingleMatchParlay)
                .Replace("4", ViewLang.PenaltyShootoutCombo)
                .Replace(",", ", ");
        }

        public static string GetSimpleParlayTitle(string parlayTypes)
        {
            if (parlayTypes == "1,2")
            {
                return ViewLang.All;
            }

            return parlayTypes.Replace("1", ViewLang.MixparLay)
                .Replace("2", ViewLang.ComboParlay)
                .Replace(",", ", ");
        }

        public static string GetSiteTitle(int site)
        {
            switch (site)
            {
                case 1:
                    return "Deposit";
                case 0:
                    return "Credit";
                default:
                    return ViewLang.All;
            }
        }

        public static string GetLeagueGroup(int leagueGroup)
        {
            switch (leagueGroup)
            {
                case -1:
                    return "None";
                case 1:
                    return "MJ";
                case 2:
                    return "M1";
                case 3:
                    return "M2";
                case 4:
                    return "M3";
                case 5:
                    return "M4 (a)";
                case 6:
                    return "M4 (b)";
                case 7:
                    return "M4 (x)";
                case 8:
                    return "Blacklist";
                default:
                    return "Virtual";
            }
        }

        public static string GetLeagueGroupTitle(string leagueGroups)
        {
            var list = leagueGroups.Split(',');
            if (list.Length == 10)
            {
                return ViewLang.All;
            }
            else if (list.Length == 1)
            {
                var sportType = Convert.ToInt32(list[0]);
                return GetLeagueGroup(sportType);
            }
            else
            {
                return string.Format("{0} options", list.Length);
            }
        }
    }
}