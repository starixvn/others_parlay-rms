// --------------------------------------------------------------------------
// <copyright file="ModelsHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Common.Models
{
    using System.Collections.Generic;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Members;
    using ParlayRMS.Utility;

    /// <summary>
    /// Models Helper.
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

        /// <summary>
        /// Gets list of member levels.
        /// </summary>
        /// <returns>List of MemberLevel objects.</returns>
        public static List<MemberLevel> GetMemberLevels()
        {
            return memberLevels;
        }

        /// <summary>
        /// Gets sport type name from resource by ID.
        /// </summary>
        /// <param name="sportType">Sport type ID.</param>
        /// <returns>Sport type name.</returns>
        public static string GetSportType(int sportType)
        {
            switch (sportType)
            {
                case 0:
                    return ViewLang.Allsports;

                case 1:
                    return ViewLang.Socceronly;

                case 2:
                    return ViewLang.Othersports;

                case 3:
                    return ViewLang.Combinedsports;
            }

            return "Undefined";
        }

        /// <summary>
        /// Gets bet type name from resource by ID.
        /// </summary>
        /// <param name="betType">Bet type ID.</param>
        /// <returns>Bet type name.</returns>
        public static string GetBetType(int betType)
        {
            switch (betType)
            {
                case (int)BetType.MixParlay:
                    return ViewLang.MixparLay;

                case (int)BetType.SystemParlay:
                    return ViewLang.SystemparLay;
            }

            return "Undefined";
        }
    }
}