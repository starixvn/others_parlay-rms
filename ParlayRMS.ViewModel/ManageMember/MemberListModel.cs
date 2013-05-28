// -----------------------------------------------------------------------
// <copyright file="MemberListModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberListModel
    {
        [AllowHtml()]
        public string UserName { get; set; }

        public int? Level { get; set; }

        public int? Page { get; set; }
    }
}