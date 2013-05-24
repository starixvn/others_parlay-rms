// -----------------------------------------------------------------------
// <copyright file="DataModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.TotalOutstanding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DataModel
    {
        public DateTime Date { get; set; }

        public string Combo { get; set; }

        public string SportsType { get; set; }

        [AllowHtml()]
        public string Username { get; set; }

        public int SearchBy { get; set; }
    }
}
