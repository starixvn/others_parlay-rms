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

    public enum ParlayType : int
    {
        All = 0,

        MixParlay = 1,

        ComboParlay = 2,

        SingleMatchParlay = 3
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

    public enum SystemParlayCombo : int
    {
        /// <summary>
        /// Trixie combo.
        /// </summary>
        Trixie = 3,

        /// <summary>
        /// Yankee combo.
        /// </summary>
        Yankee = 4,

        /// <summary>
        /// Canadian combo.
        /// </summary>
        Canadian = 5,

        /// <summary>
        /// Heinz combo.
        /// </summary>
        Heinz = 6,

        /// <summary>
        /// Super Heinz combo.
        /// </summary>
        SuperHeinz = 7,

        /// <summary>
        /// Goliath combo.
        /// </summary>
        Goliath = 8
    }

    public enum SystemParlayBets : int
    {
        /// <summary>
        /// Trixie combo.
        /// </summary>
        Trixie = 4,

        /// <summary>
        /// Yankee combo.
        /// </summary>
        Yankee = 11,

        /// <summary>
        /// Canadian combo.
        /// </summary>
        Canadian = 26,

        /// <summary>
        /// Heinz combo.
        /// </summary>
        Heinz = 57,

        /// <summary>
        /// Super Heinz combo.
        /// </summary>
        SuperHeinz = 120,

        /// <summary>
        /// Goliath combo.
        /// </summary>
        Goliath = 247
    }

    public enum Market : int
    {
        /// <summary>
        /// The today only.
        /// </summary>
        TodayOnly = 1,

        /// <summary>
        /// The early only.
        /// </summary>
        EarlyOnly = 2,

        /// <summary>
        /// The combined.
        /// </summary>
        Combined = 3
    }

    public enum ReportType : int
    {
        Combo = 1,
        SportType = 2
    }
}