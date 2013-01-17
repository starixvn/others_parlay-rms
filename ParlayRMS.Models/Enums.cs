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
}