// -----------------------------------------------------------------------
// <copyright file="MixParlayFilter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.MixPalayMonitor
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Utility;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FilterModel
    {
        [Display(Name = "Combo", ResourceType = typeof(ViewLang))]
        public int Combo { get; set; }

        [Display(Name = "SportsType", ResourceType = typeof(ViewLang))]
        public int SportsType { get; set; }

        [Display(Name = "Level", ResourceType = typeof(ViewLang))]
        public int Level { get; set; }

        [Display(Name = "MaxPayoutAfterPT", ResourceType = typeof(ViewLang))]
        public decimal MaxPayout { get; set; }

        [Display(Name = "SelectTop", ResourceType = typeof(ViewLang))]
        public int SelectTop { get; set; }

        [Display(Name = "Refreshtime", ResourceType = typeof(ViewLang))]
        public int RefreshTime { get; set; }

        [Display(Name = "Pause", ResourceType = typeof(ViewLang))]
        public bool IsPause { get; set; }
    }
}