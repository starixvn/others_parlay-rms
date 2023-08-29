namespace ParlayRMS.ViewModel.ManageAgent
{
    using System.Web.Mvc;

    public class AgentListModel
    {
        [AllowHtml]
        public string UserName { get; set; }

        public int? Page { get; set; }
    }
}
