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
            switch (parlayTypes)
            {
                case "1,2":
                    return ViewLang.All;

                case "1":
                    return ViewLang.MixparLay;

                case "2":
                    return ViewLang.ComboParlay;
            }

            return "Undefined";
        }
    }
}