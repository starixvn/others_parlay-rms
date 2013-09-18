// -----------------------------------------------------------------------
// <copyright file="DataModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.MixPalayMonitor
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Web.Mvc.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DataModel
    {
        public string Combos { get; set; }

        public string SportsTypes { get; set; }

        public string Levels { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal MaxPayout { get; set; }

        [Required, Values(10, 20, 50, 100, 200)]
        public int SelectTop { get; set; }

        [Required, Range(0, long.MaxValue)]
        public long MaxTransId { get; set; }
    }
}