namespace ParlayRMS.Models
{
    public enum StatusKickedOut
    {
        KickedOut = -1,

        Continue = 1,

        Maintenance = 2,

        Login = 3,
    }

    public enum ParlayType : int
    {
        All = 0,

        MixParlay = 1,

        ComboParlay = 2,

        SingleMatchParlay = 3,

        PenaltyShootoutCombo = 4,

        GodOfFortune = 5
    }

    public enum SportsType : int
    {
        AllSports = 0,

        SoccerOnly = 1,

        OtherSports = 2,

        CombinedSports = 3
    }

    public enum DateRange : int
    {
        Yesterday = 1,

        CurrentWeek = 2,

        CurrentMonth = 3,

        LastWeek = 4,

        LastMonth = 5,

        SinceLastMonth = 6,

        SpecificDate = 7
    }

    public enum RoleId : int
    {
        Super = 4,

        Master = 3,

        Agent = 2,

        Member = 1
    }

    public enum BetType : int
    {
        MixParlay = 9,

        SystemParlay = 29,

        SingleMatchParlay = 38,

        PenaltyShootout = 376
    }

    public enum SystemParlayCombo : int
    {
        Trixie = 3,

        Yankee = 4,

        Canadian = 5,

        Heinz = 6,

        SuperHeinz = 7,

        Goliath = 8
    }

    public enum SystemParlayBets : int
    {
        Trixie = 4,

        Yankee = 11,

        Canadian = 26,

        Heinz = 57,

        SuperHeinz = 120,

        Goliath = 247
    }

    public enum Market : int
    {
        TodayOnly = 1,

        EarlyOnly = 2,

        Combined = 3
    }

    public enum ReportType : int
    {
        Combo = 1,
        SportType = 2
    }
}
