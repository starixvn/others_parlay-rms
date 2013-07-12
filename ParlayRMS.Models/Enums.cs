// -----------------------------------------------------------------------
// <copyright file="Enums.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Models
{
    public enum StatusKickedOut
    {
        KickedOut = -1,
        Continue = 1,
        Maintenance = 2,
        Login = 3,
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
        SystemParlay = 29
    }
}