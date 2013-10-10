// --------------------------------------------------------------------------
// <copyright file="MemberLevel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Members
{
    /// <summary>
    /// Member Level Model.
    /// </summary>
    public class MemberLevel
    {
        public int LevelId { get; set; }

        public string LevelValue { get; set; }

        public string LevelColor { get; set; }
    }
}