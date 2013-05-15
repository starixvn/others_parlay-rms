// -----------------------------------------------------------------------
// <copyright file="IndexModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.TotalOutstanding
{
    using System.ComponentModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IndexModel
    {
        public string Username { get; set; }

        public string Date { get; set; }

        public string Combo { get; set; }

        public string SportsType { get; set; }

        public IndexModel()
        {
            this.Username = string.Empty;
            this.Date = string.Empty;
            this.Combo = string.Empty;
            this.SportsType = string.Empty;
        }
    }
}