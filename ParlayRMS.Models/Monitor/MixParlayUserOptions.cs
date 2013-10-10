// --------------------------------------------------------------------------
// <copyright file="MixParlayUserOptions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Monitor
{
    /// <summary>
    /// Mix Parlay User Options.
    /// </summary>
    public class MixParlayUserOptions
    {
        public MixParlayUserOptions(string username, string combos, string sportsTypes, string levels, decimal maxPayout, int selectTop)
        {
            this.Username = username;
            this.Combos = combos;
            this.SportsTypes = sportsTypes;
            this.Levels = levels;
            this.MaxPayout = maxPayout;
            this.SelectTop = selectTop;
        }

        public string Username { get; set; }

        public string Combos { get; set; }

        public string SportsTypes { get; set; }

        public string Levels { get; set; }

        public decimal MaxPayout { get; set; }

        public int SelectTop { get; set; }
    }
}