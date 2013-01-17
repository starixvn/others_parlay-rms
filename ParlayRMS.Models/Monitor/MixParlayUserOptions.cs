// -----------------------------------------------------------------------
// <copyright file="MixParlayUserOptions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Models.Monitor
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MixParlayUserOptions
    {
        public string Username { get; set; }

        public string Combos { get; set; }

        public string SportsTypes { get; set; }

        public string Levels { get; set; }

        public decimal MaxPayout { get; set; }

        public int SelectTop { get; set; }

        public MixParlayUserOptions(string username, string combos, string sportsTypes, string levels, decimal maxPayout, int selectTop)
        {
            this.Username = username;
            this.Combos = combos;
            this.SportsTypes = sportsTypes;
            this.Levels = levels;
            this.MaxPayout = maxPayout;
            this.SelectTop = selectTop;
        }
    }
}