// -----------------------------------------------------------------------
// <copyright file="ModelsHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Common.Models
{
    using System.Collections.Generic;
    using ParlayRMS.Models.Members;
    using ParlayRMS.Utility;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
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
        /// Get Color Code By Level.
        /// Levels: 10, 9, 8, 7, 6, 5, 4, 3, 2, 1
        /// Colors: #ED1C24, #669999, #CCCC99, #333399, #006600, #CC6666, #FF9900, #CC6699, #7F7F7F, #653232
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public static string GetClassByMemberLevel(int levelId)
        {
            foreach (var level in memberLevels)
            {
                if (level.LevelId == levelId)
                {
                    return level.LevelColor;
                }
            }

            return "alllevel";
        }

        public static List<MemberLevel> GetMemberLevels()
        {
            return memberLevels;
        }

        public static string GetSportTypeById(int sportType)
        {
            string result = "Undefined";
            switch (sportType)
            {
                case 0:
                    result = ViewLang.Allsports;
                    break;

                case 1:
                    result = ViewLang.Socceronly;
                    break;

                case 2:
                    result = ViewLang.Othersports;
                    break;

                case 3:
                    result = ViewLang.Combinedsports;
                    break;
            }

            return result;
        }
    }
}