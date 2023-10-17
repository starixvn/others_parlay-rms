namespace ParlayRMS.Models
{
    public enum ParlayType
    {
        All = 0,

        MixParlay = 1,

        ComboParlay = 2,

        SingleMatchParlay = 3,

        PenaltyShootoutCombo = 4,

        GodOfFortune = 5
    }

    public enum SportsType
    {
        AllSports = 0,

        SoccerOnly = 1,

        OtherSports = 2,

        CombinedSports = 3
    }

    public enum DateRange
    {
        Yesterday = 1,

        CurrentWeek = 2,

        CurrentMonth = 3,

        LastWeek = 4,

        LastMonth = 5,

        SinceLastMonth = 6,

        SpecificDate = 7
    }

    public enum RoleId
    {
        Super = 4,

        Master = 3,

        Agent = 2,

        Member = 1
    }

    public enum BetType
    {
        MixParlay = 9,

        SystemParlay = 29,

        SingleMatchParlay = 38,

        PenaltyShootout = 376
    }

    public enum Market
    {
        TodayOnly = 1,

        EarlyOnly = 2,

        Combined = 3
    }

    public enum ReportType
    {
        Combo = 1,
        SportType = 2
    }
}
