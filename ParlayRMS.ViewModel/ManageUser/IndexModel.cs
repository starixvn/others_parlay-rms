// -----------------------------------------------------------------------
// <copyright file="IndexModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Web.Mvc;
namespace ParlayRMS.ViewModel.ManageUser
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IndexModel
    {
        [AllowHtml()]
        public string UserName { get; set; }

        public int GroupId { get; set; }
    }
}