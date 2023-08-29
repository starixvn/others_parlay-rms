namespace ParlayRMS.ViewModel.ManageMember
{
    using System.Web.Mvc;

    public class MemberListModel
    {
        [AllowHtml]
        public string UserName { get; set; }

        public int? Level { get; set; }

        public int? Page { get; set; }
    }
}
