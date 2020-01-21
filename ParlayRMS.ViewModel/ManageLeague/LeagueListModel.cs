namespace ParlayRMS.ViewModel.ManageLeague
{
    using System.Web.Mvc;

    /// <summary>
    /// Member List Model.
    /// </summary>
    public class LeagueListModel
    {
        [AllowHtml]
        public string League { get; set; }

        public int Group { get; set; }

        public int Page { get; set; }
    }
}