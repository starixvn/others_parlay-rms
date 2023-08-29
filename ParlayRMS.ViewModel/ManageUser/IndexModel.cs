namespace ParlayRMS.ViewModel.ManageUser
{
    using System.Web.Mvc;

    public class IndexModel
    {
        [AllowHtml]
        public string UserName { get; set; }

        public int GroupId { get; set; }
    }
}
