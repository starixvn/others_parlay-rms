// --------------------------------------------------------------------------
// <copyright file="Enums.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models
{
    public enum StatusKickedOut
    {
        /// <summary>
        /// Kicked Out Status.
        /// </summary>
        KickedOut = -1,

        /// <summary>
        /// Continue Status.
        /// </summary>
        Continue = 1,

        /// <summary>
        /// Maintenance Status.
        /// </summary>
        Maintenance = 2,

        /// <summary>
        /// Login Status.
        /// </summary>
        Login = 3,
    }

    public enum SportsType : int
    {
        /// <summary>
        /// All Sport Type.
        /// </summary>
        AllSports = 0,

        /// <summary>
        /// Soccer Only Type.
        /// </summary>
        SoccerOnly = 1,

        /// <summary>
        /// Other Sports Type.
        /// </summary>
        OtherSports = 2,

        /// <summary>
        /// Combined Sports Type.
        /// </summary>
        CombinedSports = 3
    }

    public enum DateRange : int
    {
        /// <summary>
        /// Yesterday value.
        /// </summary>
        Yesterday = 1,

        /// <summary>
        /// Current Week value.
        /// </summary>
        CurrentWeek = 2,

        /// <summary>
        /// Current Month value.
        /// </summary>
        CurrentMonth = 3,

        /// <summary>
        /// Last Week value.
        /// </summary>
        LastWeek = 4,

        /// <summary>
        /// Last Month value.
        /// </summary>
        LastMonth = 5,

        /// <summary>
        /// Since Last Month value.
        /// </summary>
        SinceLastMonth = 6,

        /// <summary>
        /// Specific Date value.
        /// </summary>
        SpecificDate = 7
    }

    public enum RoleId : int
    {
        /// <summary>
        /// Super Role ID.
        /// </summary>
        Super = 4,

        /// <summary>
        /// Master Role ID.
        /// </summary>
        Master = 3,

        /// <summary>
        /// Agent Role ID.
        /// </summary>
        Agent = 2,

        /// <summary>
        /// Member Role ID.
        /// </summary>
        Member = 1
    }

    public enum BetType : int
    {
        /// <summary>
        /// Mix Parlay Bet type.
        /// </summary>
        MixParlay = 9,

        /// <summary>
        /// System Parlay Bet type.
        /// </summary>
        SystemParlay = 29
    }

    public enum Enums
    {
    }
}